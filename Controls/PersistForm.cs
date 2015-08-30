using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;

namespace iSpyApplication.Controls
{
    public class PersistWindowState : Component
    {
        // event info that allows form to persist extra window state data

        #region Delegates

        public delegate void WindowStateDelegate(object sender, RegistryKey key);

        #endregion

        private bool _mAllowSaveMinimized;
        private int _mNormalHeight;

        private int _mNormalLeft;
        private int _mNormalTop;
        private int _mNormalWidth;
        private Form _mParent;
        private string _mRegPath;
        private FormWindowState _mWindowState;

        public Form Parent
        {
            set
            {
                _mParent = value;

                // subscribe to parent form's events
                _mParent.Closing += OnClosing;
                _mParent.Resize += OnResize;
                _mParent.Move += OnMove;
                _mParent.Load += OnLoad;

                // get initial width and height in case form is never resized
                _mNormalWidth = _mParent.Width;
                _mNormalHeight = _mParent.Height;
            }
            get { return _mParent; }
        }

        // registry key should be set in parent form's constructor
        public string RegistryPath
        {
            set { _mRegPath = value; }
            get { return _mRegPath; }
        }

        public bool AllowSaveMinimized
        {
            set { _mAllowSaveMinimized = value; }
        }

        public event WindowStateDelegate LoadStateEvent;
        public event WindowStateDelegate SaveStateEvent;

        private void OnResize(object sender, EventArgs e)
        {
            // save width and height
            if (_mParent.WindowState == FormWindowState.Normal)
            {
                _mNormalWidth = _mParent.Width;
                _mNormalHeight = _mParent.Height;
            }
        }

        private void OnMove(object sender, EventArgs e)
        {
            // save position
            if (_mParent.WindowState == FormWindowState.Normal)
            {
                _mNormalLeft = _mParent.Left;
                _mNormalTop = _mParent.Top;
            }
            // save state
            _mWindowState = _mParent.WindowState;
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            // save position, size and state
            RegistryKey key = Registry.CurrentUser.CreateSubKey(_mRegPath);
            if (key != null)
            {
                key.SetValue("Left", _mNormalLeft);
                key.SetValue("Top", _mNormalTop);
                key.SetValue("Width", _mNormalWidth);
                key.SetValue("Height", _mNormalHeight);
            }

            // check if we are allowed to save the state as minimized (not normally)
            if (!_mAllowSaveMinimized)
            {
                if (_mWindowState == FormWindowState.Minimized)
                    _mWindowState = FormWindowState.Normal;
            }

            if (key != null)
            {
                key.SetValue("WindowState", (int) _mWindowState);

                // fire SaveState event
                SaveStateEvent?.Invoke(this, key);
            }
        }

        private void OnLoad(object sender, EventArgs e)
        {
            // attempt to read state from registry
            RegistryKey key = Registry.CurrentUser.OpenSubKey(_mRegPath);
            if (key != null)
            {
                var left = (int) key.GetValue("Left", _mParent.Left);
                var top = (int) key.GetValue("Top", _mParent.Top);
                var width = (int) key.GetValue("Width", _mParent.Width);
                var height = (int) key.GetValue("Height", _mParent.Height);
                var windowState = (FormWindowState) key.GetValue("WindowState",
                                                                 (int) _mParent.WindowState);


                _mParent.WindowState = windowState;
                if (IsOnScreen(new Point(left, top)))
                {
                    _mParent.Location = new Point(left, top);
                }
                _mParent.Size = new Size(width, height);

                // fire LoadState event
                LoadStateEvent?.Invoke(this, key);
            }
        }

        private bool IsOnScreen(Point p)
        {
            var screens = Screen.AllScreens;
            return screens.Any(screen => screen.WorkingArea.Contains(p));
        }
    }
}