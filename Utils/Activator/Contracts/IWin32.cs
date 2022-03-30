using System;

namespace Activator.Contracts
{
    public interface IWin32
    {
        IntPtr FindWindow(String className, String windowName);
        IntPtr FindWindow(String windowName);
        IntPtr SetForegroundWindow(IntPtr hWnd);
    }
}