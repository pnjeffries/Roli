using Nucleus.Extensions;
using Nucleus.Game;
using Nucleus.Game.Artitecture;
using Nucleus.Game.Components.Abilities;
using Nucleus.Geometry;
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
        public static Random RNG = new Random();

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

        public WeightedTable<Create> AllEnemies
        {
            get
            {
                return new WeightedTable<Create>
                   (
                        Rat, Bat, Snake, Groblin, Phantom
                   );
            }
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
                new OpenDoorAbility());
        }

        private GameElement Enemy(string name)
        {
            // Create enemy
            return new ActiveElement(name ,
                EnemyFaction,
                new MapCellCollider(), new Memorable(),
                new AvailableActions(), new TurnCounter(),
                new WaitAbility(),
                new MoveCellAbility()
                );
        }

        public GameElement Rat()
        {
            var result = Enemy("rat");
            result.SetData(new ASCIIStyle("r"), new PrefabStyle("Meeple"), new MapAwareness(2), new HitPoints(1), new BumpAttackAbility());
            return result;
        }

        public GameElement Snake()
        {
            var result = Enemy("snake");
            result.SetData(new ASCIIStyle("s"), new PrefabStyle("Meeple"), new MapAwareness(3), new HitPoints(2), new BumpAttackAbility(2,1));
            return result;
        }

        public GameElement Bat()
        {
            var result = Enemy("bat");
            result.SetData(new ASCIIStyle("b"), new PrefabStyle("Meeple"), new MapAwareness(4), new HitPoints(1), new BumpAttackAbility());
            result.GetData<TurnCounter>().Speed = 2;
            return result;
        }

        public GameElement Groblin()
        {
            var result = Enemy("groblin");
            result.SetData(new ASCIIStyle("g"), 
                new PrefabStyle("Meeple"), 
                new MapAwareness(5), 
                new HitPoints(3),
                new BumpAttackAbility(),
                new OpenDoorAbility(),
                new ElementGender(RNG.NextGender()));
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
            result.SetData(new ASCIIStyle("T"), new PrefabStyle("Meeple"), new MapAwareness(5), new HitPoints(10), new BumpAttackAbility(), new VisionBlocker());
            return result;
        }
    }
}
