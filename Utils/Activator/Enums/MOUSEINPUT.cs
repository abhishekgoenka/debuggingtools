using System;
using System.Runtime.InteropServices;

namespace Activator.Enums
{
    /// <summary>
    ///     The MOUSEINPUT structure contains information about a simulated mouse event. (see:
    ///     http://msdn.microsoft.com/en-us/library/ms646273(VS.85).aspx) Declared in Winuser.h, include Windows.h
    /// </summary>
    /// <remarks>
    ///     If the mouse has moved, indicated by MOUSEEVENTF_MOVE, dxand dy specify information about that movement. The
    ///     information is specified as absolute or relative integer values. If MOUSEEVENTF_ABSOLUTE value is specified, dx and
    ///     dy contain normalized absolute coordinates between 0 and 65,535. The event procedure maps these coordinates onto
    ///     the display surface. Coordinate (0,0) maps onto the upper-left corner of the display surface; coordinate
    ///     (65535,65535) maps onto the lower-right corner. In a multimonitor system, the coordinates map to the primary
    ///     monitor. Windows 2000/XP: If MOUSEEVENTF_VIRTUALDESK is specified, the coordinates map to the entire virtual
    ///     desktop. If the MOUSEEVENTF_ABSOLUTE value is not specified, dxand dy specify movement relative to the previous
    ///     mouse event (the last reported position). Positive values mean the mouse moved right (or down); negative values
    ///     mean the mouse moved left (or up). Relative mouse motion is subject to the effects of the mouse speed and the
    ///     two-mouse threshold values. A user sets these three values with the Pointer Speed slider of the Control Panel's
    ///     Mouse Properties sheet. You can obtain and set these values using the SystemParametersInfo function. The system
    ///     applies two tests to the specified relative mouse movement. If the specified distance along either the x or y axis
    ///     is greater than the first mouse threshold value, and the mouse speed is not zero, the system doubles the distance.
    ///     If the specified distance along either the x or y axis is greater than the second mouse threshold value, and the
    ///     mouse speed is equal to two, the system doubles the distance that resulted from applying the first threshold test.
    ///     It is thus possible for the system to multiply specified relative mouse movement along the x or y axis by up to
    ///     four times.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    internal struct MOUSEINPUT
    {
        public int X;
        public int Y;
        public uint MouseData;
        public uint Flags;
        public uint Time;
        public IntPtr ExtraInfo;
    }
}