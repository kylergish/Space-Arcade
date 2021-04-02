﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceArcade.Screens
{
    public class MenuEntry
    {
        string text;
        float selectionFade;
        Vector2 position;

        public string Text
        {
            private get => text;
            set => text = value;
        }

        public Vector2 Position
        {
            get => position;
            set => position = value;
        }

        public event EventHandler<PlayerIndexEventArgs> Selected;

        protected internal virtual void OnSelectEntry(PlayerIndex playerIndex)
        {
            Selected?.Invoke(this, new PlayerIndexEventArgs(playerIndex));
        }

        public MenuEntry(string text)
        {
            this.text = text;
        }

        public virtual void Update(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

            if (isSelected) selectionFade = Math.Min(selectionFade + fadeSpeed, 1);
            else selectionFade = Math.Max(selectionFade - fadeSpeed, 0);
        }

        public virtual void Draw(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            var color = isSelected ? Color.Yellow : Color.White;

            double time = gameTime.TotalGameTime.TotalSeconds;
            float pulsate = (float)Math.Sin(time * 6) + 1;
            float scale = 1 + pulsate * 0.05f * selectionFade;

            color *= screen.TransitionAlpha;

            var screenManager = screen.ScreenManager;
            var spriteBatch = screenManager.SpriteBatch;
            var font = screenManager.Font;

            var origin = new Vector2(0, font.LineSpacing / 2);

            spriteBatch.DrawString(font, text, position, color, 0, origin, scale, SpriteEffects.None, 0);
        }

        public virtual int GetHeight(MenuScreen screen)
        {
            return screen.ScreenManager.Font.LineSpacing;
        }

        public virtual int GetWidth(MenuScreen screen)
        {
            return (int)screen.ScreenManager.Font.MeasureString(Text).X;
        }
    }
}
