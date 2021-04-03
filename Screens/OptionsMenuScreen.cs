using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace SpaceArcade.Screens
{
    public class OptionsMenuScreen : MenuScreen
    {
        private enum VolumeOptions
        {
            Off,
            Low,
            Medium,
            High
        }

        VolumeOptions sfxVolumeOption = VolumeOptions.Medium;
        VolumeOptions musicVolumeOption = VolumeOptions.Medium;

        readonly MenuEntry sfxVolumeMenuEntry;
        readonly MenuEntry musicVolumeMenuEntry;
        readonly MenuEntry exitMenuEntry;

        public OptionsMenuScreen() : base("Options")
        {
            sfxVolumeMenuEntry = new MenuEntry(string.Empty);
            musicVolumeMenuEntry = new MenuEntry(string.Empty);
            exitMenuEntry = new MenuEntry("Back to Main Menu");
            SetMenuEntryText();

            sfxVolumeMenuEntry.Selected += SFXVolumeEntrySelected;
            musicVolumeMenuEntry.Selected += MusicVolumeEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            MenuEntries.Add(sfxVolumeMenuEntry);
            MenuEntries.Add(musicVolumeMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        private void SetMenuEntryText()
        {
            sfxVolumeMenuEntry.Text = $"SFX Volume: {sfxVolumeOption}";
            musicVolumeMenuEntry.Text = $"Music Volume: {musicVolumeOption}";
        }

        private void SFXVolumeEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            sfxVolumeOption++;
            if(sfxVolumeOption > VolumeOptions.High) sfxVolumeOption = VolumeOptions.Off;
            float volumeOption = (float)sfxVolumeOption;
            float newVolume = (float)volumeOption / 3.0f;
            SoundEffect.MasterVolume = newVolume;
            SetMenuEntryText();
        }

        private void MusicVolumeEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            musicVolumeOption++;
            if (musicVolumeOption > VolumeOptions.High) musicVolumeOption = VolumeOptions.Off;
            float volumeOption = (float)musicVolumeOption;
            float newVolume = (float)volumeOption / 3.0f;
            MediaPlayer.Volume = newVolume;
            SetMenuEntryText();
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            ScreenManager.RemoveScreen(this);
        }
    }
}
