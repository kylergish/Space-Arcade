using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceArcade.Collisions;

namespace SpaceArcade.Sprites
{
    public class SpaceShip
    {
        Game game;

        GamePadState gamePadState;
        KeyboardState keyboardState;

        Texture2D texture;
        Vector2 position = new Vector2(250, 250);
        Vector2 velocity;
        Vector2 direction;
        BoundingCircle bounds = new BoundingCircle(new Vector2(50, 50), 30);

        const float ANGULAR_ACCELERATION = 5;
        const float LINEAR_ACCELERATION = 10;
        float angle;
        float angularVelocity;

        public BoundingCircle Bounds => bounds;

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("sheet");
        }

        public void Update(GameTime gameTime)
        {
            gamePadState = GamePad.GetState(0);
            keyboardState = Keyboard.GetState();

            position += gamePadState.ThumbSticks.Left * new Vector2(1, -1);

            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 acceleration = new Vector2(0, 0);
            float angularAcceleration = 0;

            if(keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                acceleration += direction * LINEAR_ACCELERATION;
                angularAcceleration += ANGULAR_ACCELERATION;
            }
            if(keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                acceleration += direction * LINEAR_ACCELERATION;
                angularAcceleration -= ANGULAR_ACCELERATION;
            }

            angularVelocity += angularAcceleration * t;
            angle += angularVelocity * t;

            direction.X = (float)Math.Sin(angle);
            direction.Y = (float)-Math.Cos(angle);

            velocity += acceleration * t;
            position += velocity * t;

            var viewport = game.GraphicsDevice.Viewport;
            if (position.Y < 0) position.Y = viewport.Height;
            if (position.Y > viewport.Height) position.Y = 0;
            if (position.X < 0) position.X = viewport.Width;
            if (position.X > viewport.Width) position.X = 0;

            bounds.Center = position;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, new Rectangle(326, 0, 96, 74), Color.White, angle, new Vector2(47, 41), 1.0f, SpriteEffects.None, 0);
        }
    }
}
