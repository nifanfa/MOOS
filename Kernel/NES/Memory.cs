using System;
using System.Collections.Generic;

namespace NES
{
    public class MemoryMap
    {
        Registers registers;
        Mappers mappers;
        Input input;
        NES tn;

        /* -----   MEMORY MAP   -----
           
           ----- CPU Memory Map -----
         
        -Purpose-               -Addr-      -Length (Bytes)-
    - [32K] Memory Mapped Cartridge PRG-ROM
        PRG-ROM Upper Bank      (C000-FFFF) [16384] // Cartridge RAM - Second Bank
        PRG-ROM Lower Bank      (8000-BFFF) [16384] // Cartridge RAM - First Bank
    - [32K] Internal CPU Memory
        SRAM                    (6000-7FFF) [ 8192]
        Expansion ROM           (4020-5FFF) [ 8160]
        I/O Registers           (4000-401F) [   32] // APU (Audio) I/O control, joypads and misc., $4014 used for PPU DMA
        Mirrors                 (2008-3FFF) [ 8184] // Mirrors 2000-2007 every eight bytes from 2008 to 3FFF
        I/O Registers           (2000-2007) [    8] // PPU I/O control and misc.
        Mirrors                 (0800-1FFF) [ 6144] // Mirrors 0000-07FF 3 Times Starting at 0800, 1000 and 1800
        RAM                     (0200-07FF) [ 1536]
        Stack                   (0100-01FF) [  256]
        Zero-Page               (0000-00FF) [  256]
         
           ----- PPU Memory Map -----

        Mirror                  (4000-FFFF)[49152] // Mirrors ($0000-$3FFF) <- Logical, not physical
    - [16K] Internal NES VRAM -
        Mirror                  (3F20-3FFF) [ 224] // Mirrors ($3F00-$3F1F)
        Sprite Palette          (3F10-3F1F) [  16] // Points (does not contain) to index of colors that can be used by sprites (only 64 possiblities, bits 6 and 7 can be ignored)
        Image Palette           (3F00-3F0F) [  16] // Points (does not contain) to index of colors that can be used by background (only 64 possiblities, bits 6 and 7 can be ignored)
        Mirror                  (3000-3EFF) [3839] // Mirrors ($2000-$2EFF)
        Attribute Table 3       (2FC0-2FFF) [  64] // 
        Name Table 3            (2C00-2FBF) [ 960] // (32 x 30 Tiles. Each tile is 8bits x 8bits.  32 Bytes * 30 Bytes = 960B or one whole 256x240 screen)
        Attribute Table 2       (2BC0-2BFF) [  64] // 
        Name Table 2            (2800-2BBF) [ 960] // (32 x 30 Tiles. Each tile is 8bits x 8bits.  32 Bytes * 30 Bytes = 960B or one whole 256x240 screen)
        Attribute Table 1       (27C0-27FF) [  64] // 
        Name Table 1            (2400-27BF) [ 960] // (32 x 30 Tiles. Each tile is 8bits x 8bits.  32 Bytes * 30 Bytes = 960B or one whole 256x240 screen)
        Attribute Table 0       (23C0-23FF) [  64] // 
        Name Table 0            (2000-23BF) [ 960] // (32 x 30 Tiles)
    - [8K] Memory Mapped Cartridge CHR-ROM (VRAM/VROM) 
        Pattern Table 1         (1000-1FFF) [4096] // (256 x 2 x 8, may be VROM)
        Pattern Table 2         (0000-0FFF) [4096] // (256 x 2 x 8, may be VROM)
         
        
           ----- Sprite Memory Map -----     
        
         SPR-RAM (OAM)          (0000-00FF) [ 256] // Contains Sprite Attributes (64 sprites) x (4 bytes each) = 256 bytes
         
        */

        public byte[] memSPRRAM = new byte[256];    // SPR-RAM - Contains Sprite Attributes (64 sprites) x (4 bytes each) = 256 bytes
        public byte[] memCPU = new byte[65536];     // 64K of CPU Memory
        public byte[] memPPU = new byte[0x4000];    // 16K of PPU Memory

        public List<byte[]> memCHR;    // Undefined amount of CHR-ROM for use with mappers
        public List<byte[]> memPRG;    // Undefined amount of PRG-ROM for use with mappers

