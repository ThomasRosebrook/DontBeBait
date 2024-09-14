using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class Background
    {
        int Depth;
        Texture2D texture;

        public Background()
        {
            Depth = 0;
        }

        public void LoadContent(ContentManager contentManager)
        {
            texture = contentManager.Load<Texture2D>("background");
        }

        public void Update(GameTime gameTime, int depth)
        {
            Depth = depth;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(0,0), new Rectangle(0, Depth, 1920, 1080), Color.White);
        }
    }
}
