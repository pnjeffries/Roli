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
        /// <summary>
        /// The library of available creatures
        /// </summary>
        public CreatureLibrary Creatures { get; } = new CreatureLibrary();

        public override GameState StartingState()
        {
            // Initialise state, map and stage:
            int mapX = 24;
            int mapY = 18;
            var state = new RLState();
            var stage = new MapStage();
            var map = new SquareCellMap<MapCell>(mapX, mapY);
            stage.Map = map;
            stage.Map.InitialiseCells();
            state.Stage = stage;

            // Create player character
            var hero = Creatures.Hero();
            map[5, 6].PlaceInCell(hero);
            state.Elements.Add(hero);
            state.Controlled = hero;

            return state;
        }

    }
}