        // Mapper Stuff
        public byte MapperNumber;
        public byte MapperBank;

        // Cartridge Header
        public byte byteMirror;

        // Public Access to the VBlank Flag Status
        public bool bolVBlank = false;

        public bool spriteOverflow = false;

        // Temporary placeholder for bogus 2007 reads from addr [2000-2FFF]
        byte byteLastRead = 0;

        // PPU Access (0x2006) Counter
        /* 0x2006 is written to 2 times followed by a read/write to 0x2007.
           This happens because each memory location can only take 1 byte so
           two writes are necessary to get the full address.  This counter
           (VRAMAddrCounter) is used in the routines below to make
           sure both bytes are written to the appropriate location before
           0x2007 is called for reading/written to the location specified by
           0x2006.  A variable (VRAMAddr) has also been setup to hold the
           new 2-byte address.*/
        int VRAMAddr = 0;
        //int VRAMAddrFirstWrite = 0;

        // Scrolling Variables
        bool PPULatchToggle = false;
        public int t;
        public byte fineX;

        public void WritePRG(int addr, byte data)
        {
            // Prevents invalid reads by wrapping around after addition
            addr &= 0xFFFF;

            // Copy to 0x0000-0x07FF and Mirror at 0x0800-0x1FFF
            if (addr < 0x2000)
            {
                // To intended memory address
                memCPU[addr & 0x07FF] = data;
            }

            // Handle PPU related OpCodes (in Memory class)
            else if ((addr >= 0x2000) && (addr <= 0x2007))
            {
                // Set byte to memory
                setPPUMemoryByte(addr, data);
            }

            // Address Masking for PPU at 2008-3FFF
            else if ((addr >= 0x2008) && (addr <= 0x3FFF))
            {
                setPPUMemoryByte(addr & 0x2007, data);
            }

            #region 256 Byte DMA Write to Sprite Memory
            // Handles 256 Byte DMA Write to Sprite Memory
            else if (addr == 0x4014)
            {
                for (int i = 0; i < 256; ++i, ++memCPU[0x2003])
                {
                    memSPRRAM[memCPU[0x2003]] = memCPU[0x0100 * data + i];
                }

                // This operation takes 512 cpu cycles
                tn.AddCPUCycles(512);
            }
            #endregion

            #region Write to Joypads
            // Joypads
            else if (addr == 0x4016)
            {
                input.WriteJoypad(data);
            }
            #endregion

            #region Mappers
            else if (addr >= 0x8000 && addr <= 0xFFFF)
            {
                //switch (MapperNumber)
                //{
                //    case 00:
                //        {
                //            // Do nothing, just write to address
                //            memory.memCPU[addr] = data;
                //        }
                //        break;

                //    case 02:
                //    case 66:
                //        {
                //            if (MapperBank != (byte)((data & (memory.memPRG.Length - 1))))
                //            {
                //                MapperBank = (byte)((data & (memory.memPRG.Length - 1)));

                //                for (int i = 0; i < 0x4000; i++)
                //                {
                //                    memory.memCPU[i + 0x8000] = memory.memPRG[MapperBank][i];
                //                }
                //            }
                //        }
                //        break;

                //    case 03:
                //    case 67:
                //    case 64:
                //        {
                //            if (MapperBank != (byte)((data & (memory.memCHR.Length - 1))))
                //            {
                //                MapperBank = (byte)((data & (memory.memCHR.Length - 1)));

                //                for (int i = 0; i < 0x2000; i++)
                //                {
                //                    memory.memPPU[i] = memory.memCHR[MapperBank][i];
                //                }
                //            }
                //        }
                //        break;

                //    default:
                //        {
                //            MessageBox.Show("Unsupported Mapper Number: " + Convert.ToString(MapperNumber));
                //        }
                //        break;
                //}
            }
            #endregion
            // Handles all memory spaces not related to PPU, and controllers
            else
            {
                if (addr < 0x4000 || addr > 0x4020)
                {
                    // MessageBox.Show("[OpCodes.cs, setMemoryByte()] BAD ADDRESS: " + String.Format("{0:x2}", addr));
                    // memory.memCPU[addr] = data;
                }

                memCPU[addr] = data;
            }
        }

        public void WriteCHR(int addr, int data)
        { }

