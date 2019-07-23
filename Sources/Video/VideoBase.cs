using System;
using System.Collections.Generic;
using System.Linq;
using iSpyApplication.Controls;

namespace iSpyApplication.Sources.Video
{
    internal class VideoBase: FFmpegBase
    {
        internal readonly CameraWindow _cw;
        public string Tokenise(string sourcestring)
        {
            if (_cw == null)
                return sourcestring;

            var vss = _cw.Camobject.settings.videosourcestring;
            if (vss.IndexOf("[TOKEN]", StringComparison.Ordinal) != -1)
            {
                var t = new Tokeniser(vss, _cw.Camobject.settings.login, _cw.Camobject.settings.password, _cw.Camobject.settings.tokenconfig.tokenpath, _cw.Camobject.settings.tokenconfig.tokenport, _cw.Camobject.settings.tokenconfig.tokenpost);
                t.Populate();
                var url = vss.Replace("[TOKEN]", t.Token);
                //if (t.ReconnectInterval > 0)
                //    _source.settings.reconnectinterval = t.ReconnectInterval;
                return url;
            }
            return sourcestring;
        }

        public VideoBase(CameraWindow cw):base("FFMPEG")
        {
            _cw = cw;
        }

        public double RealFramerate = 0;
        private readonly List<DateTime> _timeStamps = new List<DateTime>();
        private readonly List<DateTime> _emittimeStamps = new List<DateTime>();
        private const int TimeStampSampleDuration = 3000;
        private long _frameCount = 0;
        private double _refCount = 0;
        public bool ShouldEmitFrame
        {
            get
            {
                if (_cw == null)
                    return true;
                if (_timeStamps.Count > 0)
                {
                    if ((DateTime.UtcNow - _timeStamps.Last()).TotalMilliseconds > 2000)
                    {
                        _timeStamps.Clear();
                        _emittimeStamps.Clear();
                        _frameCount = 0;
                        _refCount = 0;
                    }
                }

                _timeStamps.Add(DateTime.UtcNow);

                var avgint = (_timeStamps.Last() - _timeStamps.First()).TotalMilliseconds / _timeStamps.Count;
                if (avgint > 0)
                    RealFramerate = 1000d / avgint;
                else
                {
                    RealFramerate = 1;
                }

                FilterTimestamps(_timeStamps);
                if (RealFramerate < TargetFrameRate)
                    return true;

                var tfr = TargetFrameRate;

                if (tfr > 0)
                {
                    _frameCount++;
                    var r = RealFramerate / tfr;
                    _refCount = Math.Max(_refCount, r);

                    if (_frameCount < _refCount)
                    {
                        return false;
                    }
                    _refCount += r;
                    _emittimeStamps.Add(DateTime.UtcNow);
                    FilterTimestamps(_emittimeStamps);
                    return true;
                }


                return false;
            }
        }

        private void FilterTimestamps(List<DateTime> timestamps)
        {
            while (timestamps.Count > 1 && (timestamps.Last() - timestamps.First()).TotalMilliseconds > TimeStampSampleDuration)
                timestamps.RemoveAt(0);
        }

        public double TargetFrameRate
        {
            get
            {
                if (_cw == null)
                    return 5;

                double r = Convert.ToDouble(_cw.Camobject.settings.maxframerate);
                if (_cw.Recording)
                    r = Convert.ToDouble(_cw.Camobject.settings.maxframeraterecord);

                r = ThrottleFramerate(r);

                return r;

            }
        }

        //public int FrameInterval
        //{
        //    get
        //    {
        //        if (_cw == null)
        //            return 200;

        //        decimal r = _cw.Camobject.settings.maxframerate;
        //        if (_cw.Recording)
        //            r = _cw.Camobject.settings.maxframeraterecord;

        //        r = Math.Max(0.01m,Math.Min(r, MainForm.ThrottleFramerate));
        //        return Convert.ToInt32(1000m/r);

        //    }
        //}

        private int _throttleAdjust;
        private DateTime _lastFrAdjust;
        public double ThrottleFramerate(double rate)
        {
            if (_lastFrAdjust < DateTime.UtcNow.AddSeconds(-1))
            {
                if (MainForm.HighCPU)
                {
                    if (_throttleAdjust < rate)
                        _throttleAdjust++;
                }
                else
                {
                    if (_throttleAdjust > 0)
                        _throttleAdjust--;

                }

                _lastFrAdjust = DateTime.UtcNow;
            }

            return Math.Max(rate - _throttleAdjust, 1);
        }

    }
}
