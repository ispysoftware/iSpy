using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace iSpyApplication.Utilities
{
    public static class NativeMethods
    {

        [Flags]
        internal enum ErrorModes : uint
        {
            SYSTEM_DEFAULT = 0x0,
            SEM_FAILCRITICALERRORS = 0x0001,
            SEM_NOALIGNMENTFAULTEXCEPT = 0x0004,
            SEM_NOGPFAULTERRORBOX = 0x0002,
            SEM_NOOPENFILEERRORBOX = 0x8000
        }

        /// <summary>
        /// Possible flags for the SHFileOperation method.
        /// </summary>
        [Flags]
        public enum FileOperationFlags : ushort
        {
            /// <summary>
            /// Do not show a dialog during the process
            /// </summary>
            FOF_SILENT = 0x0004,
            /// <summary>
            /// Do not ask the user to confirm selection
            /// </summary>
            FOF_NOCONFIRMATION = 0x0010,
            /// <summary>
            /// Delete the file to the recycle bin.  (Required flag to send a file to the bin
            /// </summary>
            FOF_ALLOWUNDO = 0x0040,
            /// <summary>
            /// Do not show the names of the files or folders that are being recycled.
            /// </summary>
            FOF_SIMPLEPROGRESS = 0x0100,
            /// <summary>
            /// Surpress errors, if any occur during the process.
            /// </summary>
            FOF_NOERRORUI = 0x0400,
            /// <summary>
            /// Warn if files are too big to fit in the recycle bin and will need
            /// to be deleted completely.
            /// </summary>
            FOF_WANTNUKEWARNING = 0x4000,
        }

        /// <summary>
        /// File Operation Function Type for SHFileOperation
        /// </summary>
        public enum FileOperationType : uint
        {
            /// <summary>
            /// Move the objects
            /// </summary>
            FO_MOVE = 0x0001,
            /// <summary>
            /// Copy the objects
            /// </summary>
            FO_COPY = 0x0002,
            /// <summary>
            /// Delete (or recycle) the objects
            /// </summary>
            FO_DELETE = 0x0003,
            /// <summary>
            /// Rename the object(s)
            /// </summary>
            FO_RENAME = 0x0004,
        }

        /// <summary>
        /// SHFILEOPSTRUCT for SHFileOperation from COM
        /// removed Pack for compatibility with 64 bit
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SHFILEOPSTRUCT
        {

            private readonly IntPtr hwnd;
            [MarshalAs(UnmanagedType.U4)]
            public FileOperationType wFunc;
            public string pFrom;
            public string pTo;
            public FileOperationFlags fFlags;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fAnyOperationsAborted;
            private readonly IntPtr hNameMappings;
            public string lpszProgressTitle;
        }


        [DllImport("kernel32.dll")]
        internal static extern ErrorModes SetErrorMode(ErrorModes mode);

        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory")]
        internal static extern void CopyMemory(IntPtr destination, IntPtr source, uint length);

        [DllImport("netapi32.dll", EntryPoint = "NetServerEnum")]
        internal static extern int NetServerEnum([MarshalAs(UnmanagedType.LPWStr)] string servername,
            int level,
            out IntPtr bufptr,
            int prefmaxlen,
            ref int entriesread,
            ref int totalentries,
            NetApi32.SV_101_TYPES servertype,
            [MarshalAs(UnmanagedType.LPWStr)] string domain,
            IntPtr resumeHandle);

        [DllImport("netapi32.dll", EntryPoint = "NetApiBufferFree")]
        internal static extern int
            NetApiBufferFree(IntPtr buffer);

        [DllImport("Netapi32", CharSet = CharSet.Unicode)]
        internal static extern int NetMessageBufferSend(
            string servername,
            string msgname,
            string fromname,
            string buf,
            int buflen);

        // Signatures for unmanaged calls
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool SystemParametersInfo(
           int uAction, int uParam, ref int lpvParam,
           int flags);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool SystemParametersInfo(
           int uAction, int uParam, ref bool lpvParam,
           int flags);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool PostMessage(HandleRef hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        internal static extern int SHFileOperation(ref SHFILEOPSTRUCT FileOp);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern IntPtr OpenDesktop(
           string hDesktop, int flags, bool inherit,
           uint desiredAccess);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool CloseDesktop(IntPtr hDesktop);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool EnumDesktopWindows(
           IntPtr hDesktop, EnumDesktopWindowsProc callback,
           IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool IsWindowVisible(
           IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern IntPtr GetForegroundWindow();

        internal delegate bool EnumDesktopWindowsProc(IntPtr hDesktop, IntPtr lParam);

        /// <summary>
        /// Supplies a pointer to an implementation of <b>IBindCtx</b> (a bind context object).
        /// This object stores information about a particular moniker-binding operation.
        /// </summary>
        /// 
        /// <param name="reserved">Reserved for future use; must be zero.</param>
        /// <param name="ppbc">Address of <b>IBindCtx*</b> pointer variable that receives the
        /// interface pointer to the new bind context object.</param>
        /// 
        /// <returns>Returns <b>S_OK</b> on success.</returns>
        /// 
        [DllImport("ole32.dll")]
        internal static extern
        int CreateBindCtx(int reserved, out IBindCtx ppbc);

        /// <summary>
        /// Converts a string into a moniker that identifies the object named by the string.
        /// </summary>
        /// 
        /// <param name="pbc">Pointer to the IBindCtx interface on the bind context object to be used in this binding operation.</param>
        /// <param name="szUserName">Pointer to a zero-terminated wide character string containing the display name to be parsed. </param>
        /// <param name="pchEaten">Pointer to the number of characters of szUserName that were consumed.</param>
        /// <param name="ppmk">Address of <b>IMoniker*</b> pointer variable that receives the interface pointer
        /// to the moniker that was built from <b>szUserName</b>.</param>
        /// 
        /// <returns>Returns <b>S_OK</b> on success.</returns>
        /// 
        [DllImport("ole32.dll", CharSet = CharSet.Unicode)]
        internal static extern
        int MkParseDisplayName(IBindCtx pbc, string szUserName,
            ref int pchEaten, out IMoniker ppmk);

        /// <summary>
        /// Copy a block of memory.
        /// </summary>
        /// 
        /// <param name="dst">Destination pointer.</param>
        /// <param name="src">Source pointer.</param>
        /// <param name="count">Memory block's length to copy.</param>
        /// 
        /// <returns>Return's the value of <b>dst</b> - pointer to destination.</returns>
        /// 
        [DllImport("ntdll.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static unsafe extern int memcpy(
            byte* dst,
            byte* src,
            int count);

        /// <summary>
        /// Invokes a new property frame, that is, a property sheet dialog box.
        /// </summary>
        /// 
        /// <param name="hwndOwner">Parent window of property sheet dialog box.</param>
        /// <param name="x">Horizontal position for dialog box.</param>
        /// <param name="y">Vertical position for dialog box.</param>
        /// <param name="caption">Dialog box caption.</param>
        /// <param name="cObjects">Number of object pointers in <b>ppUnk</b>.</param>
        /// <param name="ppUnk">Pointer to the objects for property sheet.</param>
        /// <param name="cPages">Number of property pages in <b>lpPageClsID</b>.</param>
        /// <param name="lpPageClsID">Array of CLSIDs for each property page.</param>
        /// <param name="lcid">Locale identifier for property sheet locale.</param>
        /// <param name="dwReserved">Reserved.</param>
        /// <param name="lpvReserved">Reserved.</param>
        /// 
        /// <returns>Returns <b>S_OK</b> on success.</returns>
        /// 
        [DllImport("oleaut32.dll")]
        internal static extern int OleCreatePropertyFrame(
            IntPtr hwndOwner,
            int x,
            int y,
            [MarshalAs(UnmanagedType.LPWStr)] string caption,
            int cObjects,
            [MarshalAs(UnmanagedType.Interface, ArraySubType = UnmanagedType.IUnknown)]
            ref object ppUnk,
            int cPages,
            IntPtr lpPageClsID,
            int lcid,
            int dwReserved,
            IntPtr lpvReserved);

        [DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
        internal static extern int GetSystemMetrics(int which);

        [DllImport("user32.dll")]
        internal static extern void
            SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter,
                         int X, int Y, int width, int height, uint flags);
    }
}