        public byte ReadPRG(int addr)
        {
            // Prevents invalid reads by wrapping around after addition
            addr &= 0xFFFF;

            // Catch out of range addressing <<-- This should not happen because of the line above
            if (addr > 0xFFFF)
            {
                //MessageBox.Show("[OpCodes.cs, getMemoryByte()] BAD ADDRESS: " + String.Format("{0:x2}", addr));
                //cpu.badOpCode = true;
                return 0;
            }

            // Address Masking at 0800-1FFF
            if ((addr < 0x2000))
            {
                return memCPU[addr & 0x07FF];
            }

            // Handle PPU related OpCodes (in Memory class)
            else if ((addr >= 0x2000) && (addr <= 0x2007))
            {
                return getPPUMemoryByte(addr);
            }

            // Address Masking for PPU at 2008-3FFF
            else if ((addr >= 0x2008) && (addr <= 0x3FFF))
            {
                return getPPUMemoryByte(addr & 0x2007);
            }

            #region Read Joydpad
            // New Joystick Code
            else if (addr == 0x4016)
            {
                return input.ReadJoypad();
            }
            #endregion

            // Handles all memory spaces not related to PPU and controllers
            else
            {
                return memCPU[addr];
            }
        }

        public byte ReadCHR(int addr)
        { return 0; }

        // Push a single Byte to the Stack
        public void stackPush(byte byteTemp)
        {
            //memStack[refReg.regSP] = byteTemp;
            memCPU[registers.regSP + 0x100] = byteTemp;

            registers.regSP--;
        }

        // Get a single Byte from the Stack
        public byte stackPop()
        {
            registers.regSP++;                     // Decrement Stack Pointer

            int byteTemp = memCPU[registers.regSP + 0x100];  // Get Byte

            return Convert.ToByte(byteTemp);        // Return Byte
        }

