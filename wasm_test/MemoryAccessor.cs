using System.Runtime.InteropServices;

using WebAssembly.Runtime;

namespace WASM4
{
    public class MemoryAccessor : IMemoryAccessor
    {
        private UnmanagedMemory memory;
        public MemoryAccessor()
        {
            memory = new UnmanagedMemory(1, 1);
        }

        public UnmanagedMemory GetMemory() => memory;

        public byte ReadByte(int pointer) => Marshal.ReadByte(memory.Start + pointer);

        public byte[] ReadBytes(int pointer, int length)
        {
            byte[] result = new byte[length];
            Marshal.Copy(memory.Start + pointer, result, 0, length);
            return result;
        }

        public string ReadString(int pointer)
        {
            List<char> bytes = new List<char>();
            var p = pointer;
            while (ReadByte(p) != 0)
            {
                bytes.Add((char)ReadByte(p));
                p++;
            }
            return new string(bytes.ToArray());
        }

        public void WriteByte(int pointer, byte value) => Marshal.WriteByte(memory.Start + pointer, value);
    }
}