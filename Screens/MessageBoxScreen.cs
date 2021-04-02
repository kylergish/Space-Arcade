using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceArcade.StateManagement;

namespace SpaceArcade.Screens
{
    public class MessageBoxScreen : GameScreen
    {
        readonly string message;
        Texture2D gradientTexture;
        readonly InputAction menuSelect;
        readonly InputAction menuCancel;

        public event EventHandler<PlayerIndexEventArgs> Accepted;
        public event EventHandler<PlayerIndexEventArgs> Cancelled;

        public MessageBoxScreen(string message, bool includeUsageText = true)
        {
            const string usageText = "\nA button, Space, Enter = OK" + "\nB button, Backspace = CANCEL";

            if (includeUsageText) this.message = message + usageText;
            else this.message = message;

            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);

            menuSelect = new InputAction(new[] { Buttons.A, Buttons.Start }, new[] { Keys.Enter, Keys.Space }, true);
            menuCancel = new InputAction(new[] { Buttons.B, Buttons.Back }, new[] { Keys.Back, Keys.Escape }, true);
        }

        public override void Activate()
        {
            var content = ScreenManager.Game.Content;
            gradientTexture = content.Load<Texture2D>("gradient");
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            PlayerIndex playerIndex;

            if(menuSelect.Occurred(input, ControllingPlayer, out playerIndex))
            {
                Accepted?.Invoke(this, new PlayerIndexEventArgs(playerIndex));
                ExitScreen();
            }
            else if (menuCancel.Occurred(input, ControllingPlayer, out playerIndex))
            {
                Cancelled?.Invoke(this, new PlayerIndexEventArgs(playerIndex));
                ExitScreen();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;
            var font = ScreenManager.Font;

            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            var viewport = ScreenManager.GraphicsDevice.Viewport;
            var viewportSize = new Vector2(viewport.Width, viewport.Height);
            var textSize = font.MeasureString(message);
            var textPosition = (viewportSize - textSize) / 2;

            const int hPad = 32;
            const int vPad = 16;

            var backgroundRectangle = new Rectangle((int)textPosition.X - hPad, (int)textPosition.Y - vPad, (int)textSize.X + hPad * 2, (int)textSize.Y + vPad * 2);

            var color = Color.White * TransitionAlpha;

            spriteBatch.Begin();
            spriteBatch.Draw(gradientTexture, backgroundRectangle, color);
            spriteBatch.DrawString(font, message, textPosition, color);
            spriteBatch.End();
        }
    }
}
