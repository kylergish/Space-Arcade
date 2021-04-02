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

namespace SpaceArcade.Screens
{
    public class GameplayScreen : GameScreen
    {
        ContentManager content;

        SpaceShip spaceShip;
        Asteroid[] asteroids;
        Coin[] coins;

        SpriteFont bangers;
        SoundEffect coinPickup;

        int coinsLeft;

        readonly Random random = new Random();

        float pauseAlpha;
        readonly InputAction pauseAction;

        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            pauseAction = new InputAction(new[] { Buttons.Start, Buttons.Back }, new[] { Keys.Back, Keys.Escape }, true);

            spaceShip = new SpaceShip();

            asteroids = new Asteroid[]
            {
                new Asteroid(new Vector2((float)random.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Width, (float)random.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Height), new Vector2((float)random.NextDouble(), (float)random.NextDouble()), ScreenManager.GraphicsDevice),
                new Asteroid(new Vector2((float)random.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Width, (float)random.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Height), new Vector2((float)random.NextDouble(), (float)random.NextDouble()), ScreenManager.GraphicsDevice),
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
        }

        public override void Activate()
        {
            if (content == null) content = new ContentManager(ScreenManager.Game.Services, "Content");

            spaceShip.LoadContent(content);
            foreach (var asteroid in asteroids) asteroid.LoadContent(content);
            foreach (var coin in coins) coin.LoadContent(content);

            bangers = content.Load<SpriteFont>("bangers");
            coinPickup = content.Load<SoundEffect>("Pickup_Coin15");

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

            if(IsActive)
            {
                spaceShip.Update(gameTime);

                foreach(var asteroid in asteroids)
                {
                    asteroid.Update(gameTime);
                    if (asteroid.Bounds.CollidesWith(spaceShip.Bounds)) ScreenManager.RemoveScreen(this);
                }

                foreach(var coin in coins)
                {
                    if(!coin.Collected && coin.Bounds.CollidesWith(spaceShip.Bounds))
                    {
                        coin.Collected = true;
                        coinsLeft--;
                        coinPickup.Play();
                    }
                }
                if (coinsLeft < 1) ScreenManager.RemoveScreen(this);
            }
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            int playerIndex = (int)ControllingPlayer.Value;

            var keyboardState = input.CurrentKeyboardStates[playerIndex];
            var gamePadState = input.CurrentGamePadStates[playerIndex];

            bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];

            PlayerIndex player;
            if (pauseAction.Occurred(input, ControllingPlayer, out player) || gamePadDisconnected) ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;
            ScreenManager.GraphicsDevice.Clear(Color.DarkBlue);

            spriteBatch.Begin();
            foreach (var asteroid in asteroids) asteroid.Draw(gameTime, spriteBatch);
            foreach (var coin in coins) coin.Draw(gameTime, spriteBatch);
            spaceShip.Draw(gameTime, spriteBatch);

            spriteBatch.DrawString(bangers, "Avoid the Asteroids!!!", new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(bangers, $"{gameTime.TotalGameTime:c}", new Vector2(0, 50), Color.White);
            spriteBatch.DrawString(bangers, $"Coins Left: {coinsLeft}", new Vector2(0, 100), Color.White);
            spriteBatch.End();

            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
    }
}
