using Nucleus.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roli
{
    /// <summary>
    /// Static class of standard resource types
    /// </summary>
    public static class ResourceTypes
    {
        /// <summary>
        /// Arrows
        /// </summary>
        public static readonly ResourceType Arrows = new ResourceType("arrows", "→");

        /// <summary>
        /// Coins
        /// </summary>
        public static readonly ResourceType Coins = new ResourceType("coins", "¤");
    }
}
