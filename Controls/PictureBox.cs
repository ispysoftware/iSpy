using System.Drawing;
using System.Drawing.Imaging;

namespace iSpyApplication.Controls
{

    public class PictureBox : System.Windows.Forms.PictureBox
    {
        private Image _sourceImage;
        private Image _convertedImage;

        /// <summary>
        /// Gets or sets the image that the PictureBox displays.
        /// </summary>
        /// 
        /// <remarks><para>The property is used to set image to be displayed or to get currently
        /// displayed image.</para>
        /// 
        /// <para><note>In the case if source image has high color depth, like 16 bpp grayscale image,
        /// 48 bpp or 64 bpp color image, it is converted to lower color depth before displaying -
        /// to 8 bpp grayscale, 24 bpp or 32 bpp color image respectively.</note></para>
        /// 
        /// <para><note>During color conversion the original source image is kept unmodified, but internal
        /// converted copy is created. The property always returns original source image.</note></para>
        /// </remarks>
        /// 
        public new Image Image
        {
            get { return _sourceImage; }
            set
            {
                // check source image format
                var bitmap = value as Bitmap;
                if (
                    bitmap != null && ((bitmap.PixelFormat == PixelFormat.Format16bppGrayScale) ||
                                        (bitmap.PixelFormat == PixelFormat.Format48bppRgb) ||
                                        (bitmap.PixelFormat == PixelFormat.Format64bppArgb)))
                {
                    // convert and display image
                    Image tempImage = AForge.Imaging.Image.Convert16bppTo8bpp(bitmap);
                    base.Image = tempImage;

                    // dispose previous image if required
                    _convertedImage?.Dispose();

                    _convertedImage = tempImage;
                }
                else
                {
                    // display source image as it is
                    base.Image = value;
                }
                _sourceImage = value;
            }
        }
    }
}
