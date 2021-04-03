using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceArcade.Collisions;
using SpaceArcade.StateManagement;

namespace SpaceArcade.Sprites
{
    public class SpaceShip
    {
        GamePadState gamePadState;
        KeyboardState keyboardState;

        Texture2D texture;
        Texture2D flame;
        Vector2 position = new Vector2(250, 250);
        Vector2 velocity;
        Vector2 direction;
        BoundingCircle bounds = new BoundingCircle(new Vector2(50, 50), 30);

        bool rocketOn = false;

        const float ANGULAR_ACCELERATION = 5;
        const float LINEAR_ACCELERATION = 30;
        float angle;
        float angularVelocity;

        public BoundingCircle Bounds => bounds;

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("sheet");
            flame = content.Load<Texture2D>("Flame");
        }

        public void Update(GameTime gameTime, int screenWidth, int screenHeight)
        {
            gamePadState = GamePad.GetState(0);
            keyboardState = Keyboard.GetState();

            position += gamePadState.ThumbSticks.Left * new Vector2(1, -1);

            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 acceleration = new Vector2(0, 0);
            float angularAcceleration = 0;

            if(keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                //acceleration += direction * LINEAR_ACCELERATION;
                angularAcceleration -= ANGULAR_ACCELERATION;
            }
            if(keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                //acceleration += direction * LINEAR_ACCELERATION;
                angularAcceleration += ANGULAR_ACCELERATION;
            }
           

            if (acceleration.X != 0.0 || acceleration.Y != 0.0) acceleration -= direction * LINEAR_ACCELERATION;
            else
            {
                if (keyboardState.IsKeyDown(Keys.Space) || keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
                {
                    rocketOn = true;
                    acceleration += direction * LINEAR_ACCELERATION;
                }
                else rocketOn = false;
            }

            angularVelocity += angularAcceleration * t;
            angle += angularVelocity * t;

            direction.X = (float)Math.Sin(angle);
            direction.Y = (float)-Math.Cos(angle);

            velocity += acceleration * t;
            position += velocity * t;

            if (position.Y < 0) position.Y = screenHeight;
            if (position.Y > screenHeight) position.Y = 0;
            if (position.X < 0) position.X = screenWidth;
            if (position.X > screenWidth) position.X = 0;

            bounds.Center = position;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (rocketOn) spriteBatch.Draw(flame, position, new Rectangle(0, 0, 1024, 1024), Color.White, angle, new Vector2(500, -200), 0.1f, SpriteEffects.FlipVertically, 0);
            spriteBatch.Draw(texture, position, new Rectangle(326, 0, 96, 74), Color.White, angle, new Vector2(47, 41), 1.0f, SpriteEffects.None, 0);
        }
    }
}
