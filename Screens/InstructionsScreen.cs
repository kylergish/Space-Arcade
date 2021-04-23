using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceArcade.Screens
{
    public class InstructionsScreen : MenuScreen
    {
        readonly MenuEntry exitMenuEntry;
        readonly MenuEntry rotateInstructionsMenuEntry;
        readonly MenuEntry thrustInstructionsMenuEntry;

        public InstructionsScreen() : base("Instructions")
        {
            rotateInstructionsMenuEntry = new MenuEntry("LEFT/RIGHT arrow keys or A/D keys to rotate");
            thrustInstructionsMenuEntry = new MenuEntry("UP arrow key or SPACE key to thrust");
            exitMenuEntry = new MenuEntry("Back to Main Menu");

            exitMenuEntry.Selected += OnCancel;

            MenuEntries.Add(rotateInstructionsMenuEntry);
            MenuEntries.Add(thrustInstructionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            ScreenManager.RemoveScreen(this);
        }
    }
}
