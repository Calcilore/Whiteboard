using Adofai.Render;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Whiteboard.Misc;
using Whiteboard.Render;

namespace Whiteboard {
    public class Main : Game {
        public static Main Game;
        public delegate void UpdateEventD();
        public static UpdateEventD StaticUpdateEvent;
        public static UpdateEventD UpdateEvent;
        public static UpdateEventD StaticDrawEvent;
        public static UpdateEventD DrawEvent;
        public static UpdateEventD StaticDrawHUDEvent;
        public static UpdateEventD DrawHUDEvent;
    
        public static GraphicsDeviceManager Graphics;
        public static SpriteBatch SpriteBatch;

        public static Color BackgroundColor;
    
        private FPSCounter fpsCounter;

        public Main() {
            Game = this;
        
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            IsFixedTimeStep = false;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize() {
            fpsCounter = new FPSCounter();
        
            ARender.Init();
            StaticUpdateEvent += Keyboard.Update;
            StaticUpdateEvent += Mouse.Update;
            StaticUpdateEvent += fpsCounter.Update;
            StaticDrawHUDEvent += fpsCounter.Draw;
            
            base.Initialize();
        }

        protected override void LoadContent() {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            for (int i = 0; i < ARender.Fonts.Length; i++) {
                ARender.Fonts[i] = Content.Load<SpriteFont>("Fonts/Font" + i);
            }
        
            SceneLoader.LoadScene(new LoadingScene());
        }

        protected override void Update(GameTime gameTime) {
            GTime.FromGameTime(gameTime);

            StaticUpdateEvent?.Invoke();
            UpdateEvent?.Invoke();

            Camera.UpdateCamera(GraphicsDevice.Viewport);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            ARender.DrawPre();
            SpriteBatch.Begin(sortMode:SpriteSortMode.FrontToBack, transformMatrix:Camera.Transform);
            DrawEvent?.Invoke();
            StaticDrawEvent?.Invoke();
            SpriteBatch.End();
        
            ARender.DrawPre();
            SpriteBatch.Begin(sortMode:SpriteSortMode.FrontToBack);
            DrawHUDEvent?.Invoke();
            StaticDrawHUDEvent?.Invoke();
            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}