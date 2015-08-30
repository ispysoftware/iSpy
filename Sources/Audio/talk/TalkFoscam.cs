using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using iSpy.Video.FFMPEG;
using NAudio.Wave;

namespace iSpyApplication.Sources.Audio.talk
{
    internal class TalkFoscam: ITalkTarget, IDisposable
    {
        private const string MoIPOprFlag = "MO_O";
        private const string MoIPAvFlag = "MO_V";
        private const int OprSpeakStartNotify = 0xb;
        private const int OprSpeakEnd = 0xd;
        private const int OprParamsFetchReq = 0x10;
        private const int AvLoginReq = 0x0;
        private const int TalkData = 0x3;
        private const int TalkBufferSize = 25000;
        private readonly DateTime _dt = DateTime.UtcNow.AddHours(0 - DateTime.UtcNow.Hour);
        private readonly IMA_ADPCM _enc = new IMA_ADPCM();
        private readonly object _obj = new object();
        private readonly int _port;
        private readonly string _server;
        private byte[] _talkBuffer = new byte[TalkBufferSize];
        private const int OprKeepAlive = 0xFF;
        private const int OprLoginReq  = 0;
        private const int OprVerifyReq = 0x2;
        private NetworkStream _avstream;
        private bool _needsencodeinit;
        private NetworkStream _stream;
        private bool _bTalking;
        private readonly string _password;
        private int _seq = 10000;
        private int _talkDatalen;
        private readonly string _username;
        private readonly WaveFormat _waveFormat = new WaveFormat(8000, 16, 1);
        private readonly IAudioSource _audioSource;
        private ManualResetEvent _stopEvent;

        #region stream_stuff

        private const int HeadLength = 23;
        private int _mIBufferLen;
        private int _mILength;
        private int _mPPos;


        private static void Encode(ref byte[] cmd)
        {
            cmd[15] = cmd[19];
            cmd[16] = cmd[20];
            cmd[17] = cmd[21];
            cmd[18] = cmd[22];
        }

        private byte[] SInit(int op, IEnumerable<char> flag)
        {
            _mILength = 0;
            _mIBufferLen = 100;
            _mILength = HeadLength;
            _mPPos = HeadLength;

            var b = new byte[_mIBufferLen];
            int ipos = 0;
            foreach (var c in flag)
            {
                b[ipos] = (byte) c;
                ipos++;
            }
            b[ipos] = (byte) (op & 0x0000000F);
            ipos++;
            b[ipos] = (byte) (op & 0x000000F0);
            ipos++;

            while (ipos < 23)
            {
                b[ipos] = 0x00;
                ipos++;
            }
            return b;
        }

        private byte[] AddNext(byte[] cmd, string data, int padnull)
        {
            while (data.Length < padnull)
                data += "\0";
            return AddNext(cmd, data);
        }

        private byte[] AddNext(byte[] cmd, byte data)
        {
            return AddNext(cmd, new[] {data});
        }

        private byte[] AddNext(byte[] cmd, string data)
        {
            return AddNext(cmd, Encoding.ASCII.GetBytes(data));
        }

        private byte[] AddNext(byte[] cmd, Int32 data)
        {
            return AddNext(cmd, BitConverter.GetBytes(data));
        }

        private byte[] AddNext(byte[] cmd, byte[] newdata)
        {
            return AddNext(cmd, newdata, newdata.Length);
        }

        private byte[] AddNext(byte[] cmd, byte[] newdata, int len)
        {
            if (len + _mILength > _mIBufferLen)
            {
                int iBufferLen = _mIBufferLen;
                while (len + _mILength > iBufferLen)
                    iBufferLen = iBufferLen + 100;

                var pBuffer = new byte[iBufferLen];
                cmd.CopyTo(pBuffer, 0);

                cmd = pBuffer;
                _mPPos = _mILength;
                _mIBufferLen = iBufferLen;
            }

            for (int i = 0; i < len; i++)
                cmd[i + _mPPos] = newdata[i];

            _mPPos = _mPPos + len;
            _mILength = _mILength + len;
            int iTotalDataLen = _mILength - HeadLength;

            byte[] t = BitConverter.GetBytes(iTotalDataLen);
            t.CopyTo(cmd, 19);

            return cmd;
        }

