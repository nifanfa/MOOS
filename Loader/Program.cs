using Internal.Runtime.CompilerServices;
using Kernel;
using System;
using System.Runtime;

public static class EntryPoint
{
    const uint LOADER_VERSION = 0x00010000; // 1.0.0

    [RuntimeExport("EfiMain")]
    unsafe static long EfiMain(EFI.Handle imageHandle, EFI.SystemTable* systemTable)
    {
        EFI.EFI.Initialise(systemTable);
        ref var st = ref systemTable;
        var bs = st->BootServices;

        bs->SetWatchdogTimer(0, 0, 0, IntPtr.Zero);

        EFI.Status res;

        res = bs->OpenProtocol(
            imageHandle,
            ref EFI.Guid.LoadedImageProtocol,
            out EFI.LoadedImageProtocol* li,
            imageHandle, EFI.Handle.Zero, EFI.EFI.OPEN_PROTOCOL_GET_PROTOCOL
        );

        if (res != EFI.Status.Success)
            return 0;

        res = bs->OpenProtocol(
            li->DeviceHandle,
            ref EFI.Guid.SimpleFileSystemProtocol,
            out EFI.SimpleFileSystemProtocol* fs,
            imageHandle, EFI.Handle.Zero, EFI.EFI.OPEN_PROTOCOL_GET_PROTOCOL
        );

        if (res != EFI.Status.Success)
            return 0;

        res = fs->OpenVolume(out var rDrive);

        if (res != EFI.Status.Success)
            return 0;

        ref var drive = ref rDrive;
        res = drive->Open(out var rKernel, "Kernel", EFI.FileMode.Read, EFI.FileAttribute.ReadOnly);

        if (res != EFI.Status.Success)
            return 0;

        ref var kernel = ref rKernel;
        var fileInfoSize = (ulong)Unsafe.SizeOf<EFI.FileInfo>();
        kernel->GetInfo(ref EFI.Guid.FileInfo, ref fileInfoSize, out EFI.FileInfo fileInfo);

        kernel->Read(out PE.DOSHeader dosHdr);

        if (dosHdr.e_magic != 0x5A4D) // IMAGE_DOS_SIGNATURE ("MZ")
            return 0;

        kernel->SetPosition((ulong)dosHdr.e_lfanew);
        kernel->Read(out PE.NtHeaders64 ntHdr);

        if (ntHdr.Signature != 0x4550) // IMAGE_NT_SIGNATURE ("PE\0\0")
            return 0;

        if (!(ntHdr.FileHeader.Machine == 0x8664 /* IMAGE_FILE_MACHINE_X64 */ && ntHdr.OptionalHeader.Magic == 0x020B /* IMAGE_NT_OPTIONAL_HDR64_MAGIC */))
            return 0;

        ulong sectionCount = ntHdr.FileHeader.NumberOfSections;
        ulong virtSize = 0;
        kernel->Read(out PE.SectionHeader[] sectionHdrs, (int)sectionCount);

        for (var i = 0U; i < sectionCount; i++)
        {
            ref var sec = ref sectionHdrs[i];
            virtSize =
                virtSize > sec.VirtualAddress + sec.PhysicalAddress_VirtualSize
                ? virtSize
                : sec.VirtualAddress + sec.PhysicalAddress_VirtualSize;
        }

        ulong hdrSize = ntHdr.OptionalHeader.SizeOfHeaders;
        ulong pages = ((virtSize) >> 12) + (((virtSize) & 0xFFF) > 0 ? 1U : 0U);
        var mem = (IntPtr)ntHdr.OptionalHeader.ImageBase;
        res = bs->AllocatePages(EFI.AllocateType.Address, EFI.MemoryType.LoaderData, pages, ref mem);
        
        if (res != EFI.Status.Success)
            return 0;

        Allocator.ZeroFill(mem, pages << 12);
        kernel->SetPosition(0U);
        kernel->Read(ref hdrSize, mem);

        var modulesSeg = IntPtr.Zero;

        for (var i = 0U; i < sectionCount; i++)
        {
            ref var sec = ref sectionHdrs[i];
            var name = string.FromASCII(sec.Name, 8);

            if (sec.SizeOfRawData == 0)
            {
                continue;
            }

            if (name[1] == 'm' && name[2] == 'o')
                modulesSeg = mem + sec.VirtualAddress;

            name.Dispose();

            var addr = mem + sec.VirtualAddress;
            res = kernel->SetPosition(sec.PointerToRawData);

            if (res != EFI.Status.Success)
                return 0;

            var len = (ulong)sec.SizeOfRawData;
            res = kernel->Read(ref len, addr);

            if (res != EFI.Status.Success)
                return 0;
        }

        kernel->Close();
        drive->Close();
        sectionHdrs.Dispose();

        ulong numHandles = 0;
        bs->LocateHandleBuffer(EFI.LocateSearchType.ByProtocol, ref EFI.Guid.GraphicsOutputProtocol, IntPtr.Zero, ref numHandles, out var gopHandles);
        bs->OpenProtocol(
            gopHandles[0],
            ref EFI.Guid.GraphicsOutputProtocol,
            out EFI.GraphicsOutputProtocol* rGop,
            imageHandle, EFI.Handle.Zero, EFI.EFI.OPEN_PROTOCOL_GET_PROTOCOL
        );
        ref var gop = ref rGop;
        var gopMode = gop->Mode;

        /*
        for (var j = 0U; j < gopMode->MaxMode; j++)
        {
            gop->QueryMode(j, out var size, out var rInfo);
            ref var info = ref rInfo;
        }
        */

        var gpuMode = 0U;

        gop->SetMode(gpuMode);
        gopMode = gop->Mode;
        var mmap = new MemoryMap();
        mmap.Retrieve();

        IntPtr ptr = (IntPtr)((((ulong)Allocator.Allocate(0x6400000)) + 0x100000) & ~0x100000UL);

        res = bs->ExitBootServices(imageHandle, mmap.Key);

        // No Platform.Print* after this point!

        if (res != EFI.Status.Success)
        {
            mmap.Free();
            mmap.Retrieve();

            res = bs->ExitBootServices(imageHandle, mmap.Key);

            if (res != EFI.Status.Success)
            {
                mmap.Free();

                return (int)res;
            }
        }

        var epLoc = mem + ntHdr.OptionalHeader.AddressOfEntryPoint;
        delegate*<IntPtr, uint, uint, IntPtr, IntPtr, void> entry = (delegate*<IntPtr, uint, uint, IntPtr, IntPtr, void>)epLoc;
        entry(
            (IntPtr)gopMode->FrameBufferBase,
            gopMode->Info->HorizontalResolution,
            gopMode->Info->VerticalResolution,
            ptr,
            modulesSeg
            );

        // We should never get here!
        // QEMU shutdown
        //Native.outw(0xB004, 0x2000);
        //Platform.PrintLine("\r\n\r\nPress any key to quit...");
        //Console.ReadKey();

        return 0;
    }

    static void Main() { }
}