using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using SpaceArcade.StateManagement;

namespace SpaceArcade.Screens
{
    public class LoadingScreen : GameScreen
    {
        readonly bool loadingIsSlow;
        bool otherScreensAreGone;
        readonly GameScreen[] screensToLoad;

        private LoadingScreen(ScreenManager screenManager, bool loadingIsSlow, GameScreen[] screensToLoad)
        {
            this.loadingIsSlow = loadingIsSlow;
            this.screensToLoad = screensToLoad;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
        }

        public static void Load(ScreenManager screenManager, bool loadingIsSlow, PlayerIndex? controllingPlayer, params GameScreen[] screensToLoad)
        {
            foreach (var screen in screenManager.GetScreens()) screen.ExitScreen();

            var loadingScreen = new LoadingScreen(screenManager, loadingIsSlow, screensToLoad);

            screenManager.AddScreen(loadingScreen, controllingPlayer);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if(otherScreensAreGone)
            {
                ScreenManager.RemoveScreen(this);

                foreach(var screen in screensToLoad)
                {
                    if (screen != null) ScreenManager.AddScreen(screen, ControllingPlayer);
                }

                ScreenManager.Game.ResetElapsedTime();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (ScreenState == ScreenState.Active && ScreenManager.GetScreens().Length == 1) otherScreensAreGone = true;

            if(loadingIsSlow)
            {
                var spriteBatch = ScreenManager.SpriteBatch;
                var font = ScreenManager.Font;

                const string message = "Loading...";

                var viewport = ScreenManager.GraphicsDevice.Viewport;
                var viewportSize = new Vector2(viewport.Width, viewport.Height);
                var textSize = font.MeasureString(message);
                var textPosition = (viewportSize - textSize) / 2;

                var color = Color.White * TransitionAlpha;

                spriteBatch.Begin();
                spriteBatch.DrawString(font, message, textPosition, color);
                spriteBatch.End();
            }
        }
    }
}
