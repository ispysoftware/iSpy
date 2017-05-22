using System;
using iSpyApplication.Controls;

namespace iSpyApplication.Sources.Video
{
    public class VideoBase
    {
        private readonly CameraWindow _cw;
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

        public VideoBase(CameraWindow cw)
        {
            _cw = cw;
        }

        private DateTime _lastFrame = DateTime.MinValue;
        public bool EmitFrame
        {
            get
            {
                if (_cw == null)
                    return true;

                if ((DateTime.UtcNow - _lastFrame).TotalMilliseconds <= FrameInterval)
                {
                    return false;
                }
                _lastFrame = DateTime.UtcNow;
                return true;
            }
        }

        public int FrameInterval
        {
            get
            {
                if (_cw == null)
                    return 200;

                int r = _cw.Camobject.settings.maxframerate;
                if (_cw.Recording)
                    r = _cw.Camobject.settings.maxframeraterecord;

                r = Math.Max(1,Math.Min(r, MainForm.ThrottleFramerate));
                return 1000/r;

            }
        }

    }
}
