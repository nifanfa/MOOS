#include <efi.h>
#include <efilib.h>
#include <libsmbios.h>
#include <ia64/pe.h>

#if defined(_M_X64) || defined(__x86_64__)
static CHAR16* Arch = L"x64";
static CHAR16* ArchName = L"64-bit x86";
#elif defined(_M_IX86) || defined(__i386__)
static CHAR16* Arch = L"ia32";
static CHAR16* ArchName = L"32-bit x86";
#elif defined (_M_ARM64) || defined(__aarch64__)
static CHAR16* Arch = L"aa64";
static CHAR16* ArchName = L"64-bit ARM";
#elif defined (_M_ARM) || defined(__arm__)
static CHAR16* Arch = L"arm";
static CHAR16* ArchName = L"32-bit ARM";
#else
#  error Unsupported architecture
#endif

#define PAGES(size)					(size >> 12)+((size & 0xFFF)==0 ? 0:1)

struct BOOTINFO
{
	UINT64 Framebuffer;
	UINT32 Width;
	UINT32 Height;
	UINT64 MemoryStart;
	UINT64 NumPages;
	UINT64 Modules;
} bootinfo;

#define UEFI_MMAP_SIZE 0x4000
struct uefi_mmap {
	uint64_t nbytes;
	uint8_t buffer[UEFI_MMAP_SIZE];
	uint64_t mapkey;
	uint64_t desc_size;
	uint32_t desc_version;
} uefi_mmap;

struct _MY_IMAGE_SECTION_HEADER {
	UINT8  Name[8];
	UINT32 PhysicalAddress_VirtualSize;
	UINT32 VirtualAddress;
	UINT32 SizeOfRawData;
	UINT32 PointerToRawData;
	UINT32 PointerToRelocations;
	UINT32 PointerToLineNumbers;
	UINT16 NumberOfRelocations;
	UINT16 NumberOfLineNumbers;
	UINT32 Characteristics;
};

