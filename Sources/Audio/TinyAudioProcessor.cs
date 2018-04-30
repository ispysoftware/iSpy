//    nVLC
//    
//    Author:  Roman Ginzburg
//
//    nVLC is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    nVLC is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//    GNU General Public License for more details.
//     
// ========================================================================

using System;
using System.Runtime.InteropServices;
using Declarations;
using Implementation;

namespace iSpyApplication.Sources.Audio
{
    class TinyAudioProcessor : DisposableBase
    {
        IntPtr m_audioBuffer = IntPtr.Zero;

        public TinyAudioProcessor(int samplesSize)
        {
            m_audioBuffer = Marshal.AllocHGlobal(samplesSize);
        }

        /// <summary>
        /// Converts 16 bit per sample stereo to 32 bits per sample
        /// </summary>
        /// <param name="sound"></param>
        /// <returns></returns>
        /// http://www.codeproject.com/Articles/501521/How-to-convert-between-most-audio-formats-in-NET
        public unsafe IntPtr ConvertToFloat32(Sound sound)
        {
            short* pInput = (short*)sound.SamplesData.ToPointer();
            float* pOutput = (float*)m_audioBuffer.ToPointer();

            for (int n = 0, i = 0; n < sound.Count; n++, i += 2)
            {
                pOutput[i] = pInput[i] / 32768f;
                pOutput[i + 1] = pInput[i + 1] / 32768f;
            }
            return m_audioBuffer;
        }

        /// <summary>
        /// Converts 16 bit per sample 6 (5.1) channel audio to 32 bits per sample stereo
        /// </summary>
        /// <param name="sound"></param>
        /// <returns></returns>
        /// http://stackoverflow.com/questions/14689998/remove-channels-from-pcm
        public unsafe IntPtr DownMixAndConvertToFloat32(Sound sound)
        {
            short* pInput = (short*)sound.SamplesData.ToPointer();
            float* pOutput = (float*)m_audioBuffer.ToPointer();

            short front_left = 0;
            short front_right = 0;
            short center = 0;
            short lfe = 0; // (sub-woofer)
            short back_left = 0;
            short back_right = 0;
            short left, right;

            for (int n = 0, i = 0, j = 0; n < sound.Count; n++, i += 6, j += 2)
            {
                front_left = pInput[i + 0];
                front_right = pInput[i + 1];
                center = pInput[i + 2];
                lfe = pInput[i + 3];
                back_left = pInput[i + 4];
                back_right = pInput[i + 5];

                left = (short)((front_left + back_left) / 2 + (lfe + center) / 4);
                right = (short)((front_right + back_right) / 2 + (lfe + center) / 4);

                pOutput[j] = left / 32768f;
                pOutput[j + 1] = right / 32768f;
            }
            return m_audioBuffer;
        }

        public unsafe double StandardDeviation(IntPtr samples, int count, int channels)
        {
            short* pBuffer = (short*)samples.ToPointer();
            double sum = 0;
            for (int i = 0; i < count; i++)
            {
                short mono = pBuffer[i * channels];
                sum += mono;
            }
            double mean = sum / count;

            double temp = 0;
            for (int i = 0; i < count; i++)
            {
                short mono = pBuffer[i * channels];
                temp += (mono - mean) * (mono - mean);
            }
            double variance = temp / count;
            return Math.Sqrt(variance);
        }

        protected override void Dispose(bool disposing)
        {
            if (m_audioBuffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(m_audioBuffer);
                m_audioBuffer = IntPtr.Zero;
            }
        }
    }
}
