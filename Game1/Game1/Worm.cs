using System;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    public class Worm
    {
        public Vector2 Position;
        Texture2D texture;
        public BoundingRectangle HeadBounds;
        public BoundingRectangle BodyBounds;
        Vector2 HeadOffset = new Vector2(-46, 20);
        Vector2 BodyOffset = new Vector2(49, -36);

        bool flipped;
        int animationIndex = 0;
        public bool IsAlive = true;

        float animationTimer = 0;

        public Worm (Vector2 position)
        {
            Position = position;
            HeadBounds = new BoundingRectangle(position + HeadOffset, 100, 58);
            BodyBounds = new BoundingRectangle(position + BodyOffset, 90, 30);
        }

        public void LoadContent(ContentManager contentManager)
        {
            texture = contentManager.Load<Texture2D>("Worm");
        }

        public void Update(GameTime gameTime, Vector2 direction)
        {
            Position += direction;

            if (direction.X > 0) flipped = true;
            else flipped = false;

            animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
            if ((direction.X != 0 || direction.Y != 0) && animationTimer >= 20)
            {
                animationTimer = 0;
                if (animationIndex == 0) animationIndex = 1;
                else animationIndex = 0;
            }

            HeadBounds.X = Position.X + HeadOffset.X;
            HeadBounds.Y = Position.Y + HeadOffset.Y;
            BodyBounds.X = Position.X + BodyOffset.X;
            BodyBounds.Y = Position.Y + BodyOffset.Y;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(texture, Position, null, Color.White);
            SpriteEffects spriteEffects = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None ;

            spriteBatch.Draw(texture, Position, new Rectangle(animationIndex * 256, 0, 256, 256), Color.White, 0, new Vector2(128, 128), 1f, spriteEffects, 0);
        }
    }
}
