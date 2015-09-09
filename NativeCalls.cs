using System;
using System.Runtime.InteropServices;

namespace iSpyApplication
{
    public static class NativeCalls
    {
        const int MonitorPowerOn = -1;
        //const int MonitorStandby = 1;
        //const int MonitorPowerOff = 2;
        
        const int ScMonitorpower = 0xF170;
        const int SysCommand = 0x0112;
        const int MouseeventfMove = 0x0001;

        public const uint EsContinuous = 0x80000000;
        public const uint EsSystemRequired = 0x00000001;


        public const int WmSyscommand = 0x0112;
        public static IntPtr ScDragsizeS = (IntPtr)0xF006;
        public static IntPtr ScDragsizeE = (IntPtr)0xF002;
        public static IntPtr ScDragsizeSe = (IntPtr)0xF008;

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReleaseCapture(IntPtr hwnd);

        [DllImport("user32.dll")]
        static extern IntPtr PostMessage(int hWnd, int msg, int wParam, int lParam);

        // Import SetThreadExecutionState Win32 API and necessary flags
        [DllImport("kernel32.dll")]
        public static extern uint SetThreadExecutionState(uint esFlags);

        [DllImport("user32.dll")]
        static extern void MouseEvent(Int32 dwFlags, Int32 dx, Int32 dy, Int32 dwData, UIntPtr dwExtraInfo);

        public static void MonitorOn()
        {
            //MouseEvent(MouseeventfMove, 0, 1, 0, UIntPtr.Zero);
            PostMessage(-1, SysCommand, ScMonitorpower, MonitorPowerOn); // doesn't seem to work on windows 10
        }
        
    }
}