        public byte getPPUMemoryByte(int addr)
        {
            #region Read 0x2000
            // PPU Control Register 1
            if (addr == 0x2000)
            {
                return memCPU[0x2000];
            }
            #endregion

            #region Read 0x2001
            // PPU Control Register 2
            else if (addr == 0x2001)
            {
                return memCPU[0x2001];
            }
            #endregion

            #region Read 0x2002
            // PPU Status Register
            else if (addr == 0x2002)
            {
                // Clear the VBLANK Flag here.  This is documented (p. 5 on 2c02 doc)

                //DEBUGGING
                //if (memCPU[0x2002] != 0)
                //{
                //    temp33 = temp33;
                //}
                if (spriteOverflow)
                {
                    memCPU[0x2002] |= 0x20;
                }
                byte tempByte = memCPU[0x2002];

                //byte tempByte = memCPU[0x2002];
                setVblank(false);
                setSpriteHit(false);

                //// Also resets 1st/2nd write flipflop used by 2005 and 2006
                //memCPU[0x2005] = 0;
                //memCPU[0x2006] = 0;

                //t = 0;
                // Reading 2002 will clear the latch, NOT WRITING 2002
                PPULatchToggle = false;

                return tempByte;
            }
            #endregion

            #region Read 0x2003
            // PPU Status Register
            else if (addr == 0x2003)
            {
                // Program should not need to read this
                return memCPU[0x2003];
            }
            #endregion

            #region Read 0x2004
            // Sprite Memory Data
            else if (addr == 0x2004)
            {
                byte tempByte = memSPRRAM[memCPU[0x2003]];

                // Pg 5 of 117 in "NES Specifications says that 0x2003 is NOT incremented after a read, only writes
                //memCPU[0x2003] += 1;

                return tempByte;
            }
            #endregion

            #region Read 0x2007
            // PPU Memory Data
            else if (addr == 0x2007)// && ((memCPU[0x2002] & 0x80) == 0x80))    // Shouldn't really need to read this byte
            {
                // Temporary variable so that VRAMAddr can be incremented without affecting the current read
                int VRAMAddrTemp = VRAMAddr;

                // Determine Horizontal/Vertical write and increment according to $2000.2
                if ((memCPU[0x2000] & 0x04) == 0x00)
                {
                    VRAMAddr++;
                }
                else if ((memCPU[0x2000] & 0x04) == 0x04)
                {
                    VRAMAddr += 32;
                }

                // ----- Address Mirrors -----
                // If data is requested from 0000-2FFF
                if (VRAMAddrTemp <= 0x2FFF)
                {
                    byte temp = byteLastRead;
                    //byteLastRead = memPPU[VRAMAddrTemp];

                    // ------------------------------------NEED TO FIX THIS FOR MASKING-----------------------------------------------
                    #region ***** (NAME TABLES) Mirrors  *****
                    byteLastRead = memPPU[AddressMask(VRAMAddrTemp)];
                    #endregion

                    return temp;
                }



                // If data is requested from 3000-3EFF
                else if (VRAMAddrTemp <= 0x3EFF)
                {
                    byte temp = byteLastRead;
                    byteLastRead = memPPU[VRAMAddrTemp - 0x1000];
                    return temp;
                }

                // Address Masking for 3F00-3FFF - Palette Mirrors (Masking)
                else if ((VRAMAddrTemp >= 0x3F00) && (VRAMAddrTemp <= 0x3FFF))
                {
                    // "Mirroring" (Address Masking)
                    if (VRAMAddrTemp > 0x3F1F)
                    {
                        //byteLastRead = memPPU[VRAMAddrTemp & 0x3F1F];
                        return memPPU[VRAMAddrTemp & 0x3F1F];
                    }

                    // -- Transparency address masking --
                    // If palette memory is accessed, it is masked by 0x3F1F and placed into
                    // the appropriate sprite or bg palette within 3F00-3F1F.  Also, Transparency
                    // Bytes (like 3F10) are copied down (or up) to their respective BG/Sprite byte
                    // So, a write to 3F10 would also write to 3F00 and a write to 3F04 would also
                    // write to 3F14...and so on.

                    //byteLastRead = memPPU[VRAMAddrTemp];
                    return memPPU[VRAMAddrTemp];

                    //if (VRAMAddr < 0x3F10 && (VRAMAddr % 4) == 0)
                    //{
                    //    return memPPU[VRAMAddr + 0x10];  // Transparency Byte
                    //    //return memPPU[VRAMAddr];  // Transparency Byte
                    //}
                    //else if (VRAMAddr >= 0x3F10 && (VRAMAddr % 4) == 0)
                    //{
                    //    return memPPU[VRAMAddr - 0x10];  // Transparency Byte
                    //    //return memPPU[VRAMAddr];  // Transparency Byte
                    //}
                }

                // Address Masking for 4000-FFFF
                else if (VRAMAddrTemp >= 0x4000 && VRAMAddrTemp <= 0xFFFF)
                {
                    return memPPU[VRAMAddrTemp & 0x3FFF];
                }

                // Return data from the PPU Memory located at address VRAMAddr
                return memPPU[VRAMAddrTemp];
            }
            #endregion

            else
            {
                //MessageBox.Show("[MEMORY.cs, getPPUMemoryByte()] PPU Address read to address not accounted for: " + String.Format("{0:x4}", addr));
                return 0x00;
            }
        }

