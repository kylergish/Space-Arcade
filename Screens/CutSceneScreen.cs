using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using SpaceArcade.StateManagement;

namespace SpaceArcade.Screens
{
    public class CutSceneScreen : GameScreen
    {
        ContentManager content;
        Video video;
        VideoPlayer player;
        bool isPlaying = false;
        InputAction skip;

        public CutSceneScreen()
        {
            player = new VideoPlayer();
            skip = new InputAction(new Buttons[] { Buttons.A }, new Keys[] { Keys.Space, Keys.Enter }, true);
        }

        public override void Activate()
        {
            if (content == null) content = new ContentManager(ScreenManager.Game.Services, "Content");

            video = content.Load<Video>("liftoff_of_smap");
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if(!isPlaying)
            {
                player.Play(video);
                isPlaying = true;
            }
            PlayerIndex playerIndex;
            if(skip.Occurred(input, null, out playerIndex))
            {
                player.Stop();
                ExitScreen();
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (player.PlayPosition >= video.Duration) ExitScreen();
        }

        public override void Deactivate()
        {
            player.Pause();
            isPlaying = false;
        }

        public override void Draw(GameTime gameTime)
        {
            if(isPlaying)
            {
                ScreenManager.SpriteBatch.Begin();
                ScreenManager.SpriteBatch.Draw(player.GetTexture(), Vector2.Zero, Color.White);
                ScreenManager.SpriteBatch.End();
            }
        }
    }
}
