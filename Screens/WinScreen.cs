using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using SpaceArcade.StateManagement;

namespace SpaceArcade.Screens
{
    public class WinScreen : GameScreen
    {
        InputAction replay;
        InputAction exit;

        public WinScreen()
        {
            replay = new InputAction(new Buttons[] { Buttons.Start }, new Keys[] { Keys.Enter }, true);
            exit = new InputAction(new Buttons[] { Buttons.B }, new Keys[] { Keys.Escape }, true);
        }

        public override void Activate() { }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            PlayerIndex player;
            if(replay.Occurred(input, null, out player))
            {
                ScreenManager.RemoveScreen(this);
                ScreenManager.AddScreen(new GameplayScreen(), null);
            }
            if(exit.Occurred(input, null, out player))
            {
                ScreenManager.RemoveScreen(this);
                ScreenManager.AddScreen(new BackgroundScreen(), null);
                ScreenManager.AddScreen(new MainMenuScreen(), null);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            var font = ScreenManager.Font;
            var spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "CONGRATULATIONS YOU WIN!", new Vector2(250, 100), Color.White);
            spriteBatch.DrawString(font, "Press ENTER to play again.", new Vector2(175, 270), Color.White);
            spriteBatch.DrawString(font, "Press ESCAPE to return to menu", new Vector2(150, 320), Color.White);
            spriteBatch.End();
        }
    }
}
