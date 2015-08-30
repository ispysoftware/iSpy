using System;
using AForge.Imaging;

namespace iSpyApplication.Controls
{

    public class FishEyeCorrect
    {
        private double[] _mFisheyeCorrect;
        private int _mFeLimit = -1; //1500;
        private double _mScaleFeSize; //0.9;
        private double _aFocalLinPixels;
        private int[,] _map;
        private int _w = -1, _h = -1;
        private int _offsetx, _offsety;


        private void Init(double aFocalLinPixels, int limit, double scale, int w, int h, int offsetx, int offsety)
        {
            _mFeLimit = limit;
            _mScaleFeSize = scale;
            _aFocalLinPixels = aFocalLinPixels;
            _w = w;
            _h = h;
            _offsetx = offsetx;
            _offsety = offsety;

            _mFisheyeCorrect = new double[_mFeLimit];
            for (int i = 0; i < _mFeLimit; i++)
            {
                double result = Math.Sqrt(1 - 1/Math.Sqrt(1.0 + (double) i*i/1000000.0))*1.4142136;
                _mFisheyeCorrect[i] = result;
            }
            _map = new int[w*h, 2];
            //center point

            int c = 0;
            for (var i = 0; i < w; i++)
            {
                for (var j = 0; j < h; j++)
                {
                    var xpos = i > offsetx;
                    var ypos = j > offsety;
                    var xdif = i - offsetx;
                    var ydif = j - offsety;

                    var rusquare = xdif*xdif + ydif*ydif;
                    var theta = Math.Atan2(ydif, xdif);
                    var index = (int) (Math.Sqrt(rusquare)/aFocalLinPixels*1000);
                    if (index >= _mFeLimit) index = _mFeLimit - 1;

                    var rd = aFocalLinPixels*_mFisheyeCorrect[index]/_mScaleFeSize;

                    var xdelta = Math.Abs(rd*Math.Cos(theta));
                    var ydelta = Math.Abs(rd*Math.Sin(theta));
                    var xd = (int) (offsetx + (xpos ? xdelta : -xdelta));
                    var yd = (int) (offsety + (ypos ? ydelta : -ydelta));
                    xd = Math.Max(0, Math.Min(xd, w - 1));
                    yd = Math.Max(0, Math.Min(yd, h - 1));
                    _map[c, 0] = xd;
                    _map[c, 1] = yd;
                    c++;

                }
            }


        }

        public void Correct(UnmanagedImage img, double aFocalLinPixels, int limit, double scale, int offx, int offy)
        {
            if (Math.Abs(_aFocalLinPixels - aFocalLinPixels) > Double.Epsilon || limit != _mFeLimit ||
                Math.Abs(scale - _mScaleFeSize) > Double.Epsilon || img.Width != _w || img.Height != _h ||
                _offsetx != offx || _offsety != offy)
            {
                Init(aFocalLinPixels, limit, scale, img.Width, img.Height, offx, offy);
            }
            var correctImage = UnmanagedImage.Create(img.Width, img.Height, img.PixelFormat);
            img.Copy(correctImage);
            int c = 0;
            for (int x = 0; x < _w; x++)
            {
                for (int y = 0; y < _h; y++)
                {
                    img.SetPixel(x, y, correctImage.GetPixel(_map[c, 0], _map[c, 1]));
                    c++;
                }

            }
            correctImage.Dispose();
        }
    }
}