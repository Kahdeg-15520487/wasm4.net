namespace WASM4
{
    public class Runtime : IRuntime
    {
        private IMemoryAccessor memory;
        public Runtime(IMemoryAccessor memory)
        {
            this.memory = memory;
        }

        public void blit(int pointer, int x, int y, int width, int height, int flags)
        {
            var sprite = memory.ReadBytes(pointer, 8);
            bool bpp2 = (flags & 1) != 0;
            bool flipX = (flags & 2) != 0;
            bool flipY = (flags & 4) != 0;
            bool rotate = (flags & 8) != 0;
            for (int i = 0; i < 8; i++)
            {
                Console.WriteLine(Convert.ToString(sprite[i], 2).PadLeft(8, '0'));
            }
        }

        public (int r, int g, int b, int a) Render(int x, int y)
        {
            var p = memory.ReadByte(WASM4.Constants.MemoryLayout.ADDR_FRAMEBUFFER + x * 160 + y);
            return (255, 0, 0, 0);
        }

        public void text(int pointer, int x, int y)
        {
            Console.WriteLine("{0}|<{1},{2}>", memory.ReadString(pointer), x, y);
        }
    }
}