using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using iSpyApplication.Utilities;

namespace iSpyApplication
{
    public class McRemoteControlManager : NativeWindow
    {
        public enum InputDevice
        {
            Key,
            Mouse,
            OEM
        }

        public enum RemoteControlButton
        {
            Clear,
            Down,
            Left,
            Digit0,
            Digit1,
            Digit2,
            Digit3,
            Digit4,
            Digit5,
            Digit6,
            Digit7,
            Digit8,
            Digit9,
            Enter,
            Right,
            Up,

            Back,
            ChannelDown,
            ChannelUp,
            FastForward,
            VolumeMute,
            Pause,
            Play,
            Record,
            PreviousTrack,
            Rewind,
            NextTrack,
            Stop,
            VolumeDown,
            VolumeUp,
            RecordedTV,
            Guide,
            LiveTV,
            Details,
            DVDMenu,
            DVDAngle,
            DVDAudio,
            DVDSubtitle,
            MyMusic,
            MyPictures,
            MyVideos,
            MyTV,
            OEM1,
            OEM2,
            StandBy,
            TVJump,

            Unknown
        }


        #region RemoteControlEventArgs

        public class RemoteControlEventArgs : EventArgs
        {
            RemoteControlButton _rcb;
            InputDevice _device;

            public RemoteControlEventArgs(RemoteControlButton rcb, InputDevice device)
            {
                _rcb = rcb;
                _device = device;
            }


            public RemoteControlEventArgs()
            {
                _rcb = RemoteControlButton.Unknown;
                _device = InputDevice.Key;
            }


            public RemoteControlButton Button
            {
                get { return _rcb; }
                set { _rcb = value; }
            }

            public InputDevice Device
            {
                get { return _device; }
                set { _device = value; }
            }
        }

        #endregion


        public sealed class RemoteControlDevice : NativeWindow
        {

            [StructLayout(LayoutKind.Sequential)]
            internal struct RAWINPUTDEVICE
            {
                [MarshalAs(UnmanagedType.U2)]
                public ushort usUsagePage;
                [MarshalAs(UnmanagedType.U2)]
                public ushort usUsage;
                [MarshalAs(UnmanagedType.U4)]
                public int dwFlags;
                public IntPtr hwndTarget;
            }


            [StructLayout(LayoutKind.Sequential)]
            internal struct RAWINPUTHEADER
            {
                [MarshalAs(UnmanagedType.U4)]
                public int dwType;
                [MarshalAs(UnmanagedType.U4)]
                public int dwSize;
                public IntPtr hDevice;
                [MarshalAs(UnmanagedType.U4)]
                public int wParam;
            }


            [StructLayout(LayoutKind.Sequential)]
            internal struct RAWHID
            {
                [MarshalAs(UnmanagedType.U4)]
                public int dwSizHid;
                [MarshalAs(UnmanagedType.U4)]
                public int dwCount;
            }


            [StructLayout(LayoutKind.Sequential)]
            internal struct BUTTONSSTR
            {
                [MarshalAs(UnmanagedType.U2)]
                public ushort usButtonFlags;
                [MarshalAs(UnmanagedType.U2)]
                public ushort usButtonData;
            }


            [StructLayout(LayoutKind.Explicit)]
            internal struct RAWMOUSE
            {
                [MarshalAs(UnmanagedType.U2)]
                [FieldOffset(0)]
                public ushort usFlags;
                [MarshalAs(UnmanagedType.U4)]
                [FieldOffset(4)]
                public uint ulButtons;
                [FieldOffset(4)]
                public BUTTONSSTR buttonsStr;
                [MarshalAs(UnmanagedType.U2)]
                [FieldOffset(8)]
                public uint ulRawButtons;
                [FieldOffset(12)]
                public int lLastX;
                [FieldOffset(16)]
                public int lLastY;
                [MarshalAs(UnmanagedType.U2)]
                [FieldOffset(20)]
                public uint ulExtraInformation;
            }

            [StructLayout(LayoutKind.Sequential)]
            internal struct RAWKEYBOARD
            {
                [MarshalAs(UnmanagedType.U2)]
                public ushort MakeCode;
                [MarshalAs(UnmanagedType.U2)]
                public ushort Flags;
                [MarshalAs(UnmanagedType.U2)]
                public ushort Reserved;
                [MarshalAs(UnmanagedType.U2)]
                public ushort VKey;
                [MarshalAs(UnmanagedType.U4)]
                public uint Message;
                [MarshalAs(UnmanagedType.U4)]
                public uint ExtraInformation;
            }


