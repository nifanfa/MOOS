using System;
using System.Drawing;

namespace NES
{
    public class PPU
    {
        MemoryMap memory;
        NES tn;

        public Color BGColor;

        #region PPU Variables
        // Main draw buffer
        public byte[] byteBGFrame = new byte[(256 * 240 * 4)];
        public byte[] byteNT0 = new byte[(256 * 240 * 4)];
        public byte[] byteNT1 = new byte[(256 * 240 * 4)];
        public byte[] byteNT2 = new byte[(256 * 240 * 4)];
        public byte[] byteNT3 = new byte[(256 * 240 * 4)];

        byte setAlpha = 0xFF;
        public bool bolReadyToRender = false;
        public bool bolDrawBG = true;
        public bool bolDrawSprites = true;
        int pixelX = 0;
        int pixelM = 0;
        public bool bolDrawNameTable;

        // nametable debug offset drawing. dont draw every frame
        public int NTDebugOffset;

        // Background Variables
        int bgTileNumber = 0;
        int bgTileLineNumber = 0;
        int bgPixelNumber = 0;
        byte bgAlpha;

        public byte bgScrollX, bgScrollY;
        byte bytePTableResult;
        public int nameTable = 0x2000;
        public int NTIndex = 0;
        public int nameTableTemp;
        public byte scrollX = 0, scrollY = 0, scrollXFine = 0, scrollYFine = 0;
        public int t = 0, v = 0, fineX = 0;
        public int scrollIndex = 0;
        public bool bolRead2006 = false;
        public bool toggle = false;
        int fX;
        byte fxTemp;
        bool bolNewTile = true;
        int pal;

        int row, col, brow, bcol, bmrow, bmcol;
        public byte[,] bytePal = new byte[4, 4] {{0x00, 0x00, 0x02, 0x02},
                                                 {0x00, 0x00, 0x02, 0x02},
                                                 {0x04, 0x04, 0x06, 0x06},
                                                 {0x04, 0x04, 0x06, 0x06}};

        // Sprite Variables
        int spScanlineNumber = 0;
        bool scanlineChanged = true;

        bool spritesFound = false;
        byte[] spriteToDraw = new byte[8];
        byte spriteIndex = 0;
        byte intNumSprites = 0;

        bool spritesFoundT = false;
        byte[] spriteToDrawT = new byte[8];
        byte spriteIndexT = 0;
        byte memCPU2002T = 0;

        int spritePatternTableAddr = 0;
        int pixel;
        int sprPal = 0;

