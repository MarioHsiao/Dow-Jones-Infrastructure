//--------------------------------------------------------------------------
// 
//  Copyright (c) Microsoft Corporation.  All rights reserved. 
// 
//  File: FastBitmap.cs
//
//--------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Microsoft.Drawing
{
    public struct PixelData
    {
        public byte B;
        public byte G;
        public byte R;
    }

    public unsafe class FastBitmap : IDisposable
    {
        private readonly Bitmap bitmap;
        private int width;
        private BitmapData bitmapData;
        private byte* pBase = null;
        private readonly Point size;
        private bool locked;

        public FastBitmap(Bitmap bmp)
        {
            if (bmp == null) throw new ArgumentNullException("bmp");

            bitmap = bmp;
            size = new Point(bmp.Width, bmp.Height);

            LockBitmap();
        }

        public Point Size1
        {
            get { return size; }
        }

        public PixelData* GetInitialPixelForRow(int rowNumber)
        {
            return (PixelData*)(pBase + rowNumber * width);
        }

        public PixelData* this[int x, int y]
        {
            get { return (PixelData*)(pBase + y * width + x * sizeof(PixelData)); }
        }

        public Color GetColor(int x, int y)
        {
            var data = this[x, y];
            return Color.FromArgb(data->R, data->G, data->B);
        }

        public void SetColor(int x, int y, Color c)
        {
            var data = this[x, y];
            data->R = c.R;
            data->G = c.G;
            data->B = c.B;
        }

        private void LockBitmap()
        {
            if (locked) throw new InvalidOperationException("Already locked");

            var bounds = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            // Figure out the number of bytes in a row. This is rounded up to be a multiple 
            // of 4 bytes, since a scan line in an image must always be a multiple of 4 bytes
            // in length. 
            width = bounds.Width * sizeof(PixelData);
            if (width % 4 != 0) width = 4 * (width / 4 + 1);

            bitmapData = bitmap.LockBits(bounds, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            pBase = (byte*)bitmapData.Scan0.ToPointer();
            locked = true;
        }

/*
        private void InitCurrentPixel()
        {
            pInitPixel = (PixelData*)pBase;
        }
*/

        private void UnlockBitmap()
        {
            if (!locked) throw new InvalidOperationException("Not currently locked");

            bitmap.UnlockBits(bitmapData);
            bitmapData = null;
            pBase = null;
            locked = false;
        }

        public void Dispose()
        {
            if (locked) UnlockBitmap();
        }
    }
}
