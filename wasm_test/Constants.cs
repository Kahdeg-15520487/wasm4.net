using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WASM4.Constants
{
    public class MemoryLayout
    {
        public const int ADDR_PALETTE = 0x04;
        public const int ADDR_DRAW_COLORS = 0x14;
        public const int ADDR_GAMEPAD1 = 0x16;
        public const int ADDR_GAMEPAD2 = 0x17;
        public const int ADDR_GAMEPAD3 = 0x18;
        public const int ADDR_GAMEPAD4 = 0x19;
        public const int ADDR_MOUSE_X = 0x1a;
        public const int ADDR_MOUSE_Y = 0x1c;
        public const int ADDR_MOUSE_BUTTONS = 0x1e;
        public const int ADDR_SYSTEM_FLAGS = 0x1f;
        public const int ADDR_NETPLAY = 0x20;
        public const int ADDR_FRAMEBUFFER = 0xa0;
    }

    public class ButtonState
    {
        public const int BUTTON_X = 1;
        public const int BUTTON_Z = 2;
        // public const int BUTTON_RESERVED = 4;
        // public const int BUTTON_RESERVED = 8;
        public const int BUTTON_LEFT = 16;
        public const int BUTTON_RIGHT = 32;
        public const int BUTTON_UP = 64;
        public const int BUTTON_DOWN = 128;
    }
    public class MouseButtonState
    {
        public const int MOUSE_LEFT = 1;
        public const int MOUSE_RIGHT = 2;
        public const int MOUSE_MIDDLE = 4;
    }
}
