using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace iSpyApplication
{
    /// <summary>
    /// Selected Win AI Function Calls
    /// </summary>
    public class WinApi
    {
        private const int SM_CXSCREEN = 0;
        private const int SM_CYSCREEN = 1;
        private const int SWP_SHOWWINDOW = 64; // 0x0040
        private static readonly IntPtr HWND_TOP = IntPtr.Zero;

        public static int ScreenX
        {
            get { return GetSystemMetrics(SM_CXSCREEN); }
        }

        public static int ScreenY
        {
            get { return GetSystemMetrics(SM_CYSCREEN); }
        }

        [DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
        public static extern int GetSystemMetrics(int which);

        [DllImport("user32.dll")]
        public static extern void
            SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter,
                         int X, int Y, int width, int height, uint flags);

        public static void SetWinFullScreen(IntPtr hwnd)
        {
            SetWindowPos(hwnd, HWND_TOP, 0, 0, ScreenX, ScreenY, SWP_SHOWWINDOW);
        }

        public static void SetWinFullScreen(IntPtr hwnd, string displayName)
        {
            var screen = Screen.AllScreens.Where(s => s.DeviceName == displayName).DefaultIfEmpty(Screen.PrimaryScreen).First();
            SetWindowPos(hwnd, HWND_TOP, screen.Bounds.Location.X, screen.Bounds.Location.Y, screen.Bounds.Width, screen.Bounds.Height, SWP_SHOWWINDOW);
        }

    }
}