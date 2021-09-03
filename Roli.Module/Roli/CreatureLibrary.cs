using Nucleus.Extensions;
using Nucleus.Game;
using Nucleus.Game.Artitecture;
using Nucleus.Game.Components;
using Nucleus.Game.Components.Abilities;
using Nucleus.Game.Effects;
using Nucleus.Geometry;
using Nucleus.Logs;
using Nucleus.Model;
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
        /// The random number generator used to randomise certain creature features
        /// </summary>
        public Random RNG = new Random();

        /// <summary>
        /// The item library used to generate initial creature equipment
        /// </summary>
        public ItemLibrary Items = new ItemLibrary();

        /// <summary>
        /// The faction to which the player belongs
        /// </summary>
        public static Faction PlayerFaction { get; } = new Faction("Player");

        /// <summary>
        /// The main enemy faction
        /// </summary>
        public static Faction EnemyFaction { get; } = new Faction("Enemy");

        /// <summary>
        /// The delegate function for creating creatures
        /// </summary>
        /// <returns></returns>
        public delegate GameElement Create();

        /// <summary>
        /// A collection of all enemy creation 
        /// </summary>
        public WeightedTable<Create> AllEnemies
        {
            get
            {
                return new WeightedTable<Create>
                   (
                        Rat, Bat, Snake, Goblin, Phantom, Troll, Zombie, Archer, Orc, Wolf
                   );
            }
        }

        /// <summary>
        /// Generate a new creature element by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GameElement ByName(string name)
        {
            name = name.Replace('_', ' ').Trim();
            foreach (var create in AllEnemies)
            {
                // This is a bit inefficient, but Mono throws a wobbly
                // if we try to access the Method name itself
                var element = create.Key.Invoke();
                if (element.Name.EqualsIgnoreCase(name)) return element;
            }
            return null;
        }

        /// <summary>
        /// The game's hero
        /// </summary>
        /// <returns></returns>
        public GameElement Hero()
        {
            // Note: For the player, higher-priority abilities should go first
            return new ActiveElement("Hero",
                PlayerFaction,
                new ASCIIStyle("@"),
                new AvailableActions(), 
                new TurnCounter(),
                new WaitAbility(),
                new MapCellCollider(),
                new MapAwareness(5),
                new Memorable(),
                new BumpAttackAbility(),
                new ExitStageAbility(),
                new MoveCellAbility(),
                new HitPoints(9),
                new OpenDoorAbility(),
                new Inventory(
                    // Item slots
                    new ItemSlot("1", InputFunction.Ability_1, null),
                    new ItemSlot("2", InputFunction.Ability_2, null),
                    new ItemSlot("3", InputFunction.Ability_3, null),
                    new ItemSlot("4", InputFunction.Ability_4, null),
                    new ItemSlot("5", InputFunction.Ability_5, null),
                    new ItemSlot("6", InputFunction.Ability_6, null),
                    // Equippable slots
                    new EquipmentSlot("Hands"),
                    // Resources
                    new Resource(ResourceTypes.Coins, 0),
                    new Resource(ResourceTypes.Arrows, 0)),
                 new PickUpAbility(),
                 new UseItemAbility(),
                 new Status(),
                 new DropAbility(),
                 new ChangeSelectedItemAbility(),
                 new CritHitter() { BaseChance = 0.1}
                 );
        }

        private GameElement Enemy(string name)
        {
            // Create enemy
            return new ActiveElement(name,
                EnemyFaction,
                new MapCellCollider(), new Memorable(),
                new AvailableActions(), new TurnCounter(),
                new WaitAbility(),
                new MoveCellAbility(),
                new LogDescription("<color=#FF00FD>", "</color>"),
                new Status(),
                new CritHitter() { BaseChance = 0.1 }
                ) ;
        }

        public GameElement Rat()
        {
            var result = Enemy("rat");
            result.SetData(new ASCIIStyle("r"), new PrefabStyle("Meeple"),
                new MapAwareness(2), new HitPoints(1), new ElementWeight(15), new BumpAttackAbility(1,0));
            return result;
        }

        public GameElement Snake()
        {
            var result = Enemy("snake");
            result.SetData(new ASCIIStyle("s"), new PrefabStyle("Meeple"), 
                new MapAwareness(3), new HitPoints(2), new ElementWeight(25),
                new BumpAttackAbility(0,1, new ApplyStatusEffect(new Poisoned())));
            return result;
        }

        public GameElement Bat()
        {
            var result = Enemy("bat");
            result.SetData(new ASCIIStyle("b"), new PrefabStyle("Meeple"), 
                new MapAwareness(4), new HitPoints(1), new ElementWeight(5),
                new BumpAttackAbility(1,0));
            result.GetData<TurnCounter>().Speed = 2;
            return result;
        }

        public GameElement Wolf()
        {
            var result = Enemy("wolf");
            result.SetData(new ASCIIStyle("w"), new PrefabStyle("Meeple"),
                new MapAwareness(8), new HitPoints(3), new ElementWeight(60),
                new BumpAttackAbility(1, 1));
            result.GetData<TurnCounter>().Speed = 2;
            return result;
        }

        public GameElement Archer()
        {
            var bow = Items.Bow();
            var result = Enemy("archer");
            result.SetData(new ASCIIStyle("a"),
                new PrefabStyle("Meeple"),
                new MapAwareness(6),
                new HitPoints(3),
                new BumpAttackAbility(),
                new OpenDoorAbility(),
                new ElementGender(RNG.NextGender()),
                new UseItemAbility(),
                new Inventory(
                    new ItemSlot("1", InputFunction.Ability_1, bow),
                    // Equippable slots
                    new EquipmentSlot("Hands", bow),
                    // Resources
                    new Resource(ResourceTypes.Arrows, 6)));
            return result;
        }

        public GameElement Goblin()
        {
            var spear = Items.Spear();
            var result = Enemy("goblin");
            result.SetData(new ASCIIStyle("g"), 
                new PrefabStyle("Meeple"), 
                new MapAwareness(5), 
                new HitPoints(3),
                new BumpAttackAbility(),
                new OpenDoorAbility(),
                new ElementGender(RNG.NextGender()),
                new UseItemAbility(),
                new Inventory(
                    new ItemSlot("1", InputFunction.Ability_1, spear),
                    // Equippable slots
                    new EquipmentSlot("Hands", spear)));
            return result;
        }

        public GameElement Orc()
        {
            var weapon = Items.Mace();
            var result = Enemy("orc");
            result.SetData(new ASCIIStyle("o"),
                new PrefabStyle("Meeple"),
                new MapAwareness(5),
                new HitPoints(6),
                new BumpAttackAbility(),
                new OpenDoorAbility(),
                new ElementGender(RNG.NextGender()),
                new UseItemAbility(),
                new Inventory(
                    new ItemSlot("1", InputFunction.Ability_1, weapon),
                    // Equippable slots
                    new EquipmentSlot("Hands", weapon)));
            return result;
        }

        public GameElement Phantom()
        {
            var result = Enemy("phantom");
            result.SetData(new ASCIIStyle("p"), new PrefabStyle("Meeple"), new MapAwareness(4), new HitPoints(3), new BumpAttackAbility());
            
            // Phantoms can move through non-living elements
            result.GetData<MapCellCollider>().BlockingCheck = el =>
            (el.GetData<MapCellCollider>()?.Solid ?? false) &&
            (el.HasData<Faction>());
            
            return result;
        }

        public GameElement Troll()
        {
            var result = Enemy("troll");
            result.SetData(new ASCIIStyle("T"), new PrefabStyle("Meeple"), 
                new MapAwareness(4), new HitPoints(10), new ElementWeight(250),
                new BumpAttackAbility(2,3));
            result.GetData<TurnCounter>().Speed = 0.5;
            return result;
        }

        public GameElement Zombie()
        {
            var result = Enemy("zombie");
            result.SetData(new ASCIIStyle("z"), new PrefabStyle("Meeple"),
                new MapAwareness(4), new HitPoints(6), new ElementWeight(70),
                new BumpAttackAbility(1, 1));
            result.GetData<TurnCounter>().Speed = 0.5;
            return result;
        }
    }
}