            [StructLayout(LayoutKind.Explicit)]
            internal struct RAWINPUT
            {
                [FieldOffset(0)]
                public RAWINPUTHEADER header;
                [FieldOffset(16)]
                public RAWMOUSE mouse;
                [FieldOffset(16)]
                public RAWKEYBOARD keyboard;
                [FieldOffset(16)]
                public RAWHID hid;
            }


            [DllImport("User32.dll")]
            extern static bool RegisterRawInputDevices(RAWINPUTDEVICE[] pRawInputDevice, uint uiNumDevices, uint cbSize);

            [DllImport("User32.dll")]
            extern static uint GetRawInputData(IntPtr hRawInput, uint uiCommand, IntPtr pData, ref uint pcbSize, uint cbSizeHeader);


            private const int WM_KEYDOWN = 0x0100;
            private const int WM_APPCOMMAND = 0x0319;
            private const int WM_INPUT = 0x00FF;

            private const int APPCOMMAND_BROWSER_BACKWARD = 1;
            private const int APPCOMMAND_VOLUME_MUTE = 8;
            private const int APPCOMMAND_VOLUME_DOWN = 9;
            private const int APPCOMMAND_VOLUME_UP = 10;
            private const int APPCOMMAND_MEDIA_NEXTTRACK = 11;
            private const int APPCOMMAND_MEDIA_PREVIOUSTRACK = 12;
            private const int APPCOMMAND_MEDIA_STOP = 13;
            private const int APPCOMMAND_MEDIA_PLAY_PAUSE = 14;
            private const int APPCOMMAND_MEDIA_PLAY = 46;
            private const int APPCOMMAND_MEDIA_PAUSE = 47;
            private const int APPCOMMAND_MEDIA_RECORD = 48;
            private const int APPCOMMAND_MEDIA_FAST_FORWARD = 49;
            private const int APPCOMMAND_MEDIA_REWIND = 50;
            private const int APPCOMMAND_MEDIA_CHANNEL_UP = 51;
            private const int APPCOMMAND_MEDIA_CHANNEL_DOWN = 52;

            private const int RAWINPUT_DETAILS = 0x209;
            private const int RAWINPUT_GUIDE = 0x8D;
            private const int RAWINPUT_TVJUMP = 0x25;
            private const int RAWINPUT_STANDBY = 0x82;
            private const int RAWINPUT_OEM1 = 0x80;
            private const int RAWINPUT_OEM2 = 0x81;
            private const int RAWINPUT_MYTV = 0x46;
            private const int RAWINPUT_MYVIDEOS = 0x4A;
            private const int RAWINPUT_MYPICTURES = 0x49;
            private const int RAWINPUT_MYMUSIC = 0x47;
            private const int RAWINPUT_RECORDEDTV = 0x48;
            private const int RAWINPUT_DVDANGLE = 0x4B;
            private const int RAWINPUT_DVDAUDIO = 0x4C;
            private const int RAWINPUT_DVDMENU = 0x24;
            private const int RAWINPUT_DVDSUBTITLE = 0x4D;

            private const int RIM_TYPEMOUSE = 0;
            private const int RIM_TYPEKEYBOARD = 1;
            private const int RIM_TYPEHID = 2;

            private const int RID_INPUT = 0x10000003;
            private const int RID_HEADER = 0x10000005;

            private const int FAPPCOMMAND_MASK = 0xF000;
            private const int FAPPCOMMAND_MOUSE = 0x8000;
            private const int FAPPCOMMAND_KEY = 0;
            private const int FAPPCOMMAND_OEM = 0x1000;

            public delegate void RemoteControlDeviceEventHandler(object sender, RemoteControlEventArgs e);
            public event RemoteControlDeviceEventHandler ButtonPressed;


            //-------------------------------------------------------------
            // constructors
            //-------------------------------------------------------------

            public RemoteControlDevice()
            {
                // Register the input device to receive the commands from the 
                // remote device. See http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnwmt/html/remote_control.asp
                // for the vendor defined usage page.

                var rid = new RAWINPUTDEVICE[3];

                rid[0].usUsagePage = 0xFFBC;
                rid[0].usUsage = 0x88;
                rid[0].dwFlags = 0;
                rid[0].hwndTarget = this.Handle;

                rid[1].usUsagePage = 0x0C;
                rid[1].usUsage = 0x01;
                rid[1].dwFlags = 0;
                rid[1].hwndTarget = this.Handle;

                rid[2].usUsagePage = 0x0C;
                rid[2].usUsage = 0x80;
                rid[2].dwFlags = 0;
                rid[2].hwndTarget = this.Handle;

                if (!RegisterRawInputDevices(rid,
                    (uint)rid.Length,
                    (uint)Marshal.SizeOf(rid[0]))
                    )
                {
                    Logger.LogMessage("Failed to register raw input devices.");
                }
            }


