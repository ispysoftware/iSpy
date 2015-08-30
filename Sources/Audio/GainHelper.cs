namespace iSpyApplication.Sources.Audio
{
    public static class GainHelper
    {
        public static byte[] Gain(this byte[] arr, int gain)
        {
            if (gain == 100)
                return arr;

            var erg = new byte[arr.Length];

            for (int i = 0; i < arr.Length; i += 2)
            {
                var sample = (short)(arr[i] | (arr[i + 1] << 8));
                erg[i] = (byte)((sample * gain) & 0xff);
                erg[i + 1] = (byte)(((sample * gain) >> 8) & 0xff);
            }

            return erg;
        }
    }
}
