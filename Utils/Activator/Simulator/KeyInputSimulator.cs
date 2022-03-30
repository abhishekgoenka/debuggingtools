using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Activator.Enums;

namespace Activator.Simulator
{
    public class KeyInputSimulator
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern short GetAsyncKeyState(ushort virtualKeyCode);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern short GetKeyState(ushort virtualKeyCode);

        [DllImport("user32.dll")]
        private static extern IntPtr GetMessageExtraInfo();

        public static bool IsKeyDown(VirtualKeyCode keyCode)
        {
            return (GetKeyState((ushort) keyCode) < 0);
        }

        public static bool IsKeyDownAsync(VirtualKeyCode keyCode)
        {
            return (GetAsyncKeyState((ushort) keyCode) < 0);
        }

        public static bool IsTogglingKeyInEffect(VirtualKeyCode keyCode)
        {
            return ((GetKeyState((ushort) keyCode) & 1) == 1);
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint numberOfInputs, INPUT[] inputs, int sizeOfInputStructure);

        public static void SimulateKeyDown(VirtualKeyCode keyCode)
        {
            var down = new INPUT
            {
                Type = 1
            };
            down.Data.Keyboard = new KEYBDINPUT();
            down.Data.Keyboard.Vk = (ushort) keyCode;
            down.Data.Keyboard.Scan = 0;
            down.Data.Keyboard.Flags = 0;
            down.Data.Keyboard.Time = 0;
            down.Data.Keyboard.ExtraInfo = IntPtr.Zero;
            INPUT[] inputList = {down};
            if (SendInput(1, inputList, Marshal.SizeOf(typeof (INPUT))) == 0)
            {
                throw new Exception(string.Format("The key down simulation for {0} was not successful.", keyCode));
            }
        }

        public static void SimulateKeyPress(VirtualKeyCode keyCode)
        {
            var down = new INPUT
            {
                Type = 1
            };
            down.Data.Keyboard = new KEYBDINPUT();
            down.Data.Keyboard.Vk = (ushort) keyCode;
            down.Data.Keyboard.Scan = 0;
            down.Data.Keyboard.Flags = 0;
            down.Data.Keyboard.Time = 0;
            down.Data.Keyboard.ExtraInfo = IntPtr.Zero;
            var up = new INPUT
            {
                Type = 1
            };
            up.Data.Keyboard = new KEYBDINPUT();
            up.Data.Keyboard.Vk = (ushort) keyCode;
            up.Data.Keyboard.Scan = 0;
            up.Data.Keyboard.Flags = 2;
            up.Data.Keyboard.Time = 0;
            up.Data.Keyboard.ExtraInfo = IntPtr.Zero;
            INPUT[] inputList = {down, up};
            if (SendInput(2, inputList, Marshal.SizeOf(typeof (INPUT))) == 0)
            {
                throw new Exception(string.Format("The key press simulation for {0} was not successful.", keyCode));
            }
        }

        public static void SimulateKeyUp(VirtualKeyCode keyCode)
        {
            var up = new INPUT
            {
                Type = 1
            };
            up.Data.Keyboard = new KEYBDINPUT();
            up.Data.Keyboard.Vk = (ushort) keyCode;
            up.Data.Keyboard.Scan = 0;
            up.Data.Keyboard.Flags = 2;
            up.Data.Keyboard.Time = 0;
            up.Data.Keyboard.ExtraInfo = IntPtr.Zero;
            INPUT[] inputList = {up};
            if (SendInput(1, inputList, Marshal.SizeOf(typeof (INPUT))) == 0)
            {
                throw new Exception(string.Format("The key up simulation for {0} was not successful.", keyCode));
            }
        }

        public static void SimulateModifiedKeyStroke(IEnumerable<VirtualKeyCode> modifierKeyCodes,
            IEnumerable<VirtualKeyCode> keyCodes)
        {
            if (modifierKeyCodes != null)
            {
                modifierKeyCodes.ToList().ForEach(SimulateKeyDown);
            }
            if (keyCodes != null)
            {
                keyCodes.ToList().ForEach(SimulateKeyPress);
            }
            if (modifierKeyCodes != null)
            {
                modifierKeyCodes.Reverse().ToList<VirtualKeyCode>().ForEach(SimulateKeyUp);
            }
        }

        public static void SimulateModifiedKeyStroke(IEnumerable<VirtualKeyCode> modifierKeyCodes,
            VirtualKeyCode keyCode)
        {
            if (modifierKeyCodes != null)
            {
                modifierKeyCodes.ToList().ForEach(x => SimulateKeyDown(x));
            }
            SimulateKeyPress(keyCode);
            if (modifierKeyCodes != null)
            {
                modifierKeyCodes.Reverse().ToList<VirtualKeyCode>().ForEach(x => SimulateKeyUp(x));
            }
        }

        public static void SimulateModifiedKeyStroke(VirtualKeyCode modifierKey, IEnumerable<VirtualKeyCode> keyCodes)
        {
            SimulateKeyDown(modifierKey);
            if (keyCodes != null)
            {
                keyCodes.ToList().ForEach(x => SimulateKeyPress(x));
            }
            SimulateKeyUp(modifierKey);
        }

        public static void SimulateModifiedKeyStroke(VirtualKeyCode modifierKeyCode, VirtualKeyCode keyCode)
        {
            SimulateKeyDown(modifierKeyCode);
            SimulateKeyPress(keyCode);
            SimulateKeyUp(modifierKeyCode);
        }

        public static void SimulateTextEntry(string text)
        {
            if (text.Length > 0x7fffffffL)
            {
                throw new ArgumentException(
                    string.Format("The text parameter is too long. It must be less than {0} characters.", 0x7fffffff),
                    "text");
            }
            byte[] chars = Encoding.ASCII.GetBytes(text);
            int len = chars.Length;
            var inputList = new INPUT[len*2];
            for (int x = 0; x < len; x++)
            {
                ushort scanCode = chars[x];
                var down = new INPUT
                {
                    Type = 1
                };
                down.Data.Keyboard = new KEYBDINPUT();
                down.Data.Keyboard.Vk = 0;
                down.Data.Keyboard.Scan = scanCode;
                down.Data.Keyboard.Flags = 4;
                down.Data.Keyboard.Time = 0;
                down.Data.Keyboard.ExtraInfo = IntPtr.Zero;
                var up = new INPUT
                {
                    Type = 1
                };
                up.Data.Keyboard = new KEYBDINPUT();
                up.Data.Keyboard.Vk = 0;
                up.Data.Keyboard.Scan = scanCode;
                up.Data.Keyboard.Flags = 6;
                up.Data.Keyboard.Time = 0;
                up.Data.Keyboard.ExtraInfo = IntPtr.Zero;
                if ((scanCode & 0xff00) == 0xe000)
                {
                    down.Data.Keyboard.Flags |= 1;
                    up.Data.Keyboard.Flags |= 1;
                }
                inputList[2*x] = down;
                inputList[(2*x) + 1] = up;
            }
            uint numberOfSuccessfulSimulatedInputs = SendInput((uint) (len*2), inputList, Marshal.SizeOf(typeof (INPUT)));
        }
    }
}