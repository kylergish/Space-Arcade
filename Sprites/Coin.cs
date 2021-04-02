using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using SpaceArcade.Collisions;

namespace SpaceArcade.Sprites
{
    public class Coin
    {
        Texture2D texture;
        Vector2 position;
        BoundingCircle bounds;

        const float ANIMATION_SPEED = 0.1f;
        double animationTimer;
        int animationFrame;

        public bool Collected { get; set; } = false;
        public BoundingCircle Bounds => bounds;

        public Coin(Vector2 position)
        {
            this.position = position;
            this.bounds = new BoundingCircle(position + new Vector2(8, 8), 8);
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("coins");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Collected) return;

            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if(animationTimer > ANIMATION_SPEED)
            {
                animationFrame++;
                if (animationFrame > 7) animationFrame = 0;
                animationTimer -= ANIMATION_SPEED;
            }

            var source = new Rectangle(animationFrame * 16, 0, 16, 16);
            spriteBatch.Draw(texture, position, source, Color.White);
        }
    }
}