        // Color for Palettes to use
        public byte[,] byteColors = new byte[64, 4] {{0x75, 0x75, 0x75, 0x00}, {0x27, 0x1B, 0x8F, 0x00}, {0x00, 0x00, 0xAB, 0x00}, {0x47, 0x00, 0x9F, 0x00}, {0x8F, 0x00, 0x77, 0x00}, {0xAB, 0x00, 0x13, 0x00}, {0xA7, 0x00, 0x00, 0x00}, {0x7F, 0x0B, 0x00, 0x00},
                                                {0x43, 0x2F, 0x00, 0x00}, {0x00, 0x47, 0x00, 0x00}, {0x00, 0x51, 0x00, 0x00}, {0x00, 0x3F, 0x17, 0x00}, {0x1B, 0x3F, 0x5F, 0x00}, {0x00, 0x00, 0x00, 0x00}, {0x00, 0x00, 0x00, 0x00}, {0x00, 0x00, 0x00, 0x00},
                                                {0xBC, 0xBC, 0xBC, 0x00}, {0x00, 0x73, 0xEF, 0x00}, {0x23, 0x3B, 0xEF, 0x00}, {0x83, 0x00, 0xF3, 0x00}, {0xBF, 0x00, 0xBF, 0x00}, {0xE7, 0x00, 0x5B, 0x00}, {0xDB, 0x2B, 0x00, 0x00}, {0xCB, 0x4F, 0x0F, 0x00},
                                                {0x8B, 0x73, 0x00, 0x00}, {0x00, 0x97, 0x00, 0x00}, {0x00, 0xAB, 0x00, 0x00}, {0x00, 0x93, 0x3B, 0x00}, {0x00, 0x83, 0x8B, 0x00}, {0x00, 0x00, 0x00, 0x00}, {0x00, 0x00, 0x00, 0x00}, {0x00, 0x00, 0x00, 0x00},
                                                {0xFF, 0xFF, 0xFF, 0x00}, {0x3F, 0xBF, 0xFF, 0x00}, {0x5F, 0x97, 0xFF, 0x00}, {0xA7, 0x8B, 0xFD, 0x00}, {0xF7, 0x7B, 0xFF, 0x00}, {0xFF, 0x77, 0xB7, 0x00}, {0xFF, 0x77, 0x63, 0x00}, {0xFF, 0x9B, 0x3B, 0x00},
                                                {0xF3, 0xBF, 0x3F, 0x00}, {0x83, 0xD3, 0x13, 0x00}, {0x4F, 0xDF, 0x4B, 0x00}, {0x58, 0xF8, 0x98, 0x00}, {0x00, 0xEB, 0xDB, 0x00}, {0x00, 0x00, 0x00, 0x00}, {0x00, 0x00, 0x00, 0x00}, {0x00, 0x00, 0x00, 0x00},
                                                {0xFF, 0xFF, 0xFF, 0x00}, {0xAB, 0xE7, 0xFF, 0x00}, {0xC7, 0xD7, 0xFF, 0x00}, {0xD7, 0xCB, 0xFF, 0x00}, {0xFF, 0xC7, 0xFF, 0x00}, {0xFF, 0xC7, 0xDB, 0x00}, {0xFF, 0xBF, 0xB3, 0x00}, {0xFF, 0xDB, 0xAB, 0x00},
                                                {0xFF, 0xE7, 0xA3, 0x00}, {0xE3, 0xFF, 0xA3, 0x00}, {0xAB, 0xF3, 0xBF, 0x00}, {0xB3, 0xFF, 0xCF, 0x00}, {0x9F, 0xFF, 0xF3, 0x00}, {0x00, 0x00, 0x00, 0x00}, {0x00, 0x00, 0x00, 0x00}, {0x00, 0x00, 0x00, 0x00}};

        // RunPPU Variables
        public int tsPpu;
        public int cntScanline = -2;
        public int cntScanlineCycle;

        #endregion

