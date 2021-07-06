using Nucleus.Game;
using Nucleus.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roli
{
    /// <summary>
    /// A library of creatures
    /// </summary>
    public class CreatureLibrary
    {
        /// <summary>
        /// The faction to which the player belongs
        /// </summary>
        public Faction PlayerFaction { get; } = new Faction("Player");

        /// <summary>
        /// The game's hero
        /// </summary>
        /// <returns></returns>
        public GameElement Hero()
        {
            return new GameElement("Hero",
                PlayerFaction,
                new ASCIIStyle("@"),
                new AvailableActions(), new TurnCounter(),
                new WaitAbility(),
                new MoveCellAbility());
        }
    }
}