            //-------------------------------------------------------------
            // methods
            //-------------------------------------------------------------

            public void ProcessMessage(Message message)
            {
                int param;

                switch (message.Msg)
                {
                    case WM_KEYDOWN:
                        param = message.WParam.ToInt32();
                        ProcessKeyDown(param);
                        break;
                    case WM_APPCOMMAND:
                        param = message.LParam.ToInt32();
                        ProcessAppCommand(param);
                        break;
                    case WM_INPUT:
                        ProcessInputCommand(ref message);
                        break;
                }

            }


            //-------------------------------------------------------------
            // methods (helpers)
            //-------------------------------------------------------------

            private void ProcessKeyDown(int param)
            {
                RemoteControlButton rcb = RemoteControlButton.Unknown;

                switch (param)
                {
                    case (int)Keys.Escape:
                        rcb = RemoteControlButton.Clear;
                        break;
                    case (int)Keys.Down:
                        rcb = RemoteControlButton.Down;
                        break;
                    case (int)Keys.Left:
                        rcb = RemoteControlButton.Left;
                        break;
                    case (int)Keys.D0:
                        rcb = RemoteControlButton.Digit0;
                        break;
                    case (int)Keys.D1:
                        rcb = RemoteControlButton.Digit1;
                        break;
                    case (int)Keys.D2:
                        rcb = RemoteControlButton.Digit2;
                        break;
                    case (int)Keys.D3:
                        rcb = RemoteControlButton.Digit3;
                        break;
                    case (int)Keys.D4:
                        rcb = RemoteControlButton.Digit4;
                        break;
                    case (int)Keys.D5:
                        rcb = RemoteControlButton.Digit5;
                        break;
                    case (int)Keys.D6:
                        rcb = RemoteControlButton.Digit6;
                        break;
                    case (int)Keys.D7:
                        rcb = RemoteControlButton.Digit7;
                        break;
                    case (int)Keys.D8:
                        rcb = RemoteControlButton.Digit8;
                        break;
                    case (int)Keys.D9:
                        rcb = RemoteControlButton.Digit9;
                        break;
                    case (int)Keys.Enter:
                        rcb = RemoteControlButton.Enter;
                        break;
                    case (int)Keys.Right:
                        rcb = RemoteControlButton.Right;
                        break;
                    case (int)Keys.Up:
                        rcb = RemoteControlButton.Up;
                        break;
                }

                if (this.ButtonPressed != null && rcb != RemoteControlButton.Unknown)
                    this.ButtonPressed(this, new RemoteControlEventArgs(rcb, GetDevice(param)));
            }


            private void ProcessAppCommand(int param)
            {
                RemoteControlButton rcb = RemoteControlButton.Unknown;

                int cmd = (int)(((ushort)(param >> 16)) & ~FAPPCOMMAND_MASK);

                switch (cmd)
                {
                    case APPCOMMAND_BROWSER_BACKWARD:
                        rcb = RemoteControlButton.Back;
                        break;
                    case APPCOMMAND_MEDIA_CHANNEL_DOWN:
                        rcb = RemoteControlButton.ChannelDown;
                        break;
                    case APPCOMMAND_MEDIA_CHANNEL_UP:
                        rcb = RemoteControlButton.ChannelUp;
                        break;
                    case APPCOMMAND_MEDIA_FAST_FORWARD:
                        rcb = RemoteControlButton.FastForward;
                        break;
                    case APPCOMMAND_VOLUME_MUTE:
                        rcb = RemoteControlButton.VolumeMute;
                        break;
                    case APPCOMMAND_MEDIA_PAUSE:
                        rcb = RemoteControlButton.Pause;
                        break;
                    case APPCOMMAND_MEDIA_PLAY:
                        rcb = RemoteControlButton.Play;
                        break;
                    case APPCOMMAND_MEDIA_RECORD:
                        rcb = RemoteControlButton.Record;
                        break;
                    case APPCOMMAND_MEDIA_PREVIOUSTRACK:
                        rcb = RemoteControlButton.PreviousTrack;
                        break;
                    case APPCOMMAND_MEDIA_REWIND:
                        rcb = RemoteControlButton.Rewind;
                        break;
                    case APPCOMMAND_MEDIA_NEXTTRACK:
                        rcb = RemoteControlButton.NextTrack;
                        break;
                    case APPCOMMAND_MEDIA_STOP:
                        rcb = RemoteControlButton.Stop;
                        break;
                    case APPCOMMAND_VOLUME_DOWN:
                        rcb = RemoteControlButton.VolumeDown;
                        break;
                    case APPCOMMAND_VOLUME_UP:
                        rcb = RemoteControlButton.VolumeUp;
                        break;
                }

                if (this.ButtonPressed != null && rcb != RemoteControlButton.Unknown)
                    this.ButtonPressed(this, new RemoteControlEventArgs(rcb, GetDevice(param)));
            }


