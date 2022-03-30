using System.Runtime.InteropServices;

namespace Activator.Enums
{
    /// <summary>
    ///     The INPUT structure is used by SendInput to store information for synthesizing input events such as keystrokes,
    ///     mouse movement, and mouse clicks. (see: http://msdn.microsoft.com/en-us/library/ms646270(VS.85).aspx) Declared in
    ///     Winuser.h, include Windows.h
    /// </summary>
    /// <remarks>
    ///     This structure contains information identical to that used in the parameter list of the keybd_event or mouse_event
    ///     function. Windows 2000/XP: INPUT_KEYBOARD supports nonkeyboard input methods, such as handwriting recognition or
    ///     voice recognition, as if it were text input by using the KEYEVENTF_UNICODE flag. For more information, see the
    ///     remarks section of KEYBDINPUT.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    internal struct INPUT
    {
        public uint Type;
        public MOUSEKEYBDHARDWAREINPUT Data;
    }
}