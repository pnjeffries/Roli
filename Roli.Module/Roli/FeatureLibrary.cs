using Nucleus.Game;
using Nucleus.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roli
{
    /// <summary>
    /// A library of furniture types
    /// </summary>
    public class FeatureLibrary
    {
        /// <summary>
        /// A standard wall
        /// </summary>
        /// <returns></returns>
        public GameElement Wall()
        {
            return new GameElement("Wall", 
                new ASCIIStyle("#"), new PrefabStyle("Wall"), new MapCellCollider(),
                new VisionBlocker(), new Memorable(), new Inertia(true));
        }

        public GameElement Door()
        {
            return new StaticElement("Door", 
                new ASCIIStyle("◘"), new VisionBlocker(), new Memorable(), new Inertia(true));
        }

        public GameElement Exit(StageExit exit)
        {
            return new StaticElement("Exit",
                new ASCIIStyle(">"),
                new Memorable(), new MapCellCollider(), new Inertia(true), exit);
        }

        public GameElement Entrance()
        {
            return new StaticElement("Entrance",
                new ASCIIStyle("<"),
                new Memorable(), new MapCellCollider(false), new Inertia(true));
        }
    }
}