        public void setPPUMemoryByte(int addr, byte data)
        {
            #region Write 0x2000
            // PPU Control Register 1
            if (addr == 0x2000)
            {
                // Copy to intended memory address
                memCPU[addr] = data;

                t &= 0x73FF;
                t |= ((int)(data & 0x03) << 10);
            }
            #endregion

            #region Write 0x2001
            // PPU Control Register 2
            else if (addr == 0x2001)
            {
                memCPU[addr] = data;
            }
            #endregion

            #region Write 0x2003
            // Sprite Memory Address
            else if (addr == 0x2003 /*&& !tn.rendering*/)
            {
                // Write the address to 0x2003 so 0x2004 can access it
                memCPU[addr] = data;
            }
            #endregion

            #region Write 0x2004
            // Sprite Memory Data
            else if (addr == 0x2004 /*&& !tn.rendering*/)
            {
                // Write the byte to 0x2004 first.  Don't know if this is necessary. May just be able to bypass this step and write directly to memSPRRAM
                memSPRRAM[memCPU[0x2003]] = data;

                // Increment Memory Address in 0x2003.  Mandatory incement after each WRITE of 0x2004
                memCPU[0x2003] += 1;
            }
            #endregion

            #region Write 0x2005
            // Screen Scroll Offset
            else if (addr == 0x2005)// && ((memCPU[0x2002] & 0x80) != 0x80))
            {
                if (!PPULatchToggle)
                {
                    // Copy byte into temporary location
                    //VRAMAddrFirstWrite = data;

                    // Offset X
                    t &= 0x7FE0;
                    t |= ((data & 0xF8) >> 3);

                    // fineX is ONLY updated on the first write to 0x2005
                    fineX = (byte)(data & 0x07);
                }
                else
                {
                    // Add High Byte to the temporary VRAMAddr variable so 0x2007 has a full 2-byte address to read
                    //VRAMAddr = data + (VRAMAddrFirstWrite & 0xFF) * 16 * 16;

                    // Offset Y
                    t &= 0x0C1F;
                    t |= ((data & 0xF8) << 2);
                    t |= ((data & 0x07) << 12); // or with 0x03 instead of 0x07 because high two bits are ignored
                    //t &= 0x0FFF;
                    //t |= ((data & 0x07) << 12);
                }

                PPULatchToggle = !PPULatchToggle;
            }
            #endregion

            #region Write 0x2006
            // PPU Memory Address
            else if (addr == 0x2006)   // Written to TWICE to read/write to/from PPU Memory
            {
                if (!PPULatchToggle)
                {
                    //// Increment 0x2006 Counter To Test for High Byte on Second Pass
                    //PPULatchToggle = !PPULatchToggle;

                    // Copy byte into temporary location
                    // VRAMAddr = 0;
                    //////////VRAMAddrFirstWrite = data;

                    //if (((memCPU[0x2001] & 0x10) == 0x10) || ((memCPU[0x2001] & 0x08) == 0x08))
                    //{
                    #region Scrolling Data
                    // Get first 2006 write bits
                    t &= 0x00FF;
                    t |= ((data & 0x3F) << 8);

                    // Clear top two bits
                    //t &= 0x3FFF;
                    #endregion
                    //}
                }
                else if (PPULatchToggle)
                {
                    //// Reset Counter
                    //PPULatchToggle = !PPULatchToggle;

                    // Add High Byte to the temporary VRAMAddr variable so 0x2007 has a full 2-byte address to read
                    ///////////VRAMAddr = data + (VRAMAddrFirstWrite & 0xFF) * 16 * 16;

                    //if (((memCPU[0x2001] & 0x10) == 0x10) || ((memCPU[0x2001] & 0x08) == 0x08))
                    //{
                    #region Scrolling Data
                    // Get second 2006 write bits
                    t &= 0x7F00;
                    t |= data;

                    VRAMAddr = t;
                    #endregion
                    //}
                }

                PPULatchToggle = !PPULatchToggle;
            }
            #endregion

            #region Write 0x2007
            // PPU Memory Data
            else if (addr == 0x2007)   // Uses Memory Address in 2006 to read/write
            {

                // Write to nametable
                if (VRAMAddr < 0x2000)
                {
                    memPPU[VRAMAddr] = data;
                }

                // Address Masking for 2000-2FFF
                if ((VRAMAddr >= 0x2000) && (VRAMAddr < 0x3000))
                {
                    // ------------------------------------NEED TO FIX THIS FOR MASKING-----------------------------------------------
                    #region ***** (NAME TABLES) Mirrors  *****
                    memPPU[AddressMask(VRAMAddr)] = data;
                    #endregion
                }

                // Address Masking for 3000-3EFF
                else if ((VRAMAddr >= 0x3000) && (VRAMAddr <= 0x3EFF))
                {
                    memPPU[VRAMAddr - 0x1000] = data;
                }

                // Address Masking for 3F00-3FFF - Palette Mirrors (Masking)
                else if ((VRAMAddr >= 0x3F00) && (VRAMAddr <= 0x3FFF))
                {
                    // "Mirroring" (Address Masking)
                    if (VRAMAddr > 0x3F1F)
                    {
                        memPPU[VRAMAddr & 0x3F1F] = data;
                    }

                    // -- Transparency address masking --
                    // If palette memory is accessed, it is masked by 0x3F1F and placed into
                    // the appropriate sprite or bg palette within 3F00-3F1F.  Also, Transparency
                    // Bytes (like 3F10) are copied down (or up) to their respective BG/Sprite byte
                    // So, a write to 3F10 would also write to 3F00 and a write to3F04 would also
                    // write to 3F14...and so on.
                    if (VRAMAddr < 0x3F10 && (VRAMAddr % 4) == 0)
                    {
                        memPPU[VRAMAddr + 0x10] = data;  // Transparency Byte
                    }
                    else if (VRAMAddr >= 0x3F10 && (VRAMAddr % 4) == 0)
                    {
                        memPPU[VRAMAddr - 0x10] = data;  // Transparency Byte
                    }
                    memPPU[VRAMAddr] = data;
                }

                // Address Masking for 4000-FFFF
                else if (VRAMAddr >= 0x4000 && VRAMAddr <= 0xFFFF)
                {
                    memPPU[VRAMAddr &= 0x3FFF] = data;
                }
                else
                {
                    // Get address location from the temporary int that holds the data from two previous 0x2006 writes
                    memPPU[VRAMAddr] = data;
                    //MessageBox.Show("[Memory.cs: SetMemoryByte] Write to 2007");
                }

                // Determine Horizontal/Vertical write and increment according to $2000.2
                if ((memCPU[0x2000] & 0x04) == 0x00)
                {
                    VRAMAddr++;
                }
                else if ((memCPU[0x2000] & 0x04) == 0x04)
                {
                    VRAMAddr += 32;
                }
            }
            #endregion

            // This should never happen    
            else
            {
                //MessageBox.Show("[PPU.cs, setPPUMemoryByte()] PPU Address read to address not accounted for: " + String.Format("{0:x4}", addr));
            }
        }

