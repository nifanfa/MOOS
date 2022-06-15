namespace NES
{
    public class Registers
    {
        /* 6 Special and General Purpose Registers */
        public int regPC = 0;    // Program Counter
        public byte regSP = 0xFD; // Stack Pointer
        public byte regStatus = 0;     // Status Register ('P'rocessor Status)
        public byte regA = 0;     // Accumulator
        public byte regX = 0;     // Index Register X
        public byte regY = 0;     // Index Register Y

        /* Status (P) Register Bits 0:7, Bit 5 not active */
        public bool statusCarry = false;
        public bool statusZero = false;
        public bool statusInterrupt = true;
        public bool statusDecimal = false;
        public bool statusBreak = false;
        public bool statusBitFive = true;
        public bool statusOverflow = false;
        public bool statusNegative = false;

        /* Set Initial Status bits */
        public void setInitStatusRegister(bool Carry, bool Zero, bool Interrupt,
                                          bool Break, bool Decimal, bool Overflow, bool Negative)
        {
            statusCarry = Carry;
            statusZero = Zero;
            statusInterrupt = Interrupt;
            statusDecimal = Decimal;
            statusBreak = Break;
            statusOverflow = Overflow;
            statusNegative = Negative;
        }

        /* Set/Update All status bits based on bool status */
        public void setStatusRegister()
        {
            regStatusCarry(statusCarry);
            regStatusZero(statusZero);
            regStatusInterruptDisable(statusInterrupt);
            regStatusDecimal(statusDecimal);
            regStatusBreak(statusBreak);
            regStatusBitFive(statusBitFive);
            regStatusOverflow(statusOverflow);
            regStatusNegative(statusNegative);
        }

        /* Set/Update All status bits based on regStatus status */
        public void setStatusRegisterWithRegStatusByte()
        {
            statusCarry = (regStatus & 0x01) == 0x01;
            statusZero = (regStatus & 0x02) == 0x02;
            statusInterrupt = (regStatus & 0x04) == 0x04;
            statusDecimal = (regStatus & 0x08) == 0x08;
            statusBreak = (regStatus & 0x10) == 0x10;
            statusBitFive = (regStatus & 0x20) == 0x20;
            statusOverflow = (regStatus & 0x40) == 0x40;
            statusNegative = (regStatus & 0x80) == 0x80;
        }

        #region /* Status (P) Register Bits 0:7, Bit 5 not active */
        public void regStatusCarry(bool C)  // Carry Flag 0b0000000x
        {
            statusCarry = C;

            if (C)
            {
                regStatus |= 1;     // Set Carry Flag to 1 if C = True, (regStatus | 0b0000000x)
            }
            else
            {
                regStatus &= 254;   // Set Carry Flag to 0 if C = False, (regStatus & 0b1111111x)
            }
        }

        public void regStatusZero(bool C)  // Zero Flag 0b000000x0
        {
            statusZero = C;

            if (C)
            {
                regStatus |= 2;     // Set Zero Flag to 1 if C = True, (regStatus | 0b0000000x)
            }
            else
            {
                regStatus &= 253;   // Set Zero Flag to 0 if C = False, (regStatus & 0b1111111x)
            }
        }

        public void regStatusInterruptDisable(bool C)  // Interrupt Disable 0b00000x00
        {
            statusInterrupt = C;

            if (C)
            {
                regStatus |= 4;
            }
            else
            {
                regStatus &= 251;
            }
        }

        public void regStatusDecimal(bool C)  // Decimal Mode 0b0000x000
        {
            /* Decimal mode is active, but unused in the 2A03 */
            if (C)
            {
                regStatus |= 8;
            }
            else
            {
                regStatus &= 247;
            }
        }

        public void regStatusBreak(bool C)  // Break Command 0b000x0000
        {
            statusBreak = C;

            if (C)
            {
                regStatus |= 16;
            }
            else
            {
                regStatus &= 239;
            }
        }

        public void regStatusBitFive(bool C)  // Break Command 0b00x00000
        {
            /* Bit 5 of the Status Register is not active in the 2A03 CPU */
            if (C)
            {
                regStatus |= 32;
            }
            else
            {
                regStatus &= 223;
            }
        }

        public void regStatusOverflow(bool C)  // Overflow Flag 0b0x000000
        {
            statusOverflow = C;

            if (C)
            {
                regStatus |= 64;
            }
            else
            {
                regStatus &= 191;
            }
        }

        public void regStatusNegative(bool C)  // Negative Flag 0bx0000000
        {
            statusNegative = C;

            if (C)
            {
                regStatus |= 128;
            }
            else
            {
                regStatus &= 127;
            }
        }
        #endregion
    }
}