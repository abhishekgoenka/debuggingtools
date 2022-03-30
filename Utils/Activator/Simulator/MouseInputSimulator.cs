using System;
using System.Runtime.InteropServices;

namespace Activator.Simulator
{
    public class MouseInputSimulator
    {
        private const int CXFRAME = 0x20;
        private const int CYFRAME = 0x21;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);

        public static void ClickLeftMouseButton()
        {
            //move mouse to 500,500
            MouseMove(600, 600);

            var mouseDownInput = new INPUT();
            mouseDownInput.type = SendInputEventType.InputMouse;
            mouseDownInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_LEFTDOWN;
            // Call the API
            uint resSendInput = SendInput(1, ref mouseDownInput, Marshal.SizeOf(new INPUT()));
            if (resSendInput == 0 || Marshal.GetLastWin32Error() != 0)
                System.Diagnostics.Debug.WriteLine(Marshal.GetLastWin32Error());

            var mouseUpInput = new INPUT();
            mouseUpInput.type = SendInputEventType.InputMouse;
            mouseUpInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_LEFTUP;
            resSendInput = SendInput(1, ref mouseUpInput, Marshal.SizeOf(new INPUT()));
            if (resSendInput == 0 || Marshal.GetLastWin32Error() != 0)
                System.Diagnostics.Debug.WriteLine(Marshal.GetLastWin32Error());
        }

        public static void ClickRightMouseButton()
        {
            //move mouse to 500,500
            MouseMove(500, 500);

            var mouseDownInput = new INPUT();
            mouseDownInput.type = SendInputEventType.InputMouse;
            mouseDownInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_RIGHTDOWN;
            uint resSendInput = SendInput(1, ref mouseDownInput, Marshal.SizeOf(new INPUT()));
            if (resSendInput == 0 || Marshal.GetLastWin32Error() != 0)
                System.Diagnostics.Debug.WriteLine(Marshal.GetLastWin32Error());

            var mouseUpInput = new INPUT();
            mouseUpInput.type = SendInputEventType.InputMouse;
            mouseUpInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_RIGHTUP;
            resSendInput = SendInput(1, ref mouseUpInput, Marshal.SizeOf(new INPUT()));
            if (resSendInput == 0 || Marshal.GetLastWin32Error() != 0)
                System.Diagnostics.Debug.WriteLine(Marshal.GetLastWin32Error());
        }

        private static void MouseMove(int x, int y)
        {
            double fScreenWidth = GetSystemMetrics(CXFRAME) - 1;
            double fScreenHeight = GetSystemMetrics(CYFRAME) - 1;
            double fx = x*(65535.0f/fScreenWidth);
            double fy = y*(65535.0f/fScreenHeight);
            var mouseMove = new INPUT();
            mouseMove.type = SendInputEventType.InputMouse;
            mouseMove.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_MOVE | MouseEventFlags.MOUSEEVENTF_ABSOLUTE;
            mouseMove.mkhi.mi.dx = fx;
            mouseMove.mkhi.mi.dy = fy;
            SendInput(1, ref mouseMove, Marshal.SizeOf(new INPUT()));
        }

        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);

        [StructLayout(LayoutKind.Sequential)]
        private struct HARDWAREINPUT
        {
            public readonly int uMsg;
            public readonly short wParamL;
            public readonly short wParamH;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct INPUT
        {
            public SendInputEventType type;
            public MouseKeybdhardwareInputUnion mkhi;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KEYBDINPUT
        {
            public readonly ushort wVk;
            public readonly ushort wScan;
            public readonly uint dwFlags;
            public readonly uint time;
            public readonly IntPtr dwExtraInfo;
        }

        [Flags]
        private enum MouseEventFlags : uint
        {
            MOUSEEVENTF_MOVE = 0x0001,
            MOUSEEVENTF_LEFTDOWN = 0x0002,
            MOUSEEVENTF_LEFTUP = 0x0004,
            MOUSEEVENTF_RIGHTDOWN = 0x0008,
            MOUSEEVENTF_RIGHTUP = 0x0010,
            MOUSEEVENTF_MIDDLEDOWN = 0x0020,
            MOUSEEVENTF_MIDDLEUP = 0x0040,
            MOUSEEVENTF_XDOWN = 0x0080,
            MOUSEEVENTF_XUP = 0x0100,
            MOUSEEVENTF_WHEEL = 0x0800,
            MOUSEEVENTF_VIRTUALDESK = 0x4000,
            MOUSEEVENTF_ABSOLUTE = 0x8000
        }

        private struct MouseInputData
        {
            public IntPtr dwExtraInfo;
            public MouseEventFlags dwFlags;
            public double dx;
            public double dy;
            public uint mouseData;
            public uint time;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct MouseKeybdhardwareInputUnion
        {
            [FieldOffset(0)] public MouseInputData mi;

            [FieldOffset(0)] public readonly KEYBDINPUT ki;

            [FieldOffset(0)] public readonly HARDWAREINPUT hi;
        }

        private enum SendInputEventType
        {
            InputMouse,
            InputKeyboard,
            InputHardware
        }
    }
}