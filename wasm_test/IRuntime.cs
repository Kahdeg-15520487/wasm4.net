namespace WASM4
{
    public interface IRuntime
    {
        void text(int pointer, int x, int y);
        void blit(int pointer, int x, int y, int width, int height, int flag);

        (int r, int g, int b, int a) Render(int x, int y);
    }
}