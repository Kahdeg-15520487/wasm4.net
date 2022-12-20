using System;

using WASM4;
using WASM4.Constants;

using WebAssembly;
using WebAssembly.Instructions;
using WebAssembly.Runtime;

var module = Module.ReadFromBinary("mouse-demo.wasm");

module.Imports.ToList().ForEach(Console.WriteLine);
module.Exports.ToList().ForEach(Console.WriteLine);

var t = Compile.FromBinary<WASM4Runtime>("mouse-demo.wasm");
var memory = new MemoryAccessor();
var runtime = new Runtime(memory);
using (var instance = t(new ImportDictionary
{
    {"env", "memory", new MemoryImport(()=>memory.GetMemory()) },
    {"env", "text", new FunctionImport(runtime.text) },//new Action<int,int,int>((p,x,y)=>Console.WriteLine("t{0},{1},{2},{3}",p,x,y,GetText(p)))) },
    {"env", "blit", new FunctionImport(runtime.blit) },//new Action<int,int,int,int,int,int>((p,x,y,w,h,f)=>Console.WriteLine("b{0},{1},{2},{3},{4},{5}",p,x,y,w,h,f))) },
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
        memory.WriteByte(MemoryLayout.ADDR_MOUSE_BUTTONS, (byte)mbs);
        instance.Exports.update();
    }
}

Console.ReadLine();