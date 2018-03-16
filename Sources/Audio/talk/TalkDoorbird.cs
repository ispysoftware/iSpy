using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using iSpyApplication.Sources.Audio.codecs;
using iSpyApplication.Utilities;
using NAudio.Wave;

namespace iSpyApplication.Sources.Audio.talk
{
    internal class TalkDoorbird : ITalkTarget, IDisposable
    {
        private readonly object _obj = new object();
        private readonly int _port;
        private readonly string _server;
        private Stream _avstream;
        private readonly string _username;
        private readonly string _password;
        private bool _bTalking;
        private readonly WaveFormat _waveFormat = new WaveFormat(8000, 16, 1);
        private readonly IAudioSource _audioSource;
        private byte[] _talkBuffer = new byte[2500];
        private int _talkDatalen;
        //readonly byte[] _hdr = Encoding.ASCII.GetBytes("\r\n\r\n--myboundary\r\nContent-type: audio/basic\r\n\r\n");

        private AcmMuLawChatCodec _muLawCodec;

        public TalkDoorbird(string server, int port, string username, string password, IAudioSource audioSource)
        {
            _server = server;
            _port = port;
            _audioSource = audioSource;
            _username = username;
            _password = password;
        }

        public void Start()
        {
            try
            {
                StartTalk();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "TalkAxis");
                TalkStopped?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Stop()
        {
            StopTalk();
        }

        public event TalkStoppedEventHandler TalkStopped;
        private TcpClient _client;
        private void StartTalk()
        {
            if (_bTalking)
            {
                StopTalk();
            }
            if (_muLawCodec != null)
            {
                lock (_obj)
                {
                    _muLawCodec.Dispose();
                }
                _muLawCodec = null;
            }
            _muLawCodec = new AcmMuLawChatCodec();

            string sPost = "POST /bha-api/audio/transmit.cgi HTTP/1.0\r\n";
            //sPost += "Content-Type: multipart/x-mixed-replace; boundary=--myboundary\r\n";
            sPost += "Content-Type: audio/basic\r\n";
            sPost += "Content-Length: 9999999\r\n";
            sPost += "Connection: Keep-Alive\r\n";
            sPost += "Cache-Control: no-cache\r\n";

            string usernamePassword = _username + ":" + _password;
            sPost += "Authorization: Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(usernamePassword)) + "\r\n\r\n";

            _client = new TcpClient(_server, _port);
            _avstream = _client.GetStream();

            byte[] hdr = Encoding.ASCII.GetBytes(sPost);
            _avstream.Write(hdr, 0, hdr.Length);

            _audioSource.DataAvailable += AudioSourceDataAvailable;
            _talkBuffer = new byte[2500];
            _talkDatalen = 0;
            _bTalking = true;
        }

        public bool Connected => (_client != null);

        private void StopTalk()
        {
            if (_bTalking)
            {
                lock (_obj)
                {
                    if (_bTalking)
                    {
                        _bTalking = false;
                    }
                    if (_client != null)
                    {
                        _client.Close();
                        _client = null;
                    }

                    if (_avstream != null)
                    {
                        _avstream.Close();
                        _avstream.Dispose();
                        _avstream = null;
                    }

                    if (_muLawCodec != null)
                    {
                        _muLawCodec.Dispose();
                        _muLawCodec = null;
                    }

                    _audioSource.DataAvailable -= AudioSourceDataAvailable;
                    TalkStopped?.Invoke(this, EventArgs.Empty);
                }
            }
        }



        private void AudioSourceDataAvailable(object sender, DataAvailableEventArgs e)
        {

            try
            {
                lock (_obj)
                {
                    if (_bTalking && _avstream != null)
                    {
                        byte[] bSrc = e.RawData;
                        int totBytes = bSrc.Length;
                        int j = -1;
                        if (!_audioSource.RecordingFormat.Equals(_waveFormat))
                        {
                            var ws = new TalkHelperStream(bSrc, totBytes, _audioSource.RecordingFormat);

                            var bDst = new byte[44100];
                            totBytes = 0;
                            using (var helpStm = new WaveFormatConversionStream(_waveFormat, ws))
                            {
                                while (j != 0)
                                {
                                    j = helpStm.Read(bDst, totBytes, 10000);
                                    totBytes += j;
                                }
                            }
                            bSrc = bDst;

                        }
                        var enc = _muLawCodec.Encode(bSrc, 0, totBytes);
                        ALawEncoder.ALawEncode(bSrc, totBytes, enc);


                        Buffer.BlockCopy(enc, 0, _talkBuffer, _talkDatalen, enc.Length);
                        _talkDatalen += enc.Length;


                        j = 0;
                        try
                        {
                            while (j + 240 < _talkDatalen)
                            {
                                //need to write out in 240 byte packets
                                var pkt = new byte[240];
                                Buffer.BlockCopy(_talkBuffer, j, pkt, 0, 240);

                                // _avstream.Write(_hdr, 0, _hdr.Length);
                                _avstream.Write(pkt, 0, 240);
                                j += 240;
                            }
                            if (j < _talkDatalen)
                            {
                                Buffer.BlockCopy(_talkBuffer, j, _talkBuffer, 0, _talkDatalen - j);
                                _talkDatalen = _talkDatalen - j;
                            }
                        }
                        catch (SocketException)
                        {
                            StopTalk();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "TalkDoorbird");
                StopTalk();
            }
        }

        private bool _disposed;
        // Public implementation of Dispose pattern callable by consumers. 
        public void Dispose()
        {
            Dispose(true);
        }

        // Protected implementation of Dispose pattern. 
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _muLawCodec?.Dispose();
                _client?.Close();
            }

            // Free any unmanaged objects here. 
            //
            _disposed = true;
        }
    }
}