// Application entrypoint (must be set to 'efi_main' for gnu-efi crt0 compatibility)
EFI_STATUS efi_main(EFI_HANDLE ImageHandle, EFI_SYSTEM_TABLE* SystemTable)
{
#if defined(_GNU_EFI)
	InitializeLib(ImageHandle, SystemTable);
#endif

#pragma region LOADKERNEL
	EFI_STATUS sts;

	uefi_call_wrapper(ST->ConOut->ClearScreen, 1, ST->ConOut);
	uefi_call_wrapper(ST->ConOut->EnableCursor, 2, ST->ConOut, TRUE);

	sts = uefi_call_wrapper(BS->SetWatchdogTimer, 4, 0, 0, 0, NULL);
	if (EFI_ERROR(sts)) { Print(L"Can't disable watch dog timer\r\n"); for (;;); }

	EFI_LOADED_IMAGE_PROTOCOL* loaded_image;
	EFI_GUID loaded_image_guid = EFI_LOADED_IMAGE_PROTOCOL_GUID;

	sts = uefi_call_wrapper(BS->HandleProtocol, 3, ImageHandle, &loaded_image_guid, &loaded_image);
	if (EFI_ERROR(sts)) { Print(L"Can't handle loaded image\r\n"); for (;;); }

	EFI_SIMPLE_FILE_SYSTEM_PROTOCOL* simple_fs_protocol;
	EFI_GUID simple_fs_protocol_guid = EFI_SIMPLE_FILE_SYSTEM_PROTOCOL_GUID;
	sts = uefi_call_wrapper(BS->HandleProtocol, 3, loaded_image->DeviceHandle, &simple_fs_protocol_guid, &simple_fs_protocol);
	if (EFI_ERROR(sts)) { Print(L"Can't handle simple file system protocol\r\n"); for (;;); }

	EFI_FILE_HANDLE volume;
	sts = uefi_call_wrapper(simple_fs_protocol->OpenVolume, 2, simple_fs_protocol, &volume);
	if (EFI_ERROR(sts)) { Print(L"Can't open volume\r\n"); for (;;); }

	EFI_FILE_HANDLE kernel_handle;
	sts = uefi_call_wrapper(volume->Open, 5, volume, &kernel_handle, L"kernel", EFI_FILE_MODE_READ, EFI_FILE_READ_ONLY | EFI_FILE_HIDDEN | EFI_FILE_SYSTEM);
	if (EFI_ERROR(sts)) { Print(L"Can't open kernel\r\n"); for (;;); }

	EFI_FILE_INFO* finfo;
	finfo = LibFileInfo(kernel_handle);
	Print(L"Kernel size: %dKiB\r\n", finfo->FileSize / 1024);

	UINT8* kernel = AllocatePool(finfo->FileSize);
	sts = uefi_call_wrapper(kernel_handle->Read, 3, kernel_handle, &finfo->FileSize, kernel);
	if (EFI_ERROR(sts)) { Print(L"Can't read kernel\r\n"); for (;;); }

	Print(L"Kernel read at: 0x%x\r\n", kernel);
#pragma endregion
	
	struct _IMAGE_DOS_HEADER* dos = kernel;
	struct _IMAGE_NT_HEADERS* nthdr = kernel + dos->e_lfanew;

	EFI_PHYSICAL_ADDRESS base = nthdr->OptionalHeader.ImageBase;
	Print(L"Image base: 0x%x\r\n", base);
	
	struct _MY_IMAGE_SECTION_HEADER* sec = kernel + dos->e_lfanew + sizeof(struct _IMAGE_NT_HEADERS);

	UINT64 virtSize = 0;
	for (INT32 i = 0; i < nthdr->FileHeader.NumberOfSections; i++)
	{
		virtSize = virtSize > sec[i].VirtualAddress + sec[i].PhysicalAddress_VirtualSize
			? virtSize
			: sec[i].VirtualAddress + sec[i].PhysicalAddress_VirtualSize;
	}
	Print(L"Virtual size: %dKiB\r\n", virtSize / 1024);

	sts = uefi_call_wrapper(BS->AllocatePages, 4, AllocateAddress, EfiLoaderData, PAGES(virtSize), &base);
	if (EFI_ERROR(sts)) { Print(L"Can't allocate pages for kernel\r\n"); for (;;); }

	uefi_call_wrapper(BS->CopyMem, 3, base, kernel, finfo->FileSize);

	UINT64 modules = NULL;
	for (INT32 i = 0; i < nthdr->FileHeader.NumberOfSections; i++)
	{
		Print(L"Loading Section: ");
		for (INT32 k = 0; k < 8; k++)
			Print(L"%c", sec[i].Name[k]);
		Print(L"\r\n");
		if (*(UINT64*)sec[i].Name == 0x73656C75646F6D2E)modules = base + sec[i].VirtualAddress;
		uefi_call_wrapper(BS->CopyMem, 3, base + sec[i].VirtualAddress, kernel + sec[i].PointerToRawData, sec[i].SizeOfRawData);
	}
	Print(L"Modules section is at: 0x%x\r\n", modules);

	EFI_GUID gop_guid = EFI_GRAPHICS_OUTPUT_PROTOCOL_GUID;
	EFI_GRAPHICS_OUTPUT_PROTOCOL* gop;
	sts = uefi_call_wrapper(BS->LocateProtocol, 3, &gop_guid, NULL, &gop);
	if (EFI_ERROR(sts)) { Print(L"Can't locate graphics output protocol\r\n"); for (;;); }

	UINT32 width = gop->Mode->Info->HorizontalResolution;
	UINT32 height = gop->Mode->Info->VerticalResolution;
	UINT64 fb = gop->Mode->FrameBufferBase;

	Print(L"GOP width: %d\r\n", width);
	Print(L"GOP height: %d\r\n", height);
	Print(L"GOP fb: 0x%x\r\n", fb);

	//https://blog.llandsmeer.com/tech/2019/07/21/uefi-x64-userland.html
	/* call GetMemoryMap(size, buffer, mapkey, desc_size, desc_version) */
	uefi_mmap.nbytes = UEFI_MMAP_SIZE;
	sts = uefi_call_wrapper(BS->GetMemoryMap, 5,
		&uefi_mmap.nbytes,
		uefi_mmap.buffer,
		&uefi_mmap.mapkey,
		&uefi_mmap.desc_size,
		&uefi_mmap.desc_version);
	if (EFI_ERROR(sts)) { Print(L"Can't get memory map\r\n"); for (;;); }
	/* find largest continuous chunk of EfiConventionalMemory */
	uint64_t best_alloc_start = 0;
	uint64_t best_number_of_pages = PAGES(0x2000000);
	for (int i = 0; i < uefi_mmap.nbytes; i += uefi_mmap.desc_size) {
		EFI_MEMORY_DESCRIPTOR* desc = (EFI_MEMORY_DESCRIPTOR*)&uefi_mmap.buffer[i];
		if (desc->Type != EfiConventionalMemory) continue;
		if (desc->NumberOfPages > best_number_of_pages) {
			best_number_of_pages = desc->NumberOfPages;
			best_alloc_start = desc->PhysicalStart;
		}
	}
	if (best_alloc_start == 0) { Print(L"No is memory for use\r\n"); for (;;); }
	/* call ExitBootServices(ImageHandle, mapkey) */
	sts = uefi_call_wrapper(BS->ExitBootServices, 2,
		ImageHandle,
		uefi_mmap.mapkey);
	if (EFI_ERROR(sts)) { Print(L"Can't exit boot services\r\n"); for (;;); }
	/* we are in control of the memory map now :) */

	bootinfo.Framebuffer = fb;
	bootinfo.Width = width;
	bootinfo.Height = height;
	bootinfo.MemoryStart = best_alloc_start;
	bootinfo.NumPages = best_number_of_pages;
	bootinfo.Modules = modules;

	void (*entry)(struct BOOTINFO*) = (nthdr->OptionalHeader.ImageBase + nthdr->OptionalHeader.AddressOfEntryPoint);
	(*entry)(&bootinfo);

	for (;;);

	return EFI_SUCCESS;
}