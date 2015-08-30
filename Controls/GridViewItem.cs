using System.Drawing;
using iSpyApplication.Sources;

namespace iSpyApplication.Controls
{
    public class GridViewItem
    {
        private readonly string _name;
        internal readonly int ObjectID;
        internal readonly int TypeID;
        private Bitmap _bitmap;
        private readonly object _lock = new object();
        internal MainForm MainClass;
        public Bitmap LastFrame
        {
            get
            {
                lock (_lock)
                {
                    return (Bitmap) _bitmap?.Clone();
                }
            }
            set {
                lock (_lock)
                {
                    _bitmap?.Dispose();
                    _bitmap = value;
                } 
            }
        }

        public GridViewItem(string name, int objectid, int typeid, GridView gv)
        {
            _name = name;
            ObjectID = objectid;
            TypeID = typeid;
            if (gv != null)
                Init(gv);
        }

        public void Init(GridView gv)
        {
            MainClass = gv.MainClass;
            if (TypeID == 2)
            {
                var ctrl = MainClass.GetCameraWindow(ObjectID);
                if (ctrl != null)
                {
                    LastFrame = ctrl.LastFrame;
                    ctrl.NewFrame -= CameraNewFrame;
                    ctrl.NewFrame += CameraNewFrame;
                }
            }            
        }

        public void DeInit()
        {
            if (TypeID == 2 && MainClass!=null)
            {
                var ctrl = MainClass.GetCameraWindow(ObjectID);
                if (ctrl != null)
                {
                    ctrl.NewFrame -= CameraNewFrame;
                }
            }            
        }

        public void CameraNewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            LastFrame = (Bitmap)eventArgs.Frame.Clone();
        }

        public override string ToString()
        {
            return _name;
        }
    }
}
