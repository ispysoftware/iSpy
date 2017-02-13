using iSpyApplication.Controls;

namespace iSpyApplication
{
    public class Delegates
    {
        public delegate void NotificationEventHandler(object sender, NotificationType e);

        public delegate void DisableDelegate(bool stopSource);

        public delegate void EnableDelegate();

        public delegate void AddAudioDelegate();

        public delegate void FileListUpdatedEventHandler(object sender);

        public delegate void RemoteCommandEventHandler(object sender, ThreadSafeCommand e);

        public delegate void NewDataAvailable(object sender, NewDataAvailableArgs eventArgs);

        public delegate void CloseDelegate();

        public delegate void RunStorageManagementDelegate(bool abortIfRunning);

        public delegate object AddObjectExternalDelegate(int sourceIndex, string url, int width, int height, string name);

        public delegate void CameraCommandDelegate(CameraWindow target);

        public delegate void ExternalCommandDelegate(string command);

        public delegate void MicrophoneCommandDelegate(VolumeLevel target);

        public delegate void InvokeMethod(string command);

        public delegate void ErrorHandler(string message);

        public delegate void RunCheckJoystick();

        public delegate void SimpleDelegate();
    }

    
}
