using System.Drawing.Imaging;
using AForge.Imaging;
using AForge.Imaging.Filters;

namespace iSpyApplication.Vision
{
    public static class Tools
    {
        public static int BytesPerPixel(PixelFormat pixelFormat)
        {
            int bytesPerPixel;

            // calculate bytes per pixel
            switch ( pixelFormat )
            {
                case PixelFormat.Format8bppIndexed:
                    bytesPerPixel = 1;
                    break;
                case PixelFormat.Format16bppGrayScale:
                    bytesPerPixel = 2;
                    break;
                case PixelFormat.Format24bppRgb:
                    bytesPerPixel = 3;
                    break;
                case PixelFormat.Format32bppRgb:
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                    bytesPerPixel = 4;
                    break;
                case PixelFormat.Format48bppRgb:
                    bytesPerPixel = 6;
                    break;
                case PixelFormat.Format64bppArgb:
                case PixelFormat.Format64bppPArgb:
                    bytesPerPixel = 8;
                    break;
                default:
                    throw new UnsupportedImageFormatException( "Can not create image with specified pixel format." );
            }
            return bytesPerPixel;
        }

        public static void ConvertToGrayscale(UnmanagedImage source, UnmanagedImage destination)
        {
            if (source.PixelFormat != PixelFormat.Format8bppIndexed)
            {
                Grayscale.CommonAlgorithms.BT709.Apply(source, destination);
            }
            else
            {
                source.Copy(destination);
            }
        }
    }
}
