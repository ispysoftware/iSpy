using System;
using System.IO;
using System.Speech.Synthesis;
using System.Threading;
using iSpyApplication.Controls;
using iSpyApplication.Sources.Audio.streams;
using iSpyApplication.Sources.Audio.talk;
using NAudio.Wave;

namespace iSpyApplication.Sources.Audio
{
    static class SpeechSynth
    {
        public static void Say(string text,CameraWindow cw)
        {
            var t = new Thread(() => SynthToCam(Uri.UnescapeDataString(text), cw));
            t.Start();
        }
        private static void SynthToCam(string text, CameraWindow cw)
        {
            var synthFormat = new System.Speech.AudioFormat.SpeechAudioFormatInfo(System.Speech.AudioFormat.EncodingFormat.Pcm, 11025, 16, 1, 22100, 2, null);
            using (var synthesizer = new SpeechSynthesizer())
            {
                using (var waveStream = new MemoryStream())
                {

                    //write some silence to the stream to allow camera to initialise properly
                    var silence = new byte[1 * 22050];
                    waveStream.Write(silence, 0, silence.Length);

                    var pbuilder = new PromptBuilder();
                    var pStyle = new PromptStyle
                    {
                        Emphasis = PromptEmphasis.Strong,
                        Rate = PromptRate.Slow,
                        Volume = PromptVolume.ExtraLoud
                    };

                    pbuilder.StartStyle(pStyle);
                    pbuilder.StartParagraph();
                    pbuilder.StartVoice(VoiceGender.Male, VoiceAge.Adult, 2);
                    pbuilder.StartSentence();
                    pbuilder.AppendText(text);
                    pbuilder.EndSentence();
                    pbuilder.EndVoice();
                    pbuilder.EndParagraph();
                    pbuilder.EndStyle();

                    synthesizer.SetOutputToAudioStream(waveStream, synthFormat);
                    synthesizer.Speak(pbuilder);
                    synthesizer.SetOutputToNull();

                    //write some silence to the stream to allow camera to end properly
                    waveStream.Write(silence, 0, silence.Length);
                    waveStream.Seek(0, SeekOrigin.Begin);

                    var ds = new DirectStream(waveStream) { RecordingFormat = new WaveFormat(11025, 16, 1) };
                    var talkTarget = TalkHelper.GetTalkTarget(cw.Camobject, ds); 
                    ds.Start();
                    talkTarget.Start();
                    while (ds.IsRunning)
                    {
                        Thread.Sleep(100);
                    }
                    ds.Stop();
                    talkTarget.Stop();
                    talkTarget = null;
                    ds = null;
                }
            }


        }
    }
}