        public void renderPixel()
        {
            #region Draw Background to Frame Buffer

            // ---------------- BACKGROUND DRAWING ------------------

            // Check to see if BG is turned on
            if ((memory.memCPU[0x2001] & 0x08) == 0x08)
            { bolDrawBG = true; }
            else
            { bolDrawBG = false; }

            UpdateDrawLocation();

            if (fX >= 0x08)
            {
                fX = 0;

                if ((scrollIndex & 0x001F) == 0x001F)
                {
                    NTIndex ^= 0x01;
                    scrollIndex &= ~0x001F;
                }
                else
                {
                    scrollIndex++;
                    bolNewTile = true;
                }
            }

            if (bolDrawBG)
            {
                //UpdateDrawLocation();

                #region Draw NameTables for Debugging
                // Draw each of the 4 nametables
                if (bolDrawNameTable && (NTDebugOffset == 0))
                {
                    DrawNameTable(0x2000, byteNT0);
                    DrawNameTable(0x2400, byteNT1);
                    DrawNameTable(0x2800, byteNT2);
                    DrawNameTable(0x2C00, byteNT3);
                }
                #endregion

                #region Get Palette for each tile

                // Only get this 1 time for each tile so that you're not wasting
                // cycles by getting the same info over and over again for each
                // of the remaining 7 pixels in that tile. x-------.
                //if (bolNewTile)
                //{
                bolNewTile = false;

                // Get tile Row (0-29) and Column (0-31)
                row = bgTileNumber / 32;
                col = bgTileNumber % 32;

                // Get Block Row (0-7) and Column (0-7)
                brow = row / 4;
                bcol = col / 4;

                // Get modulus within each Block's Row (0-3) and Column (0-3)
                bmrow = row % 4;
                bmcol = col % 4;

                // Get the attribute byte associated with the tile using the Block Row and Block Column
                byte byteAttr = memory.memPPU[nameTable + 0x03C0 + (8 * brow + bcol)];

                // Get the Palette to use by shifting a certain number of bits found in the lookup table
                pal = 0x3F00 + ((byteAttr >> (bytePal[bmrow, bmcol])) & 0x03) * 4;
                //}

                // Get Nametable Byte
                byte byteNTable = memory.memPPU[nameTable + bgTileNumber];
                #endregion

                #region Draw each BG pixel to the Frame Buffer
                // Get each of the 8 lines in the tile

                // Get 
                int intPTLocation = (((memory.memCPU[0x2000] & 0x10) >> 4) * 0x1000) + byteNTable * 16 + bgTileLineNumber;

                byte bytePTable1 = memory.memPPU[intPTLocation];  // Get background pattern table byte 1 of 2 for combining
                byte bytePTable1a = memory.memPPU[intPTLocation + 0x08];  // Get background pattern table byte 2 of 2 for combining

                // Get Pixel Data
                // Add high and low pattern table bytes to get corresponding color for each pixel
                byte bytePTableResult = (byte)(((bytePTable1 >> (7 - bgPixelNumber)) & 0x01) | (((bytePTable1a >> (7 - bgPixelNumber)) & 0x01) * 2)); // Get color

                // Reset Alpha
                setAlpha = 0xFF;

                if (bytePTableResult == 0)
                {
                    setAlpha = 0x00;
                }

                // Get BGAlpha because the sprite needs to know whether the BG is transparent or not
                bgAlpha = setAlpha;

                // Get data for drawing tiles without the scroll offsets
                int cntScanlineTemp = pixelM / 256;
                int bgTileLineNumberTemp = cntScanlineTemp % 8;
                int pixelXTemp = pixelM % 256;
                int bgPixelNumberTemp = pixelXTemp % 8;
                int bgTileNumberTemp = (pixelXTemp / 8) + ((cntScanlineTemp / 8) * 32);

                // ***Clean THIS UP...READ MEMORY ONCE INTO VARIABLE AND USE THAT TO SET EACH COLOR INSTEAD OF READING MEMORY 3 TIMES ****************************************************
                int intPixelColor = memory.memPPU[pal + bytePTableResult] & 0x3F;
                drawBGTile(bgTileNumberTemp, bgTileLineNumberTemp, bgPixelNumberTemp, byteColors[intPixelColor, 0], 2);
                drawBGTile(bgTileNumberTemp, bgTileLineNumberTemp, bgPixelNumberTemp, byteColors[intPixelColor, 1], 1);
                drawBGTile(bgTileNumberTemp, bgTileLineNumberTemp, bgPixelNumberTemp, byteColors[intPixelColor, 2], 0);
                drawBGTile(bgTileNumberTemp, bgTileLineNumberTemp, bgPixelNumberTemp, setAlpha, 3);

                #endregion
            }
            #endregion

            #region Draw Sprites to Frame Buffer

            // Check to see if BG is turned on
            if ((memory.memCPU[0x2001] & 0x10) == 0x10)
            { bolDrawSprites = true; }
            else
            { bolDrawSprites = false; }

            if (bolDrawSprites)
            {
                pixelX = (pixelM % 256);

                if (scanlineChanged)
                {
                    // Reset scanlineChanged so it does not enter 'if' unless new scanline
                    scanlineChanged = false;

                    // Check each sprite in OAM until you find a good one (64 Sprites * 4 Attribute Bytes)
                    CheckScanlineForSprites();
                }

                if (spritesFound)
                {
                    if (GetSpriteToDraw())
                    {
                        int y = cntScanline;
                        int x = pixelX;

                        // Check for 0 Sprite Hit
                        if (bgAlpha != 0 && spriteToDraw[spriteIndex] == 0)// && setAlpha != 0 && ((memory.memCPU[0x2001] & 0x10) == 0x10) && ((memory.memCPU[0x2001] & 0x08) == 0x08))
                        {
                            memory.memCPU[0x2002] |= 0x40;
                        }

                        if (bytePTableResult != 0 && ((memory.memSPRRAM[spriteToDraw[spriteIndex] + 2] & 0x20) == 0x00) || (bgAlpha == 0))
                        {
                            int intPixelColor = memory.memPPU[sprPal + bytePTableResult] & 0x3F;

                            drawSprites(x, y, pixel, byteColors[intPixelColor, 2], 0);
                            drawSprites(x, y, pixel, byteColors[intPixelColor, 1], 1);
                            drawSprites(x, y, pixel, byteColors[intPixelColor, 0], 2);
                            drawSprites(x, y, pixel, setAlpha, 3);
                        }
                    }
                }
            }
            #endregion
        }

