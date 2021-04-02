using System;
using System.Collections.Generic;
using System.Text;
using SpaceArcade.StateManagement;

namespace SpaceArcade
{
    public class ScreenFactory : IScreenFactory
    {
        public GameScreen CreateScreen(Type screenType)
        {
            return Activator.CreateInstance(screenType) as GameScreen;
        }
    }
}
