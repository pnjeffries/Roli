using Nucleus.Game;
using Nucleus.Game.Debug;
using Nucleus.Geometry;
using Nucleus.Logs;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roli
{
    /// <summary>
    /// Debug command library for Roli-specific commands
    /// </summary>
    [Serializable]
    public class RoliDebugCommandLibrary : DebugCommandLibrary
    {
        /// <summary>
        /// Write the player's location to the log
        /// </summary>
        public void player_cell()
        {
            var state = GameEngine.Instance.State as RoliGameState;
            int index = state.Controlled.GetData<MapData>().MapCell.Index;
            state.Log.WriteLine();
            state.Log.WriteLine("Player CellIndex:" + index);
        }

        /// <summary>
        /// Spawn an item at the specified location
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="cellIndex"></param>
        public void spawn_item(string itemName)
        {
            var items = new ItemLibrary();
            var item = items.ByName(itemName);
            if (item == null) throw new MethodAccessException("Item '" + itemName + "' not found.");
            var state = GameEngine.Instance.State as RoliGameState;
            int cellIndex = state.Controlled.GetData<MapData>().MapCell.Index;
            state.Stage.AddElement(item, cellIndex);
        }

        /// <summary>
        /// Spawn a creature at the specified location
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="cellIndex"></param>
        public void spawn_creature(string itemName, int cellIndex)
        {
            var creatures = new CreatureLibrary();
            var creature = creatures.ByName(itemName);
            if (creature == null) throw new MethodAccessException("Creature '" + itemName + "' not found.");
            var state = GameEngine.Instance.State as RoliGameState;
            state.Stage.AddElement(creature, cellIndex);
        }

        public void hello(string name)
        {
            var state = GameEngine.Instance.State as RoliGameState;
            state.Log.WriteLine();
            state.Log.WriteLine("Hello " + name + "!");
        }
    }
}
