using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceArcade.Collisions;

namespace SpaceArcade.Sprites
{
    public class Asteroid
    {
        GraphicsDevice graphics;

        Texture2D texture;
        Vector2 position;
        Vector2 velocity;
        BoundingCircle bounds;

        public BoundingCircle Bounds => bounds;

        public Asteroid(Vector2 position, Vector2 velocity, GraphicsDevice graphics)
        {
            this.position = position;
            this.velocity = velocity;
            this.graphics = graphics;
            this.bounds = new BoundingCircle(position - new Vector2(-60, -50), 50);
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("sheet");
        }

        public void Update(GameTime gameTime)
        {
            position += (float)gameTime.ElapsedGameTime.TotalSeconds * velocity * 60;

            if (position.X > graphics.Viewport.Width || position.X < graphics.Viewport.X) velocity.X *= -1;
            if (position.Y > graphics.Viewport.Height || position.Y < graphics.Viewport.Y) velocity.Y *= -1;

            bounds.Center = new Vector2(position.X, position.Y);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, new Rectangle(0, 516, 118, 102), Color.White, 0, new Vector2(60, 50), 1.0f, SpriteEffects.None, 0);
        }
    }
}
