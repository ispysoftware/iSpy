using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using iSpyApplication.Controls;
using iSpyApplication.Utilities;
using Cursor = System.Windows.Forms.Cursor;
using Cursors = System.Windows.Forms.Cursors;

namespace iSpyApplication.Sources.Video
{
    internal class DesktopStream : VideoBase, IVideoSource
    {
        private int _screenindex;
        private readonly Rectangle _area;
        private ManualResetEvent _abort;
        private ReasonToFinishPlaying _res = ReasonToFinishPlaying.DeviceLost;
        private Thread _thread;
        private bool _error;


        public DesktopStream(CameraWindow source) : base(source)
        {
            _screenindex = Convert.ToInt32(source.Camobject.settings.videosourcestring);
            MousePointer = source.Camobject.settings.desktopmouse;
            Rectangle area = Rectangle.Empty;
            if (!string.IsNullOrEmpty(source.Camobject.settings.desktoparea))
            {
                var i = Array.ConvertAll(source.Camobject.settings.desktoparea.Split(','), int.Parse);
                area = new Rectangle(i[0], i[1], i[2], i[3]);
            }
            _area = area;
            
        }

        #region IVideoSource Members

        public event NewFrameEventHandler NewFrame;
        
        public event PlayingFinishedEventHandler PlayingFinished;


        public virtual string Source
        {
            get { return _screenindex.ToString(CultureInfo.InvariantCulture); }
            set { _screenindex = Convert.ToInt32(value); }
        }


        public bool IsRunning
        {
            get
            {
                if (_thread == null)
                    return false;

                try
                {
                    return !_thread.Join(TimeSpan.Zero);
                }
                catch
                {
                    return true;
                }
            }
        }

        public bool MousePointer;

        public void Start()
        {
            if (IsRunning) return;

            // create events
            _res = ReasonToFinishPlaying.DeviceLost;

            // create and start new thread
            _thread = new Thread(WorkerThread) { Name = "desktop" + _screenindex, IsBackground = true};
            
            _thread.Start();
        }

        public void Restart()
        {
            if (!IsRunning) return;
            _res = ReasonToFinishPlaying.Restart;
            _abort?.Set();
        }


        public void Stop()
        {
            if (IsRunning)
            {
                _res = ReasonToFinishPlaying.StoppedByUser;
                _abort?.Set();
            }
            else
            {
                _res = ReasonToFinishPlaying.StoppedByUser;
                PlayingFinished?.Invoke(this, new PlayingFinishedEventArgs(_res));
            }
        }

        #endregion
        [DllImport("User32.dll")]
        private static extern IntPtr MonitorFromPoint([In]System.Drawing.Point pt, [In]uint dwFlags);

        [DllImport("Shcore.dll")]
        private static extern IntPtr GetDpiForMonitor([In]IntPtr hmonitor, [In]DpiType dpiType, [Out]out uint dpiX, [Out]out uint dpiY);
        public enum DpiType
        {
            Effective = 0,
            Angular = 1,
            Raw = 2,
        }

        private Rectangle _screenSize = Rectangle.Empty;
        
        // Worker thread
        private void WorkerThread()
        {
            _abort = new ManualResetEvent(false);
            double multiX = 0, multiY=0;
            while (!_abort.WaitOne(10) && !MainForm.ShuttingDown)
            {
                try
                {
                    var nf = NewFrame;
                    // provide new image to clients
                    if (nf != null && ShouldEmitFrame)
                    {
                        Screen s = Screen.AllScreens[_screenindex];                        
                        if (_screenSize == Rectangle.Empty)
                        {
                            if (_area != Rectangle.Empty)
                                _screenSize = _area;
                            else
                            {
                                _screenSize = s.Bounds;
                                //virtual clients can have odd dimensions
                                if (_screenSize.Width % 2 != 0)
                                    _screenSize.Width = _screenSize.Width - 1;
                                if (_screenSize.Height % 2 != 0)
                                    _screenSize.Height = _screenSize.Height - 1;
                            }

                        }

                        using (var target = new Bitmap(_screenSize.Width, _screenSize.Height,PixelFormat.Format24bppRgb))
                        {
                            using (Graphics g = Graphics.FromImage(target))
                            {
                                try
                                {
                                    g.CopyFromScreen(s.Bounds.X + _screenSize.X,
                                        s.Bounds.Y + _screenSize.Y, 0, 0,
                                        new Size(_screenSize.Width, _screenSize.Height));
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception("Error grabbing screen (" + ex.Message +
                                                        ") - disable screensaver.");
                                    //probably remote desktop or screensaver has kicked in

                                }

                                if (MousePointer)
                                {
                                    //if (multiX < 0.01)
                                    //{
                                    //    uint dpiX = 0, dpiY = 0;
                                    //    var p = new Point(s.Bounds.Left + 1, s.Bounds.Top + 1);
                                    //    var mon = MonitorFromPoint(p, 2);
                                    //    GetDpiForMonitor(mon, DpiType.Effective, out dpiX, out dpiY);
                                    //    multiX = Convert.ToDouble(dpiX) /96d;
                                    //    multiY = Convert.ToDouble(dpiY) /96d;
                                    //}
                                    multiX = 1; multiY = 1;
                                    var mx=Convert.ToInt32(Cursor.Position.X * multiX - s.Bounds.X - _screenSize.X);
                                    var my=Convert.ToInt32(Cursor.Position.Y * multiY - s.Bounds.Y - _screenSize.Y);
                                    var cursorBounds = new Rectangle(
                                        mx, my,
                                        Cursors.Default.Size.Width,
                                        Cursors.Default.Size.Height);
                                    Cursors.Default.Draw(g, cursorBounds);
                                }
                            }
                            // notify client
                            nf.Invoke(this, new NewFrameEventArgs(target));
                            _error = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!_error)
                    {
                        Logger.LogException(ex, "Desktop");
                        _error = true;
                    }
                    // provide information to clients
                    _res = ReasonToFinishPlaying.DeviceLost;
                    // wait for a while before the next try
                    _abort.WaitOne(250);
                    break;
                }
            }

            PlayingFinished?.Invoke(this, new PlayingFinishedEventArgs(_res));
            _abort.Close();
        }

        private bool _disposed;
        // Public implementation of Dispose pattern callable by consumers. 
        public void Dispose()
        {
            Dispose(true);
        }

        // Protected implementation of Dispose pattern. 
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
            }
            // Free any unmanaged objects here. 
            //
            _disposed = true;
        }
    }
}
