using WebAssembly.Runtime;

namespace WASM4
{
    public interface IMemoryAccessor
    {
        UnmanagedMemory GetMemory();
        byte ReadByte(int pointer);
        void WriteByte(int pointer, byte value);
        string ReadString(int pointer);
        byte[] ReadBytes(int pointer, int length);
    }
}