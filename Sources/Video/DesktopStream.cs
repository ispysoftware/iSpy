using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using iSpyApplication.Controls;
using iSpyApplication.Utilities;
using Cursor = System.Windows.Forms.Cursor;
using Cursors = System.Windows.Forms.Cursors;

namespace iSpyApplication.Sources.Video
{
    public class DesktopStream : VideoBase, IVideoSource
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

        
        private Rectangle _screenSize = Rectangle.Empty;

        // Worker thread
        private void WorkerThread()
        {
            _abort = new ManualResetEvent(false);
            while (!_abort.WaitOne(0) && !MainForm.ShuttingDown)
            {
                try
                {
                    DateTime start = DateTime.UtcNow;

                    var nf = NewFrame;
                    // provide new image to clients
                    if (nf != null && EmitFrame)
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
                                    var cursorBounds = new Rectangle(
                                        Cursor.Position.X - s.Bounds.X - _screenSize.X,
                                        Cursor.Position.Y - s.Bounds.Y - _screenSize.Y, Cursors.Default.Size.Width,
                                        Cursors.Default.Size.Height);
                                    Cursors.Default.Draw(g, cursorBounds);
                                }
                            }
                            // notify client
                            nf.Invoke(this, new NewFrameEventArgs(target));
                            _error = false;
                        }
                    }

                    // wait for a while ?
                    if (FrameInterval > 0)
                    {
                        // get download duration
                        TimeSpan span = DateTime.UtcNow.Subtract(start);
                        // milliseconds to sleep
                        int msec = FrameInterval - (int)span.TotalMilliseconds;
                        if (msec > 0)
                        {
                            _abort.WaitOne(msec);
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
