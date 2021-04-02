using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceArcade.StateManagement
{
    public class InputAction
    {
        readonly Buttons[] buttons;
        readonly Keys[] keys;
        readonly bool firstPressOnly;

        delegate bool ButtonPress(Buttons button, PlayerIndex? controllingPlayer, out PlayerIndex player);
        delegate bool KeyPress(Keys key, PlayerIndex? controllingPlayer, out PlayerIndex player);

        public InputAction(Buttons[] triggerButtons, Keys[] triggerKeys, bool firstPressOnly)
        {
            buttons = triggerButtons != null ? triggerButtons.Clone() as Buttons[] : new Buttons[0];
            keys = triggerKeys != null ? triggerKeys.Clone() as Keys[] : new Keys[0];
            this.firstPressOnly = firstPressOnly;
        }

        public bool Occurred(InputState stateToTest, PlayerIndex? playerToTest, out PlayerIndex player)
        {
            ButtonPress buttonTest;
            KeyPress keyTest;

            if(firstPressOnly)
            {
                buttonTest = stateToTest.IsNewButtonPress;
                keyTest = stateToTest.IsNewKeyPress;
            }
            else
            {
                buttonTest = stateToTest.IsButtonPressed;
                keyTest = stateToTest.IsKeyPressed;
            }

            foreach(var button in buttons)
            {
                if (buttonTest(button, playerToTest, out player)) return true;
            }
            foreach(var key in keys)
            {
                if (keyTest(key, playerToTest, out player)) return true;
            }

            player = PlayerIndex.One;
            return false;
        }
    }
}
