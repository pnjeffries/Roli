using Nucleus.Game;
using Nucleus.Geometry;
using Nucleus.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roli
{
    /// <summary>
    /// A Roguelike game module
    /// </summary>
    public class RoliModule : GameModule
    {
        public override void Initialise()
        {
            GameEngine.Instance.LanguagePack.LoadFrom("English", GameEngine.Instance.Resources);
            base.Initialise();
        }

        public override GameState StartingState()
        {
            var state = new RoliGameState();
            return state;
        }

    }

    
}
