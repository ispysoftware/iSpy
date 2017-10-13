using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using FFmpeg.AutoGen;
using iSpyApplication.Utilities;

namespace iSpyApplication.Sources.Video
{
    abstract unsafe class FFmpegBase
    {
        internal FFmpegBase(string classType)
        {
            ClassType = classType;
        }

        public string ClassType;
        private string CheckError(string method, int code)
        {
            if (code < 0)
            {
                byte[] buff = new byte[255];
                GCHandle pinnedArray = GCHandle.Alloc(buff, GCHandleType.Pinned);
                IntPtr ptr = pinnedArray.AddrOfPinnedObject();
                ffmpeg.av_strerror(code, (byte*)ptr, 255);
                pinnedArray.Free();

                var s = Encoding.UTF8.GetString(buff, 0, buff.Length);
                int pos = s.IndexOf('\0');
                if (pos >= 0)
                    s = s.Substring(0, pos);
                return method + ": " + s;

            }
            return "";
        }

        internal string GetText(byte* ptr)
        {
            var buff = new byte[255];
            Marshal.Copy((IntPtr)ptr, buff, 0, 255);

            var s = Encoding.UTF8.GetString(buff, 0, buff.Length);
            int pos = s.IndexOf('\0');
            if (pos >= 0)
                s = s.Substring(0, pos);
            return s;
        }
        internal void Throw(string method, int code)
        {
            string err = CheckError(method, code);
            if (err != "")
            {
                throw new Exception(ClassType + ": " + err);
            }
        }

        private readonly int[] _nologcodes = { ffmpeg.AVERROR_EOF }; //end of file (playback)
        internal bool Log(string method, int code)
        {
            string err = CheckError(method, code);
            if (err != "")
            {
                if (!_nologcodes.Contains(code))
                    LogError(err);
                return true;
            }
            return false;
        }

        internal void LogError(string err)
        {
            Logger.LogError(err, ClassType);
        }
    }
}
