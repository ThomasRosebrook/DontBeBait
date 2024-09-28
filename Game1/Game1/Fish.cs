using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    public enum FishType
    {
        Small,
        Medium,
        Large
    }

    public class Fish
    {
        public BoundingCircle bounds;

        public FishType fishType;
        Vector2 position;
        int velocity = 20;
        Vector2 direction;
        Texture2D texture;
        float angle;

        static Texture2D smallFishTexture;
        static Texture2D mediumFishTexture;
        static Texture2D largeFishTexture;

        public static SoundEffect PopSound1;
        public static SoundEffect PopSound2;

        bool flipped;

        int mouthIndex = 0;
        int finIndex = 0;
        int finAdd = 1;

        float finTimer = 0;
        float mouthTimer = 0;

        public Fish (FishType type, Vector2 position)
        {
            fishType = type;
            this.position = position;

            float radius = 0;
            switch (type)
            {
                case FishType.Small:
                    velocity = 40;
                    radius = 60;
                    texture = smallFishTexture;
                    break;
                case FishType.Medium:
                    velocity = 30;
                    radius = 90;
                    texture = mediumFishTexture;
                    break;
                case FishType.Large:
                    velocity = 20;
                    radius = 120;
                    texture = largeFishTexture;
                    break;
            }

            bounds = new BoundingCircle(position, radius);
        }

        public void LoadContent(ContentManager contentManager)
        {
            switch (fishType)
            {
                case FishType.Small:
                    texture = smallFishTexture;
                    break;
                case FishType.Medium:
                    texture = mediumFishTexture;
                    break;
                case FishType.Large:
                    texture = largeFishTexture;
                    break;
            }
        }

        public void Update(GameTime gameTime, Vector2 target)
        {
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            finTimer += t * 100;
            if (finTimer >= 10)
            {
                finTimer = 0;
                if (finIndex == 2) finAdd = -1;
                else if (finIndex == 0) finAdd = 1;
                finIndex += finAdd;
                
            }

            float dist = (float)Math.Pow(target.X - position.X, 2) + (float)Math.Pow(target.Y - position.Y, 2);

            mouthTimer += t * 100;
            if (dist < 125000 && mouthTimer >= 12)
            {
                mouthTimer = 0;
                if (mouthIndex == 0)
                {
                    mouthIndex = 1;
                    PopSound1.Play();
                    //if (RandomHelper.Next(0, 1) == 0) PopSound1.Play();
                    //else PopSound2.Play();
                }
                else mouthIndex = 0;
            }

            flipped = target.X - position.X < 0;
            angle = (float)Math.Atan(Math.Abs((target.Y - position.Y) / (target.X - position.X)));

            if (flipped) angle = -angle;
            if (target.Y - position.Y < 0) angle = -angle;

            direction.X = (float)Math.Cos(angle);
            direction.Y = (float)Math.Sin(angle);

            if (flipped) position -= direction * velocity * t;
            else position += direction * velocity * t;

            bounds.Center = position;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            SpriteEffects spriteEffects = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(texture, position, new Rectangle(finIndex * 256, mouthIndex * 256, 256, 256), Color.White, angle, new Vector2(128, 128), 1f, spriteEffects, 0);
        }

        public static void LoadFishTextures(ContentManager content)
        {
            if (smallFishTexture == null) smallFishTexture = content.Load<Texture2D>("small_fish");
            if (mediumFishTexture == null) mediumFishTexture = content.Load<Texture2D>("medium_fish");
            if (largeFishTexture == null) largeFishTexture = content.Load<Texture2D>("big_fish");
        }
    }
}
