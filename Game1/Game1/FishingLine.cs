using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.MediaFoundation;

namespace Game1
{
    public class FishingLine
    {
        public Vector2 Position;
        Texture2D texture;
        float angle;
        Vector2 topPos;

        public FishingLine(Vector2 hookPosition, Vector2 topPosition)
        {
            Position = hookPosition;
            topPos = topPosition;
        }

        public void LoadContent(ContentManager contentManager)
        {
            texture = contentManager.Load<Texture2D>("fishing_line");
        }

        public void Update(GameTime gameTime, Vector2 direction)
        {
            Position += direction;
            
            bool flipped = Position.X - topPos.X > 0;

            angle = (float)Math.PI / 2 - (float)Math.Atan(Math.Abs((Position.Y - topPos.Y) / (Position.X - topPos.X)));
            if (flipped) angle = -angle;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, angle, new Vector2(4, 1080), 1f, SpriteEffects.None, 0);
        }
    }
}