        private void Send(byte[] cmd)
        {

            Encode(ref cmd);
            try
            {
                _stream.Write(cmd, 0, _mILength);
            }
            catch (Exception)
            {
                StopTalk(true);
            }
        }

        private void SendAv(byte[] cmd)
        {
            Encode(ref cmd);
            lock (_obj)
            {
                _avstream.Write(cmd, 0, _mILength);
            }
        }

        #endregion

        public TalkFoscam(string server, int port, string username, string password, IAudioSource audioSource)
        {
            _server = server;
            _port = port;
            _username = username;
            _password = password;
            _audioSource = audioSource;
        }

        public void Start()
        {
            _stopEvent = new ManualResetEvent(false);
            
            var t = new Thread(CommsThread);
            t.Start();
        }
        
        public void Stop()
        {
            StopTalk(false);
        }

        public event TalkStoppedEventHandler TalkStopped;
        
        private void StartTalk()
        {
            if (_bTalking)
            {
                StopTalk(false);
            }
            
            byte[] cmd = SInit(OprSpeakStartNotify, MoIPOprFlag);
            cmd = AddNext(cmd, (byte) 0x1);
            Send(cmd);

            _audioSource.DataAvailable += AudioSourceDataAvailable;
            _needsencodeinit = true;
            _talkDatalen = 0;
            _talkBuffer = new byte[TalkBufferSize];
        }