        public void RunPPU(int runto)
        {
            // Idle Scanline - 1 Scanline  (AKA Scanline 240)
            while (cntScanline == -2 && !tn.bolReset)
            {
                while (cntScanlineCycle < 341)
                {
                    tsPpu += 5;
                    cntScanlineCycle++;

                    if (tsPpu >= runto)
                    { return; }
                }

                cntScanline = -1;
                scanlineChanged = true;
            }

            // VBLANK Time - 20 Scanlines
            if (tsPpu < tn.VBlankTime)
            {
                if (!tn.bolReset)
                    memory.setVblank(true);

                // Make sure the system has not just been reset and check to see if Interrupt is enabled on VBLANK
                if ((memory.memCPU[0x2000] & 0x80) == 0x80 && !tn.bolReset)
                {
                    tn.NMIHandler();
                }

                tn.bolReset = false;
                tsPpu = tn.VBlankTime;   // Do nothing during VBlank until the next 'if' statement is false
                //tsPpu += tn.VBlankTime;  // (FIX ME!!!!!!)  THIS SHOULD BE +=, but it slows down a lot for some reason!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                cntScanline = -1;
                cntScanlineCycle = 0;
            }

            // Check for VBlank over
            if (tsPpu >= runto)
            { return; }
            else
            {
                // Clear Sprite Hit, Sprite Overflow and VBLANK at the end of VBlank
                // For reason, clearing Sprite Hit causes the scroll offset to change seemingly randomly
                // So I have disabled the reset of that below
                if (!tn.bolReset)
                    memory.memCPU[0x2002] &= 0x40;  // (FIX) Should also clear Sprite Hit 0

                // Clear VBlank
                memory.setVblank(false);
            }

            // PreRender Scanline - 1 Scanline
            while (cntScanline == -1)
            {
                while (cntScanlineCycle < 340)
                {
                    tsPpu += 5;
                    cntScanlineCycle++;

                    if (tsPpu >= runto)
                    { return; }
                }

                pixelM = 0;
                cntScanlineCycle = 0;
                cntScanline++;
                scanlineChanged = true;
            }

            // Render Scanlines - 240 Scanlines
            while (cntScanline < 240)
            {
                // Set Rendering TRUE
                tn.rendering = true;

                while (cntScanlineCycle < 256)
                {
                    if ((memory.memCPU[0x2001] & 0x08) == 0x08 || (memory.memCPU[0x2001] & 0x10) == 0x10)
                    {
                        renderPixel();
                    }
                    pixelM++;

                    tsPpu += 5;
                    cntScanlineCycle++;

                    if (tsPpu >= runto)
                    { return; }
                }

                while (cntScanlineCycle < 341)
                {
                    tsPpu += 5;
                    cntScanlineCycle++;

                    if (tsPpu >= runto)
                    { return; }
                }

                cntScanlineCycle = 0;
                cntScanline++;
                scanlineChanged = true;
            }

            if (cntScanline >= 240)
            {
                // Set Rendering to FALSE
                tn.rendering = false;

                tsPpu = 0;
                bolReadyToRender = true;
                cntScanline = -2;
            }
        }

