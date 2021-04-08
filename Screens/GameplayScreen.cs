using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using SpaceArcade.Sprites;
using SpaceArcade.StateManagement;
using SpaceArcade.Particles;

namespace SpaceArcade.Screens
{
    public class GameplayScreen : GameScreen
    {
        Game game;
        ContentManager content;

        SpaceShip spaceShip;
        Asteroid[] asteroids;
        Coin[] coins;

        SpriteFont bangers;
        SoundEffect coinPickup;

        bool collisionOn = false;

        int coinsLeft;

        readonly Random random = new Random();

        float pauseAlpha;
        readonly InputAction pauseAction;

        ShootingStarParticleSystem shootingStar;
        ExplosionParticleSystem explosion;
        CoinPickupParticleSystem pickup;

        Texture2D starBackground;

        double seconds = 0.0;

        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            pauseAction = new InputAction(new[] { Buttons.Start, Buttons.Back }, new[] { Keys.Back, Keys.Escape }, true);

            spaceShip = new SpaceShip();

            ResetTime();
        }

        public override void Activate()
        {
            if (content == null) content = new ContentManager(ScreenManager.Game.Services, "Content");

            asteroids = new Asteroid[]
            {
                new Asteroid(new Vector2((float)random.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Width, (float)random.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Height), new Vector2((float)random.NextDouble(), (float)random.NextDouble()), ScreenManager.GraphicsDevice),
                new Asteroid(new Vector2((float)random.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Width, (float)random.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Height), new Vector2((float)random.NextDouble(), (float)random.NextDouble()), ScreenManager.GraphicsDevice),
                new Asteroid(new Vector2((float)random.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Width, (float)random.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Height), new Vector2((float)random.NextDouble(), (float)random.NextDouble()), ScreenManager.GraphicsDevice),
            };
            coins = new Coin[]
            {
                new Coin(new Vector2((float)random.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Width, (float)random.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Height)),
                new Coin(new Vector2((float)random.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Width, (float)random.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Height)),
                new Coin(new Vector2((float)random.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Width, (float)random.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Height)),
                new Coin(new Vector2((float)random.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Width, (float)random.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Height)),
                new Coin(new Vector2((float)random.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Width, (float)random.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Height)),
                new Coin(new Vector2((float)random.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Width, (float)random.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Height)),
                new Coin(new Vector2((float)random.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Width, (float)random.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Height))
            };

            coinsLeft = coins.Length;

            spaceShip.LoadContent(content);
            foreach (var asteroid in asteroids) asteroid.LoadContent(content);
            foreach (var coin in coins) coin.LoadContent(content);

            bangers = content.Load<SpriteFont>("bangers");
            coinPickup = content.Load<SoundEffect>("Pickup_Coin15");
            starBackground = content.Load<Texture2D>("star-background");

            shootingStar = new ShootingStarParticleSystem(ScreenManager.Game, new Rectangle(0, -20, 800, 10));
            ScreenManager.Game.Components.Add(shootingStar);

            explosion = new ExplosionParticleSystem(ScreenManager.Game, 20);
            ScreenManager.Game.Components.Add(explosion);

            pickup = new CoinPickupParticleSystem(ScreenManager.Game, 20);
            ScreenManager.Game.Components.Add(pickup);

            Thread.Sleep(1000);

            ScreenManager.Game.ResetElapsedTime();
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (coveredByOtherScreen) pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            seconds += gameTime.ElapsedGameTime.TotalSeconds;

            if(seconds > 5.0 && !collisionOn)
            {
                collisionOn = true;
            }

            if(IsActive)
            {
                spaceShip.Update(gameTime, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height);

                foreach(var asteroid in asteroids)
                {
                    asteroid.Update(gameTime);
                    
                    if (asteroid.Bounds.CollidesWith(spaceShip.Bounds) && collisionOn)
                    {
                        explosion.PlaceExplosion(spaceShip.Position);
                        ScreenManager.RemoveScreen(this);
                        ScreenManager.Game.Components.Remove(shootingStar);
                        ScreenManager.AddScreen(new BackgroundScreen(), null);
                        ScreenManager.AddScreen(new LoseScreen(), null);
                    }
                }

                foreach(var coin in coins)
                {
                    if(!coin.Collected && coin.Bounds.CollidesWith(spaceShip.Bounds))
                    {
                        pickup.PlaceParticle(coin.Bounds.Center);
                        coin.Collected = true;
                        coinsLeft--;
                        coinPickup.Play();
                    }
                }
                if (coinsLeft < 1)
                {
                    ScreenManager.RemoveScreen(this);
                    ScreenManager.Game.Components.Remove(shootingStar);
                    ScreenManager.AddScreen(new BackgroundScreen(), null);
                    ScreenManager.AddScreen(new WinScreen(), null);
                }
            }
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            base.HandleInput(gameTime, input);

            PlayerIndex player;
            if(pauseAction.Occurred(input, ControllingPlayer, out player))
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;
            ScreenManager.GraphicsDevice.Clear(Color.DarkBlue);

            float spaceShipX = MathHelper.Clamp(spaceShip.Position.X, 0, 5000);
            float offsetX = -spaceShipX;
            float spaceShipY = MathHelper.Clamp(spaceShip.Position.Y, 0, 2812);
            float offsetY = -spaceShipY;

            Matrix transform;

            transform = Matrix.CreateTranslation(offsetX * 0.333f, offsetY * 0.333f, 0);
            spriteBatch.Begin(transformMatrix: transform);
            spriteBatch.Draw(starBackground, Vector2.Zero, Color.White);
            spriteBatch.End();

            spriteBatch.Begin();
            foreach (var asteroid in asteroids) asteroid.Draw(gameTime, spriteBatch);
            foreach (var coin in coins) coin.Draw(gameTime, spriteBatch);
            spaceShip.Draw(gameTime, spriteBatch);

            spriteBatch.DrawString(bangers, "Avoid the Asteroids!!!", Vector2.Zero, Color.White);
            spriteBatch.DrawString(bangers, $"Coins Left: {coinsLeft}", new Vector2(0, 50), Color.White);
            spriteBatch.End();

            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        private void ResetTime()
        {
            seconds = 0.0;
        }
    }
}
