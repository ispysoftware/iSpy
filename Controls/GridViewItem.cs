using System.Deployment.Internal;
using System.Drawing;
using System.Windows.Forms;
using iSpyApplication.Sources;

namespace iSpyApplication.Controls
{
    public class GridViewItem
    {
        private readonly string _name;
        internal readonly int ObjectID;
        internal readonly int TypeID;

        public GridViewItem(string name, int objectid, int typeid)
        {
            _name = name;
            ObjectID = objectid;
            TypeID = typeid;
        }

        public override string ToString()
        {
            return _name;
        }
    }
}