        void UpdateDrawLocation()
        {
            #region V and T operations
            if (pixelM == 0)
            {
                v = memory.t;

                scrollYFine = (byte)((v & 0x7000) >> 12);

                NTIndex = (v >> 10) & 0x03;
                scrollIndex = (v & 0x03FF);
                nameTable = 0x2000 + (NTIndex * 0x0400);

                scanlineChanged = true;

                bolNewTile = true;
            }

            else if (pixelM % 256 == 0)
            {
                // END OF SCANLINE
                if ((v & 0x7000) == 0x7000)
                {
                    v &= ~0x7000;

                    if ((v & 0x03E0) == 0x3A0)
                    { v ^= 0x800 | 0x3A0; }
                    else if ((v & 0x03E0) == 0x3E0)
                    { v ^= 0x3E0; }
                    else
                    { v += 0x0020; }
                }
                else
                {
                    v += 0x1000;
                }

                // Look for changes in fineX to update it below
                fxTemp = memory.fineX;
                fX = memory.fineX;

                // BEGINNING OF SCANLINE
                int temp = memory.t & 0x041F; //041f
                //int temp = t & 0x041F; //041f
                v &= ~0x041F;
                v |= temp;

                // Set X Offset
                //scrollX = (byte)(v & 0x001F); //<<<< --- THIS LINE CAUSES A PROBLEM WHEN CHANGING t to v.  THIS IS LIKELY BECAUSE OF 2006 WRITES HAPPENING THAT ARE NOT RELATED TO SCROLLING
                scrollIndex = (v & 0x03FF);

                // Get NameTable
                NTIndex = (byte)((v & 0x0C00) >> 10);
                nameTable = 0x2000 + (NTIndex * 0x0400);

                bolNewTile = true;
            }
            #endregion

            // FineX update
            if (fxTemp != memory.fineX)
            {
                fX = memory.fineX;
            }

            nameTable = 0x2000 + (NTIndex * 0x0400);

            /********************************* NEED TO FIX THIS MEMORY MASKING STUFF ****************************/
            nameTable = memory.AddressMask(nameTable);
            /********************************* NEED TO FIX THIS MEMORY MASKING STUFF ****************************/

            bgPixelNumber = fX++;
            bgTileLineNumber = (v & 0x7000) >> 12;
            bgTileNumber = scrollIndex;

            // Set background color and set ReadyToRender
            if (pixelM == (256 * 240 - 1))
            {
                // Set backgound color
                byte bTemp = (byte)(memory.memPPU[0x3F00] & 0x3F);
                BGColor = Color.FromArgb(byteColors[bTemp, 0], byteColors[bTemp, 1], byteColors[bTemp, 2]);
                //BGColor.R = byteColors[bTemp, 0]; // R
                //BGColor.G = byteColors[bTemp, 1]; // G
                //BGColor.B = byteColors[bTemp, 2]; // B
                //BGColor.A = 0xFF;//byteColors[memory.memPPU[0x3F00 + bytePaletteSelected + bytePTableResult[m]], 3]; // A

                //bolReadyToRender = true;

                // Counter for drawing the 4 nametable for debugging - Only draw ever N frames                
                if (NTDebugOffset++ >= 30)
                {
                    NTDebugOffset = 0;
                }
            }
        }

