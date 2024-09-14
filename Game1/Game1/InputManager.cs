using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class InputManager
    {
        //MouseState currentMouseState;
        //MouseState previousMouseState;

        GamePadState currentGamePadState;
        GamePadState previousGamePadState;

        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        public int Velocity = 100;

        public bool spaceBarPressed { get; private set; } = false;

        public bool Exit { get; private set; } = false;

        public Vector2 Direction { get; private set; }

        public void Update(GameTime gameTime)
        {
            //previousMouseState = currentMouseState;
            //currentMouseState = Mouse.GetState();

            previousGamePadState = currentGamePadState;
            currentGamePadState = GamePad.GetState(0);

            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Direction = currentGamePadState.ThumbSticks.Left * Velocity * time * new Vector2(1, -1);

            if (currentKeyboardState.IsKeyDown(Keys.Left)
                || currentKeyboardState.IsKeyDown(Keys.A))
            {
                Direction += new Vector2(-Velocity * time, 0);
            }

            if (currentKeyboardState.IsKeyDown(Keys.Right)
                || currentKeyboardState.IsKeyDown(Keys.D))
            {
                Direction += new Vector2(Velocity * time, 0);
            }

            if (currentKeyboardState.IsKeyDown(Keys.Up)
                || currentKeyboardState.IsKeyDown(Keys.W))
            {
                Direction += new Vector2(0, -Velocity * time);
            }

            if (currentKeyboardState.IsKeyDown(Keys.Down)
                || currentKeyboardState.IsKeyDown(Keys.S))
            {
                Direction += new Vector2(0, Velocity * time);
            }


            spaceBarPressed = false;
            if ((currentKeyboardState.IsKeyDown(Keys.Space) && previousKeyboardState.IsKeyUp(Keys.Space)) 
                || (currentGamePadState.Buttons.A == ButtonState.Pressed && previousGamePadState.Buttons.A == ButtonState.Released))
            {
                spaceBarPressed = true;
            }

            if (currentGamePadState.Buttons.Back == ButtonState.Pressed || currentKeyboardState.IsKeyDown(Keys.Escape))
            {
                Exit = true;
            }
        }
    }
}
