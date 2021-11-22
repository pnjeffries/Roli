using Nucleus.Game;
using Nucleus.Game.Components;
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
        /// The main furniture faction
        /// </summary>
        public static Faction FurnitureFaction { get; } = new Faction("Furniture");

        /// <summary>
        /// A standard wall
        /// </summary>
        /// <returns></returns>
        public GameElement Wall()
        {
            return new GameElement("wall", new Outlined(),
                new ASCIIStyle("#"), new PrefabStyle("Wall"), new MapCellCollider(),
                new VisionBlocker(), new Memorable(), new Inertia(true));
        }

        public GameElement Door()
        {
            return new StaticElement("door", 
                new ASCIIStyle("◙"), new VisionBlocker(), new Memorable(), new Inertia(true),
                new MapCellCollider(), new Door());
        }

        public GameElement LockedDoor(string keyCode)
        {
            var door = Door();
            var doorDat = door.GetData<Door>();
            doorDat.Locked = true;
            doorDat.KeyCode = keyCode;
            return door;
        }
        public GameElement Exit(StageExit exit)
        {
            return new StaticElement("exit",
                new ASCIIStyle(">"),
                new Memorable(), new MapCellCollider(), new Inertia(true), exit);
        }

        public GameElement Entrance()
        {
            return new StaticElement("entrance",
                new ASCIIStyle("<"),
                new Memorable(), new MapCellCollider(false), new Inertia(true));
        }

        public GameElement Crate()
        {
            return new StaticElement("crate", FurnitureFaction,
                new ASCIIStyle("□"), new PrefabStyle("Crate"), new MapCellCollider(),
                new Memorable(), new HitPoints(5));
        }
    }
}
