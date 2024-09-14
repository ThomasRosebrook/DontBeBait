using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using Microsoft.VisualBasic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace Game1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        InputManager inputManager;

        Random rand;

        Worm worm;
        List<Fish> fishes;
        Background background;
        FishingLine line;

        bool Alive = true;

        int width;
        int height;
        int depth = 0;
        float depthTimer = 0;
        SpriteFont lusitana;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.Title = "Don't Be Bait";

            _graphics.PreferredBackBufferWidth = 1500;
            _graphics.PreferredBackBufferHeight = 852;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            width = _graphics.GraphicsDevice.Viewport.Width;
            height = _graphics.GraphicsDevice.Viewport.Height;

            rand = new Random();
            worm = new Worm(new Vector2(width / 2 - 22, height / 2 - 100));
            line = new FishingLine(worm.Position - new Vector2(-22,2), new Vector2(width / 2, -100));
            fishes = new List<Fish>() 
            { 
                new Fish(FishType.Small, new Vector2(rand.Next(width - 128), rand.Next((int)worm.Position.Y + 256, height - 128)))
            };
            background = new Background();
            inputManager = new InputManager();
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Fish.smallFishTexture = Content.Load<Texture2D>("small_fish");
            Fish.mediumFishTexture = Content.Load<Texture2D>("medium_fish");
            Fish.largeFishTexture = Content.Load<Texture2D>("big_fish");
            lusitana = Content.Load<SpriteFont>("Lusitana");

            background.LoadContent(Content);
            worm.LoadContent(Content);
            line.LoadContent(Content);
            foreach (Fish fish in fishes)
            {
                fish.LoadContent(Content);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            inputManager.Update(gameTime);
            if (inputManager.Exit) Exit();

            // TODO: Add your update logic here
            if (Alive)
            {
                if (gameTime.TotalGameTime.TotalSeconds - depthTimer >= 0.05f)
                {
                    depthTimer = (float)gameTime.TotalGameTime.TotalSeconds;
                    depth++;
                    if (depth % 100 == 0) AddFish();
                }

                background.Update(gameTime, depth);

                Vector2 wormDirection = new Vector2(inputManager.Direction.X, inputManager.Direction.Y);
                if (wormDirection.X > 0 && worm.Position.X >= width - 100) wormDirection.X = 0;
                if (wormDirection.X < 0 && worm.Position.X <= 100) wormDirection.X = 0;
                if (wormDirection.Y < 0 && worm.Position.Y <= 50) wormDirection.Y = 0;
                if (wormDirection.Y > 0 && worm.Position.Y >= height - 50) wormDirection.Y = 0;

                worm.Update(gameTime, wormDirection);
                line.Update(gameTime, wormDirection);
            }
            else
            {
                if (inputManager.spaceBarPressed)
                {
                    Alive = true;
                    depth = 0;
                    worm.Position = new Vector2(width / 2 - 22, height / 2 - 100);
                    line.Position = worm.Position - new Vector2(-22, 2);
                    fishes.Clear();
                    fishes.Add(new Fish(FishType.Small, new Vector2(rand.Next(width - 128), rand.Next((int)worm.Position.Y + 256, height - 128))));
                }
            }

            foreach (Fish fish in fishes)
            {
                fish.Update(gameTime, new Vector2(worm.Position.X, worm.Position.Y));
                if (Alive && fish.bounds.CollidesWith(worm.bounds)) Alive = false;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();
            background.Draw(gameTime, _spriteBatch);

            worm.Draw(gameTime, _spriteBatch);
            line.Draw(gameTime, _spriteBatch);
            foreach (Fish fish in fishes)
            {
                fish.Draw(gameTime, _spriteBatch);
            }

            _spriteBatch.DrawString(lusitana, $"{depth} feet", new Vector2(GraphicsDevice.DisplayMode.Width / 2 - 300, 20), Color.Black);

            if (!Alive) _spriteBatch.DrawString(lusitana, "Press SPACE or A to restart", new Vector2(GraphicsDevice.DisplayMode.Width / 2 - 750, 200), Color.Black);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void AddFish ()
        {
            FishType fishType = FishType.Small;
            if (depth > 200 && rand.Next(0, 500 / depth) <= 0) fishType = FishType.Medium;
            if (depth > 400 && rand.Next(0, 1000 / depth) <= 0) fishType = FishType.Large;
            fishes.Add(new Fish(fishType, new Vector2(rand.Next(width - 128), height)));
        }
    }
}