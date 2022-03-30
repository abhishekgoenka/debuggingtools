using System;
using Activator.Contracts;
using Activator.Enums;
using Activator.Simulator;

namespace Activator.BussinessLogic
{
    public class KeyboardInput : IInput
    {
        private  readonly  Random rnd = new Random(5);
        //[DllImport("user32.dll", CharSet = CharSet.Unicode)]
        //static extern short VkKeyScan(char ch);

        //[DllImport("user32.dll", SetLastError = true)]
        //public static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);
        //public const int INPUT_KEYBOARD = 1;

        //[DllImport("user32.dll")]
        //static extern void keybd_event(byte bVk, byte bScan, uint dwFlags,UIntPtr dwExtraInfo);

        //[DllImport("user32.dll")]
        //public static extern IntPtr GetMessageExtraInfo();

        //[StructLayout(LayoutKind.Explicit)]
        //public struct INPUT
        //{
        //    [FieldOffset(0)]
        //    public int type;
        //    [FieldOffset(4)]
        //    public KEYBDINPUT ki;
        //}

        //[StructLayout(LayoutKind.Sequential)]
        //public struct KEYBDINPUT
        //{
        //    public ushort wVk;
        //    public ushort wScan;
        //    public uint dwFlags;
        //    public uint time;
        //    public IntPtr dwExtraInfo;
        //}

        public void SendInput()
        {
            const string text = "a"; // text we will send
            const int KEYEVENTF_EXTENDEDKEY = 0x1;
            const int KEYEVENTF_KEYUP = 0x2;
            const byte VK_MENU = 0x12;
            const byte VK_TAB = 0x09;
            const byte VK_CONTROL = 0x11;

            // create an INPUT structure with default values
            //INPUT input = new INPUT
            //{
            //    type = INPUT_KEYBOARD,
            //    ki = new KEYBDINPUT {dwExtraInfo = GetMessageExtraInfo(), dwFlags = 0, time = 0, wScan = 0}
            //};

            // Simulating a Alt+Tab keystroke
            KeyInputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.TAB);
            //keybd_event(VK_CONTROL, 0xb8, 0, (UIntPtr)0); //Alt Press
            //keybd_event(VK_TAB, 0x8f, 0, (UIntPtr)0); // Tab Press
            //keybd_event(VK_TAB, 0x8f, KEYEVENTF_KEYUP, (UIntPtr)0); // Tab Release
            //keybd_event(VK_CONTROL, 0xb8, KEYEVENTF_KEYUP, (UIntPtr)0); // Alt Release

            MouseInputSimulator.ClickRightMouseButton();
            MouseInputSimulator.ClickLeftMouseButton();
            //KeyInputSimulator.SimulateTextEntry(text);

            for (Int32 i = 0; i <= 20; i++)
            {
                Int32 key = rnd.Next(26);
                switch (key)
                {
                    case 1:
                        KeyInputSimulator.SimulateKeyPress(VirtualKeyCode.VK_A);
                        break;

                    case 2:
                        KeyInputSimulator.SimulateKeyPress(VirtualKeyCode.VK_B);
                        break;

                    case 3:
                        KeyInputSimulator.SimulateKeyPress(VirtualKeyCode.VK_C);
                        break;

                    case 4:
                        KeyInputSimulator.SimulateKeyPress(VirtualKeyCode.VK_D);
                        break;

                    case 5:
                        KeyInputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E);
                        break;

                    case 6:
                        KeyInputSimulator.SimulateKeyPress(VirtualKeyCode.VK_F);
                        break;

                    case 7:
                        KeyInputSimulator.SimulateKeyPress(VirtualKeyCode.VK_G);
                        break;

                    case 8:
                        KeyInputSimulator.SimulateKeyPress(VirtualKeyCode.VK_H);
                        break;

                    case 9:
                        KeyInputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I);
                        break;

                    case 10:
                        KeyInputSimulator.SimulateKeyPress(VirtualKeyCode.VK_J);
                        break;

                    case 11:
                        KeyInputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K);
                        break;

                    case 12:
                        KeyInputSimulator.SimulateKeyPress(VirtualKeyCode.VK_L);
                        break;

                    case 13:
                        KeyInputSimulator.SimulateKeyPress(VirtualKeyCode.VK_M);
                        break;

                    case 14:
                        KeyInputSimulator.SimulateKeyPress(VirtualKeyCode.VK_N);
                        break;

                    case 15:
                        KeyInputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O);
                        break;

                    case 16:
                        KeyInputSimulator.SimulateKeyPress(VirtualKeyCode.VK_P);
                        break;

                    case 17:
                        KeyInputSimulator.SimulateKeyPress(VirtualKeyCode.VK_Q);
                        break;

                    case 18:
                        KeyInputSimulator.SimulateKeyPress(VirtualKeyCode.VK_R);
                        break;

                    case 19:
                        KeyInputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S);
                        break;

                    case 20:
                        KeyInputSimulator.SimulateKeyPress(VirtualKeyCode.VK_T);
                        break;

                    case 21:
                        KeyInputSimulator.SimulateKeyPress(VirtualKeyCode.VK_U);
                        break;

                    case 22:
                        KeyInputSimulator.SimulateKeyPress(VirtualKeyCode.VK_V);
                        break;

                    case 23:
                        KeyInputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W);
                        break;

                    case 24:
                        KeyInputSimulator.SimulateKeyPress(VirtualKeyCode.VK_X);
                        break;

                    case 25:
                        KeyInputSimulator.SimulateKeyPress(VirtualKeyCode.VK_Y);
                        break;

                    case 26:
                        KeyInputSimulator.SimulateKeyPress(VirtualKeyCode.VK_Z);
                        break;
                }
            }
            //KeyInputSimulator.SimulateKeyPress(VirtualKeyCode.NUMLOCK);
            foreach (char c in text)
            {
                //KeyInputSimulator.SimulateKeyPress(VirtualKeyCode.);
                //keybd_event((byte)VkKeyScan(c), 0x45, KEYEVENTF_EXTENDEDKEY | 0, (UIntPtr)0);
                //keybd_event((byte)VkKeyScan(c), 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, (UIntPtr)0);
            }
            //foreach (char c in text)
            //{
            //    short key = VkKeyScan(c);                       //variable where we will hold the value of each key
            //    input.ki.wVk = (ushort)key;                     //update the input structure
            //    SendInput(1, ref input, Marshal.SizeOf(input)); //send the key to notepad
            //}
        }
    }
}