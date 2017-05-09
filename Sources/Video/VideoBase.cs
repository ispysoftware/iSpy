using System;

namespace iSpyApplication.Sources.Video
{
    public class VideoBase
    {
        private readonly objectsCamera _source;
        public string Tokenise(string sourcestring)
        {
            if (_source == null)
                return sourcestring;

            var vss = _source.settings.videosourcestring;
            if (vss.IndexOf("[TOKEN]", StringComparison.Ordinal) != -1)
            {
                var t = new Tokeniser(vss, _source.settings.login, _source.settings.password, _source.settings.tokenconfig.tokenpath, _source.settings.tokenconfig.tokenport, _source.settings.tokenconfig.tokenpost);
                t.Populate();
                var url = vss.Replace("[TOKEN]", t.Token);
                //if (t.ReconnectInterval > 0)
                //    _source.settings.reconnectinterval = t.ReconnectInterval;
                return url;
            }
            return sourcestring;
        }

        public VideoBase(objectsCamera source)
        {
            _source = source;
        }
    }
}
