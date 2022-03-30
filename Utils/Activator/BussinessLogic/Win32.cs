using System;
using System.Runtime.InteropServices;
using Activator.Contracts;

namespace Activator.BussinessLogic
{
    public class Win32 : IWin32
    {
        //Import the FindWindow API to find our window
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindowNative(String className, String windowName);

        //Import the SetForeground API to activate it
        [DllImport("User32.dll", EntryPoint = "SetForegroundWindow")]
        private static extern IntPtr SetForegroundWindowNative(IntPtr hWnd);

        public IntPtr FindWindow(String className, String windowName)
        {
            return FindWindowNative(className, windowName);
        }

        public IntPtr FindWindow(String windowName)
        {
            return FindWindowNative(null, windowName);
        }

        public IntPtr SetForegroundWindow(IntPtr hWnd)
        {
            return SetForegroundWindowNative(hWnd);
        }
    }
}