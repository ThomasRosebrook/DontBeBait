using Microsoft.Xna.Framework;
using Game1.StateManagement;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Windows.Forms;

namespace Game1.Screens
{
    public class MainMenuScreen : GameScreen
    {
        private ContentManager _content;
        private SpriteFont _menuFont;
        private SpriteFont _gameFont;
        private int width;
        private int height;

        private Texture2D BackgroundTexture;

        public MainMenuScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void Activate()
        {
            if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");

            if (width <= 0) width = ScreenManager.GraphicsDevice.Viewport.Width;
            if (height <= 0) height = ScreenManager.GraphicsDevice.Viewport.Height;

            if (BackgroundTexture == null) BackgroundTexture = _content.Load<Texture2D>("MainMenu");
            _menuFont = _content.Load<SpriteFont>("LusitanaMenu");
            _gameFont = _content.Load<SpriteFont>("Lusitana");
        }

        public override void Unload()
        {
            _content.Unload();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (IsActive)
            {
                
            }
        }

        public override void HandleInput(GameTime gameTime, InputManager input)
        {
            base.HandleInput(gameTime, input);

            if (input.Exit) ScreenManager.Game.Exit();
            if (input.spaceBarPressed)
            {
                ScreenManager.AddScreen(new GameplayScreen(), null);
                this.ExitScreen();
            }

        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

            var spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            //192, 108
            //1536, 864
            spriteBatch.Draw(BackgroundTexture, new Vector2(0, 0), new Rectangle(192, 108, 1536, 864), Color.White);

            string currentText = "Don't Be Bait";
            Vector2 size = _menuFont.MeasureString(currentText);
            spriteBatch.DrawString(_menuFont, currentText, new Vector2(width / 2 - size.X / 2, 20), Color.HotPink);

            currentText = "Press SPACE or A Button to Start";
            size = _gameFont.MeasureString(currentText);
            spriteBatch.DrawString(_gameFont, currentText, new Vector2(width / 2 - size.X / 2, 200), Color.DarkBlue);

            currentText = "Exit: ESC or Back";
            size = _gameFont.MeasureString(currentText);
            spriteBatch.DrawString(_gameFont, currentText, new Vector2(width / 2 - size.X / 2, height - size.Y), Color.DarkBlue);

            spriteBatch.End();
        }
    }
}
