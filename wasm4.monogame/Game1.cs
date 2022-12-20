using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Linq;

using WASM4;

using WebAssembly;
using WebAssembly.Runtime;

namespace wasm4.monogame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private IMemoryAccessor memory;
        private IRuntime runtime;
        private Instance<WASM4Runtime> instance;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferHeight = 160;
            _graphics.PreferredBackBufferWidth = 160;

            var t = Compile.FromBinary<WASM4Runtime>("mouse-demo.wasm");
            memory = new MemoryAccessor();
            runtime = new Runtime(memory);
            instance = t(new ImportDictionary
            {
                {"env", "memory", new MemoryImport(()=>memory.GetMemory()) },
                {"env", "text", new FunctionImport(runtime.text) },//new Action<int,int,int>((p,x,y)=>Console.WriteLine("t{0},{1},{2},{3}",p,x,y,GetText(p)))) },
                {"env", "blit", new FunctionImport(runtime.blit) },//new Action<int,int,int,int,int,int>((p,x,y,w,h,f)=>Console.WriteLine("b{0},{1},{2},{3},{4},{5}",p,x,y,w,h,f))) },
            });
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var keyboardState = Keyboard.GetState();
            var gamepadState = GamePad.GetState(PlayerIndex.One);
            var mouseState = Mouse.GetState();
            var mouseX = mouseState.X;
            var mouseY = mouseState.Y;
            int gamepad = 0;
            int mouseButton = 0;

            memory.WriteByte(WASM4.Constants.MemoryLayout.ADDR_MOUSE_X, (byte)mouseX);
            memory.WriteByte(WASM4.Constants.MemoryLayout.ADDR_MOUSE_Y, (byte)mouseY);
            memory.WriteByte(WASM4.Constants.MemoryLayout.ADDR_MOUSE_BUTTONS, (byte)mouseButton);

            instance.Exports.update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            using (var rendered = CreateTexture(GraphicsDevice, 160, 160, i =>
            {
                var c = runtime.Render(i / 160, i % 160);
                return new Color(c.r, c.g, c.b, c.a);
            }))
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(rendered, Vector2.Zero, Color.White);
                _spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        public static Texture2D CreateTexture(GraphicsDevice device, int width, int height, Func<int, Color> paint)
        {
            //initialize a texture
            Texture2D texture = new Texture2D(device, width, height);

            //the array holds the color for each pixel in the texture
            Color[] data = new Color[width * height];
            for (int pixel = 0; pixel < data.Count(); pixel++)
            {
                //the function applies the color according to the specified pixel
                data[pixel] = paint(pixel);
            }

            //set the color
            texture.SetData(data);

            return texture;
        }
    }
}