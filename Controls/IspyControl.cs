using System;
using System.Drawing;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public interface ISpyControl
    {
        int ObjectTypeID { get; }
        int ObjectID { get; }
        bool IsEnabled { get;}
        bool Talking { get; }
        bool Listening { get; }
        bool Recording { get; }
        bool ForcedRecording { get; }
        bool CanTalk { get; }
        bool CanListen { get; }
        bool CanRecord { get; }
        bool CanEnable { get; }
        bool CanGrab { get; }
        bool HasFiles { get; }
        string ObjectName { get; }
        string Folder { get; }
        void Disable(bool stopSource=true);
        void Enable();
        string RecordSwitch(bool record);
        void Talk(IWin32Window f = null);
        void Listen();
        string SaveFrame(Bitmap bmp = null);
        void Alert(object sender, EventArgs e);
        void Detect(object sender, EventArgs e);

        void Apply();
        void ReloadSchedule();

        int Order { get; set; }

        bool Highlighted { get; set; }
        void LoadFileList();
        void SaveFileList();

        Color BorderColor { get; }

    }
}
