using System;
using System.Drawing;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    class MediaPanelControlButton: PictureBox
    {
        private Point _offset = new Point(0,0);
        public Point Offset { 
            get { return _offset; }
            set { _offset = value; }
        } 
        private readonly Rectangle _dstRect = new Rectangle(0,0,30,30);

        private Rectangle SrcRect
        {
            get { return new Rectangle(Offset, new Size(30, 30)); }
        }
        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.DrawImage(Properties.Resources.media_icons, _dstRect, SrcRect, GraphicsUnit.Pixel);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _offset.Y = 30;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _offset.Y = 0;
            Invalidate();
            base.OnMouseEnter(e);
        }
    }
}
