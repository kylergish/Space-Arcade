using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceArcade.StateManagement
{
    public interface IScreenFactory
    {
        GameScreen CreateScreen(Type screenType);
    }
}
