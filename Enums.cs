using System;

namespace iSpyApplication
{
    public class Enums
    {
        public enum LayoutMode
        {
            Freeform,
            AutoGrid
        };
        public enum AlertMode { Movement, NoMovement };

        public enum PtzCommand
        {
            Center,
            Left,
            UpLeft,
            Up,
            UpRight,
            Right,
            DownRight,
            Down,
            DownLeft,
            ZoomIn,
            ZoomOut,
            Stop
        } ;

        public enum PlaybackMode
        {
            Website=0,iSpy,Default
        }

        public enum MatchMode
        {
            IsInList = 0,
            NotInList = 1
        } ;

        public enum FrameType {Video,Audio}

        public enum AudioStreamMode
        {
            PCM,
            MP3
            //,M4A
        }

        [Flags]
        public enum Features: long
        {  
            ALL_FEATURES =      1,
            IPCameras =         2,
            Microphones =       4,           
            Floorplans =        8,
            Source_JPEG =       16,
            Source_MJPEG =      32,
            Source_FFmpeg =     64, 
            Source_Local =      128,
            Source_Desktop =    256,
            Source_VLC =        512,
            Source_Ximea =      1024,
            Source_Kinect =     2048,
            Source_Custom =     4096,
            Access_Media =      8192,
            Remote_Commands =   16384,
            Web_Settings =      32768,
            Plugins =           65536,
            Motion_Detection =  131072,
            Recording=          262144,
            PTZ  =              524288,
            Save_Frames =       1048576,
            Cloud =             2097152,
            Scheduling =        4194304,
            Alerts =            8388608,
            Source_ONVIF =      16777216,
            Source_Clone =      33554432,
            Storage =           67108864,
            Settings =          134217728,
            Grid_Views =        268435456,
            Logs =              536870912,
            Edit =              1073741824,
            View_File_Menu =    8589934592,
            View_Ispy_Links =   17179869184,
            View_Status_Bar =   34359738368,
            View_Layout_Options = 68719476736,
            Open_Web_Interface  = 137438953472,
            View_Media          = 274877906944,
            View_Media_on_mobile = 549755813888,
            View_Files =           1099511627776,
            High_Level_User =      2199023255552
           
        }

    }
}
