using Nucleus.Game;
using Nucleus.Game.Artitecture;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roli
{
    /// <summary>
    /// Stage style specific to Roli levels
    /// </summary>
    [Serializable]
    public class RoliStageStyle : StageStyle
    {
        /// <summary>
        /// The roughness of the stage wall lines.
        /// Used to simulate caves, rough-hewn walls etc.
        /// </summary>
        public double Roughness { get; set; } = 0;

        /// <summary>
        /// The library of the most common creatures found on this stage
        /// </summary>
        public WeightedTable<CreatureLibrary.Create> Creatures { get; set; }

        public RoliStageStyle(params RoomTemplate[] templates) : base(templates)
        {
        }

        public RoliStageStyle(string name, params RoomTemplate[] templates) : base(name, templates)
        {
        }
    }
}
