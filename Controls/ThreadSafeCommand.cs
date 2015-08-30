using System;

namespace iSpyApplication.Controls
{
    public class ThreadSafeCommand : EventArgs
    {
        public string Command;
        // Constructor
        public ThreadSafeCommand(string command)
        {
            Command = command;
        }
    }
}
