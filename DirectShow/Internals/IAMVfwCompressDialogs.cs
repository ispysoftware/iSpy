// AForge Direct Show Library
// AForge.NET framework
//
// Copyright © Andrew Kirillov, 2008
// andrew.kirillov@gmail.com
//

namespace iSpyPRO.DirectShow.Internals
{
    using System;
    using System.Runtime.InteropServices;

    // ---------------------------------------------------------------------------------------
    public enum VfwCompressDialogs
    {
        Config = 0x01,
        About = 0x02,
        QueryConfig = 0x04,
        QueryAbout = 0x08
    }

    /// <summary>
    /// The interface indicates that an object supports property pages.
    /// </summary>
    /// 
    [ComVisible(true), ComImport,
    Guid("D8D715A3-6E5E-11D0-B3F0-00AA003761C5"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IAMVfwCompressDialogs
    {
        [PreserveSig]
        // Bring up a dialog for this codec
        int ShowDialog(
            [In]  VfwCompressDialogs iDialog,
            [In]  IntPtr hwnd);

        // Calls ICGetState and gives you the result
        int GetState(
            [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] pState,
            ref int pcbState);

        // Calls ICSetState
        int SetState(
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] pState,
            [In] int cbState);

        // Send a codec specific message
        int SendDriverMessage(
            int uMsg,
            long dw1,
            long dw2);
    }
}
