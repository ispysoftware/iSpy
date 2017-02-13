using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public sealed class WinFormsAppIdleHandler
    {
        private readonly object _completedEventLock = new object();
        private event EventHandler AppLoopDoWork;

        public WinFormsAppIdleHandler()
        {
            Enabled = true;
            SleepTime = 10;

        }

        private static readonly Lazy<WinFormsAppIdleHandler> Lazy = new Lazy<WinFormsAppIdleHandler>(() => new WinFormsAppIdleHandler());
        public static WinFormsAppIdleHandler Instance => Lazy.Value;

        private bool _enabled;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                if (value)
                {
                    Application.Idle -= ApplicationIdle;
                    Application.Idle += ApplicationIdle;
                }
                else
                    Application.Idle -= ApplicationIdle;

                _enabled = value;
            }
        }

        public int SleepTime { get; set; }

        public event EventHandler ApplicationLoopDoWork
        {
            add
            {
                lock (_completedEventLock)
                    AppLoopDoWork += value;
            }

            remove
            {
                lock (_completedEventLock)
                    AppLoopDoWork -= value;
            }
        }

        private void ApplicationIdle(object sender, EventArgs e)
        {
            while (Enabled && IsAppIdle())
            {
                OnApplicationIdleDoWork(EventArgs.Empty);
                Thread.Sleep(SleepTime);
            }
        }

        private void OnApplicationIdleDoWork(EventArgs e)
        {
            var handler = AppLoopDoWork;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public static bool IsAppIdle()
        {
            bool isIdle = false;
            try
            {
                Message msg;
                isIdle = !PeekMessage(out msg, IntPtr.Zero, 0, 0, 0);
            }
            catch
            {

            }
            return isIdle;
        }

        #region  Unmanaged Get PeekMessage
        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool PeekMessage(out Message msg, IntPtr hWnd, uint messageFilterMin, uint messageFilterMax, uint flags);
        #endregion
    }
}