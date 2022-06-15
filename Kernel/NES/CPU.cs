using System;

namespace NES
{
    public class CPU
    {
        MemoryMap memory;
        Registers registers;
        Input input;
        PPU ppu;

        #region Local Variables
        public int intTotalCpuCycles = 0;
        public int intOpCodeCpuCycles = 0;
        public int intCpuCycles = 0;        // Cpu cycles to execute
        public int intCpuCyclesOffset = 0;  // Cpu cycles to add when accessing different pages in memory
        public int intLastAddress = 0;
        public bool bolPageChanged = false;
        public bool badOpCode = false;

        // Joypad Variables
        byte joypad;
        int j = 0;
        #endregion

        #region /* ----- OpCodes ----- */
        public int execOpCode()
        {
            // N Z C I D V (sigN Zero Carry InterDis Decim oVerflow)

            int tempPC = registers.regPC;   // Save cpu cycles by not accessing 'registers' three times

            byte byteOpCode = memory.memCPU[tempPC];
            byte byteOne = memory.memCPU[(tempPC + 1) & 0xFFFF];
            byte byteTwo = memory.memCPU[(tempPC + 2) & 0xFFFF];

            // Debug Testing
            if (tempPC == 0xC5FB)
            {
                tempPC = tempPC;
            }

            switch (byteOpCode)
            {
                #region OpCode ADC
                // ADC - Add memory to accumulator with Carry
                //  N Z C I D V
                //  / / / _ _ /
                case 0x69:  // ADC - Immediate
                    {
                        uint intRegA;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // Add Accumulator + Byte Given + Carry
                        intRegA = (uint)(registers.regA + byteOne + Convert.ToUInt16(registers.statusCarry));

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero((int)(intRegA & 0xFF));
                        registers.statusCarry = checkStatusCarry((int)intRegA);
                        registers.statusOverflow = checkStatusOverflowADC(registers.regA, intRegA, byteOne);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        //if (registers.statusOverflow)
                        //{
                        //    registers.statusCarry = true;
                        //}

                        //registers.statusCarry = (intRegA > 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 2;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0x65:  // ADC - Absolute Zero-Page
                    {
                        uint intRegA;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // Add Accumulator + Byte Given + Carry
                        intRegA = (uint)(registers.regA + addrAbsZP(byteOne) + Convert.ToUInt16(registers.statusCarry));

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero((int)(intRegA & 0xFF));
                        registers.statusCarry = checkStatusCarry((int)intRegA);
                        registers.statusOverflow = checkStatusOverflowADC(registers.regA, intRegA, addrAbsZP(byteOne));

                        //if (registers.statusOverflow)
                        //{
                        //    registers.statusCarry = true;
                        //}

                        //registers.statusCarry = (intRegA > 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 3;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0x75:  // ADC - Indexed Zero-Page (X)
                    {
                        uint intRegA;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // Add Accumulator + Byte Given + Carry
                        intRegA = (uint)(registers.regA + addrIdxZP(byteOne, registers.regX) + Convert.ToUInt16(registers.statusCarry));

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero((int)(intRegA & 0xFF));
                        registers.statusCarry = checkStatusCarry((int)intRegA);
                        registers.statusOverflow = checkStatusOverflowADC(registers.regA, intRegA, addrIdxZP(byteOne, registers.regX));

                        //if (registers.statusOverflow)
                        //{
                        //    registers.statusCarry = true;
                        //}

                        //registers.statusCarry = (intRegA > 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 4;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0x6D:  // ADC - Absolute
                    {
                        uint intRegA;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // Add Accumulator + Byte Given + Carry
                        intRegA = (uint)(registers.regA + addrAbs(byteOne, byteTwo) + Convert.ToUInt16(registers.statusCarry));

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero((int)(intRegA & 0xFF));
                        registers.statusCarry = checkStatusCarry((int)intRegA);
                        registers.statusOverflow = checkStatusOverflowADC(registers.regA, intRegA, addrAbs(byteOne, byteTwo));

                        //if (registers.statusOverflow)
                        //{
                        //    registers.statusCarry = true;
                        //}

                        //registers.statusCarry = (intRegA > 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 4;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;

                case 0x7D:  // ADC - Absolute, Indexed (X)
                    {
                        uint intRegA;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // Add Accumulator + Byte Given + Carry
                        intRegA = (uint)(registers.regA + addrIdx(byteOne, byteTwo, registers.regX) + Convert.ToUInt16(registers.statusCarry));

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero((int)(intRegA & 0xFF));
                        registers.statusCarry = checkStatusCarry((int)intRegA);
                        registers.statusOverflow = checkStatusOverflowADC(registers.regA, intRegA, addrIdx(byteOne, byteTwo, registers.regX));

                        //if (registers.statusOverflow)
                        //{
                        //    registers.statusCarry = true;
                        //}

                        //registers.statusCarry = (intRegA > 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        pageChangeCheck();
                        intCpuCycles = 4 + intCpuCyclesOffset;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;

                case 0x79:  // ADC - Absolute, Indexed (Y)
                    {
                        uint intRegA;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // Add Accumulator + Byte Given + Carry
                        intRegA = (uint)(registers.regA + addrIdx(byteOne, byteTwo, registers.regY) + Convert.ToUInt16(registers.statusCarry));

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero((int)(intRegA & 0xFF));
                        registers.statusCarry = checkStatusCarry((int)intRegA);
                        registers.statusOverflow = checkStatusOverflowADC(registers.regA, intRegA, addrIdx(byteOne, byteTwo, registers.regY));

                        //if (registers.statusOverflow)
                        //{
                        //    registers.statusCarry = true;
                        //}

                        //registers.statusCarry = (intRegA > 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        pageChangeCheck();
                        intCpuCycles = 4 + intCpuCyclesOffset;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;

                case 0x61:  // ADC - Indirect (X)
                    {
                        uint intRegA;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // Add Accumulator + Byte Given + Carry
                        intRegA = (uint)(registers.regA + addrIndX(byteOne, registers.regX) + Convert.ToUInt16(registers.statusCarry));

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero((int)(intRegA & 0xFF));
                        registers.statusCarry = checkStatusCarry((int)intRegA);
                        registers.statusOverflow = checkStatusOverflowADC(registers.regA, intRegA, addrIndX(byteOne, registers.regX));

                        //if (registers.statusOverflow)
                        //{
                        //    registers.statusCarry = true;
                        //}

                        //registers.statusCarry = (intRegA > 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 6;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0x71:  // ADC - Indirect (Y)
                    {
                        uint intRegA;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // Add Accumulator + Byte Given + Carry
                        intRegA = (uint)(registers.regA + addrIndY(byteOne, registers.regY) + Convert.ToUInt16(registers.statusCarry));

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero((int)(intRegA & 0xFF));
                        registers.statusCarry = checkStatusCarry((int)intRegA);
                        registers.statusOverflow = checkStatusOverflowADC(registers.regA, intRegA, addrIndY(byteOne, registers.regY));

                        //if (registers.statusOverflow)
                        //{
                        //    registers.statusCarry = true;
                        //}

                        //registers.statusCarry = (intRegA > 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        pageChangeCheck();
                        intCpuCycles = 5 + intCpuCyclesOffset;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;
                #endregion  

                #region OpCode AND
                // AND - AND Memory with Accumulator
                //  N Z C I D V
                //  / / _ _ _ _
                case 0x29:  // AND - Immediate
                    {
                        int intRegA;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // AND Accumulator + Byte Given
                        intRegA = registers.regA & byteOne;

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 2;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0x25:  // AND - Absolute Zero-Page
                    {
                        int intRegA;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // AND Accumulator + Byte Given
                        intRegA = registers.regA & addrAbsZP(byteOne);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 3;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0x35:  // AND - Indexed Zero-Page (X)
                    {
                        int intRegA;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // AND Accumulator + Byte Given
                        intRegA = registers.regA & addrIdxZP(byteOne, registers.regX);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 4;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0x2D:  // AND - Absolute
                    {
                        int intRegA;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // AND Accumulator + Byte Given
                        intRegA = registers.regA & addrAbs(byteOne, byteTwo);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 4;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;

                case 0x3D:  // AND - Absolute, Indexed (X)
                    {
                        int intRegA;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // AND Accumulator + Byte Given
                        intRegA = registers.regA & addrIdx(byteOne, byteTwo, registers.regX);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        pageChangeCheck();
                        intCpuCycles = 4 + intCpuCyclesOffset;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;

                case 0x39:  // AND - Absolute, Indexed (Y)
                    {
                        int intRegA;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // AND Accumulator + Byte Given
                        intRegA = registers.regA & addrIdx(byteOne, byteTwo, registers.regY);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        pageChangeCheck();
                        intCpuCycles = 4 + intCpuCyclesOffset;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;

                case 0x21:  // AND - Indirect (X)
                    {
                        int intRegA;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // AND Accumulator + Byte Given
                        intRegA = registers.regA & addrIndX(byteOne, registers.regX);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 6;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0x31:  // AND - Indirect (Y)
                    {
                        int intRegA;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // AND Accumulator + Byte Given
                        intRegA = registers.regA & addrIndY(byteOne, registers.regY);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 5;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;
                #endregion

                #region OpCode ASL
                // ASL - ASL Shift Left One Bit (Accumulator or Memory)
                //  N Z C I D V
                //  / / / _ _ _
                case 0x0A:  // ASL - Accumulator
                    {
                        int intRegA;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // Shift Memory or Accumulator One Bit to the Left
                        intRegA = registers.regA << 1;

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);
                        registers.statusCarry = checkStatusCarry(intRegA);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 2;

                        // Update Program Counter
                        registers.regPC += 1;
                    }
                    break;

                case 0x06:  // ASL - Absolute Zero-Page
                    {
                        int intMem;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // Shift Memory or Accumulator One Bit to the Left
                        intMem = addrAbsZP(byteOne) << 1;

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intMem & 0xFF);
                        registers.statusZero = checkStatusZero(intMem & 0xFF);
                        registers.statusCarry = checkStatusCarry(intMem);

                        memory.WritePRG(byteOne, (Convert.ToByte(intMem & 0xFF)));

                        // Update CPU Cycles
                        intCpuCycles = 5;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0x16:  // ASL - Indexed Zero-Page (X)
                    {
                        int intMem;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // Shift Memory or Accumulator One Bit to the Left
                        intMem = addrIdxZP(byteOne, registers.regX) * 2;

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intMem & 0xFF);
                        registers.statusZero = checkStatusZero(intMem & 0xFF);
                        registers.statusCarry = checkStatusCarry(intMem);

                        memory.WritePRG((byte)(byteOne + registers.regX), (Convert.ToByte(intMem & 0xFF)));

                        // Update CPU Cycles
                        intCpuCycles = 6;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0x0E:  // ASL - Absolute
                    {
                        int intMem;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // Shift Memory or Accumulator One Bit to the Left
                        intMem = addrAbs(byteOne, byteTwo) * 2;

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intMem & 0xFF);
                        registers.statusZero = checkStatusZero(intMem & 0xFF);
                        registers.statusCarry = checkStatusCarry(intMem);

                        memory.WritePRG((byteOne + byteTwo * 16 * 16), (Convert.ToByte(intMem & 0xFF)));

                        // Update CPU Cycles
                        intCpuCycles = 6;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;

                case 0x1E:  // ASL - Absolute, Indexed (X)
                    {
                        int intMem;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // Shift Memory or Accumulator One Bit to the Left
                        intMem = addrIdx(byteOne, byteTwo, registers.regX) << 1;

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intMem & 0xFF);
                        registers.statusZero = checkStatusZero(intMem & 0xFF);
                        registers.statusCarry = checkStatusCarry(intMem);

                        memory.WritePRG((byteOne + byteTwo * 16 * 16) + registers.regX, (Convert.ToByte(intMem & 0xFF)));

                        // Update CPU Cycles
                        intCpuCycles = 7;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;
                #endregion

                #region OpCode BCC
                // BCC - Branch on Carry Clear
                //  N Z C I D V
                //  _ _ _ _ _ _
                case 0x90:  // BCC - Relative
                    {
                        // Add or Subtract from Program Counter if Carry Flag is CLEAR
                        if (registers.statusCarry == false)
                        {
                            /* Store offset value in pcTemp (byteOne & 0b01111111) 
                               We ignore the sign bit */
                            //int pcTemp = byteOne & 0x7F;

                            //if ((byteOne & 0x80) == 0x80)  // Check sign bit in offset
                            //{
                            //    pcTemp = -pcTemp;       // Make value negative if sign bit is 1
                            //}

                            //registers.regPC += pcTemp;   // Add pcTemp value to current regPC (program counter)
                            registers.regPC += 2;
                            registers.regPC += (sbyte)byteOne;

                            pageChangeCheck(registers.regPC);

                            intCpuCyclesOffset = 1 + 1 * Convert.ToInt16(bolPageChanged);
                        }
                        else // If not clear, just update program counter
                        {
                            // Update Program Counter
                            registers.regPC += 2;
                        }

                        // Update CPU Cycles
                        intCpuCycles = 2 + intCpuCyclesOffset;
                    }
                    break;
                #endregion

                #region OpCode BCS
                // BCS - Branch on Carry Set
                //  N Z C I D V
                //  _ _ _ _ _ _
                case 0xB0:  // BCS - Relative
                    {
                        // Add or Subtract from Program Counter if Carry Flag is SET
                        if (registers.statusCarry == true)
                        {
                            /* Store offset value in pcTemp (byteOne & 0b01111111) 
                               We ignore the sign bit */
                            //int pcTemp = byteOne & 0x7F;    // Store value in pcTemp

                            //if ((byteOne & 0x80) == 0x80)  // Check sign bit in offset
                            //{
                            //    pcTemp = -pcTemp;       // Make value negative if sign bit is 1
                            //}

                            //registers.regPC += pcTemp;   // Add pcTemp value to current regPC (program counter)
                            registers.regPC += 2;
                            registers.regPC += (sbyte)byteOne;

                            pageChangeCheck(registers.regPC);

                            intCpuCyclesOffset = 1 + 1 * Convert.ToInt16(bolPageChanged);
                        }
                        else // If not clear, just update program counter
                        {
                            // Update Program Counter
                            registers.regPC += 2;
                        }

                        // Update CPU Cycles
                        intCpuCycles = 2 + intCpuCyclesOffset;
                    }
                    break;
                #endregion

                #region OpCode BEQ
                // BEQ - Branch on Zero
                //  N Z C I D V
                //  _ _ _ _ _ _
                case 0xF0:  // BCS - Relative
                    {
                        // Add or Subtract from Program Counter if Zero Flag is SET
                        if (registers.statusZero == true)
                        {
                            /* Store offset value in pcTemp (byteOne & 0b01111111) 
                               We ignore the sign bit */
                            //int pcTemp = byteOne & 0x7F;    // Store value in pcTemp

                            //if ((byteOne & 0x80) == 0x80)
                            //{
                            //    pcTemp = -pcTemp;       // Make value negative if sign bit is 1
                            //}

                            //registers.regPC += pcTemp;   // Add pcTemp value to current regPC (program counter)
                            registers.regPC += 2;
                            registers.regPC += (sbyte)byteOne;

                            pageChangeCheck(registers.regPC);

                            intCpuCyclesOffset = 1 + 1 * Convert.ToInt16(bolPageChanged);
                        }
                        else // If not clear, just update program counter
                        {
                            // Update Program Counter
                            registers.regPC += 2;
                        }

                        // Update CPU Cycles
                        intCpuCycles = 2 + intCpuCyclesOffset;
                    }
                    break;
                #endregion

                #region OpCode BIT
                // BIT - BIT Test Bits in Memory with Accumulator
                //  N Z C I D V
                //  M7/ _ _ _ M6
                case 0x24:  // BIT - Absolute Zero-Page
                    {
                        /* First AND Accu and Memory, then check to see if bits 6 and 7 equal 1
                           NOTE: When doing bitwise operations like the one below:
                           'registers.regA & addrAbsZP(byteOne)', you cannot directly place the
                           result into a BYTE in case of overflow. The compiler will not let you.
                           You must place it into a larger TYPE (int in this case) first and then
                           you can convert to a BYTE afterward*/
                        byte intTemp = /* registers.regA & */ addrAbsZP(byteOne);

                        // Set Zero Flag based on whether or not Bit 6 AND Bit 7 Equal 1
                        if ((registers.regA & intTemp) == 0)//(byteTemp & 0x80) == 0x80 && (byteTemp & 0x40) == 0x40)
                        {
                            registers.statusZero = true;
                        }
                        else { registers.statusZero = false; }

                        // Place Bit 7 value in Negative(sign) status bit
                        if ((intTemp & 0x80) == 0x80)
                        {
                            registers.statusNegative = true;
                        }
                        else { registers.statusNegative = false; }

                        // Place Bit 6 value in Overflow status bit
                        if ((intTemp & 0x40) == 0x40)
                        {
                            registers.statusOverflow = true;
                        }
                        else { registers.statusOverflow = false; }

                        // Update CPU Cycles
                        intCpuCycles = 3;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0x2C:  // BIT - Absolute
                    {
                        /* First AND Accu and Memory, then check to see if bits 6 and 7 equal 1
                           NOTE: When doing bitwise operations like the one below:
                           'registers.regA & addrAbsZP(byteOne)', you cannot directly place the
                           result into a BYTE in case of overflow. The compiler will not let you.
                           You must place it into a larger TYPE (int in this case) first and then
                           you can convert to a BYTE afterward*/
                        byte intTemp = /*registers.regA & */ addrAbs(byteOne, byteTwo);

                        // Set Zero Flag based on whether or not Bit 6 AND Bit 7 Equal 1
                        if ((registers.regA & intTemp) == 0)//(byteTemp & 0x80) == 0x80 && (byteTemp & 0x40) == 0x40)
                        {
                            registers.statusZero = true;
                        }
                        else { registers.statusZero = false; }

                        // Place Bit 7 value in Negative(sign) status bit
                        if ((intTemp & 0x80) == 0x80)
                        {
                            registers.statusNegative = true;
                        }
                        else { registers.statusNegative = false; }

                        // Place Bit 6 value in Overflow status bit
                        if ((intTemp & 0x40) == 0x40)
                        {
                            registers.statusOverflow = true;
                        }
                        else { registers.statusOverflow = false; }

                        // Update CPU Cycles
                        pageChangeCheck();
                        intCpuCycles = 4 + intCpuCyclesOffset;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;
                #endregion

                #region OpCode BMI
                // BMI - Branch on Minus (Negative)
                //  N Z C I D V
                //  _ _ _ _ _ _
                case 0x30:  // BCS - Relative
                    {
                        // Add or Subtract from Program Counter if Sign Flag is SET (Negative)
                        if (registers.statusNegative == true)
                        {
                            /* Store offset value in pcTemp (byteOne & 0b01111111) 
                               We ignore the sign bit */
                            //int pcTemp = byteOne & 0x7F;    // Store value in pcTemp

                            //if ((byteOne & 0x80) == 0x80)
                            //{
                            //    pcTemp = -pcTemp;       // Make value negative if sign bit is 1
                            //}

                            //registers.regPC += pcTemp;   // Add pcTemp value to current regPC (program counter)
                            registers.regPC += 2;
                            registers.regPC += (sbyte)byteOne;

                            pageChangeCheck(registers.regPC);

                            intCpuCyclesOffset = 1 + 1 * Convert.ToInt16(bolPageChanged);
                        }
                        else // If not clear, just update program counter
                        {
                            // Update Program Counter
                            registers.regPC += 2;
                        }

                        // Update CPU Cycles
                        intCpuCycles = 2 + intCpuCyclesOffset;
                    }
                    break;
                #endregion

                #region OpCode BNE
                // BNE - Branch on Result NOT Zero
                //  N Z C I D V
                //  _ _ _ _ _ _
                case 0xD0:  // BNE - Relative
                    {
                        // Add or Subtract from Program Counter if Zero Flag is NOT SET
                        if (registers.statusZero == false)
                        {
                            /* Store offset value in pcTemp (byteOne & 0b01111111) 
                               We ignore the sign bit */
                            //int pcTemp = byteOne & 0x7F;    // Store value in pcTemp

                            //if ((byteOne & 0x80) == 0x80)
                            //{
                            //    pcTemp = -pcTemp;       // Make value negative if sign bit is 1
                            //}

                            //registers.regPC += pcTemp;   // Add pcTemp value to current regPC (program counter)
                            registers.regPC += 2;
                            registers.regPC += (sbyte)byteOne;

                            pageChangeCheck(registers.regPC);

                            intCpuCyclesOffset = 1 + 1 * Convert.ToInt16(bolPageChanged);
                        }
                        else // If not clear, just update program counter
                        {
                            // Update Program Counter
                            registers.regPC += 2;
                        }

                        // Update CPU Cycles
                        intCpuCycles = 2 + intCpuCyclesOffset;
                    }
                    break;
                #endregion

                #region OpCode BPL
                // BPL - Branch on Plus (Not Negative)
                //  N Z C I D V
                //  _ _ _ _ _ _
                case 0x10:  // BPL - Relative
                    {
                        // Add or Subtract from Program Counter if Sign Flag is NOT SET (Positive)
                        if (registers.statusNegative == false)
                        {
                            /* Store offset value in pcTemp (byteOne & 0b01111111) 
                               We ignore the sign bit FOR NOW */
                            //int pcTemp = byteOne & 0x7F;    // Store value in pcTemp

                            //if ((byteOne & 0x80) == 0x80)
                            //{
                            //    pcTemp = -pcTemp;       // Make value negative if sign bit is 1
                            //}

                            //registers.regPC += pcTemp;   // Add pcTemp value to current regPC (program counter)
                            registers.regPC += 2;
                            registers.regPC += (sbyte)byteOne;

                            pageChangeCheck(registers.regPC);

                            intCpuCyclesOffset = 1 + 1 * Convert.ToInt16(bolPageChanged);
                        }
                        else // If not clear, just update program counter
                        {
                            // Update Program Counter
                            registers.regPC += 2;
                        }

                        // Update CPU Cycles
                        intCpuCycles = 2 + intCpuCyclesOffset;
                    }
                    break;
                #endregion

                #region OpCode BRK
                // BRK - Force Break
                //  N Z C I D V
                //  _ _ _ _ _ _
                case 0x00:  // BRK - Implied
                    {
                        /* Several things happen when there is a Break
                            1. Program Counter increases by 1 to bypass the current instuction on return
                            2. The address in the Program Counter is PUSHed to the Stack
                            3. Break flag is set
                            4. Status Register is PUSHed to the Stack
                            5. Interrupt Disable Flag is SET to 1
                            6. Program Counter is Loaded with the default 'Break Address' 0xFFFE and 0xFFFF in this case
                            7. Interrupt is handled by the code at that address
                            8. Program Counter and Status Register are POPped from the Stack
                            9. Control is returned to our program and it continues where it left off
                         */

                        // Increment Program Counter
                        registers.regPC++;
                        registers.regPC++;
                        //registers.statusBreak = false;
                        // Push memory to stack
                        memory.stackPush(Convert.ToByte(registers.regPC / 16 / 16)); // Push High Byte
                        memory.stackPush(Convert.ToByte(registers.regPC & 0xFF));    // Push Low Byte

                        // Set Break Flag (REMEMBER TO DO THIS **AFTER** pushing SR to the stack so interrupt is cleared upon return
                        // ?????? ?????? ?????? ?????? ????? ????????????????????? Reference (pg 25) says to set it before pushing SR ?????
                        // WIKI says BEFORE, so I put it before http://wiki.nesdev.com/w/index.php/CPU_status_flag_behavior

                        registers.statusBreak = true;
                        registers.statusBitFive = true;
                        registers.setStatusRegister();
                        // Push Status Register to Stack
                        memory.stackPush(registers.regStatus);




                        // Set Interrupt flag (Side effect after pushing according to wiki - http://wiki.nesdev.com/w/index.php/CPU_status_flag_behavior)
                        registers.statusInterrupt = true;

                        // Set Program Counter to 0xFFFE and 0xFFFF to execute appropriate ISR
                        /* This will get the Address of the next instructions to execute from
                           Memory location 0xFFFE and 0xFFFF.  It should automatically be instructed to
                           come back and continue where it left off after executing its ISR routine */
                        registers.regPC = memory.ReadPRG(0xFFFE) | (memory.ReadPRG(0xFFFF) * 16 * 16);

                        // Update CPU Cycles
                        intCpuCycles = 7 + intCpuCyclesOffset;
                    }
                    break;
                #endregion

                #region OpCode BVC
                // BVC - Branch on Overflow Clear
                //  N Z C I D V
                //  _ _ _ _ _ _
                case 0x50:  // BVC - Relative
                    {
                        // Add or Subtract from Program Counter if Overflow is Clear
                        if (registers.statusOverflow == false)
                        {
                            /* Store offset value in pcTemp (byteOne & 0b01111111) 
                               We ignore the sign bit */
                            //int pcTemp = byteOne & 0x7F;    // Store value in pcTemp

                            //if ((byteOne & 0x80) == 0x80)
                            //{
                            //    pcTemp = -pcTemp;       // Make value negative if sign bit is 1
                            //}

                            //registers.regPC += pcTemp;   // Add pcTemp value to current regPC (program counter)
                            registers.regPC += 2;
                            registers.regPC += (sbyte)byteOne;

                            pageChangeCheck(registers.regPC);

                            intCpuCyclesOffset = 1 + 1 * Convert.ToInt16(bolPageChanged);
                        }
                        else // If not clear, just update program counter
                        {
                            // Update Program Counter
                            registers.regPC += 2;
                        }

                        // Update CPU Cycles
                        intCpuCycles = 2 + intCpuCyclesOffset;
                    }
                    break;
                #endregion

                #region OpCode BVS
                // BVS - Branch on Overflow Set
                //  N Z C I D V
                //  _ _ _ _ _ _
                case 0x70:  // BVS - Relative
                    {
                        // Add or Subtract from Program Counter if Overflow is Set
                        if (registers.statusOverflow == true)
                        {
                            /* Store offset value in pcTemp (byteOne & 0b01111111) 
                               We ignore the sign bit */
                            //int pcTemp = byteOne & 0x7F;    // Store value in pcTemp

                            //if ((byteOne & 0x80) == 0x80)
                            //{
                            //    pcTemp = -pcTemp;       // Make value negative if sign bit is 1
                            //}

                            //registers.regPC += pcTemp;   // Add pcTemp value to current regPC (program counter)
                            registers.regPC += 2;
                            registers.regPC += (sbyte)byteOne;

                            pageChangeCheck(registers.regPC);

                            intCpuCyclesOffset = 1 + 1 * Convert.ToInt16(bolPageChanged);
                        }
                        else // If not clear, just update program counter
                        {
                            // Update Program Counter
                            registers.regPC += 2;
                        }

                        // Update CPU Cycles
                        intCpuCycles = 2 + intCpuCyclesOffset;
                    }
                    break;
                #endregion

                #region OpCode CLC
                // CLC - Clear Carry Flag
                //  N Z C I D V
                //  _ _ 0 _ _ _
                case 0x18:  // CLC - Implied
                    {
                        // Clear Carry Flag
                        registers.statusCarry = false;

                        // Update Program Counter
                        registers.regPC += 1;

                        // Update CPU Cycles
                        intCpuCycles = 2;
                    }
                    break;
                #endregion

                #region OpCode CLD
                // CLD - Clear Decimal Flag
                //  N Z C I D V
                //  _ _ _ _ 0 _
                case 0xD8:  // CLD - Implied
                    {
                        // Clear Decimal Flag
                        registers.statusDecimal = false;

                        // Update Program Counter
                        registers.regPC += 1;

                        // Update CPU Cycles
                        intCpuCycles = 2;
                    }
                    break;
                #endregion

                #region OpCode CLI
                // CLI - Clear Interrupt Disable Flag
                //  N Z C I D V
                //  _ _ _ 0 _ _
                case 0x58:  // CLI - Implied
                    {
                        // Clear Interrup Disable Flag
                        registers.statusInterrupt = false;

                        // Update Program Counter
                        registers.regPC += 1;

                        // Update CPU Cycles
                        intCpuCycles = 2;
                    }
                    break;
                #endregion

                #region OpCode CLV
                // CLV - Clear Overflow Flag
                //  N Z C I D V
                //  _ _ _ _ _ 0
                case 0xB8:  // CLV - Implied
                    {
                        // Clear Overflow Flag
                        registers.statusOverflow = false;

                        // Update Program Counter
                        registers.regPC += 1;

                        // Update CPU Cycles
                        intCpuCycles = 2;
                    }
                    break;
                #endregion

                #region OpCode CMP
                // CMP - Compare Memory and Accumulator
                //  N Z C I D V
                //  / / / _ _ _
                case 0xC9:  // CMP - Immediate
                    {
                        int intRegA;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // Compare Memory and Accumulator (Subtract Memory from Accumulator)
                        intRegA = registers.regA - byteOne;

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        setCompareFlags(registers.regA, byteOne);

                        // Update CPU Cycles
                        intCpuCycles = 2;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0xC5:  // CMP - Absolute Zero-Page
                    {
                        int intRegA;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // Compare Memory and Accumulator (Subtract Memory from Accumulator)
                        intRegA = registers.regA - addrAbsZP(byteOne);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        setCompareFlags(registers.regA, addrAbsZP(byteOne));

                        // Update CPU Cycles
                        intCpuCycles = 3;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0xD5:  // CMP - Indexed Zero-Page (X)
                    {
                        int intRegA;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // Compare Memory and Accumulator (Subtract Memory from Accumulator)
                        intRegA = registers.regA - addrIdxZP(byteOne, registers.regX);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        setCompareFlags(registers.regA, addrIdxZP(byteOne, registers.regX));

                        // Update CPU Cycles
                        intCpuCycles = 4;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0xCD:  // CMP - Absolute
                    {
                        int intRegA;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // Compare Memory and Accumulator (Subtract Memory from Accumulator)
                        intRegA = registers.regA - addrAbs(byteOne, byteTwo);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        setCompareFlags(registers.regA, addrAbs(byteOne, byteTwo));

                        // Update CPU Cycles
                        intCpuCycles = 4;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;

                case 0xDD:  // CMP - Absolute, Indexed (X)
                    {
                        int intRegA;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // Compare Memory and Accumulator (Subtract Memory from Accumulator)
                        intRegA = registers.regA - addrIdx(byteOne, byteTwo, registers.regX);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        setCompareFlags(registers.regA, addrIdx(byteOne, byteTwo, registers.regX));

                        // Update CPU Cycles
                        pageChangeCheck();
                        intCpuCycles = 4 + intCpuCyclesOffset;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;

                case 0xD9:  // CMP - Absolute, Indexed (Y)
                    {
                        int intRegA;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // Compare Memory and Accumulator (Subtract Memory from Accumulator)
                        intRegA = registers.regA - addrIdx(byteOne, byteTwo, registers.regY);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        setCompareFlags(registers.regA, addrIdx(byteOne, byteTwo, registers.regY));

                        // Update CPU Cycles
                        pageChangeCheck();
                        intCpuCycles = 4 + intCpuCyclesOffset;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;

                case 0xC1:  // CMP - Indirect (X)
                    {
                        int intRegA;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // Compare Memory and Accumulator (Subtract Memory from Accumulator)
                        intRegA = registers.regA - addrIndX(byteOne, registers.regX);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        setCompareFlags(registers.regA, addrIndX(byteOne, registers.regX));

                        // Update CPU Cycles
                        intCpuCycles = 6;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0xD1:  // CMP - Indirect (Y)
                    {
                        int intRegA;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // Compare Memory and Accumulator (Subtract Memory from Accumulator)
                        intRegA = registers.regA - addrIndY(byteOne, registers.regY);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        setCompareFlags(registers.regA, addrIndY(byteOne, registers.regY));

                        // Update CPU Cycles
                        pageChangeCheck();
                        intCpuCycles = 5 + intCpuCyclesOffset;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;
                #endregion 

                #region OpCode CPX
                // CPX - Compare Memory and Register X
                //  N Z C I D V
                //  / / / _ _ _
                case 0xE0:  // CPX - Immediate
                    {
                        int intRegX;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // Compare Memory and Register X (Subtract Memory from Register X)
                        intRegX = registers.regX - byteOne;

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegX & 0xFF);
                        setCompareFlags(registers.regX, byteOne);

                        // Update CPU Cycles
                        intCpuCycles = 2;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0xE4:  // CPX - Absolute Zero-Page
                    {
                        int intRegX;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // Compare Memory and Register X (Subtract Memory from Register X)
                        intRegX = registers.regX - addrAbsZP(byteOne);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegX & 0xFF);
                        setCompareFlags(registers.regX, addrAbsZP(byteOne));

                        // Update CPU Cycles
                        intCpuCycles = 3;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0xEC:  // CPX - Absolute
                    {
                        int intRegX;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // Compare Memory and Register X (Subtract Memory from Register X)
                        intRegX = registers.regX - addrAbs(byteOne, byteTwo);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegX & 0xFF);
                        setCompareFlags(registers.regX, addrAbs(byteOne, byteTwo));

                        // Update CPU Cycles
                        intCpuCycles = 4;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;
                #endregion 

                #region OpCode CPY
                // CPY - Compare Memory and Register Y
                //  N Z C I D V
                //  / / / _ _ _
                case 0xC0:  // CPY - Immediate
                    {
                        int intRegY;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // Compare Memory and Register X (Subtract Memory from Register Y)
                        intRegY = registers.regY - byteOne;

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegY & 0xFF);
                        setCompareFlags(registers.regY, byteOne);

                        // Update CPU Cycles
                        intCpuCycles = 2;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0xC4:  // CPY - Absolute Zero-Page
                    {
                        int intRegY;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // Compare Memory and Register X (Subtract Memory from Register Y)
                        intRegY = registers.regY - addrAbsZP(byteOne);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegY & 0xFF);
                        setCompareFlags(registers.regY, addrAbsZP(byteOne));

                        // Update CPU Cycles
                        intCpuCycles = 3;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0xCC:  // CPY - Absolute
                    {
                        int intRegY;    // Temporary placeholder for Accum to check for N,Z,C,V before converting back to byte

                        // Compare Memory and Register Y (Subtract Memory from Register Y)
                        intRegY = registers.regY - addrAbs(byteOne, byteTwo);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegY & 0xFF);
                        setCompareFlags(registers.regY, addrAbs(byteOne, byteTwo));

                        // Update CPU Cycles
                        intCpuCycles = 4;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;
                #endregion 

                #region OpCode DEC
                // DEC - Decrement Memory by 1
                //  N Z C I D V
                //  / / _ _ _ _
                case 0xC6:  // DEC - Absolute Zero-Page
                    {
                        byte byteMem;

                        // Decrement 1 from Memory Contents of Memory Location byteOne
                        byteMem = (byte)(addrAbsZP(byteOne) - 1);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(byteMem & 0xFF);
                        registers.statusZero = checkStatusZero(byteMem & 0xFF);

                        // Write decremented value back to memory
                        memory.WritePRG(byteOne, (byte)(byteMem & 0xFF));

                        // Update CPU Cycles
                        intCpuCycles = 5;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0xD6:  // DEC - Indexed Zero-Page (X)
                    {
                        byte byteMem;

                        // Decrement 1 from Memory Contents of Memory Location byteOne
                        byteMem = (byte)(addrIdxZP(byteOne, registers.regX) - 1);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(byteMem & 0xFF);
                        registers.statusZero = checkStatusZero(byteMem & 0xFF);

                        // Write decremented value back to memory
                        memory.WritePRG((byte)(byteOne + registers.regX), (byte)(byteMem & 0xFF));

                        // Update CPU Cycles
                        intCpuCycles = 6;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0xCE:  // DEC - Absolute
                    {
                        byte byteMem;

                        // Decrement 1 from Memory Contents of Memory Location byteOne
                        byteMem = (byte)(addrAbs(byteOne, byteTwo) - 1);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(byteMem & 0xFF);
                        registers.statusZero = checkStatusZero(byteMem & 0xFF);

                        // Write decremented value back to memory
                        memory.WritePRG((byteOne + byteTwo * 16 * 16), (byte)(byteMem & 0xFF));

                        // Update CPU Cycles
                        intCpuCycles = 6;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;

                case 0xDE:  // DEC - Absolute, Indexed (X)
                    {
                        byte byteMem;

                        // Decrement 1 from Memory Contents of Memory Location byteOne
                        byteMem = (byte)(addrIdx(byteOne, byteTwo, registers.regX) - 1);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(byteMem & 0xFF);
                        registers.statusZero = checkStatusZero(byteMem & 0xFF);

                        // Write decremented value back to memory
                        memory.WritePRG((byteOne + byteTwo * 16 * 16) + registers.regX, (byte)(byteMem & 0xFF));

                        // Update CPU Cycles
                        intCpuCycles = 6;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;
                #endregion

                #region OpCode DEX
                // DEX - Decrement X by 1
                //  N Z C I D V
                //  / / _ _ _ _
                case 0xCA:  // DEX - Immediate
                    {
                        //uint intX = (uint)(registers.regX - 1) & 0xFF;

                        // Decrement 1 from X
                        //registers.regX = Convert.ToByte(intX);
                        registers.regX = (byte)(registers.regX - 1);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(registers.regX & 0xFF);
                        registers.statusZero = checkStatusZero(registers.regX & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 2;

                        // Update Program Counter
                        registers.regPC += 1;
                    }
                    break;
                #endregion

                #region OpCode DEY
                // DEY - Decrement Y by 1
                //  N Z C I D V
                //  / / _ _ _ _
                case 0x88:  // DEY - Immediate
                    {
                        //uint intY = (uint)(registers.regY - 1) & 0xFF;

                        // Decrement 1 from Y
                        //registers.regY = Convert.ToByte(intY);
                        registers.regY = (byte)(registers.regY - 1);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(registers.regY & 0xFF);
                        registers.statusZero = checkStatusZero(registers.regY & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 2;

                        // Update Program Counter
                        registers.regPC += 1;
                    }
                    break;
                #endregion

                #region OpCode EOR
                // EOR - XOR memory with accumulator
                //  N Z C I D V
                //  / / / _ _ /
                case 0x49:  // EOR - Immediate
                    {
                        int intRegA;

                        // XOR Memory with Accumulator
                        intRegA = registers.regA ^ byteOne;

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 2;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0x45:  // EOR - Absolute Zero-Page
                    {
                        int intRegA;

                        // XOR Memory with Accumulator
                        intRegA = registers.regA ^ addrAbsZP(byteOne);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 3;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0x55:  // EOR - Indexed Zero-Page (X)
                    {
                        int intRegA;

                        // XOR Memory with Accumulator
                        intRegA = registers.regA ^ addrIdxZP(byteOne, registers.regX);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 4;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0x4D:  // EOR - Absolute
                    {
                        int intRegA;

                        // XOR Memory with Accumulator
                        intRegA = registers.regA ^ addrAbs(byteOne, byteTwo);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 4;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;

                case 0x5D:  // EOR - Absolute, Indexed (X)
                    {
                        int intRegA;

                        // XOR Memory with Accumulator
                        intRegA = registers.regA ^ addrIdx(byteOne, byteTwo, registers.regX);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        pageChangeCheck();
                        intCpuCycles = 4 + intCpuCyclesOffset;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;

                case 0x59:  // EOR - Absolute, Indexed (Y)
                    {
                        int intRegA;

                        // XOR Memory with Accumulator
                        intRegA = registers.regA ^ addrIdx(byteOne, byteTwo, registers.regY);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        pageChangeCheck();
                        intCpuCycles = 4 + intCpuCyclesOffset;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;

                case 0x41:  // EOR - Indirect (X)
                    {
                        int intRegA;

                        // XOR Memory with Accumulator
                        intRegA = registers.regA ^ addrIndX(byteOne, registers.regX);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 6;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0x51:  // EOR - Indirect (Y)
                    {
                        int intRegA;

                        // XOR Memory with Accumulator
                        intRegA = registers.regA ^ addrIndY(byteOne, registers.regY);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        pageChangeCheck();
                        intCpuCycles = 5 + intCpuCyclesOffset;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;
                #endregion  

                #region OpCode INC
                // INC - Increment Memory by 1
                //  N Z C I D V
                //  / / _ _ _ _
                case 0xE6:  // INC - Absolute Zero-Page
                    {
                        byte byteMem;

                        // Increment Memory Contents of Memory Location byteOne by 1
                        byteMem = (byte)(addrAbsZP(byteOne) + 1);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(byteMem & 0xFF);
                        registers.statusZero = checkStatusZero(byteMem & 0xFF);

                        // Write incremented value back to memory
                        memory.WritePRG(byteOne, (byte)(byteMem & 0xFF));

                        // Update CPU Cycles
                        intCpuCycles = 5;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0xF6:  // INC - Indexed Zero-Page (X)
                    {
                        byte byteMem;

                        // Increment Memory Contents of Memory Location byteOne by 1
                        byteMem = (byte)(addrIdxZP(byteOne, registers.regX) + 1);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(byteMem & 0xFF);
                        registers.statusZero = checkStatusZero(byteMem & 0xFF);

                        // Write incremented value back to memory
                        memory.WritePRG((byte)(byteOne + registers.regX), (byte)(byteMem & 0xFF));

                        // Update CPU Cycles
                        intCpuCycles = 6;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0xEE:  // INC - Absolute
                    {
                        byte byteMem;

                        // Increment Memory Contents of Memory Location byteOne by 1
                        byteMem = (byte)(addrAbs(byteOne, byteTwo) + 1);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(byteMem & 0xFF);
                        registers.statusZero = checkStatusZero(byteMem & 0xFF);

                        // Write incremented value back to memory
                        memory.WritePRG((byteOne + byteTwo * 16 * 16), (byte)(byteMem & 0xFF));

                        // Update CPU Cycles
                        intCpuCycles = 3;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;

                case 0xFE:  // INC - Absolute, Indexed (X)
                    {
                        byte byteMem;

                        // Decrement 1 from Memory Contents of Memory Location byteOne
                        byteMem = (byte)(addrIdx(byteOne, byteTwo, registers.regX) + 1);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(byteMem & 0xFF);
                        registers.statusZero = checkStatusZero(byteMem & 0xFF);

                        // Write decremented value back to memory
                        memory.WritePRG((byteOne + byteTwo * 16 * 16) + registers.regX, (byte)(byteMem & 0xFF));

                        // Update CPU Cycles
                        intCpuCycles = 7;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;
                #endregion

                #region OpCode INX
                // INX - Increment X by 1
                //  N Z C I D V
                //  / / _ _ _ _
                case 0xE8:  // INX - Immediate
                    {
                        //uint intX = (uint)(registers.regX + 1) & 0xFF;

                        // Increment X by 1
                        //registers.regX = Convert.ToByte(intX);
                        registers.regX = (byte)(registers.regX + 1);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(registers.regX & 0xFF);
                        registers.statusZero = checkStatusZero(registers.regX & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 2;

                        // Update Program Counter
                        registers.regPC += 1;
                    }
                    break;
                #endregion

                #region OpCode INY
                // INY - Increment Y by 1
                //  N Z C I D V
                //  / / _ _ _ _
                case 0xC8:  // INY - Immediate
                    {
                        //uint intY = (uint)(registers.regY + 1) & 0xFF;

                        // Increment Y by 1
                        //registers.regY = Convert.ToByte(intY);
                        registers.regY = (byte)(registers.regY + 1);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(registers.regY & 0xFF);
                        registers.statusZero = checkStatusZero(registers.regY & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 2;

                        // Update Program Counter
                        registers.regPC += 1;
                    }
                    break;
                #endregion

                #region OpCode JMP
                // JMP - Jump to new location
                //  N Z C I D V
                //  _ _ _ _ _ _
                case 0x4C:  // JMP - Absolute
                    {
                        int intMem = 0x0000;    // Temporary Storage Location for Memory Address to Jump To

                        if (byteOne == 0xa2)
                        {
                            byteOne = byteOne;
                        }

                        // Get Low and High Byte of new memory location
                        intMem += byteOne + byteTwo * 16 * 16;  // Add High Byte to Low Byte

                        registers.regPC = intMem;    // Set Program Counter to new location

                        // Update CPU Cycles
                        intCpuCycles = 3;
                    }
                    break;

                case 0x6C:  // JMP - Indirect
                    {
                        int intMem = 0x0000;    // Temporary Storage Location for Memory Address to Jump To

                        // Get Low and High Byte of new memory location
                        intMem = addrAbs(byteOne, byteTwo);             // Low Byte
                        intMem += addrAbs(++byteOne, byteTwo) * 16 * 16;  // Add High Byte to Low Byte

                        registers.regPC = intMem;    // Set Program Counter to new location

                        // Update CPU Cycles
                        intCpuCycles = 3;
                    }
                    break;
                #endregion

                #region OpCode JSR
                // JSR - Jump to new location and Save Current Address
                //  N Z C I D V
                //  _ _ _ _ _ _
                case 0x20:  // JSR - Absolute
                    {
                        int intMem = 0x0000;    // Temporary Storage Location for Memory Address to Jump To

                        // Decrement Program Counter
                        registers.regPC += 2;
                        // Says pc-- in ref material because the program counter automatically updates when opcodes are executed.
                        // This means that instead of pointing to the next instruction PC+3, we tell it to decrement(pc--) or in our case only add 2.
                        // We do this because when the RTS instruction is executed, it automatically increments the PC value that is
                        // stored on the stack.  So when we return, the PC is now at PC+3 (or the next instruction).

                        // Push memory to stack
                        memory.stackPush(Convert.ToByte(registers.regPC / 16 / 16)); // Push High Byte
                        memory.stackPush(Convert.ToByte(registers.regPC & 0xFF));    // Push Low Byte

                        // Get Low and High Byte of new memory location
                        intMem = byteOne + byteTwo * 16 * 16;  // Add High Byte to Low Byte

                        registers.regPC = intMem;    // Set Program Counter to new location

                        // Update CPU Cycles
                        intCpuCycles = 6;
                    }
                    break;
                #endregion

                #region OpCode LDA
                // LDA - Load Memory into Accumulator
                //  N Z C I D V
                //  / / _ _ _ _
                case 0xA9:  // LDA - Immediate
                    {
                        int intRegA;

                        // Load Memory into Accumulator
                        intRegA = byteOne;

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 2;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0xA5:  // LDA - Absolute Zero-Page
                    {
                        int intRegA;

                        // Load Memory into Accumulator
                        intRegA = addrAbsZP(byteOne);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 3;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0xB5:  // LDA - Indexed Zero-Page (X)
                    {
                        int intRegA;

                        // Load Memory into Accumulator
                        intRegA = addrIdxZP(byteOne, registers.regX);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 4;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0xAD:  // LDA - Absolute
                    {
                        int intRegA;

                        // Load Memory into Accumulator
                        intRegA = addrAbs(byteOne, byteTwo);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 4;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;

                case 0xBD:  // LDA - Absolute, Indexed (X)
                    {
                        int intRegA;

                        // Load Memory into Accumulator
                        intRegA = addrIdx(byteOne, byteTwo, registers.regX);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        pageChangeCheck();
                        intCpuCycles = 4 + intCpuCyclesOffset;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;

                case 0xB9:  // LDA - Absolute, Indexed (Y)
                    {
                        int intRegA;

                        if (registers.regY == 0x34)
                        {

                        }

                        // Load Memory into Accumulator
                        intRegA = addrIdx(byteOne, byteTwo, registers.regY);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        pageChangeCheck();
                        intCpuCycles = 4 + intCpuCyclesOffset;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;

                case 0xA1:  // LDA - Indirect (X)
                    {
                        int intRegA;

                        // Load Memory into Accumulator
                        intRegA = addrIndX(byteOne, registers.regX);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 6;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0xB1:  // LDA - Indirect (Y)
                    {
                        int intRegA;

                        // Load Memory into Accumulator
                        intRegA = addrIndY(byteOne, registers.regY);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        pageChangeCheck();
                        intCpuCycles = 5 + intCpuCyclesOffset;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;
                #endregion

                #region OpCode LDX
                // LDX - Load Memory into X
                //  N Z C I D V
                //  / / _ _ _ _
                case 0xA2:  // LDX - Immediate
                    {
                        int intRegX;

                        // Load Memory into X
                        intRegX = byteOne;

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegX & 0xFF);
                        registers.statusZero = checkStatusZero(intRegX & 0xFF);

                        registers.regX = Convert.ToByte(intRegX & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 2;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0xA6:  // LDX - Absolute Zero-Page
                    {
                        int intRegX;

                        // Load Memory into X
                        intRegX = addrAbsZP(byteOne);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegX & 0xFF);
                        registers.statusZero = checkStatusZero(intRegX & 0xFF);

                        registers.regX = Convert.ToByte(intRegX & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 3;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0xB6:  // LDX - Indexed Zero-Page (Y)
                    {
                        int intRegX;

                        // Load Memory into X
                        intRegX = addrIdxZP(byteOne, registers.regY);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegX & 0xFF);
                        registers.statusZero = checkStatusZero(intRegX & 0xFF);

                        registers.regX = Convert.ToByte(intRegX & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 4;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0xAE:  // LDX - Absolute
                    {
                        int intRegX;

                        // Load Memory into X
                        intRegX = addrAbs(byteOne, byteTwo);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegX & 0xFF);
                        registers.statusZero = checkStatusZero(intRegX & 0xFF);

                        registers.regX = Convert.ToByte(intRegX & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 4;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;

                case 0xBE:  // LDX - Absolute, Indexed (Y)
                    {
                        int intRegX;

                        // Load Memory into X
                        intRegX = addrIdx(byteOne, byteTwo, registers.regY);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegX & 0xFF);
                        registers.statusZero = checkStatusZero(intRegX & 0xFF);

                        registers.regX = Convert.ToByte(intRegX & 0xFF);

                        // Update CPU Cycles
                        pageChangeCheck();
                        intCpuCycles = 4 + intCpuCyclesOffset;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;
                #endregion

                #region OpCode LDY
                // LDY - Load Memory into Y
                //  N Z C I D V
                //  / / _ _ _ _
                case 0xA0:  // LDY - Immediate
                    {
                        int intRegY;

                        // Load Memory into Y
                        intRegY = byteOne;

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegY & 0xFF);
                        registers.statusZero = checkStatusZero(intRegY & 0xFF);

                        registers.regY = Convert.ToByte(intRegY & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 2;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0xA4:  // LDY - Absolute Zero-Page
                    {
                        int intRegY;

                        // Load Memory into Y
                        intRegY = addrAbsZP(byteOne);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegY & 0xFF);
                        registers.statusZero = checkStatusZero(intRegY & 0xFF);

                        registers.regY = Convert.ToByte(intRegY & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 3;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0xB4:  // LDY - Indexed Zero-Page (X)
                    {
                        int intRegY;

                        // Load Memory into Y
                        intRegY = addrIdxZP(byteOne, registers.regX);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegY & 0xFF);
                        registers.statusZero = checkStatusZero(intRegY & 0xFF);

                        registers.regY = Convert.ToByte(intRegY & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 4;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0xAC:  // LDY - Absolute
                    {
                        int intRegY;

                        // Load Memory into Y
                        intRegY = addrAbs(byteOne, byteTwo);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegY & 0xFF);
                        registers.statusZero = checkStatusZero(intRegY & 0xFF);

                        registers.regY = Convert.ToByte(intRegY & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 4;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;

                case 0xBC:  // LDY - Absolute, Indexed (X)
                    {
                        int intRegY;

                        // Load Memory into Y
                        intRegY = addrIdx(byteOne, byteTwo, registers.regX);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegY & 0xFF);
                        registers.statusZero = checkStatusZero(intRegY & 0xFF);

                        registers.regY = Convert.ToByte(intRegY & 0xFF);

                        // Update CPU Cycles
                        pageChangeCheck();
                        intCpuCycles = 4 + intCpuCyclesOffset;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;
                #endregion

                #region OpCode LSR
                // LSR - LSR Shift Right One Bit (Accumulator or Memory)
                //  N Z C I D V
                //  / / / _ _ _
                case 0x4A:  // LSR - Accumulator
                    {
                        int intRegA;

                        // Check Low Bit for Carry to the Right before doing Shift Right
                        if ((registers.regA & 0x01) == 0x01)
                        {
                            registers.statusCarry = true;
                        }
                        else { registers.statusCarry = false; }

                        // Shift Memory or Accumulator One Bit to the Right
                        intRegA = registers.regA / 2;

                        // Set status bits
                        registers.statusNegative = false;
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 2;

                        // Update Program Counter
                        registers.regPC += 1;
                    }
                    break;

                case 0x46:  // LSR - Absolute Zero-Page
                    {
                        int intMem;

                        // Get Memory before Shift Right to Check Carry Flag
                        intMem = addrAbsZP(byteOne);

                        // Check Low Bit for Carry to the Right before doing Shift Right
                        if ((intMem & 0x01) == 0x01)
                        {
                            registers.statusCarry = true;
                        }
                        else { registers.statusCarry = false; }

                        // Shift Memory or Accumulator One Bit to the Right
                        intMem = intMem / 2;

                        // Set status bits
                        registers.statusNegative = false;
                        registers.statusZero = checkStatusZero(intMem & 0xFF);

                        memory.WritePRG(Convert.ToInt16(byteOne), (Convert.ToByte(intMem & 0xFF)));

                        // Update CPU Cycles
                        intCpuCycles = 5;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0x56:  // LSR - Indexed Zero-Page (X)
                    {
                        int intMem;

                        // Get Memory before Shift Right to Check Carry Flag
                        intMem = addrIdxZP(byteOne, registers.regX);

                        // Check Low Bit for Carry to the Right before doing Shift Right
                        if ((intMem & 0x01) == 0x01)
                        {
                            registers.statusCarry = true;
                        }
                        else { registers.statusCarry = false; }

                        // Shift Memory or Accumulator One Bit to the Right
                        intMem = intMem / 2;

                        // Set status bits
                        registers.statusNegative = false;
                        registers.statusZero = checkStatusZero(intMem & 0xFF);

                        memory.WritePRG((byte)(byteOne + registers.regX), (Convert.ToByte(intMem & 0xFF)));

                        // Update CPU Cycles
                        intCpuCycles = 6;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0x4E:  // LSR - Absolute
                    {
                        int intMem;

                        // Get Memory before Shift Right to Check Carry Flag
                        intMem = addrAbs(byteOne, byteTwo);

                        // Check Low Bit for Carry to the Right before doing Shift Right
                        if ((intMem & 0x01) == 0x01)
                        {
                            registers.statusCarry = true;
                        }
                        else { registers.statusCarry = false; }

                        // Shift Memory or Accumulator One Bit to the Right
                        intMem = intMem / 2;

                        // Set status bits
                        registers.statusNegative = false;
                        registers.statusZero = checkStatusZero(intMem & 0xFF);

                        memory.WritePRG((byteOne + byteTwo * 16 * 16), (Convert.ToByte(intMem & 0xFF)));

                        // Update CPU Cycles
                        intCpuCycles = 6;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;

                case 0x5E:  // LSR - Absolute, Indexed (X)
                    {
                        int intMem;

                        // Get Memory before Shift Right to Check Carry Flag
                        intMem = addrIdx(byteOne, byteTwo, registers.regX);

                        // Check Low Bit for Carry to the Right before doing Shift Right
                        if ((intMem & 0x01) == 0x01)
                        {
                            registers.statusCarry = true;
                        }
                        else { registers.statusCarry = false; }

                        // Shift Memory or Accumulator One Bit to the Right
                        intMem = intMem / 2;

                        // Set status bits
                        registers.statusNegative = false;
                        registers.statusZero = checkStatusZero(intMem & 0xFF);

                        memory.WritePRG((byteOne + byteTwo * 16 * 16) + registers.regX, (Convert.ToByte(intMem & 0xFF)));

                        // Update CPU Cycles
                        intCpuCycles = 7;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;
                #endregion

                #region OpCode NOP
                // NOP - No OPeration (2 CPU Cycles)
                //  N Z C I D V
                //  _ _ _ _ _ _
                case 0xEA:  // NOP - Implied
                case 0x1A:
                case 0x3A:
                case 0x5A:
                case 0x7A:
                case 0xDA:
                case 0xFA:
                    {
                        // Do Nothing for 2 CPU Cycles - ADD CODE HERE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                        // Update CPU Cycles
                        intCpuCycles = 2;

                        // Update Program Counter
                        registers.regPC += 1;
                    }
                    break;

                case 0x04:  // NOP
                case 0x14:
                case 0x34:
                case 0x44:
                case 0x54:
                case 0x64:
                case 0x74:
                case 0xD4:
                case 0xF4:
                    {
                        // Do Nothing for 3 CPU Cycles - ADD CODE HERE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                        // Update CPU Cycles
                        intCpuCycles = 3;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0x0C:  // NOP
                case 0x1C:
                case 0x3C:
                case 0x5C:
                case 0x7C:
                case 0xDC:
                case 0xFC:
                    {
                        // Do Nothing for 4 CPU Cycles - ADD CODE HERE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                        // Update CPU Cycles
                        intCpuCycles = 4;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;
                #endregion

                #region OpCode ORA
                // ORA - OR Memory with Accumulator
                //  N Z C I D V
                //  / / _ _ _ _
                case 0x09:  // ORA - Immediate
                    {
                        int intRegA;

                        // OR Accumulator + Byte Given
                        intRegA = registers.regA | byteOne;

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 2;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0x05:  // ORA - Absolute Zero-Page
                    {
                        int intRegA;

                        // OR Accumulator + Byte Given
                        intRegA = registers.regA | addrAbsZP(byteOne);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 3;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0x15:  // ORA - Indexed Zero-Page (X)
                    {
                        int intRegA;

                        // OR Accumulator + Byte Given
                        intRegA = registers.regA | addrIdxZP(byteOne, registers.regX);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 4;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0x0D:  // ORA - Absolute
                    {
                        int intRegA;

                        // OR Accumulator + Byte Given
                        intRegA = registers.regA | addrAbs(byteOne, byteTwo);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 4;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;

                case 0x1D:  // ORA - Absolute, Indexed (X)
                    {
                        int intRegA;

                        // OR Accumulator + Byte Given
                        intRegA = registers.regA | addrIdx(byteOne, byteTwo, registers.regX);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        pageChangeCheck();
                        intCpuCycles = 4 + intCpuCyclesOffset;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;

                case 0x19:  // ORA - Absolute, Indexed (Y)
                    {
                        int intRegA;

                        // OR Accumulator + Byte Given
                        intRegA = registers.regA | addrIdx(byteOne, byteTwo, registers.regY);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        pageChangeCheck();
                        intCpuCycles = 4 + intCpuCyclesOffset;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;

                case 0x01:  // ORA - Indirect (X)
                    {
                        int intRegA;

                        // OR Accumulator + Byte Given
                        intRegA = registers.regA | addrIndX(byteOne, registers.regX);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 6;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0x11:  // ORA - Indirect (Y)
                    {
                        int intRegA;

                        // OR Accumulator + Byte Given
                        intRegA = registers.regA | addrIndY(byteOne, registers.regY);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 5;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;
                #endregion

                #region OpCode PHA
                // PHA - Push Accumulator to Stack
                //  N Z C I D V
                //  _ _ _ _ _ _
                case 0x48:  // PHA - Implied
                    {
                        // Push Accumulator to Stack
                        memory.stackPush(registers.regA);

                        // Update CPU Cycles
                        intCpuCycles = 3;

                        // Update Program Counter
                        registers.regPC += 1;
                    }
                    break;
                #endregion

                #region OpCode PHP
                // PHP - Push Status Register to Stack
                //  N Z C I D V
                //  _ _ _ _ _ _
                case 0x08:  // PHP - Implied
                    {
                        // Set BREAK and BIT5 before pushing to stack (according to wiki - http://wiki.nesdev.com/w/index.php/CPU_status_flag_behavior)
                        registers.statusBreak = true;
                        //registers.statusBitFive = true;

                        // Update Status Register Byte Before Pushing to Stack
                        registers.setStatusRegister();

                        // Set Interrupt flag (Side effect after pushing according to wiki - http://wiki.nesdev.com/w/index.php/CPU_status_flag_behavior)
                        //registers.statusInterrupt = true;

                        // Push Status Register to Stack
                        memory.stackPush(registers.regStatus);

                        // Clear BREAK after pushing to stack
                        //registers.statusBreak = false;

                        // Update Status Register Byte Before Pushing to Stack
                        //registers.setStatusRegister();

                        // Update CPU Cycles
                        intCpuCycles = 3;

                        // Update Program Counter
                        registers.regPC += 1;
                    }
                    break;
                #endregion

                #region OpCode PLA
                // PLA - Pull Accumulator from Stack
                //  N Z C I D V
                //  / / _ _ _ _
                case 0x68:  // PLA - Implied
                    {
                        // Pull Accumulator from Stack
                        registers.regA = memory.stackPop();

                        // Set the Status Register based on new Popped Status
                        registers.setStatusRegisterWithRegStatusByte();

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(registers.regA);
                        registers.statusZero = checkStatusZero(registers.regA);

                        // Update CPU Cycles
                        intCpuCycles = 4;

                        // Update Program Counter
                        registers.regPC += 1;
                    }
                    break;
                #endregion

                #region OpCode PLP
                // PLP - Pull Status Register from Stack
                //  N Z C I D V
                //  _ _ _ _ _ _
                case 0x28:  // PLP - Implied
                    {
                        // Store bits 4 and 5 because they are ignored on PLP stack PoP (http://wiki.nesdev.com/w/index.php/CPU_status_flag_behavior)
                        bool tempBreak = registers.statusBreak;
                        bool tempBitFive = registers.statusBitFive;

                        // Pull Accumulator from Stack
                        registers.regStatus = memory.stackPop();

                        // Set the Status Register based on new Popped Status
                        registers.setStatusRegisterWithRegStatusByte();

                        // Restore bit 4 and 5
                        registers.statusBreak = tempBreak;
                        registers.statusBitFive = tempBitFive;

                        // Update Status Register Byte before setting flags below
                        registers.setStatusRegister();

                        //// Set status bits
                        //registers.statusNegative = checkStatusNegative(registers.regA);
                        //registers.statusZero = checkStatusZero(registers.regA);

                        // Update CPU Cycles
                        intCpuCycles = 4;

                        // Update Program Counter
                        registers.regPC += 1;
                    }
                    break;
                #endregion

                #region OpCode ROL
                // ROL - ROL Rotate Left One Bit (Accumulator or Memory) with Carry
                //  N Z C I D V
                //  / / / _ _ _
                case 0x2A:  // ROL - Accumulator
                    {
                        int intRegA;

                        /* The Carry bit (bit 7) is read AND changed in this operation, but cannot
                           be done in the same step.  We must read the initial Accum/Memory
                           value in first to check whether the Carry bit will be filled by
                           the Rotate. Then we Rotate (including the current Carry) and then
                           set the Carry bit after the operation is complete. */
                        bool bolRegACarry;

                        // Get Memory or Accumulator to Check for Potential Carry
                        intRegA = registers.regA;

                        // Set Carry based on intRegA
                        if ((intRegA & 0x80) == 0x80)
                        {
                            bolRegACarry = true;
                        }
                        else { bolRegACarry = false; }

                        // Shift Memory or Accumulator One Bit to the Left
                        intRegA = intRegA * 2 + Convert.ToInt16(registers.statusCarry);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);
                        registers.statusCarry = bolRegACarry;

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 2;

                        // Update Program Counter
                        registers.regPC += 1;
                    }
                    break;

                case 0x26:  // ROL - Absolute Zero-Page
                    {
                        int intMem;

                        /* The Carry bit (bit 7) is read AND changed in this operation, but cannot
                        be done in the same step.  We must read the initial Accum/Memory
                        value in first to check whether the Carry bit will be filled by
                        the Rotate. Then we Rotate (including the current Carry) and then
                        set the Carry bit after the operation is complete. */
                        bool bolRegACarry;

                        // Get Memory or Accumulator to Check for Potential Carry
                        intMem = addrAbsZP(byteOne);

                        // Set Carry based on intRegA
                        if ((intMem & 0x80) == 0x80)
                        {
                            bolRegACarry = true;
                        }
                        else { bolRegACarry = false; }

                        // Shift Memory or Accumulator One Bit to the Left
                        intMem = addrAbsZP(byteOne) * 2 + Convert.ToInt16(registers.statusCarry);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intMem & 0xFF);
                        registers.statusZero = checkStatusZero(intMem & 0xFF);
                        registers.statusCarry = bolRegACarry;

                        memory.WritePRG(Convert.ToInt16(byteOne), (Convert.ToByte(intMem & 0xFF)));

                        // Update CPU Cycles
                        intCpuCycles = 5;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0x36:  // ROL - Indexed Zero-Page (X)
                    {
                        int intMem;

                        /* The Carry bit (bit 7) is read AND changed in this operation, but cannot
                        be done in the same step.  We must read the initial Accum/Memory
                        value in first to check whether the Carry bit will be filled by
                        the Rotate. Then we Rotate (including the current Carry) and then
                        set the Carry bit after the operation is complete. */
                        bool bolRegACarry;

                        // Get Memory or Accumulator to Check for Potential Carry
                        intMem = addrIdxZP(byteOne, registers.regX);

                        // Set Carry based on intRegA
                        if ((intMem & 0x80) == 0x80)
                        {
                            bolRegACarry = true;
                        }
                        else { bolRegACarry = false; }

                        // Shift Memory or Accumulator One Bit to the Left
                        intMem = addrIdxZP(byteOne, registers.regX) * 2 + Convert.ToInt16(registers.statusCarry);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intMem & 0xFF);
                        registers.statusZero = checkStatusZero(intMem & 0xFF);
                        registers.statusCarry = bolRegACarry;

                        memory.WritePRG((byte)(byteOne + registers.regX), (Convert.ToByte(intMem & 0xFF)));

                        // Update CPU Cycles
                        intCpuCycles = 6;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0x2E:  // ROL - Absolute
                    {
                        int intMem;

                        /* The Carry bit (bit 7) is read AND changed in this operation, but cannot
                        be done in the same step.  We must read the initial Accum/Memory
                        value in first to check whether the Carry bit will be filled by
                        the Rotate. Then we Rotate (including the current Carry) and then
                        set the Carry bit after the operation is complete. */
                        bool bolRegACarry;

                        // Get Memory or Accumulator to Check for Potential Carry
                        intMem = addrAbs(byteOne, byteTwo);

                        // Set Carry based on intRegA
                        if ((intMem & 0x80) == 0x80)
                        {
                            bolRegACarry = true;
                        }
                        else { bolRegACarry = false; }

                        // Shift Memory or Accumulator One Bit to the Left
                        intMem = addrAbs(byteOne, byteTwo) * 2 + Convert.ToInt16(registers.statusCarry);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intMem & 0xFF);
                        registers.statusZero = checkStatusZero(intMem & 0xFF);
                        registers.statusCarry = bolRegACarry;

                        memory.WritePRG((byteOne + byteTwo * 16 * 16), (Convert.ToByte(intMem & 0xFF)));

                        // Update CPU Cycles
                        intCpuCycles = 6;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;

                case 0x3E:  // ROL - Absolute, Indexed (X)
                    {
                        int intMem;

                        /* The Carry bit (bit 7) is read AND changed in this operation, but cannot
                        be done in the same step.  We must read the initial Accum/Memory
                        value in first to check whether the Carry bit will be filled by
                        the Rotate. Then we Rotate (including the current Carry) and then
                        set the Carry bit after the operation is complete. */
                        bool bolRegACarry;

                        // Get Memory or Accumulator to Check for Potential Carry
                        intMem = addrIdx(byteOne, byteTwo, registers.regX);

                        // Set Carry based on intRegA
                        if ((intMem & 0x80) == 0x80)
                        {
                            bolRegACarry = true;
                        }
                        else { bolRegACarry = false; }

                        // Shift Memory or Accumulator One Bit to the Left
                        intMem = addrIdx(byteOne, byteTwo, registers.regX) * 2 + Convert.ToInt16(registers.statusCarry);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intMem & 0xFF);
                        registers.statusZero = checkStatusZero(intMem & 0xFF);
                        registers.statusCarry = bolRegACarry;

                        memory.WritePRG((byteOne + byteTwo * 16 * 16) + registers.regX, (Convert.ToByte(intMem & 0xFF)));

                        // Update CPU Cycles
                        intCpuCycles = 7;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;
                #endregion

                #region OpCode ROR
                // ROR - ROR Rotate Right One Bit (Accumulator or Memory) with Carry
                //  N Z C I D V
                //  / / / _ _ _
                case 0x6A:  // ROR - Accumulator
                    {
                        int intRegA;

                        /* The Carry bit (bit 0) is read AND changed in this operation, but cannot
                           be done in the same step.  We must read the initial Accum/Memory
                           value in first to check whether the Carry bit will be filled by
                           the Rotate. Then we Rotate (including the current Carry) and then
                           set the Carry bit after the operation is complete. */
                        bool bolRegACarry;

                        // Get Memory or Accumulator to Check for Potential Carry
                        intRegA = registers.regA;

                        // Set Carry based on intRegA
                        if ((intRegA & 0x01) == 0x01)
                        {
                            bolRegACarry = true;
                        }
                        else { bolRegACarry = false; }

                        // Shift Memory or Accumulator One Bit to the Left
                        // Multiply Carry by 128 to place it into Bit 7 place before adding.
                        intRegA = intRegA / 2 + (Convert.ToInt16(registers.statusCarry) * 128);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intRegA & 0xFF);
                        registers.statusZero = checkStatusZero(intRegA & 0xFF);
                        registers.statusCarry = bolRegACarry;

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 2;

                        // Update Program Counter
                        registers.regPC += 1;
                    }
                    break;

                case 0x66:  // ROR - Absolute Zero-Page
                    {
                        int intMem;

                        /* The Carry bit (bit 0) is read AND changed in this operation, but cannot
                        be done in the same step.  We must read the initial Accum/Memory
                        value in first to check whether the Carry bit will be filled by
                        the Rotate. Then we Rotate (including the current Carry) and then
                        set the Carry bit after the operation is complete. */
                        bool bolRegACarry;

                        // Get Memory or Accumulator to Check for Potential Carry
                        intMem = addrAbsZP(byteOne);

                        // Set Carry based on intRegA
                        if ((intMem & 0x01) == 0x01)
                        {
                            bolRegACarry = true;
                        }
                        else { bolRegACarry = false; }

                        // Shift Memory or Accumulator One Bit to the Left
                        // Multiply Carry by 128 to place it into Bit 7 place before adding.
                        intMem = addrAbsZP(byteOne) / 2 + (Convert.ToInt16(registers.statusCarry) * 128);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intMem & 0xFF);
                        registers.statusZero = checkStatusZero(intMem & 0xFF);
                        registers.statusCarry = bolRegACarry;

                        memory.WritePRG(Convert.ToInt16(byteOne), (Convert.ToByte(intMem & 0xFF)));

                        // Update CPU Cycles
                        intCpuCycles = 5;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0x76:  // ROR - Indexed Zero-Page (X)
                    {
                        int intMem;

                        /* The Carry bit (bit 0) is read AND changed in this operation, but cannot
                        be done in the same step.  We must read the initial Accum/Memory
                        value in first to check whether the Carry bit will be filled by
                        the Rotate. Then we Rotate (including the current Carry) and then
                        set the Carry bit after the operation is complete. */
                        bool bolRegACarry;

                        // Get Memory or Accumulator to Check for Potential Carry
                        intMem = addrIdxZP(byteOne, registers.regX);

                        // Set Carry based on intRegA
                        if ((intMem & 0x01) == 0x01)
                        {
                            bolRegACarry = true;
                        }
                        else { bolRegACarry = false; }

                        // Shift Memory or Accumulator One Bit to the Left
                        // Multiply Carry by 128 to place it into Bit 7 place before adding.
                        intMem = addrIdxZP(byteOne, registers.regX) / 2 + (Convert.ToInt16(registers.statusCarry) * 128);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intMem & 0xFF);
                        registers.statusZero = checkStatusZero(intMem & 0xFF);
                        registers.statusCarry = bolRegACarry;

                        memory.WritePRG((byte)(byteOne + registers.regX), (Convert.ToByte(intMem & 0xFF)));

                        // Update CPU Cycles
                        intCpuCycles = 6;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0x6E:  // ROR - Absolute
                    {
                        int intMem;

                        /* The Carry bit (bit 0) is read AND changed in this operation, but cannot
                        be done in the same step.  We must read the initial Accum/Memory
                        value in first to check whether the Carry bit will be filled by
                        the Rotate. Then we Rotate (including the current Carry) and then
                        set the Carry bit after the operation is complete. */
                        bool bolRegACarry;

                        // Get Memory or Accumulator to Check for Potential Carry
                        intMem = addrAbs(byteOne, byteTwo);

                        // Set Carry based on intRegA
                        if ((intMem & 0x01) == 0x01)
                        {
                            bolRegACarry = true;
                        }
                        else { bolRegACarry = false; }

                        // Shift Memory or Accumulator One Bit to the Left
                        // Multiply Carry by 128 to place it into Bit 7 place before adding.
                        intMem = addrAbs(byteOne, byteTwo) / 2 + (Convert.ToInt16(registers.statusCarry) * 128);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intMem & 0xFF);
                        registers.statusZero = checkStatusZero(intMem & 0xFF);
                        registers.statusCarry = bolRegACarry;

                        memory.WritePRG((byteOne + byteTwo * 16 * 16), (Convert.ToByte(intMem & 0xFF)));

                        // Update CPU Cycles
                        intCpuCycles = 6;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;

                case 0x7E:  // ROR - Absolute, Indexed (X)
                    {
                        int intMem;

                        /* The Carry bit (bit 0) is read AND changed in this operation, but cannot
                        be done in the same step.  We must read the initial Accum/Memory
                        value in first to check whether the Carry bit will be filled by
                        the Rotate. Then we Rotate (including the current Carry) and then
                        set the Carry bit after the operation is complete. */
                        bool bolRegACarry;

                        // Get Memory or Accumulator to Check for Potential Carry
                        intMem = addrIdx(byteOne, byteTwo, registers.regX);

                        // Set Carry based on intRegA
                        if ((intMem & 0x01) == 0x01)
                        {
                            bolRegACarry = true;
                        }
                        else { bolRegACarry = false; }

                        // Shift Memory or Accumulator One Bit to the Left
                        // Multiply Carry by 128 to place it into Bit 7 place before adding.
                        intMem = addrIdx(byteOne, byteTwo, registers.regX) / 2 + (Convert.ToInt16(registers.statusCarry) * 128);

                        // Set status bits
                        registers.statusNegative = checkStatusNegative(intMem & 0xFF);
                        registers.statusZero = checkStatusZero(intMem & 0xFF);
                        registers.statusCarry = bolRegACarry;

                        memory.WritePRG((byteOne + byteTwo * 16 * 16) + registers.regX, (Convert.ToByte(intMem & 0xFF)));

                        // Update CPU Cycles
                        intCpuCycles = 7;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;
                #endregion

                #region OpCode RTI
                // RTI - Return from Interrupt
                //  N Z C I D V
                //  _ _ _ _ _ _ <- From Stack
                case 0x40:  // RTI - Implied
                    {
                        int intPCAddr;

                        // Store bits 4 and 5 because they are ignored on PLP stack PoP (http://wiki.nesdev.com/w/index.php/CPU_status_flag_behavior)
                        bool tempBreak = registers.statusBreak;
                        bool tempBitFive = registers.statusBitFive;

                        // Get Register Status from Stack
                        registers.regStatus = memory.stackPop();
                        registers.setStatusRegisterWithRegStatusByte();

                        // Restore bit 4 and 5
                        registers.statusBreak = tempBreak;
                        registers.statusBitFive = tempBitFive;

                        // Update Status Register Byte before setting flags below
                        registers.setStatusRegister();

                        // Get Program Counter Address from Stack
                        intPCAddr = memory.stackPop();
                        intPCAddr += memory.stackPop() * 16 * 16;

                        // Copy new address to Program Counter
                        registers.regPC = intPCAddr;

                        // Update CPU Cycles
                        intCpuCycles = 6;
                    }
                    break;
                #endregion

                #region OpCode RTS
                // RTS - Return from Subroutine
                //  N Z C I D V
                //  _ _ _ _ _ _
                case 0x60:  // RTS - Implied
                    {
                        int intPCAddr;

                        // Get Program Counter Address from Stack
                        // 1 is added to the PC because the PC is always left in the last instruction(or last byte of an instruction set)
                        // If you didn't add one, you would end up reading the last byte or instruction (that you've already read) instead
                        // of moving on to the next instruction.  A good example is JSR, it pushed the program counter to the stack and the
                        // location that is stored is that of Byte Two.  When we returne from that jump, we want to skip that last instruction
                        // and move on to the next one.
                        intPCAddr = memory.stackPop() + 1;
                        intPCAddr += memory.stackPop() * 16 * 16;

                        // Copy new address to Program Counter
                        registers.regPC = intPCAddr;

                        // Update CPU Cycles
                        intCpuCycles = 6;
                    }
                    break;
                #endregion

                #region OpCode SBC
                // SBC - Subtract Memory from Accumulator with Borrow with Carry
                //  N Z C I D V
                //  / / / _ _ /
                case 0xE9:  // SBC - Immediate
                    {
                        uint intRegA;

                        // Add Accumulator - Byte Given - Carry
                        intRegA = (uint)(registers.regA - byteOne - (1 - Convert.ToInt16(registers.statusCarry)));

                        // Set status bits
                        registers.statusNegative = checkStatusNegative((byte)intRegA & 0xFF);
                        registers.statusZero = checkStatusZero((byte)intRegA & 0xFF);
                        //registers.statusCarry = checkStatusCarry(intRegA);
                        registers.statusOverflow = checkStatusOverflowSBC(registers.regA, intRegA, byteOne);

                        //if (intRegA >= 0)
                        //{
                        //    registers.statusCarry = true;
                        //}
                        //else if (intRegA < 0)
                        //{
                        //    registers.statusCarry = true;
                        ////}

                        //if (intRegA > 0xFF)
                        //{
                        //    registers.statusCarry = true;
                        //}
                        registers.statusCarry = (intRegA < 0x100);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 2;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0xE5:  // SBC - Absolute Zero-Page
                    {
                        uint intRegA;

                        // Add Accumulator - Byte Given - Carry
                        intRegA = (uint)(registers.regA - addrAbsZP(byteOne) - (1 - Convert.ToInt16(registers.statusCarry)));

                        // Set status bits
                        registers.statusNegative = checkStatusNegative((byte)intRegA & 0xFF);
                        registers.statusZero = checkStatusZero((byte)intRegA & 0xFF);
                        //registers.statusCarry = checkStatusCarry(intRegA);
                        registers.statusOverflow = checkStatusOverflowSBC(registers.regA, intRegA, addrAbsZP(byteOne));

                        //if (intRegA > 0xFF)
                        //{
                        //    registers.statusCarry = true;
                        //}

                        registers.statusCarry = (intRegA < 0x100);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 3;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0xF5:  // SBC - Indexed Zero-Page (X)
                    {
                        uint intRegA;

                        // Add Accumulator - Byte Given - Carry
                        intRegA = (uint)(registers.regA - addrIdxZP(byteOne, registers.regX) - (1 - Convert.ToInt16(registers.statusCarry)));

                        // Set status bits
                        registers.statusNegative = checkStatusNegative((byte)intRegA & 0xFF);
                        registers.statusZero = checkStatusZero((byte)intRegA & 0xFF);
                        //registers.statusCarry = checkStatusCarry(intRegA);
                        registers.statusOverflow = checkStatusOverflowSBC(registers.regA, intRegA, addrIdxZP(byteOne, registers.regX));

                        //if (intRegA > 0xFF)
                        //{
                        //    registers.statusCarry = true;
                        //}

                        registers.statusCarry = (intRegA < 0x100);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 4;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0xED:  // SBC - Absolute
                    {
                        uint intRegA;

                        // Add Accumulator - Byte Given - Carry
                        intRegA = (uint)(registers.regA - addrAbs(byteOne, byteTwo) - (1 - Convert.ToInt16(registers.statusCarry)));

                        // Set status bits
                        registers.statusNegative = checkStatusNegative((byte)intRegA & 0xFF);
                        registers.statusZero = checkStatusZero((byte)intRegA & 0xFF);
                        //registers.statusCarry = checkStatusCarry(intRegA);
                        registers.statusOverflow = checkStatusOverflowSBC(registers.regA, intRegA, addrAbs(byteOne, byteTwo));

                        //if (intRegA > 0xFF)
                        //{
                        //    registers.statusCarry = true;
                        //}

                        registers.statusCarry = (intRegA < 0x100);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 4;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;

                case 0xFD:  // SBC - Absolute, Indexed (X)
                    {
                        uint intRegA;

                        // Add Accumulator - Byte Given - Carry
                        intRegA = (uint)(registers.regA - addrIdx(byteOne, byteTwo, registers.regX) - (1 - Convert.ToInt16(registers.statusCarry)));

                        // Set status bits
                        registers.statusNegative = checkStatusNegative((byte)intRegA & 0xFF);
                        registers.statusZero = checkStatusZero((byte)intRegA & 0xFF);
                        //registers.statusCarry = checkStatusCarry(intRegA);
                        registers.statusOverflow = checkStatusOverflowSBC(registers.regA, intRegA, addrIdx(byteOne, byteTwo, registers.regX));

                        //if (intRegA > 0xFF)
                        //{
                        //    registers.statusCarry = true;
                        //}

                        registers.statusCarry = (intRegA < 0x100);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        pageChangeCheck();
                        intCpuCycles = 4 + intCpuCyclesOffset;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;

                case 0xF9:  // SBC - Absolute, Indexed (Y)
                    {
                        uint intRegA;

                        // Add Accumulator - Byte Given - Carry
                        intRegA = (uint)(registers.regA - addrIdx(byteOne, byteTwo, registers.regY) - (1 - Convert.ToInt16(registers.statusCarry)));

                        // Set status bits
                        registers.statusNegative = checkStatusNegative((byte)intRegA & 0xFF);
                        registers.statusZero = checkStatusZero((byte)intRegA & 0xFF);
                        //registers.statusCarry = checkStatusCarry(intRegA);
                        registers.statusOverflow = checkStatusOverflowSBC(registers.regA, intRegA, addrIdx(byteOne, byteTwo, registers.regY));

                        //if (intRegA > 0xFF)
                        //{
                        //    registers.statusCarry = true;
                        //}

                        registers.statusCarry = (intRegA < 0x100);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        pageChangeCheck();
                        intCpuCycles = 4 + intCpuCyclesOffset;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;

                case 0xE1:  // SBC - Indirect (X)
                    {
                        uint intRegA;

                        // Add Accumulator - Byte Given - Carry
                        intRegA = (uint)(registers.regA - addrIndX(byteOne, registers.regX) - (1 - Convert.ToInt16(registers.statusCarry)));

                        // Set status bits
                        registers.statusNegative = checkStatusNegative((byte)intRegA & 0xFF);
                        registers.statusZero = checkStatusZero((byte)intRegA & 0xFF);
                        //registers.statusCarry = checkStatusCarry(intRegA);
                        registers.statusOverflow = checkStatusOverflowSBC(registers.regA, (byte)intRegA, addrIndX(byteOne, registers.regX));

                        //if (intRegA > 0xFF)
                        //{
                        //    registers.statusCarry = true;
                        //}

                        registers.statusCarry = (intRegA < 0x100);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 6;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0xF1:  // SBC - Indirect (Y)
                    {
                        uint intRegA;

                        // Add Accumulator - Byte Given - Carry
                        intRegA = (uint)(registers.regA - addrIndY(byteOne, registers.regY) - (1 - Convert.ToInt16(registers.statusCarry)));

                        // Set status bits
                        registers.statusNegative = checkStatusNegative((byte)intRegA & 0xFF);
                        registers.statusZero = checkStatusZero((byte)intRegA & 0xFF);
                        //registers.statusCarry = checkStatusCarry(intRegA);
                        registers.statusOverflow = checkStatusOverflowSBC(registers.regA, intRegA, addrIndY(byteOne, registers.regY));

                        //if (intRegA > 0xFF)
                        //{
                        //    registers.statusCarry = true;
                        //}

                        registers.statusCarry = (intRegA < 0x100);

                        registers.regA = Convert.ToByte(intRegA & 0xFF);

                        // Update CPU Cycles
                        intCpuCycles = 5;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;
                #endregion  

                #region OpCode SEC
                // SEC - Set Carry Flag
                //  N Z C I D V
                //  _ _ 1 _ _ _
                case 0x38:  // SEC - Implied
                    {
                        // Set Carry Flag
                        registers.statusCarry = true;

                        // Update CPU Cycles
                        intCpuCycles = 2;

                        // Update Program Counter
                        registers.regPC += 1;
                    }
                    break;
                #endregion

                #region OpCode SED
                // SED - Clear Interrupt Disable Flag
                //  N Z C I D V
                //  _ _ _ _ 1 _
                case 0xF8:  // SED - Implied
                    {
                        // Set Decimal Mode Flag
                        registers.statusDecimal = true;

                        // Update CPU Cycles
                        intCpuCycles = 2;

                        // Update Program Counter
                        registers.regPC += 1;
                    }
                    break;
                #endregion

                #region OpCode SEI
                // SEI - Set Interrupt Disable Flag
                //  N Z C I D V
                //  _ _ _ 1 _ _
                case 0x78:  // SEI - Implied
                    {
                        // Set Interrup Disable Flag
                        registers.statusInterrupt = true;

                        // Update CPU Cycles
                        intCpuCycles = 2;

                        // Update Program Counter
                        registers.regPC += 1;
                    }
                    break;
                #endregion

                #region OpCode STA
                // STA - Store Accumulator in Memory
                //  N Z C I D V
                //  _ _ _ _ _ _
                case 0x85:  // STA - Absolute Zero-Page
                    {
                        // Store Accumulator in Memory
                        memory.WritePRG(byteOne, registers.regA);

                        // Update CPU Cycles
                        intCpuCycles = 3;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0x95:  // STA - Indexed Zero-Page (X)
                    {
                        // Store Accumulator in Memory+Offset
                        memory.WritePRG((byte)(byteOne + registers.regX), registers.regA);

                        // Update CPU Cycles
                        intCpuCycles = 4;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0x8D:  // STA - Absolute
                    {
                        if (registers.regA == 0xFF)
                        {

                        }

                        // Store Accumulator in Memory
                        memory.WritePRG((byteOne + byteTwo * 16 * 16), registers.regA);

                        // Update CPU Cycles
                        intCpuCycles = 4;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;

                case 0x9D:  // STA - Absolute, Indexed (X)
                    {
                        // Store Accumulator in Memory
                        memory.WritePRG((byteOne + byteTwo * 16 * 16) + registers.regX, registers.regA);

                        // Update CPU Cycles
                        intCpuCycles = 5;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;

                case 0x99:  // STA - Absolute, Indexed (Y)
                    {
                        // Store Accumulator in Memory
                        memory.WritePRG((byteOne + byteTwo * 16 * 16) + registers.regY, registers.regA);

                        // Update CPU Cycles
                        intCpuCycles = 5;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;

                case 0x81:  // STA - Indirect (X)
                    {
                        int bLow, bHigh;
                        byte tLow = (byte)(byteOne + registers.regX);

                        // Get rLow and rHigh to find new address which 
                        bLow = memory.ReadPRG(tLow);    // Value from first Zero-Page Address
                        bHigh = memory.ReadPRG(Convert.ToInt32((byte)(tLow + 1)));    // Value from second Zero-Page Address

                        // Store Accumulator in Memory
                        memory.WritePRG(bLow + bHigh * 16 * 16, registers.regA);

                        // Update CPU Cycles
                        intCpuCycles = 6;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0x91:  // STA - Indirect (Y)
                    {
                        byte bLow, bHigh;

                        // Get rLow and rHigh to find new address which 
                        bLow = memory.ReadPRG(Convert.ToInt16(byteOne));    // Value from first Zero-Page Address
                        bHigh = memory.ReadPRG(Convert.ToInt16((byte)(byteOne + 1)));    // Value from second Zero-Page Address

                        // Add Low Byte with X Offset after Getting Second Address (unlike addrIndX)
                        //bLow += registers.regY;
                        //int intTemp = bLow + bHigh + registers.regY;

                        // Store Accumulator in Memory
                        memory.WritePRG(bLow + bHigh * 16 * 16 + registers.regY, registers.regA);

                        // Update CPU Cycles
                        intCpuCycles = 6;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;
                #endregion 

                #region OpCode STX
                // STX - Store Register X in Memory
                //  N Z C I D V
                //  _ _ _ _ _ _
                case 0x86:  // STX - Absolute Zero-Page
                    {
                        // Store Register X in Memory
                        memory.WritePRG(byteOne, registers.regX);

                        // Update CPU Cycles
                        intCpuCycles = 3;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0x96:  // STA - Indexed Zero-Page (Y)
                    {
                        // Store Register X in Memory+Offset
                        memory.WritePRG((byte)(byteOne + registers.regY), registers.regX);

                        // Update CPU Cycles
                        intCpuCycles = 4;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0x8E:  // STA - Absolute
                    {
                        // Store Register X in Memory
                        memory.WritePRG((byteOne + byteTwo * 16 * 16), registers.regX);

                        // Update CPU Cycles
                        intCpuCycles = 4;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;
                #endregion

                #region OpCode STY
                // STY - Store Register X in Memory
                //  N Z C I D V
                //  _ _ _ _ _ _
                case 0x84:  // STY - Absolute Zero-Page
                    {
                        // Store Register Y in Memory
                        memory.WritePRG(byteOne, registers.regY);

                        // Update CPU Cycles
                        intCpuCycles = 3;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0x94:  // STA - Indexed Zero-Page (X)
                    {
                        // Store Register Y in Memory+Offset
                        memory.WritePRG((byte)(byteOne + registers.regX), registers.regY);

                        // Update CPU Cycles
                        intCpuCycles = 4;

                        // Update Program Counter
                        registers.regPC += 2;
                    }
                    break;

                case 0x8C:  // STA - Absolute
                    {
                        // Store Register Y in Memory
                        memory.WritePRG((byteOne + byteTwo * 16 * 16), registers.regY);

                        // Update CPU Cycles
                        intCpuCycles = 4;

                        // Update Program Counter
                        registers.regPC += 3;
                    }
                    break;
                #endregion

                #region OpCode TAX
                // TAX - Transfer Accumulator to Register X
                //  N Z C I D V
                //  / / _ _ _ _
                case 0xAA:  // TAX - Implied
                    {
                        uint intRegA = registers.regA;

                        // Check Status Registers
                        registers.statusNegative = checkStatusNegative(intRegA);
                        registers.statusZero = checkStatusZero(registers.regA);

                        // Transfer Axxumulator to Register X
                        registers.regX = registers.regA;

                        // Update CPU Cycles
                        intCpuCycles = 2;

                        // Update Program Counter
                        registers.regPC += 1;
                    }
                    break;
                #endregion

                #region OpCode TAY
                // TAY - Transfer Accumulator to Register Y
                //  N Z C I D V
                //  / / _ _ _ _
                case 0xA8:  // TAY - Implied
                    {
                        uint intRegA = registers.regA;

                        // Check Status Registers
                        registers.statusNegative = checkStatusNegative(intRegA);
                        registers.statusZero = checkStatusZero(registers.regA);

                        // Transfer Axxumulator to Register X
                        registers.regY = registers.regA;

                        // Update CPU Cycles
                        intCpuCycles = 2;

                        // Update Program Counter
                        registers.regPC += 1;
                    }
                    break;
                #endregion

                #region OpCode TSX
                // TSX - Transfer Stack Pointer to Register X
                //  N Z C I D V
                //  / / _ _ _ _
                case 0xBA:  // TSX - Implied
                    {
                        uint intRegSP = registers.regSP;

                        // Check Status Registers
                        registers.statusNegative = checkStatusNegative(intRegSP);
                        registers.statusZero = checkStatusZero(registers.regSP);

                        // Transfer Stack Pointer to Register X
                        registers.regX = Convert.ToByte(registers.regSP);

                        // Update CPU Cycles
                        intCpuCycles = 2;

                        // Update Program Counter
                        registers.regPC += 1;
                    }
                    break;
                #endregion

                #region OpCode TXA
                // TXA - Transfer Register X to Accumulator
                //  N Z C I D V
                //  / / _ _ _ _
                case 0x8A:  // TXA - Implied
                    {
                        uint intRegX = registers.regX;

                        // Check Status Registers
                        registers.statusNegative = checkStatusNegative(intRegX);
                        registers.statusZero = checkStatusZero(registers.regX);

                        // Transfer Register X to Accumulator
                        registers.regA = registers.regX;

                        // Update CPU Cycles
                        intCpuCycles = 2;

                        // Update Program Counter
                        registers.regPC += 1;
                    }
                    break;
                #endregion

                #region OpCode TXS
                // TXS - Transfer Register X to Stack Pointer
                //  N Z C I D V
                //  _ _ _ _ _ _
                case 0x9A:  // TXS - Implied
                    {
                        // Transfer Register X to Stack Pointer
                        registers.regSP = registers.regX;

                        // Update CPU Cycles
                        intCpuCycles = 2;

                        // Update Program Counter
                        registers.regPC += 1;
                    }
                    break;
                #endregion

                #region OpCode TYA
                // TYA - Transfer Register Y to Accumulator
                //  N Z C I D V
                //  / / _ _ _ _
                case 0x98:  // TXA - Implied
                    {
                        uint intRegY = registers.regY;

                        // Check Status Registers
                        registers.statusNegative = checkStatusNegative(intRegY);
                        registers.statusZero = checkStatusZero(registers.regY);

                        // Transfer Register Y to Accumulator
                        registers.regA = registers.regY;

                        // Update CPU Cycles
                        intCpuCycles = 2;

                        // Update Program Counter
                        registers.regPC += 1;
                    }
                    break;
                #endregion 

                default:
                    {
                        // This is a test &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&& REMOVE ME !!!!!
                        registers.regPC += 1;

                        intCpuCycles = 0;
                        //MessageBox.Show("Bad OpCode: 0x" + String.Format("{0:x2}", byteOpCode));
                        //badOpCode = true;
                    }
                    break;
            }

            // Prevents invalid reads by wrapping around after addition
            registers.regPC &= 0xFFFF;


            /* (FIX) CHeck the following four lines.  I most recently commented out
             * the first two because it looks like the intCpuCyclesOffset was being
             * added in the opcodes AND down here.  */

            //intOpCodeCpuCycles = intCpuCycles + intCpuCyclesOffset;
            //intTotalCpuCycles += intOpCodeCpuCycles * 15;

            //intOpCodeCpuCycles = intCpuCycles + intCpuCyclesOffset;
            intTotalCpuCycles += intCpuCycles * 15;


            // Reset all cycle specific values
            intCpuCycles = 0;
            intCpuCyclesOffset = 0;
            bolPageChanged = false;

            // Update Status Register Byte After Executing OpCodes
            registers.setStatusRegister();

            return 0;       //  Return value after executing OpCode
        }
        #endregion

        public void NMIHandler(MemoryMap memory)
        {
            /* Several things happen when there is a Break
                1. Program Counter increases by 1 to bypass the current instuction on return
                2. The address in the Program Counter is PUSHed to the Stack
                3. Break flag is set
                4. Status Register is PUSHed to the Stack
                5. Interrupt Disable Flag is SET to 1
                6. Program Counter is Loaded with the default 'Break Address' 0xFFFE and 0xFFFF in this case
                7. Interrupt is handled by the code at that address
                8. Program Counter and Status Register are POPped from the Stack
                9. Control is returned to our program and it continues where it left off
             */

            // Increment Program Counter
            // registers.regPC++;

            // Push memory to stack
            memory.stackPush(Convert.ToByte(registers.regPC / 16 / 16)); // Push High Byte
            memory.stackPush(Convert.ToByte(registers.regPC & 0xFF));    // Push Low Byte

            // Set Break Flag (REMEMBER TO DO THIS **AFTER** pushing SR to the stack so interrupt is cleared upon return
            // ?????? ?????? ?????? ?????? ????? ????????????????????? Reference (pg 25) says to set it before pushing SR ?????
            // WIKI says BEFORE, so I put it before http://wiki.nesdev.com/w/index.php/CPU_status_flag_behavior
            registers.statusBreak = false;
            registers.setStatusRegister();
            //registers.statusBitFive = true;
            // Push Status Register to Stack
            memory.stackPush(registers.regStatus);




            // Set Interrupt flag (Side effect after pushing according to wiki - http://wiki.nesdev.com/w/index.php/CPU_status_flag_behavior)
            registers.statusInterrupt = true;
            registers.setStatusRegister();
            // Set Program Counter to 0xFFFE and 0xFFFF to execute appropriate ISR
            /* This will get the Address of the next instructions to execute from
               Memory location 0xFFFE and 0xFFFF.  It should automatically be instructed to
               come back and continue where it left off after executing its ISR routine */
            registers.regPC = memory.ReadPRG(0xFFFA) | (memory.ReadPRG(0xFFFB) * 16 * 16);

            // Update CPU Cycles
            intCpuCycles = 7;

            intOpCodeCpuCycles = intCpuCycles + intCpuCyclesOffset;
            intTotalCpuCycles += intOpCodeCpuCycles * 15;

            // Reset all cycle specific values
            intCpuCycles = 0;
            intCpuCyclesOffset = 0;
            bolPageChanged = false;
        }

        #region /* ----- Set Flags for CoMPare OpCodes (CMP, CPX, CPY) ----- */
        public void setCompareFlags(int regAXY, byte byteOne)
        {
            if (regAXY > byteOne)
            {
                registers.statusCarry = true;
                registers.statusZero = false;
            }
            else if (regAXY == byteOne)
            {
                registers.statusCarry = true;
                registers.statusZero = true;
            }
            else if (regAXY < byteOne)
            {
                registers.statusCarry = false;
                registers.statusZero = false;
            }
        }
        #endregion

        #region /* ----- Check Status Flag Bits ----- */
        bool checkStatusCarry(int result)           // Check to see if result had a Carry
        {
            if (result >= 256)
            {
                return true;
            }
            return false;
        }

        bool checkStatusZero(int result)            // Check to see if result was Zero
        {
            if (result == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        bool checkStatusInterrupt(int result)       // Check for Interrupt
        {
            return Convert.ToBoolean(result);
        }

        bool checkStatusBreak(int result)           // Check Break
        {
            return Convert.ToBoolean(result);
        }

        bool checkStatusOverflowADC(byte regA, uint result, byte byteOne)        // Check for Overflow
        {
            /*The overflow flag is only set under two circumstances:
                1. There is a Carry from D6 to D7, but no Carry out of D7 (CF=0)
                2. There is a Carry from D7 out (CF=1), but no Carry from D6 to D7
                
                In other words, the overflow flag is set to 1 if there is a Carry from
                D6 to D7 or from D7 out, BUT NOT BOTH.
             */

            if ((((regA ^ byteOne) & 0x80) == 0) && (((regA ^ result) & 0x80) != 0))
            {
                /* MessageBox.Show("True"); */
                return true;
            }
            else { /* MessageBox.Show("False");*/ return false; }
        }

        bool checkStatusOverflowSBC(byte regA, uint result, byte byteOne)        // Check for Overflow
        {
            /*The overflow flag is only set under two circumstances:
                1. There is a Carry from D6 to D7, but no Carry out of D7 (CF=0)
                2. There is a Carry from D7 out (CF=1), but no Carry from D6 to D7
                
                In other words, the overflow flag is set to 1 if there is a Carry from
                D6 to D7 or from D7 out, BUT NOT BOTH.
             */

            if ((((regA ^ byteOne) & 0x80) != 0) && (((regA ^ result) & 0x80) != 0))
            {
                /* MessageBox.Show("True"); */
                return true;
            }
            else { /* MessageBox.Show("False");*/ return false; }
        }

        bool checkStatusNegative(int result)        // Check Sign
        {
            if ((result & 0x80) == 0x80)
            {
                return true;
            }
            else { return false; }
        }

        bool checkStatusNegative(uint result)        // Check Sign
        {
            if ((result & 0x80) == 0x80)
            {
                return true;
            }
            else { return false; }
        }
        #endregion

        #region /* ----- Set intCpuCyclesOffset ----- */
        void pageChangeCheck()
        {
            /* Insert code here to check to see if the memory page changes
               so that the number of CPU Cycles can be modified with the 
               correct value */
            intCpuCyclesOffset = Convert.ToInt16(bolPageChanged);
        }
        #endregion

        #region /* ----- Check for Page Crossing to update intCpuCyclesOffset ----- */
        void pageChangeCheck(int intAddr)
        {
            /* Insert code here to check to see if the memory page changes
               so that the number of CPU Cycles can be modified with the 
               correct value */

            #region Check for Page Change
            // Store new address
            int intNewAddress = intAddr;

            // Compare previous(Last) address with the new one to see if page has changed
            if ((intLastAddress & 0xFF00) == (intNewAddress & 0xFF00))
            {
                bolPageChanged = false;
            }
            else
            {
                bolPageChanged = true;
            }

            // Store new address in intLastAddress for comparison next time
            intLastAddress = intNewAddress;
            #endregion
        }
        #endregion

        #region /* ----- Addressing Modes ----- */
        public byte addrAbs(byte bLow, byte bHigh)  // Absolute Addressing Mode
        {
            byte result;

            pageChangeCheck(Convert.ToInt32(bLow) + (Convert.ToInt32(bHigh * 16 * 16)));

            // Add Low and High Bytes.  First need to shift High Byte by two Hex Places (16 * 16 = 256)
            result = memory.ReadPRG(Convert.ToInt32(bLow) + (Convert.ToInt32(bHigh * 16 * 16)));

            return result;
        }

        public byte addrAbsZP(byte bLow)  // Absolute[Zero-Page] Addressing Mode
        {
            byte result;

            pageChangeCheck(Convert.ToInt32(bLow));

            // Absolute[ZP] only reqiures Low Byte, High Byte is 0x00
            result = memory.ReadPRG(Convert.ToInt32(bLow));

            return result;
        }

        public byte addrIdx(byte bLow, byte bHigh, byte regXY)  // Indexed Addressing Mode
        {
            byte result;

            pageChangeCheck(((bLow + bHigh * 16 * 16) + regXY) & 0xFFFF);

            // Add Low and High Byte with X or Y Offset
            result = memory.ReadPRG(((bLow + bHigh * 16 * 16) + regXY) & 0xFFFF);

            return result;
        }

        public byte addrIdxZP(byte bLow, byte regXY)  // Indexed[Zero-Page] Addressing Mode
        {
            byte result;

            pageChangeCheck((byte)(bLow + regXY));

            // Add Low Byte with X or Y Offset, High Byte is 0x00
            result = memory.ReadPRG((byte)(bLow + regXY));

            return result;
        }

        public int addrInd(byte bLow, byte bHigh)  // Indirect Addressing Mode (ONLY USED BY JMP OPCODE)
        {
            /* In this mode, we are give an address that points to another address that is used with the JMP command.
               We get the low byte from the address given, then increment that address and get the high byte from the 
               next memory location.*/

            byte rLow, rHigh;   // Low Byte Result, High Byte Result
            int result;         // Address attained after adding Low and High Result Bytes

            // Add Low Byte with X or Y Offset, High Byte is 0x00
            rLow = memory.ReadPRG(Convert.ToInt32(bLow) + (Convert.ToInt32(bHigh * 16 * 16)));

            bLow += 1;      // Add 1 to get the value from the next address
            if (bLow == 0x00)   // If rLow value changed from 0xFF to 0x00 when adding 1, we add 1 to rHigh
            {
                bHigh += 1;
            }

            // Get High Byte from Next Memory Address
            rHigh = memory.ReadPRG(Convert.ToInt32(bLow) + (Convert.ToInt32(bHigh * 16 * 16)));

            pageChangeCheck(Convert.ToInt32(rLow) + (Convert.ToInt32(rHigh * 16 * 16)));

            // Add the Low Result and High Result to get the new address to return
            result = Convert.ToInt32(rLow) + (Convert.ToInt32(rHigh * 16 * 16));

            return result;
        }

        public byte addrIndX(byte bLow, byte regX)  // Pre-Indexed Indirect Addressing Mode (Only X Register & Only ZERO-PAGE)
        {
            /* In this mode we add the contents of the X Register to the initial address given (hence 'Pre-Indexed') */

            byte rLow, rHigh;   // Low Byte Result, High Byte Result
            byte result;        // Address attained after adding Low and High Result Bytes

            // Add Low Byte with X Offset, High Byte is 0x00
            // Get rLow and rHigh to find new address which holds value we are looking for
            rLow = memory.ReadPRG((byte)((bLow) + regX));    // Value from first Zero-Page Address
            rHigh = memory.ReadPRG(Convert.ToInt32((byte)(bLow + regX + 1)));    // Value from second Zero-Page Address

            pageChangeCheck(rLow + rHigh * 16 * 16);

            // Add the Low Result and High Result to get the new address of the desired value
            result = memory.ReadPRG(rLow + rHigh * 16 * 16);

            return result;
        }

        public byte addrIndY(byte bLow, byte regY)  // Post-Indexed Indirect Addressing Mode (Only Y Register & Only ZERO-PAGE)
        {
            /* In this mode we add the contents of the X Register to the Indirect Address, not the given one ('Post-Indexed ) */

            byte rLow, rHigh;   // Low Byte Result, High Byte Result
            byte result;         // Address attained after adding Low and High Result Bytes

            int temp;

            // Get rLow and rHigh to find new address which holds value we are looking for
            rLow = memory.ReadPRG(bLow);    // Value from first Zero-Page Address
            rHigh = memory.ReadPRG((byte)(bLow + 1));    // Value from second Zero-Page Address

            // Add Low Byte with X Offset after Getting Second Address (unlike addrIndX)
            // Must AND with 0xFFFF because the addresses rollover at 0xFFFF and you need
            // to get rid of the junk in the higher bits if the result is larger than 0xFFFF
            temp = ((rLow + rHigh * 16 * 16) + regY) & 0xFFFF;

            pageChangeCheck(temp);

            // Add the Low Result and High Result to get the new address of the desired value
            result = memory.ReadPRG(temp);

            return result;
        }
        #endregion

        public CPU(MemoryMap refMemory, Input refInput, PPU refPpu, Registers refRegisters)
        {
            memory = refMemory;
            input = refInput;
            ppu = refPpu;
            registers = refRegisters;
        }
    }
}