        public int AddressMask(int VRAMAddr)
        {
            // Horizontal Name Table Mirroring
            //if (byteMirror == 0x00 && (VRAMAddr >= 0x2000) && (VRAMAddr < 0x2400))
            //{
            //    return VRAMAddrTemp;
            //}
            if (byteMirror == 0x00 && (VRAMAddr >= 0x2400) && (VRAMAddr < 0x2800))
            {
                return VRAMAddr & 0x23FF;
            }
            //else if (byteMirror == 0x00 && (VRAMAddr >= 0x2800) && (VRAMAddr < 0x2C00))
            //{
            //    return VRAMAddrTemp;
            //}
            else if (byteMirror == 0x00 && (VRAMAddr >= 0x2C00) && (VRAMAddr < 0x3000))
            {
                return VRAMAddr & 0x2BFF;
            }

            // Vertical Name Table Mirroring
            //else if (byteMirror == 0x01 && (VRAMAddr >= 0x2000) && (VRAMAddr < 0x2400))
            //{
            //    return VRAMAddrTemp;
            //}
            //else if (byteMirror == 0x01 && (VRAMAddr >= 0x2400) && (VRAMAddr < 0x2800))
            //{
            //    return VRAMAddrTemp;
            //}
            else if (byteMirror == 0x01 && (VRAMAddr >= 0x2800) && (VRAMAddr < 0x2C00))
            {
                return VRAMAddr - 0x0800;
            }
            else if (byteMirror == 0x01 && (VRAMAddr >= 0x2C00) && (VRAMAddr < 0x3000))
            {
                return VRAMAddr - 0x0800;
            }

            return VRAMAddr;
        }

        private void copyPPUMirrors(int addr, byte data)
        {
            // Copy to mirrored address by adding offset
            // This will copy an individual byte to every eighth location in the PPU Register Mirror
            //for (int i = 0; i < (0x3FFF - 0x2008 + 1); i += 8)
            //{
            //    memCPU[addr + 8 + i] = data;
            //}
        }

        // VBlank Set/Clear
        public void setVblank(bool bolSetClear)
        {
            bolVBlank = bolSetClear;

            if (bolSetClear)
            {
                memCPU[0x2002] |= 0x80;
            }
            else
            {
                memCPU[0x2002] &= 0x7F;
            }
        }

        // Sprite Hit Set/Clear
        public void setSpriteHit(bool bolSetClear)
        {
            if (bolSetClear)
            {
                memCPU[0x2002] |= 0x40;
            }
            else
            {
                memCPU[0x2002] &= 0xBF;
            }
        }

        // Memory Map Constructor
        public MemoryMap(Registers refReg, Input refInput, NES refTn, Mappers refMappers)
        {
            registers = refReg;
            input = refInput;
            tn = refTn;
            mappers = refMappers;

            //memCPU[0x2000] = 0x00;
            //memCPU[0x2002] = 0x10;

            //memCPU[0x07DC] = 0xFF;
        }
    }
}