            private void ProcessInputCommand(ref Message message)
            {
                RemoteControlButton rcb = RemoteControlButton.Unknown;
                uint dwSize = 0;

                GetRawInputData(message.LParam,
                    RID_INPUT,
                    IntPtr.Zero,
                    ref dwSize,
                    (uint)Marshal.SizeOf(typeof(RAWINPUTHEADER)
                    ));

                IntPtr buffer = Marshal.AllocHGlobal((int)dwSize);
                try
                {
                    if (buffer == IntPtr.Zero)
                        return;

                    if (GetRawInputData(message.LParam,
                        RID_INPUT,
                        buffer,
                        ref dwSize,
                        (uint)Marshal.SizeOf(typeof(RAWINPUTHEADER))) != dwSize
                        )
                    {
                        return;
                    }

                    RAWINPUT raw;

                    try
                    {
                        raw = (RAWINPUT) Marshal.PtrToStructure(buffer, typeof (RAWINPUT));
                    }
                    catch
                    {
                        //Cannot marshal field 'mouse' of type 'RAWINPUT': The type definition of this field has layout information but has an invalid managed/unmanaged type combination or is unmarshalable.
                        return;
                    }

                    if (raw.header.dwType == RIM_TYPEHID)
                    {
                        byte[] bRawData = new byte[raw.hid.dwSizHid];
                        int pRawData = buffer.ToInt32() + Marshal.SizeOf(typeof(RAWINPUT)) + 1;

                        Marshal.Copy(new IntPtr(pRawData), bRawData, 0, raw.hid.dwSizHid - 1);
                        int rawData = bRawData[0] | bRawData[1] << 8;

                        switch (rawData)
                        {
                            case RAWINPUT_DETAILS:
                                rcb = RemoteControlButton.Details;
                                break;
                            case RAWINPUT_GUIDE:
                                rcb = RemoteControlButton.Guide;
                                break;
                            case RAWINPUT_TVJUMP:
                                rcb = RemoteControlButton.TVJump;
                                break;
                            case RAWINPUT_STANDBY:
                                rcb = RemoteControlButton.StandBy;
                                break;
                            case RAWINPUT_OEM1:
                                rcb = RemoteControlButton.OEM1;
                                break;
                            case RAWINPUT_OEM2:
                                rcb = RemoteControlButton.OEM2;
                                break;
                            case RAWINPUT_MYTV:
                                rcb = RemoteControlButton.MyTV;
                                break;
                            case RAWINPUT_MYVIDEOS:
                                rcb = RemoteControlButton.MyVideos;
                                break;
                            case RAWINPUT_MYPICTURES:
                                rcb = RemoteControlButton.MyPictures;
                                break;
                            case RAWINPUT_MYMUSIC:
                                rcb = RemoteControlButton.MyMusic;
                                break;
                            case RAWINPUT_RECORDEDTV:
                                rcb = RemoteControlButton.RecordedTV;
                                break;
                            case RAWINPUT_DVDANGLE:
                                rcb = RemoteControlButton.DVDAngle;
                                break;
                            case RAWINPUT_DVDAUDIO:
                                rcb = RemoteControlButton.DVDAudio;
                                break;
                            case RAWINPUT_DVDMENU:
                                rcb = RemoteControlButton.DVDMenu;
                                break;
                            case RAWINPUT_DVDSUBTITLE:
                                rcb = RemoteControlButton.DVDSubtitle;
                                break;
                        }

                        if (rcb != RemoteControlButton.Unknown)
                            this.ButtonPressed?.Invoke(this, new RemoteControlEventArgs(rcb, GetDevice(message.LParam.ToInt32())));
                    }
                    else if (raw.header.dwType == RIM_TYPEMOUSE)
                    {
                        // do mouse handling...	
                    }
                    else if (raw.header.dwType == RIM_TYPEKEYBOARD)
                    {
                        // do keyboard handling...
                    }
                }
                finally
                {
                    Marshal.FreeHGlobal(buffer);
                }
            }


            private InputDevice GetDevice(int param)
            {
                InputDevice inputDevice;

                switch ((int)(((ushort)(param >> 16)) & FAPPCOMMAND_MASK))
                {
                    case FAPPCOMMAND_OEM:
                        inputDevice = InputDevice.OEM;
                        break;
                    case FAPPCOMMAND_MOUSE:
                        inputDevice = InputDevice.Mouse;
                        break;
                    default:
                        inputDevice = InputDevice.Key;
                        break;
                }

                return inputDevice;
            }
        }
    
    }
}