        private void StopTalk(bool becauseOfException)
        {
            if (_bTalking)
            {
                lock (_obj)
                {
                    _stopEvent.Set();
                    _audioSource.DataAvailable -= AudioSourceDataAvailable;

                    if (!becauseOfException)
                    {
                        byte[] cmd = SInit(OprSpeakEnd, MoIPOprFlag);
                        Send(cmd);
                    }

                    if (_avstream != null)
                    {
                        _avstream.Close();
                        _avstream.Dispose();
                        _avstream = null;
                    }

                    if (_bTalking)
                    {
                        _bTalking = false;
                        _talkDatalen = 0;
                    }
                    TalkStopped?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        
        public bool Connected => (_avstream != null);

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

                        if (_needsencodeinit)
                        {
                            _enc.EncodeInit(BitConverter.ToInt16(e.RawData, 0), BitConverter.ToInt16(e.RawData, 2));
                            _needsencodeinit = false;
                        }

                        var buff = new byte[25000];
                        int c;
                        unsafe
                        {
                            fixed (byte* src = bSrc)
                            {
                                fixed (byte* dst = buff)
                                {
                                    c = (int)_enc.EncodeFoscam(src, totBytes, dst);
                                }
                            }
                        }
                        Buffer.BlockCopy(buff,0,_talkBuffer,_talkDatalen,c);
                        _talkDatalen += c;

                        var dtms = (int) (DateTime.UtcNow - _dt).TotalMilliseconds;
                        int i = 0;
                        j = 0;
                        try
                        {
                            while (j + 160 < _talkDatalen)
                            {
                                //need to write out in 160 byte packets for 40ms
                                byte[] cmd = SInit(TalkData, MoIPAvFlag);

                                cmd = AddNext(cmd, dtms + (i*40));
                                cmd = AddNext(cmd, _seq);
                                cmd = AddNext(cmd, (int) (DateTime.UtcNow - _dt).TotalSeconds);
                                cmd = AddNext(cmd, (byte) 0x0);
                                cmd = AddNext(cmd, 160);

                                var pkt = new byte[160];
                                Buffer.BlockCopy(_talkBuffer, j, pkt, 0, 160);
                                cmd = AddNext(cmd, pkt, 160);
                                Encode(ref cmd);

                                _avstream.Write(cmd, 0, cmd.Length);
                                j += 160;
                                _seq++;
                                i++;
                            }
                            if (j < _talkDatalen)
                            {
                                Buffer.BlockCopy(_talkBuffer, j, _talkBuffer, 0, _talkDatalen-j);
                                _talkDatalen = _talkDatalen - j;
                            }
                        }
                        catch (SocketException)
                        {
                            StopTalk(true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex,"TalkFoscam");
                StopTalk(true);
            }
        }

        #region CommsThread
        private void CommsThread()
        {
            _needsencodeinit = true;
            try
            {
                var tcp = new TcpClient(_server, _port);
                _stream = tcp.GetStream();
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex, "TalkFoscam");
                return;
            }
           

            byte[] cmd = SInit(OprLoginReq, MoIPOprFlag);
            Send(cmd);

            DateTime dtref = DateTime.UtcNow;

            bool bConnected = false;

            try
            {
                while (true)
                {
                    if (_bTalking)
                        Thread.Sleep(100);
                    if (bConnected)
                    {
                        if (DateTime.UtcNow - dtref > TimeSpan.FromSeconds(120))
                        {
                            byte[] ka = SInit(OprKeepAlive, MoIPOprFlag);
                            Send(ka);
                            dtref = DateTime.UtcNow;
                        }
                    }
                    if (_stream.DataAvailable)
                    {
                        var data = new byte[256];
                        int iReceive = _stream.Read(data, 0, data.Length);

                        if (iReceive > 5)
                        {
                            string r = "";
                            for (int j = 0; j < 5; j++)
                            {
                                r += (char) data[j];
                            }

                            if (r.StartsWith(MoIPOprFlag) && data.Length >= 23)
                            {
                                string code = (data[4] + data[5]).ToString(CultureInfo.InvariantCulture);

                                switch (code)
                                {
                                    case "1": //OPR_LOGIN_RESP
                                        //login required
                                        cmd = SInit(OprVerifyReq, MoIPOprFlag);
                                        cmd = AddNext(cmd, _username, 13);
                                        cmd = AddNext(cmd, _password, 13);
                                        Send(cmd);
                                        break;
                                    case "3": //OPR_VERIFY_RESP
                                        if (data[25] == 0x2)
                                            bConnected = true;
                                        else
                                        {
                                            MainForm.LogErrorToFile("Login to foscam camera failed", "TalkFoscam");
                                            _stopEvent.Set();
                                            break;
                                        }

                                        cmd = SInit(OprParamsFetchReq, MoIPOprFlag);
                                        Send(cmd);

                                        // start talking
                                        StartTalk();
                                        break;
                                    case "12": //ON_START_SPEAK_RESP
                                        int iDataLength = BitConverter.ToInt32(data, 15);
                                        if (iDataLength == 0)
                                        {
                                            MainForm.LogErrorToFile("Foscam start speak request failed", "TalkFoscam");
                                            _stopEvent.Set();
                                            break;
                                        }
                                        var resp = new byte[iDataLength];
                                        for (int j = 0; j < iDataLength; j++)
                                            resp[j] = data[HeadLength + j];

                                        int err = BitConverter.ToInt16(resp, 0);
                                        if (err != 0)
                                        {
                                            MainForm.LogErrorToFile("Foscam AV Port request failed", "TalkFoscam");
                                            _stopEvent.Set();
                                            break;
                                        }

                                        int dwAvConnId = BitConverter.ToInt32(resp, 2);

                                        var av = new TcpClient(_server, _port) {NoDelay = true};
                                        _avstream = av.GetStream();

                                        cmd = SInit(AvLoginReq, MoIPAvFlag);
                                        cmd = AddNext(cmd, dwAvConnId);
                                        SendAv(cmd);

                                        _bTalking = true;
                                        _talkDatalen = 0;
                                        break;
                                    case "255": //OPR_KEEP_ALIVE
                                        break;
                                }
                            }
                        }
                    }
                    if (_stopEvent.WaitOne(0, true))
                        break;
                }
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex, "TalkFoscam");
            }
            TalkStopped?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        private bool _disposed;
        // Public implementation of Dispose pattern callable by consumers. 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern. 
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _stopEvent?.Close();
            }

            // Free any unmanaged objects here. 
            //
            _disposed = true;
        }
    }
}