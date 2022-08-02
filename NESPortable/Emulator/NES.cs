using System;
using System.Collections.Generic;

namespace NES
{
    public partial class NES
    {
        Registers registers;
        MemoryMap memory;
        Mappers mappers;
        CPU cpu;
        public GameRender gameRender;
        Input input;
        PPU ppu;

        #region Global/Local Variables
        public bool bolRunGame;
        public bool bolStartFrame, bolReset = true;
        int intFPS = 0;
        int intMaxCPUCycles = 29780 * 15;
        public int tsMaster;
        public int tsCpuNTSC;
        public int tsCpuPAL;
        public int tsPpu;
        public int cntScanline = -2;
        public int cntScanlineCycle;
        public int VBlankTime = 20 * 341 * 5;
        public byte MapperNumber;
        public bool rendering = false;
        #endregion

        public void runGame()
        {
            if (bolRunGame && !cpu.badOpCode)
            {
                if (cpu.intTotalCpuCycles < intMaxCPUCycles)
                {
                    cpu.execOpCode();

                    ppu.RunPPU(cpu.intTotalCpuCycles);
                }

                if (ppu.bolReadyToRender)
                {
                    gameRender.WriteBitmap(ppu.byteBGFrame, ppu.BGColor);

                    ppu.bolReadyToRender = false;

                    cpu.intTotalCpuCycles = 0;
                    tsPpu = 0;

                    intFPS++;
                }
            }
        }

        public unsafe void openROM(byte* prom)
        {
            {
                byte* rom = prom;

                byte[] temp = new byte[16];

                fixed (byte* p = temp)
                {
                    for(int i = 0;i< temp.Length; i++) 
                    {
                        p[i] = rom[i];
                    }
                }
                rom += temp.Length;
                memory.byteMirror = (byte)(temp[6] & 0x01);

                mappers.MapperNumber = (byte)((temp[6] >> 0x04) | (temp[7] & 0xF0));
                memory.MapperNumber = mappers.MapperNumber;

                byte byteNumPRGBanks = temp[4];
                byte byteNumCHRBanks = temp[5];

                memory.memPRG = new List<byte[]>(byteNumPRGBanks);
                memory.memPRG.Count = byteNumPRGBanks;
                if (byteNumPRGBanks == 0x01)
                {
                    fixed (byte* p = memory.memCPU)
                    {
                        for(int i = 0;i< 0x4000; i++) 
                        {
                            (p + 0xC000)[i] = rom[i];
                        }
                    }
                    rom += 0x4000;

                    for (int i = 0x0000; i < 0x4000; i++)
                    {
                        memory.memCPU[0x8000 + i] = memory.memCPU[0xC000 + i];
                    }
                }
                else
                {
                    for (int j = 0; j < memory.memPRG.Count; j++)
                    {
                        memory.memPRG[j] = new byte[0x4000];
                    }

                    for (int k = 0; k < memory.memPRG.Count; k++)
                    {
                        fixed (byte* p = memory.memPRG[k])
                        {
                            for(int i = 0;i< 0x4000; i++) 
                            {
                                p[i] = rom[i];
                            }
                        }
                        rom += 0x4000;
                    }

                    for (int l = 0; l < 0x4000; l++)
                    {
                        memory.memCPU[l + 0x8000] = memory.memPRG[0][l];
                        memory.memCPU[l + 0xC000] = memory.memPRG[memory.memPRG.Count - 1][l];
                    }
                }

                if (byteNumCHRBanks != 0)
                {
                    memory.memCHR = new List<byte[]>(byteNumCHRBanks);
                    memory.memCHR.Count = byteNumCHRBanks;


                    for (int x = 0; x < memory.memCHR.Count; x++)
                    {
                        memory.memCHR[x] = new byte[0x2000];
                    }

                    for (int y = 0; y < memory.memCHR.Count; y++)
                    {
                        fixed (byte* p = memory.memCHR[y])
                        {
                            for(int i = 0;i< 0x2000; i++)
                            {
                                p[i] = rom[i];
                            }
                        }
                        rom += 0x2000;
                    }

                    for (int z = 0; z < 0x2000; z++)
                    {
                        memory.memPPU[z] = memory.memCHR[0][z];
                    }
                }

                registers.regPC = memory.memCPU[0xFFFC] + memory.memCPU[0xFFFD] * 16 * 16;


                memory.memCPU[0x4016] = 0x40;
                memory.memCPU[0x4017] = 0x40;

                bolRunGame = true;
            }
        }

        public void resetGame()
        {
            registers = new Registers();
            input = new Input();
            mappers = new Mappers();
            memory = new MemoryMap(registers, input, this, mappers);
            ppu = new PPU(memory, this);
            cpu = new CPU(memory, input, ppu, registers);

            gameRender = new GameRender(this);

            bolStartFrame = true;
            bolReset = true;

            var handler = (EventHandler<ConsoleKeyInfo>)PS2Keyboard_OnKeyChangedHandler;

            Program.BindOnKeyChangedHandler(handler);
        }

        public void PS2Keyboard_OnKeyChangedHandler(object sender,ConsoleKeyInfo key)
        {
            ConsoleKey c = key.Key;
            if (key.KeyState == ConsoleKeyState.Pressed)
            {
                if (c == ConsoleKey.Q)        // A
                {
                    input.joypadOne |= 0x01;
                }
                else if (c == ConsoleKey.E)   // B
                {
                    input.joypadOne |= 0x02;
                }
                else if (c == ConsoleKey.Z)   // Select
                {
                    input.joypadOne |= 0x04;
                }
                else if (c == ConsoleKey.C)   // Start
                {
                    input.joypadOne |= 0x08;
                }
                else if (c == ConsoleKey.W)   // Up
                {
                    input.joypadOne |= 0x10;
                }
                else if (c == ConsoleKey.S)   // Down
                {
                    input.joypadOne |= 0x20;
                }
                else if (c == ConsoleKey.A)   // Left
                {
                    input.joypadOne |= 0x40;
                }
                else if (c == ConsoleKey.D)   // Right
                {
                    input.joypadOne |= 0x80;
                }
            }else
            {
                if (c == ConsoleKey.Q)        // A
                {
                    input.joypadOne &= 0xFE;
                }
                else if (c == ConsoleKey.E)   // B
                {
                    input.joypadOne &= 0xFD;
                }
                else if (c == ConsoleKey.Z)   // Select
                {
                    input.joypadOne &= 0xFB;
                }
                else if (c == ConsoleKey.C)   // Start
                {
                    input.joypadOne &= 0xF7;
                }
                else if (c == ConsoleKey.W)   // Up
                {
                    input.joypadOne &= 0xEF;
                }
                else if (c == ConsoleKey.S)   // Down
                {
                    input.joypadOne &= 0xDF;
                }
                else if (c == ConsoleKey.A)   // Left
                {
                    input.joypadOne &= 0xBF;
                }
                else if (c == ConsoleKey.D)   // Right
                {
                    input.joypadOne &= 0x7F;
                }
            }
        }

        public void NMIHandler()
        {
            cpu.NMIHandler(memory);
        }

        public void AddCPUCycles(int cycles)
        {
            cpu.intTotalCpuCycles += cycles * 15;
        }

        public NES()
        {
            resetGame();
        }
    }
}