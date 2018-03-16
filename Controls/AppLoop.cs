using System;
using System.Diagnostics;
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
        }

        private static readonly Lazy<WinFormsAppIdleHandler> Lazy = new Lazy<WinFormsAppIdleHandler>(() => new WinFormsAppIdleHandler());
        public static WinFormsAppIdleHandler Instance => Lazy.Value;


        private bool Enabled
        {
            set
            {
                if (value)
                {
                    Application.Idle -= ApplicationIdle;
                    Application.Idle += ApplicationIdle;
                }
                else
                    Application.Idle -= ApplicationIdle;
            }
        }

        private int _refCount = 0;


        public event EventHandler ApplicationLoopDoWork
        {
            add
            {
                lock (_completedEventLock)
                {
                    AppLoopDoWork += value;
                    _refCount++;
                    Enabled = true;
                }
            }

            remove
            {
                lock (_completedEventLock)
                {
                    AppLoopDoWork -= value;
                    _refCount--;
                    if (_refCount == 0)
                        Enabled = false;
                }
            }
        }

        private void ApplicationIdle(object sender, EventArgs e)
        {
            AppLoopDoWork?.Invoke(this, e);
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