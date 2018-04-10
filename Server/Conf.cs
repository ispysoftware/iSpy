using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace iSpyApplication.Server
{
    public static class Conf
    {
        public static List<SourceItem> DeviceList = new List<SourceItem>
                                                    {
                                                        new SourceItem("LocalDevice", "LocalVideoSource", 2, 3),
                                                        new SourceItem("Desktop", "LocalVideoSource", 2, 4),
                                                        new SourceItem("KinectV1", "LocalVideoSource", 2, 7,false,true),
                                                        //cannot do this until 64 bit version of kinect driver is available
                                                        //new SourceItem("KinectV2","LocalVideoSource",2,11),
                                                        new SourceItem("XIMEA", "LocalVideoSource", 2, 6,false,true),
                                                        new SourceItem("Clone", "LocalVideoSource", 2, 10),
                                                        new SourceItem("JPEG", "NetworkVideoSource", 2, 0),
                                                        new SourceItem("MJPEG", "NetworkVideoSource", 2, 1),
                                                        new SourceItem("FFMPEG", "NetworkVideoSource", 2, 2),
                                                        new SourceItem("VLC", "NetworkVideoSource", 2, 5),
                                                        new SourceItem("ONVIF", "NetworkVideoSource", 2, 9),
                                                        new SourceItem("iSpyKinect", "NetworkVideoSource", 2, 8),
                                                        new SourceItem("iSpyServer", "NetworkVideoSource", 2, 12),
                                                        //end video
                                                        new SourceItem("LocalDevice", "LocalAudioSource", 1, 0),
                                                        new SourceItem("Camera", "LocalAudioSource", 1, 4, true),
                                                        new SourceItem("Clone", "LocalAudioSource", 1, 5),
                                                        new SourceItem("iSpyServer", "NetworkAudioSource", 1, 1),
                                                        new SourceItem("FFMPEG", "NetworkAudioSource", 1, 3),
                                                        new SourceItem("VLC", "NetworkAudioSource", 1, 2),
                                                        new SourceItem("WavStream", "NetworkAudioSource", 1, 6),
                                                        //end audio
                                                        new SourceItem("FloorPlan", "Other", 3, 0,false,true),
                                                        //new SourceItem("Sensor", "Other", 4, 0,false,true),
                                                        //new SourceItem("ScreenCapture", "VisioForge", 4, 0)
                                                    };
        public static object[] SampleRates = { 8000, 11025, 12000, 16000, 22050, 32000, 44100, 48000 };

        public static int GetSourceType(string source, int objectType)
        {
            source = source.ToUpperInvariant();

            var d = DeviceList.FirstOrDefault(p => p.Name.ToUpperInvariant() == source && p.ObjectTypeID == objectType);
            if (d == null)
                return 2;
            return d.SourceTypeID;
        }
    }
}
