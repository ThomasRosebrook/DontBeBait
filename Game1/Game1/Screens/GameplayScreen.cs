using Game1.StateManagement;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace Game1.Screens
{
    public class GameplayScreen : GameScreen
    {
        private ContentManager _content;
        private SpriteFont _gameFont;
        private int width;
        private int height;

        private Worm worm;
        private List<Fish> fishes;
        private Background background;
        private FishingLine line;
        private int depth = 0;
        float depthTimer = 0;
        Vector2 wormDirection = new Vector2(0,0);

        private Song BeginningBackgroundMusic;
        private Song EndingBackgroundMusic;

        private SoundEffect nomSound;
        private SoundEffect crunchSound;
        float NomTimer = 0;

        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void Activate()
        {
            if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");

            if (width <= 0) width = ScreenManager.GraphicsDevice.Viewport.Width;
            if (height <= 0) height = ScreenManager.GraphicsDevice.Viewport.Height;

            worm = new Worm(new Vector2(width / 2 - 22, height / 2 - 100));
            worm.LoadContent(_content);

            line = new FishingLine(worm.Position - new Vector2(-22, 2), new Vector2(width / 2, -100));
            line.LoadContent(_content);

            fishes = new List<Fish>()
            {
                new Fish(FishType.Small, new Vector2(RandomHelper.Next(width - 128), RandomHelper.Next((int)worm.Position.Y + 256, height - 128)))
            };

            foreach (Fish fish in fishes)
            {
                fish.LoadContent(_content);
            }

            background = new Background();
            background.LoadContent(_content);

            _gameFont = _content.Load<SpriteFont>("Lusitana");

            BeginningBackgroundMusic = _content.Load<Song>("Funk Game Loop");
            EndingBackgroundMusic = _content.Load<Song>("Wah Game Loop");

            Fish.PopSound1 = _content.Load<SoundEffect>("Pop1");
            Fish.PopSound2 = _content.Load<SoundEffect>("Pop2");
            nomSound = _content.Load<SoundEffect>("Nom");
            crunchSound = _content.Load<SoundEffect>("Crunch");

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.3f;
            MediaPlayer.Play(BeginningBackgroundMusic);
        }

        public override void Unload()
        {
            _content.Unload();
            fishes.Clear();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            if (IsActive)
            {
                if (worm.IsAlive)
                {
                    if (gameTime.TotalGameTime.TotalSeconds - depthTimer >= 0.05f)
                    {
                        depthTimer = (float)gameTime.TotalGameTime.TotalSeconds;
                        depth++;
                        if (depth % 150 == 0) AddFish();
                    }

                    if (depth == 700)
                    {
                        MediaPlayer.Play(EndingBackgroundMusic);
                    }

                    background.Update(gameTime, depth);

                    if (wormDirection.X > 0 && worm.Position.X >= width - 100) wormDirection.X = 0;
                    if (wormDirection.X < 0 && worm.Position.X <= 100) wormDirection.X = 0;
                    if (wormDirection.Y < 0 && worm.Position.Y <= 50) wormDirection.Y = 0;
                    if (wormDirection.Y > 0 && worm.Position.Y >= height - 50) wormDirection.Y = 0;

                    worm.Update(gameTime, wormDirection);
                    line.Update(gameTime, wormDirection);
                }
                
                foreach (Fish fish in fishes)
                {
                    fish.Update(gameTime, new Vector2(worm.Position.X, worm.Position.Y));
                    if (fish.bounds.CollidesWith(worm.HeadBounds) || fish.bounds.CollidesWith(worm.BodyBounds))
                    {
                        if (worm.IsAlive) worm.IsAlive = false;
                        NomTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (NomTimer <= 0f)
                        {
                            if (fish.fishType == FishType.Large) crunchSound.Play();
                            else nomSound.Play();
                            NomTimer = 1;
                        }
                        
                    }
                }
            }
        }

        public override void HandleInput(GameTime gameTime, InputManager input)
        {
            base.HandleInput(gameTime, input);
            //if (input.Exit) Exit();

            if (input.Exit)
            {
                //input.Exit = false;
                MediaPlayer.Stop();
                ExitScreen();
                ScreenManager.AddScreen(new MainMenuScreen(), null);
            }

            wormDirection = new Vector2(input.Direction.X, input.Direction.Y);
            if (!worm.IsAlive && input.spaceBarPressed) ResetGame();

        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

            var spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            background.Draw(gameTime, spriteBatch);

            worm.Draw(gameTime, spriteBatch);
            line.Draw(gameTime, spriteBatch);
            foreach (Fish fish in fishes)
            {
                fish.Draw(gameTime, spriteBatch);
            }

            string depthText = $"{depth} feet";
            Vector2 depthTextSize = _gameFont.MeasureString(depthText);
            spriteBatch.DrawString(_gameFont, depthText, new Vector2(width / 2 - depthTextSize.X / 2, 20), Color.Black);
            if (!worm.IsAlive)
            {
                string retryText = "Press SPACE or A to restart";
                Vector2 retryTextSize = _gameFont.MeasureString(retryText);
                spriteBatch.DrawString(_gameFont, retryText, new Vector2(width / 2 - retryTextSize.X / 2, 200), Color.Black);

                retryText = "Press ESC or Back to return to menu";
                retryTextSize = _gameFont.MeasureString(retryText);
                spriteBatch.DrawString(_gameFont, retryText, new Vector2(width / 2 - retryTextSize.X / 2, 300), Color.Black);
            }

            spriteBatch.End();
        }

        void AddFish()
        {
            FishType fishType = FishType.Small;
            if (depth > 200 && RandomHelper.Next(0, 500 / depth) <= 0) fishType = FishType.Medium;
            if (depth > 400 && RandomHelper.Next(0, 1000 / depth) <= 0) fishType = FishType.Large;
            fishes.Add(new Fish(fishType, new Vector2(RandomHelper.Next(width - 128), height)));
        }

        void ResetGame()
        {
            worm.IsAlive = true;
            depth = 0;
            worm.Position = new Vector2(width / 2 - 22, height / 2 - 100);
            line.Position = worm.Position - new Vector2(-22, 2);
            fishes.Clear();
            fishes.Add(new Fish(FishType.Small, new Vector2(RandomHelper.Next(width - 128), RandomHelper.Next((int)worm.Position.Y + 256, height - 128))));
            MediaPlayer.Play(BeginningBackgroundMusic);
        }
    }
}
