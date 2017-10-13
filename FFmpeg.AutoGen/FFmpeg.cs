using System;

namespace FFmpeg.AutoGen
{
    public static partial class ffmpeg
    {
        public static int MKTAG<T1, T2, T3, T4>(T1 a, T2 b, T3 c, T4 d)
            => (int) (Convert.ToUInt32(a) | (Convert.ToUInt32(b) << 8) | (Convert.ToUInt32(c) << 16) | (Convert.ToUInt32(d) << 24));

        public static int FFERRTAG<T1, T2, T3, T4>(T1 a, T2 b, T3 c, T4 d)
            => -MKTAG(a, b, c, d);

        public static ulong UINT64_C<T>(T a)
            => Convert.ToUInt64(a);
    }
}