        // Only called when DEBUGGING
        void DrawNameTable(int nTable, byte[] byteNT)
        {
            /********************************* NEED TO FIX THIS MEMORY MASKING STUFF ****************************/
            nTable = memory.AddressMask(nTable);
            /***************************************************************************************************/

            // UpdateDrawLocation();

            #region Get Palette for each tile

            // Get data for drawing tiles without the scroll offsets
            int cntScanlineTemp = pixelM / 256;
            int bgTileLineNumberTemp = cntScanlineTemp % 8;
            int pixelXTemp = pixelM % 256;
            int bgPixelNumberTemp = pixelXTemp % 8;
            int bgTileNumberTemp = (pixelXTemp / 8) + ((cntScanlineTemp / 8) * 32);

            // Get tile Row (0-29) and Column (0-31)
            row = bgTileNumberTemp / 32;
            col = bgTileNumberTemp % 32;

            // Get Block Row (0-7) and Column (0-7)
            brow = row / 4;
            bcol = col / 4;

            // Get modulus within each Block's Row (0-3) and Column (0-3)
            bmrow = row % 4;
            bmcol = col % 4;

            // Get the attribute byte associated with the tile using the Block Row and Block Column
            byte byteAttr = memory.memPPU[nTable + 0x03C0 + (8 * brow + bcol)];

            // Get the Palette to use by shifting a certain number of bits found in the lookup table
            int pal = 0x3F00 + ((byteAttr >> (bytePal[bmrow, bmcol])) & 0x03) * 4;
            #endregion

            #region Draw each BG pixel to the Frame Buffer
            // Get Nametable Byte
            byte byteNTable = memory.memPPU[nTable + bgTileNumberTemp];

            // Saving cycles
            int intPTLocation = (((memory.memCPU[0x2000] & 0x10) >> 4) * 0x1000) + byteNTable * 16 + bgTileLineNumberTemp;

            byte bytePTable1 = memory.memPPU[intPTLocation];  // Get background pattern table byte 1 of 2 for combining
            byte bytePTable1a = memory.memPPU[intPTLocation + 0x08];  // Get background pattern table byte 2 of 2 for combining

            // Get Pixel Data
            // Add high and low pattern table bytes to get corresponding color for each pixel
            byte bytePTableResult = (byte)(((bytePTable1 >> (7 - bgPixelNumberTemp)) & 0x01) | (((bytePTable1a >> (7 - bgPixelNumberTemp)) & 0x01) * 2)); // Get color

            // Reset Alpha
            byte alpha = 0xFF;

            if (bytePTableResult == 0)
            {
                alpha = 0x00;
            }

            // Draw RGBA of each pixel
            int intPixelColor = memory.memPPU[pal + bytePTableResult] & 0x3F;
            drawBGTile(byteNT, bgTileNumberTemp, bgTileLineNumberTemp, bgPixelNumberTemp, byteColors[intPixelColor, 0], 2);
            drawBGTile(byteNT, bgTileNumberTemp, bgTileLineNumberTemp, bgPixelNumberTemp, byteColors[intPixelColor, 1], 1);
            drawBGTile(byteNT, bgTileNumberTemp, bgTileLineNumberTemp, bgPixelNumberTemp, byteColors[intPixelColor, 2], 0);
            drawBGTile(byteNT, bgTileNumberTemp, bgTileLineNumberTemp, bgPixelNumberTemp, alpha, 3);

            #endregion
        }

