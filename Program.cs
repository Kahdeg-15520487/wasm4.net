using System;
using System.Runtime.InteropServices;

using WebAssembly;
using WebAssembly.Instructions;
using WebAssembly.Runtime;

var module = Module.ReadFromBinary("mouse-demo.wasm");

module.Imports.ToList().ForEach(Console.WriteLine);
module.Exports.ToList().ForEach(Console.WriteLine);

var t = Compile.FromBinary<wasm4>("mouse-demo.wasm");
var memory = new UnmanagedMemory(1, 1);
using (var instance = t(new ImportDictionary
{
    {"env", "memory", new MemoryImport(()=>memory) },
    {"env", "text", new FunctionImport(new Action<int,int,int>((p,x,y)=>Console.WriteLine("t{0},{1},{2},{3}",p,x,y,GetText(p)))) },
    {"env", "blit", new FunctionImport(new Action<int,int,int,int,int,int>((p,x,y,w,h,f)=>Console.WriteLine("b{0},{1},{2},{3},{4},{5}",p,x,y,w,h,f))) },
}))
{
    for (int i = 0; i < 6000; i++)
    {
        var mbs = 0;
        if (i % 2 == 0)
        {
            mbs = mbs | MouseButtonState.MOUSE_LEFT;
        }
        if (i % 3 == 0)
        {
            mbs = mbs | MouseButtonState.MOUSE_RIGHT;
        }
        if (i % 5 == 0)
        {
            mbs = mbs | MouseButtonState.MOUSE_MIDDLE;
        }
        SetByte(MemoryLayout.ADDR_MOUSE_BUTTONS, (byte)mbs);
        instance.Exports.update();
    }
}

Console.ReadLine();

void SetByte(int pointer, byte value)
{
    Marshal.WriteByte(memory.Start + pointer, value);
}

string GetText(int pointer)
{
    List<char> bytes = new List<char>();
    var p = pointer;
    while (Marshal.ReadByte(memory.Start + p) != 0)
    {
        bytes.Add((char)Marshal.ReadByte(memory.Start + p));
        p++;
    }
    return new string(bytes.ToArray());
}

public abstract class wasm4
{
    public abstract void update();
}

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