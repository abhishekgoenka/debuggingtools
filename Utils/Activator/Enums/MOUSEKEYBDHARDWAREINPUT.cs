using System.Runtime.InteropServices;

namespace Activator.Enums
{
    /// <summary>
    ///     The combined/overlayed structure that includes Mouse, Keyboard and Hardware Input message data (see:
    ///     http://msdn.microsoft.com/en-us/library/ms646270(VS.85).aspx)
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct MOUSEKEYBDHARDWAREINPUT
    {
        // Fields
        [FieldOffset(0)] public HARDWAREINPUT Hardware;
        [FieldOffset(0)] public KEYBDINPUT Keyboard;
        [FieldOffset(0)] public MOUSEINPUT Mouse;
    }
}