        #region Sprite Rendering Methods
        void CheckScanlineForSprites()
        {
            // Update all previously retreived sprite values as the new current values
            spritesFound = spritesFoundT;           // Stores the location of each sprite found
            spriteToDraw = spriteToDrawT;           // Number of sprites found
            spriteIndex = spriteIndexT;             // Any sprites found?
            memory.memCPU[0x2002] &= memCPU2002T;   // Reset '8 sprites on a scanline' bit

            // Reset Temp Sprite Values for next evaluation
            //spriteToDrawT = new byte[8];    // Stores the location of each sprite found
            spriteIndexT = 0x00;            // Number of sprites found
            spritesFoundT = false;          // Any sprites found?
            memCPU2002T = 0xDF;             // Reset '8 sprites on a scanline' bit

            // Check each sprite in OAM until you find a good one (64 Sprites * 4 Attribute Bytes)
            for (int i = 0; i < 0xFF; i += 4)
            {
                // Set default sprite height
                int spriteHeight = ((memory.memCPU[0x2000] & 0x20) == 0x20) ? 16 : 8;

                // Check to see if the sprite is on the current scanline in the Y direction
                if (cntScanline - (byte)(memory.memSPRRAM[i]) < spriteHeight &&    // Is the sprite within 8 or 16 pixels of current scanline
                    cntScanline - (byte)(memory.memSPRRAM[i]) >= 0 &&              // ...
                    spriteIndexT < 8 &&         // Less than 8 sprites found?
                    memory.memSPRRAM[i] > 0 && // Is the sprite on screen between the 0 and EF scanline?
                    memory.memSPRRAM[i] < 0xEF) // ...
                {
                    // Check to see if the sprite is on the line in the X direction within the screen limits
                    if (memory.memSPRRAM[i + 3] >= 0 && memory.memSPRRAM[i + 3] <= 255)
                    {
                        spriteToDrawT[spriteIndexT++] = (byte)i;

                        spritesFoundT = true;

                        if (spriteIndexT >= 8)
                        {
                            // Set '8 sprites on a scanline' bit
                            memCPU2002T = 0xFF;

                            // Get out of loop and stop checking for sprites to be drawn. Go to next scanline.
                            i = 64 * 4;
                        }
                    }
                }
            }

            if (spritesFound)
            {
                // Get the total number of sprite for each scanline
                intNumSprites = (byte)(spriteIndex - 1);

                // (FIX) (REMOVE) This seems unnecessary...but may not be
                // Reset sprite index to 0; 
                spriteIndex = 0;
            }
        }

        bool GetSpriteToDraw()
        {
            for (byte i = 0; i <= intNumSprites; i++)
            {
                // Set flipping
                int sprPixel = memory.memSPRRAM[spriteToDraw[i] + 3];

                if (pixelX - sprPixel < 8 && pixelX - sprPixel >= 0)
                {
                    if (CheckTransparency(i))
                    {
                        spriteIndex = i;
                        return true;
                    }
                }
            }

            return false;
        }

