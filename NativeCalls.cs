using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace iSpyApplication
{
    public static class NativeCalls
    {
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

        // Import SetThreadExecutionState Win32 API and necessary flags
        [DllImport("kernel32.dll")]
        public static extern uint SetThreadExecutionState(uint esFlags);

        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy,
                      int dwData, UIntPtr dwExtraInfo);


        public static void WakeScreen()
        {
            mouse_event(MouseeventfMove,0,1,0,UIntPtr.Zero);
            Thread.Sleep(40);
            mouse_event(MouseeventfMove, 0, -1, 0, UIntPtr.Zero);
        }

    }
}