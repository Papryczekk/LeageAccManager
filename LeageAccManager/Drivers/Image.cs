using System;
using System.Drawing;

namespace LeageAccManager
{
    public static class Image
    {
        // Wartości ARGB
        public const int Alpha = 255;
        public const int Red = 252;
        public const int Green = 252;
        public const int Blue = 252;
        public static Color GetPixelColor(int x, int y)
        {
            IntPtr hdc = NativeImports.GetDC(IntPtr.Zero);
            uint pixel = NativeImports.GetPixel(hdc, x, y);
            NativeImports.ReleaseDC(IntPtr.Zero, hdc);
            Color color = Color.FromArgb((int)(pixel & 0x000000FF),
                (int)(pixel & 0x0000FF00) >> 8,
                (int)(pixel & 0x00FF0000) >> 16);
            return color;
        }
    }
}