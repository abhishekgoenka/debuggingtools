namespace Activator.Enums
{
    public enum MouseFlag : uint
    {
        ABSOLUTE = 0x8000,
        LEFTDOWN = 2,
        LEFTUP = 4,
        MIDDLEDOWN = 0x20,
        MIDDLEUP = 0x40,
        MOVE = 1,
        RIGHTDOWN = 8,
        RIGHTUP = 0x10,
        VIRTUALDESK = 0x4000,
        WHEEL = 0x800,
        XDOWN = 0x80,
        XUP = 0x100
    }
}