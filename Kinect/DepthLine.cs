using System;
using System.Drawing;

namespace iSpyApplication.Kinect
{
    public class DepthLine
    {
        public Point StartPoint;
        public Point EndPoint;
        public int DepthMin;
        public int DepthMax;
        private Point _summaryPoint = Point.Empty;
        private int _summaryWidth;
        public bool WidthChanged = true;

        public DepthLine(Point startPoint, Point endPoint)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
            DepthMin = 0;
            DepthMax = 20000;
        }
        public DepthLine(Point startPoint, Point endPoint, int depthMin, int depthMax)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
            DepthMin = depthMin;
            DepthMax = depthMax;
        }

        public Point SummaryPoint
        {
            get
            {
                if (_summaryPoint != Point.Empty)
                    return _summaryPoint;
                int x = StartPoint.X + (EndPoint.X - StartPoint.X) / 2;
                int y = StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 2;
                y -= 6;
                x -= SummaryWidth / 2;
                if (x < 0) x = 0;
                if (y > 465)
                    y = 465;
                if (y < 0)
                    y = 0;
                if (x + SummaryWidth > 640)
                    x = 640 - SummaryWidth;
                _summaryPoint = new Point(x, y);
                return _summaryPoint;
            }
        }

        public void RecalculateSummaryPoint()
        {
            _summaryPoint = Point.Empty;

        }

        public string SummaryText
        {
            get
            {
                var dMin = String.Format("{0:#.##}", Convert.ToDouble(DepthMin) / 1000);
                var dMax = String.Format("{0:#.##}", Convert.ToDouble(DepthMax) / 1000);
                if (dMin == "")
                    dMin = "0";
                if (dMax == "")
                    dMax = "0";

                return String.Format("{0}m - {1}m", dMin, dMax);
            }
        }

        public int SummaryWidth
        {
            get
            {
                if (_summaryWidth > 0)
                    return _summaryWidth;
                return 60;
            }
            set
            {
                _summaryWidth = value + 5;
                _summaryPoint = Point.Empty;
                WidthChanged = false;
            }
        }

        public Rectangle Rectangle
        {
            get
            {
                Point p = StartPoint;
                int w = EndPoint.X - p.X;
                int h = EndPoint.Y - p.Y;
                return new Rectangle(p.X, p.Y, w, h);
            }
        }
    }
}
