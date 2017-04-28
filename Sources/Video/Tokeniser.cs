using System;
using System.IO;
using System.Net;
using iSpyApplication.Utilities;
using Newtonsoft.Json;

namespace iSpyApplication.Sources.Video
{
    public class Tokeniser
    {
        private string _post;
        private string _url;
        private readonly string _tokenPath;
        private readonly int _tokenPort;
        private readonly string _postData, _username, _password, _vurl;
        public int ReconnectInterval = -1;
        public string Token;

        public Tokeniser(string vurl, string username, string password, string tokenPath, int tokenPort, string postData)
        {
            _username = username;
            _password = password;
            _tokenPath = tokenPath;
            _postData = postData;
            _vurl = vurl;
            _tokenPort = tokenPort;
        }
        private readonly ConnectionFactory _connectionFactory = new ConnectionFactory();
        private void GetLoginToken()
        {
            Token = "";
            ReconnectInterval = 0;
            try
            {
                HttpWebRequest wreq;
                using (var wr = _connectionFactory.GetResponse(_url, "POST", _post, out wreq))
                {
                    if (wr == null)
                    {
                        Logger.LogError("Could not get login token", "Tokens");
                        return;
                    }
                    var stream = wr.GetResponseStream();
                    if (stream != null)
                    {
                        using (StreamReader streamReader = new StreamReader(stream, true))
                        {
                            try
                            {
                                var resp = streamReader.ReadToEnd().Trim();
                                dynamic d = JsonConvert.DeserializeObject(resp);
                                if (d[0].value.Token.name != null)
                                {
                                    Token = d[0].value.Token.name.ToString();
                                    ReconnectInterval = Convert.ToInt32(d[0].value.Token.leaseTime);
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.LogException(ex, "Tokens");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public void Populate()
        {
            string vss = _vurl;
            try
            {
                var uri = new Uri(vss);

                var url = "http://" + uri.Host +":"+ _tokenPort + "/ "+_tokenPath;
                var post = _postData;
                post = post.Replace("[USERNAME]", _username);
                post = post.Replace("[PASSWORD]", _password);
                _post = post;
                _url = url;
                GetLoginToken();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex,"Tokens");
            }
        }
        



    }
}
