using Nucleus.Game;
using Nucleus.Game.Artitecture;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roli
{
    /// <summary>
    /// A library of stage styles
    /// </summary>
    [Serializable]
    public class StageLibrary
    {
        /// <summary>
        /// The library of room templates
        /// </summary>
        public RoomLibrary Rooms { get; set; } = new RoomLibrary();

        /// <summary>
        /// The library of creature templates
        /// </summary>
        public CreatureLibrary Creatures { get; set; } = new CreatureLibrary();

        /// <summary>
        /// The main level progression
        /// </summary>
        /// <returns></returns>
        public IList<RoliStageStyle> MainLevels()
        {
            return new List<RoliStageStyle>()
            {
                Floor1(), 
                Floor2(),
                Floor3(),
                Floor4(),
                Floor5(),
                Floor6(),
                Floor7(),
                Floor8(),
                Floor9(),
                Floor10(),
                Floor11(),
                Floor12(),
                Floor13(),
                Floor14()
            };
        }

        /// <summary>
        /// Standard dungeon floor
        /// </summary>
        /// <returns></returns>
        public RoliStageStyle Dungeon(string name)
        {
            var result = new RoliStageStyle(name,
                Rooms.Corridor(), Rooms.Hall(), Rooms.StandardRoom(), Rooms.LargeRoom(), Rooms.Exit());
            return result;
        }

        /// <summary>
        /// Cave floor
        /// </summary>
        /// <returns></returns>
        public RoliStageStyle Cave(string name)
        {
            return new RoliStageStyle(name,
                Rooms.Cavern(), Rooms.Crevice(), Rooms.Exit())
            { Roughness = 0.075, DoorChance = 0 , MinLoopSize = 0};
        }

        #region Levels

        public RoliStageStyle Floor1()
        {
            var result = Dungeon("Secret Passageways");
            result.Creatures = new WeightedTable<CreatureLibrary.Create>();
            result.Creatures.Add(Creatures.Rat, 2);
            result.Creatures.Add(Creatures.Snake, 1);
            result.Creatures.Add(Creatures.Zombie, 0.5);
            return result;
        }

        public RoliStageStyle Floor2()
        {
            var result = Dungeon("Forgotten Vaults");
            result.Creatures = new WeightedTable<CreatureLibrary.Create>();
            result.Creatures.Add(Creatures.Rat, 2);
            result.Creatures.Add(Creatures.Snake, 1);
            result.Creatures.Add(Creatures.Zombie, 1);
            result.Creatures.Add(Creatures.Phantom, 0.5);
            result.Creatures.Add(Creatures.Bat, 0.25);
            return result;
        }

        public RoliStageStyle Floor3()
        {
            var result = Cave("Earthen Halls");
            result.Creatures = new WeightedTable<CreatureLibrary.Create>();
            result.Creatures.Add(Creatures.Rat, 2);
            result.Creatures.Add(Creatures.Bat, 2);
            result.Creatures.Add(Creatures.Snake, 0.5);
            result.Creatures.Add(Creatures.Zombie, 2);
            result.Creatures.Add(Creatures.Phantom, 0.5);
            return result;
        }

        public RoliStageStyle Floor4()
        {
            var result = Cave("Buried Valley");
            result.Creatures = new WeightedTable<CreatureLibrary.Create>();
            result.Creatures.Add(Creatures.Snake, 2);
            result.Creatures.Add(Creatures.Zombie, 1);
            result.Creatures.Add(Creatures.Bat, 1);
            result.Creatures.Add(Creatures.Wolf, 0.5);
            result.Creatures.Add(Creatures.Troll, 0.25);
            result.Creatures.Add(Creatures.Orc, 1);
            return result;
        }

        public RoliStageStyle Floor5()
        {
            var result = Dungeon("Lonely Gaol");
            result.Creatures = new WeightedTable<CreatureLibrary.Create>();
            result.Creatures.Add(Creatures.Rat, 1);
            result.Creatures.Add(Creatures.Zombie, 2);
            result.Creatures.Add(Creatures.Orc, 1);
            result.Creatures.Add(Creatures.Goblin, 1);
            result.Creatures.Add(Creatures.Wolf, 0.5);
            result.Creatures.Add(Creatures.Phantom, 0.5);
            return result;
        }

        public RoliStageStyle Floor6()
        {
            var result = Cave("Haunted Caves");
            result.Creatures = new WeightedTable<CreatureLibrary.Create>();
            result.Creatures.Add(Creatures.Rat, 1);
            result.Creatures.Add(Creatures.Snake, 1);
            result.Creatures.Add(Creatures.Zombie, 1);
            result.Creatures.Add(Creatures.Bat, 1);
            result.Creatures.Add(Creatures.Phantom, 2);
            result.Creatures.Add(Creatures.Troll, 1);
            return result;
        }

        public RoliStageStyle Floor7()
        {
            var result = Dungeon("Raiders' Hideout");
            result.Creatures = new WeightedTable<CreatureLibrary.Create>();
            result.Creatures.Add(Creatures.Rat, 1);
            result.Creatures.Add(Creatures.Wolf, 1);
            result.Creatures.Add(Creatures.Orc, 1);
            result.Creatures.Add(Creatures.Goblin, 1);
            result.Creatures.Add(Creatures.Archer, 0.5);
            return result;
        }

        public RoliStageStyle Floor8()
        {
            var result = Cave("Hollow Graves");
            result.Creatures = new WeightedTable<CreatureLibrary.Create>();
            result.Creatures.Add(Creatures.Zombie, 1);
            result.Creatures.Add(Creatures.Bat, 1);
            result.Creatures.Add(Creatures.Phantom, 2);
            return result;
        }

        public RoliStageStyle Floor9()
        {
            var result = Cave("Dread Mines");
            result.Creatures = new WeightedTable<CreatureLibrary.Create>();
            result.Creatures.Add(Creatures.Orc, 1);
            result.Creatures.Add(Creatures.Goblin, 1);
            result.Creatures.Add(Creatures.Archer, 0.5);
            result.Creatures.Add(Creatures.Phantom, 0.5);
            result.Creatures.Add(Creatures.Troll, 2);
            return result;
        }

        public RoliStageStyle Floor10()
        {
            var result = Cave("Hunting Grounds");
            result.Creatures = new WeightedTable<CreatureLibrary.Create>();
            result.Creatures.Add(Creatures.Archer, 2);
            result.Creatures.Add(Creatures.Wolf, 2);
            result.Creatures.Add(Creatures.Troll, 1);
            return result;
        }

        public RoliStageStyle Floor11()
        {
            var result = Dungeon("Sunken Walls");
            result.Creatures = new WeightedTable<CreatureLibrary.Create>();
            result.Creatures.Add(Creatures.Wolf, 1);
            result.Creatures.Add(Creatures.Orc, 1);
            result.Creatures.Add(Creatures.Goblin, 1);
            result.Creatures.Add(Creatures.Archer, 1);
            result.Creatures.Add(Creatures.Troll, 1);
            return result;
        }

        public RoliStageStyle Floor12()
        {
            var result = Dungeon("Bloodstained Hallways");
            result.Creatures = new WeightedTable<CreatureLibrary.Create>();
            result.Creatures.Add(Creatures.Wolf, 1);
            result.Creatures.Add(Creatures.Orc, 1);
            result.Creatures.Add(Creatures.Goblin, 1);
            result.Creatures.Add(Creatures.Archer, 1);
            result.Creatures.Add(Creatures.Troll, 1);
            result.Creatures.Add(Creatures.Phantom, 1);
            return result;
        }

        public RoliStageStyle Floor13()
        {
            var result = Dungeon("Library of Ignorance");
            result.Creatures = new WeightedTable<CreatureLibrary.Create>();
            result.Creatures.Add(Creatures.Wolf, 1);
            result.Creatures.Add(Creatures.Orc, 1);
            result.Creatures.Add(Creatures.Goblin, 1);
            result.Creatures.Add(Creatures.Archer, 1);
            result.Creatures.Add(Creatures.Troll, 1);
            result.Creatures.Add(Creatures.Phantom, 1);
            return result;
        }

        public RoliStageStyle Floor14()
        {
            var result = Dungeon("The Underkeep");
            result.Creatures = new WeightedTable<CreatureLibrary.Create>();
            result.Creatures.Add(Creatures.Wolf, 1);
            result.Creatures.Add(Creatures.Orc, 1);
            result.Creatures.Add(Creatures.Goblin, 1);
            result.Creatures.Add(Creatures.Archer, 1);
            result.Creatures.Add(Creatures.Troll, 1);
            result.Creatures.Add(Creatures.Phantom, 1);
            return result;
        }

        #endregion
    }
}
