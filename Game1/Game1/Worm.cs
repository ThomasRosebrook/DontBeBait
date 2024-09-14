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
        public BoundingRectangle bounds;

        //bool flipped;
        int animationIndex = 0;

        float animationTimer = 0;

        public Worm (Vector2 position)
        {
            Position = position;
            bounds = new BoundingRectangle(position, 192, 98);
        }

        public void LoadContent(ContentManager contentManager)
        {
            texture = contentManager.Load<Texture2D>("Worm");
        }

        public void Update(GameTime gameTime, Vector2 direction)
        {
            Position += direction;

            //if (direction.X > 0) flipped = true;
            //else flipped = false;

            animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
            if ((direction.X != 0 || direction.Y != 0) && animationTimer >= 20)
            {
                animationTimer = 0;
                if (animationIndex == 0) animationIndex = 1;
                else animationIndex = 0;
            }

            bounds.X = Position.X;
            bounds.Y = Position.Y;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(texture, Position, null, Color.White);
            //SpriteEffects spriteEffects = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None ;

            spriteBatch.Draw(texture, Position, new Rectangle(animationIndex * 256, 0, 256, 256), Color.White, 0, new Vector2(128, 128), 1f, SpriteEffects.None, 0);
        }
    }
}
