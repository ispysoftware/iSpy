using System.Collections;
using System.Drawing;

namespace iSpyApplication
{
    public class HeightComparer : IComparer
    {
        // Return -1, 0, or 1 to indicate whether
        // x belongs before, the same as, or after y.
        // Sort by height, width descending.
        public int Compare(object x, object y)
        {
            Rectangle xrect = (Rectangle)x;
            Rectangle yrect = (Rectangle)y;
            if (xrect.Height < yrect.Height) return 1;
            if (xrect.Height > yrect.Height) return -1;
            if (xrect.Width < yrect.Width) return 1;
            if (xrect.Width > yrect.Width) return -1;
            return 0;
        }
    }

    public class WidthComparer : IComparer
    {
        // Return -1, 0, or 1 to indicate whether
        // x belongs before, the same as, or after y.
        // Sort by height, width descending.
        public int Compare(object x, object y)
        {
            Rectangle xrect = (Rectangle)x;
            Rectangle yrect = (Rectangle)y;
            if (xrect.Width < yrect.Width) return 1;
            if (xrect.Width > yrect.Width) return -1;
            if (xrect.Height < yrect.Height) return 1;
            if (xrect.Height > yrect.Height) return -1;
            return 0;
        }
    }

    public class AreaComparer : IComparer
    {
        // Return -1, 0, or 1 to indicate whether
        // x belongs before, the same as, or after y.
        // Sort by area, height, width descending.
        public int Compare(object x, object y)
        {
            Rectangle xrect = (Rectangle)x;
            Rectangle yrect = (Rectangle)y;
            int xarea = xrect.Width * xrect.Height;
            int yarea = yrect.Width * yrect.Height;
            if (xarea < yarea) return 1;
            if (xarea > yarea) return -1;
            if (xrect.Height < yrect.Height) return 1;
            if (xrect.Height > yrect.Height) return -1;
            if (xrect.Width < yrect.Width) return 1;
            if (xrect.Width > yrect.Width) return -1;
            return 0;
        }
    }

    public class SquarenessComparer : IComparer
    {
        // Return -1, 0, or 1 to indicate whether
        // x belongs before, the same as, or after y.
        // Sort by squareness, area, height, width descending.
        public int Compare(object x, object y)
        {
            Rectangle xrect = (Rectangle)x;
            Rectangle yrect = (Rectangle)y;
            int xsq = System.Math.Abs(xrect.Width - xrect.Height);
            int ysq = System.Math.Abs(yrect.Width - yrect.Height);
            if (xsq < ysq) return -1;
            if (xsq > ysq) return 1;
            int xarea = xrect.Width * xrect.Height;
            int yarea = yrect.Width * yrect.Height;
            if (xarea < yarea) return 1;
            if (xarea > yarea) return -1;
            if (xrect.Height < yrect.Height) return 1;
            if (xrect.Height > yrect.Height) return -1;
            if (xrect.Width < yrect.Width) return 1;
            if (xrect.Width > yrect.Width) return -1;
            return 0;
        }
    }
}