        bool CheckTransparency(int i)
        {
            // NO NEED TO CHECK FOR spLineNumber, pal, Pattern Table, flipping on every pixel .... only every line except for flipping which is every tile (and mybe others too)
            int spLineNumber = (byte)(cntScanline - (byte)(memory.memSPRRAM[spriteToDraw[i]] + 1));

            // Get the current line number that sprites are being drawn on
            int line = spLineNumber;

            // Get sprite palette
            sprPal = 0x3F10 + (memory.memSPRRAM[spriteToDraw[i] + 2] & 0x03) * 4;

            // Check for sprite flipping
            bool flipY = ((memory.memSPRRAM[spriteToDraw[i] + 2] & 0x80) == 0x80);
            bool flipX = ((memory.memSPRRAM[spriteToDraw[i] + 2] & 0x40) == 0x40);

            // Set flipping
            pixel = pixelX - memory.memSPRRAM[spriteToDraw[i] + 3];
            int temp = 7 - pixel;
            if (flipX)
            {
                temp = pixel;       //  THIS IS MESSED UP
                //x = x + (7 - pixel) * 2;
            }

            // #### Get bytes from SPRRAM ####
            #region Get OAM Pattern Table Bytes

            // If the sprites are 8x8
            if ((memory.memCPU[0x2000] & 0x20) == 0x00) // for 8x8 sprites
            {
                if (flipY)
                {
                    line = (byte)(7 - spLineNumber);
                }

                // Get location of the sprites (Pattern Table 0x0000 or 0x1000)
                spritePatternTableAddr = ((memory.memCPU[0x2000] & 0x08) >> 3) * 0x1000;

                byte bytePTable2 = memory.memPPU[spritePatternTableAddr + memory.memSPRRAM[spriteToDraw[i] + 1] * 16 + line];  // Get sprite data piece 1 to combine with data piece 2 to get color of pixel
                byte bytePTable2a = memory.memPPU[spritePatternTableAddr + memory.memSPRRAM[spriteToDraw[i] + 1] * 16 + line + 8];  // Get sprite data piece 2 to combine with data piece 1 to get color of pixel

                // Combine data and get color of pixel
                bytePTableResult = (byte)(((bytePTable2 >> temp) & 0x01) | (((bytePTable2a >> temp) & 0x01) * 2));
            }
            // Else If the sprites are 8x16
            else if ((memory.memCPU[0x2000] & 0x20) == 0x20) // for 8x16 sprites
            {
                // Check to see if there is a 0x1000 offset
                int intOffset = (memory.memSPRRAM[spriteToDraw[i] + 1] & 0x01) * 0x1000;

                // Get tile number of the specific sprite
                int byteTemp = Convert.ToByte(memory.memSPRRAM[spriteToDraw[i] + 1] & 0xFE);

                // Looks at the top half of the sprite
                if (spLineNumber < 8)
                {
                    if (flipY)
                    {
                        line = (byte)(7 - spLineNumber) + 16;
                    }

                    byte bytePTable2 = memory.memPPU[byteTemp * 16 + intOffset + line];  // Get sprite data piece 1 to combine with data piece 2 to get color of pixel
                    byte bytePTable2a = memory.memPPU[byteTemp * 16 + intOffset + line + 8];  // Get sprite data piece 2 to combine with data piece 1 to get color of pixel

                    // Combine data and get color of pixel
                    bytePTableResult = (byte)(((bytePTable2 >> temp) & 0x01) | (((bytePTable2a >> temp) & 0x01) * 2));
                }

                else if (spLineNumber >= 8)
                {
                    if (flipY)
                    {
                        line = (byte)(15 - spLineNumber) - 8;
                    }

                    byte bytePTable2b = memory.memPPU[byteTemp * 16 + intOffset + line + 8];  // Get sprite data piece 1 to combine with data piece 2 to get color of pixel
                    byte bytePTable2c = memory.memPPU[byteTemp * 16 + intOffset + line + 8 + 8];  // Get sprite data piece 2 to combine with data piece 1 to get color of pixel

                    // Combine data and get color of pixel
                    bytePTableResult = (byte)(((bytePTable2b >> temp) & 0x01) | (((bytePTable2c >> temp) & 0x01) * 2));
                }
            }
            #endregion

            // Set transparency to off
            setAlpha = 0xFF;

            if (bytePTableResult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region Drawing Methods (per Pixel)
        // Draws Background
        public void drawBGTile(int tile, int line, int pixel, byte bytePixel, byte rgbPixel)
        {
            byteBGFrame[(tile * 32 + line * 1024 + (pixel) * 4) + (int)(tile / 32) * 32 * 224 + rgbPixel] = bytePixel;
        }

        // Draws Background - (FOR NAMETABLE DEBUGGING)
        public void drawBGTile(byte[] byteNT, int tile, int line, int pixel, byte bytePixel, byte rgbPixel)
        {
            byteNT[(tile * 32 + line * 1024 + (pixel) * 4) + (int)(tile / 32) * 32 * 224 + rgbPixel] = bytePixel;
        }

        // Draws Sprites
        public void drawSprites(int x, int y, int pixel, byte bytePixel, byte rgbPixel)
        {
            // pixel - pixel number
            // bytePixel - current byte to place into the R, G, or B spot
            // rgbPixel - determines when the bytePixel is R, G, or B

            if (y < 240)
            {
                byteBGFrame[x * 4 + y * 1024 + rgbPixel] = bytePixel;
            }
        }
        #endregion

        public PPU(MemoryMap memoryRef, NES tnRef)
        {
            memory = memoryRef;
            tn = tnRef;

            spriteToDrawT = new byte[8];    // Stores the location of each sprite found
        }
    }
}