using System;

namespace iSpyApplication.Sources.Audio.talk
{
    public static class TalkHelper
    {
        public static ITalkTarget GetTalkTarget(objectsCamera cam, IAudioSource source)
        {
            ITalkTarget talkTarget;
            switch (cam.settings.audiomodel)
            {
                default://local playback
                    talkTarget = new TalkLocal(source);
                    break;
                case "Foscam":
                    talkTarget = new TalkFoscam(cam.settings.audioip, cam.settings.audioport,
                        cam.settings.audiousername, cam.settings.audiopassword, source);
                    break;
                case "iSpyServer":
                    talkTarget = new TalkiSpyServer(cam.settings.audioip, cam.settings.audioport,
                        source);
                    break;
                case "NetworkKinect":
                    talkTarget = new TalkNetworkKinect(cam.settings.audioip, cam.settings.audioport,
                        source);
                    break;
                case "Axis":
                    talkTarget = new TalkAxis(cam.settings.audioip, cam.settings.audioport,
                        cam.settings.audiousername, cam.settings.audiopassword, source);
                    break;
                case "Doorbird":
                    talkTarget = new TalkDoorbird(cam.settings.audioip, cam.settings.audioport,
                        cam.settings.audiousername, cam.settings.audiopassword, source);
                    break;
                case "IP Webcam (Android)":
                    talkTarget = new TalkIPWebcamAndroid(new Uri(cam.settings.videosourcestring), source);
                    break;
                case "Amcrest":
                    talkTarget = new TalkAmcrest(cam.settings.audioip, cam.settings.audioport, source);
                    break;
            }
            return talkTarget;
        }
    }
}
