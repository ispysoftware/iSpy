using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public class LayoutPanel:Panel
    {
        public LayoutPanel()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | 
                ControlStyles.ResizeRedraw | 
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint, true);

            UpdateStyles();
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            if (BrandedImage != null)
            {
                BrandedImage.Left = Width / 2 - BrandedImage.Width / 2;
                BrandedImage.Top = Height / 2 - BrandedImage.Height / 2;
            }
            Invalidate();
            base.OnScroll(se);
        }

        public PictureBox BrandedImage;

        protected override void OnPaint(PaintEventArgs pe)
        {
            if (BrandedImage != null)
            {
                BrandedImage.Left = Width / 2 - BrandedImage.Width / 2;
                BrandedImage.Top = Height / 2 - BrandedImage.Height / 2;
            }
            
            base.OnPaint(pe);
        }
   }
}
