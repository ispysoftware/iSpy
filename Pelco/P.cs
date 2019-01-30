using System;
using System.Globalization;

namespace iSpyApplication.Pelco
{
    public class PelcoP
    {
        public enum Action
        {
            Start,
            Stop
        }

        public enum Focus
        {
            Near = FocusNear,
            Far = FocusFar
        }

        public enum Iris
        {
            Open = IrisOpen,
            Close = IrisClose
        }

        public enum Pan
        {
            Left = PanLeft,
            Right = PanRight
        }

        public enum PatternAction
        {
            Start,
            Stop,
            Run
        }


        public enum PresetAction
        {
            Set,
            Clear,
            Goto
        }

        public enum Switch
        {
            On,
            Off
        }

        public enum Tilt
        {
            Up = TiltUp,
            Down = TiltDown
        }

        public enum Zoom
        {
            Wide = ZoomWide,
            Telephoto = ZoomTelephoto
        }

        private const byte Stx = 0xA0;
        private const byte Etx = 0xAF;

        private const byte FocusFar = 0x01;
        private const byte FocusNear = 0x02;
        private const byte IrisOpen = 0x04;
        private const byte IrisClose = 0x08;
        private const byte CameraOnOff = 0x10;

        private const byte PanRight = 0x02;
        private const byte PanLeft = 0x04;
        private const byte TiltUp = 0x08;
        private const byte TiltDown = 0x10;
        private const byte ZoomTelephoto = 0x20;
        private const byte ZoomWide = 0x40;

        private const byte PanSpeedMax = 0x40;        
        private const byte TiltSpeedMax = 0x3F;


        public byte[] Preset(uint deviceAddress, byte preset, PresetAction action)
        {
            byte mAction;
            switch (action)
            {
                case PresetAction.Set:
                    mAction = 0x03;
                    break;
                case PresetAction.Clear:
                    mAction = 0x05;
                    break;
                case PresetAction.Goto:
                    mAction = 0x07;
                    break;
                default:
                    mAction = 0x03;
                    break;
            }

            return Message.GetMessage(deviceAddress, 0x00, mAction, 0x00, preset);
        }

        public byte[] Flip(uint deviceAddress)
        {
            return Message.GetMessage(deviceAddress, 0x00, 0x07, 0x00, 0x21);
        }

        public byte[] ZeroPanPosition(uint deviceAddress)
        {
            return Message.GetMessage(deviceAddress, 0x00, 0x07, 0x00, 0x22);
        }

        public byte[] RemoteReset(uint deviceAddress)
        {
            return Message.GetMessage(deviceAddress, 0x00, 0x0F, 0x00, 0x00);
        }

        public byte[] Zone(uint deviceAddress, byte zone, Action action)
        {
            if ((zone < 0x01) & (zone > 0x08))
                throw new Exception("Zone value should be between 0x01 and 0x08 include");
            byte mAction;
            if (action == Action.Start)
                mAction = 0x11;
            else
                mAction = 0x13;

            return Message.GetMessage(deviceAddress, 0x00, mAction, 0x00, zone);
        }

        public byte[] ClearScreen(uint deviceAddress)
        {
            return Message.GetMessage(deviceAddress, 0x00, 0x17, 0x00, 0x00);
        }

        public byte[] ZoneScan(uint deviceAddress, Action action)
        {
            byte mAction;
            if (action == Action.Start)
                mAction = 0x1B;
            else
                mAction = 0x1D;
            return Message.GetMessage(deviceAddress, 0x00, mAction, 0x00, 0x00);
        }

        public byte[] Pattern(uint deviceAddress, PatternAction action)
        {
            byte mAction;
            switch (action)
            {
                case PatternAction.Start:
                    mAction = 0x1F;
                    break;
                case PatternAction.Stop:
                    mAction = 0x21;
                    break;
                case PatternAction.Run:
                    mAction = 0x23;
                    break;
                default:
                    mAction = 0x23;
                    break;
            }

            return Message.GetMessage(deviceAddress, 0x00, mAction, 0x00, 0x00);
        }

        public byte[] CameraSwitch(uint deviceAddress, Switch action)
        {
            var mAction = CameraOnOff;
            if (action == Switch.On)
                mAction += CameraOnOff;
            return Message.GetMessage(deviceAddress, mAction, 0x00, 0x00, 0x00);
        }

        public byte[] CameraIrisSwitch(uint deviceAddress, Iris action)
        {
            return Message.GetMessage(deviceAddress, (byte) action, 0x00, 0x00, 0x00);
        }

        public byte[] CameraFocus(uint deviceAddress, Focus action)
        {
            return Message.GetMessage(deviceAddress, (byte) action, 0x00, 0x00, 0x00);
        }

        public byte[] CameraZoom(uint deviceAddress, Zoom action)
        {
            return Message.GetMessage(deviceAddress, 0x00, (byte) action, 0x00, 0x00);
        }

        public byte[] CameraTilt(uint deviceAddress, Tilt action, uint speed)
        {
            if (speed < TiltSpeedMax)
                speed = TiltSpeedMax;

            return Message.GetMessage(deviceAddress, 0x00, (byte) action, 0x00, (byte) speed);
        }

        public byte[] CameraPan(uint deviceAddress, Pan action, uint speed)
        {
            if (speed < PanSpeedMax)
                speed = PanSpeedMax;

            return Message.GetMessage(deviceAddress, 0x00, (byte) action, (byte) speed, 0x00);
        }


        public byte[] CameraStop(uint deviceAddress)
        {
            return Message.GetMessage(deviceAddress, 0x00, 0x00, 0x00, 0x00);
        }

        public struct Message
        {
            public static byte Address;
            public static byte CheckSum;
            public static byte Data1, Data2, Data3, Data4;

            public static byte[] GetMessage(uint address, byte data1, byte data2, byte data3, byte data4)
            {
                if (address > 32)
                    throw new Exception("Pelco P Protocol supports 32 devices only");

                Address = byte.Parse((address - 1).ToString(CultureInfo.InvariantCulture));
                Data1 = data1;
                Data2 = data2;
                Data3 = data3;
                Data4 = data4;

                CheckSum = (byte) (Stx ^ Address ^ Data1 ^ Data2 ^ Data3 ^ Data4 ^ Etx);

                return new[] {Stx, Address, Data1, Data2, Data3, Data4, Etx, CheckSum};
            }
        }
    }
}