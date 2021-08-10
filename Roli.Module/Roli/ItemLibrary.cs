using Nucleus.Game;
using Nucleus.Game.Actions;
using Nucleus.Game.Artitecture;
using Nucleus.Game.Components;
using Nucleus.Game.Effects;
using Nucleus.Geometry;
using Nucleus.Logs;
using Nucleus.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roli
{
    /// <summary>
    /// A library of items
    /// </summary>
    public class ItemLibrary
    {
        /// <summary>
        /// The delegate function for creating creatures
        /// </summary>
        /// <returns></returns>
        public delegate GameElement Create();

        public WeightedTable<Create> AllItems
        {
            get
            {
                return new WeightedTable<Create>
                   (
                        Sword, Mace, Spear, Bow, HealthPotion, Poison
                   );
            }
        }

        /// <summary>
        /// Base setup for all weapons
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private GameElement Weapon(string name)
        {
            var result = new StaticElement(name);
            result.SetData(
                new EquippableItem(),
                new PickUp(),
                new LogDescription("<color=#FF00FD>", "</color>"));
            return result;
        }

        private GameElement Potion(string name)
        {
            var result = new StaticElement(name);
            result.SetData(
                new PickUp(),
                new LogDescription("<color=#FF00FD>", "</color>"));
            return result;
        }

        /// <summary>
        /// A standard sword
        /// </summary>
        /// <returns></returns>
        public GameElement Sword()
        {
            var sword = Weapon("sword");
            sword.SetData(
                new ASCIIStyle("↑"),
                new PrefabStyle("Sword"),
                new QuickAttack(2, DamageType.Base, 1),
                new ItemActions(
                    new WindUpAction("WindUp_sword",
                        new AOEAttackActionFactory(new IEffect[]
                        {
                            new SFXImpactEffect(),
                            new KnockbackEffect(Vector.UnitX, 2),
                            new DamageEffect(3)
                        },
                        "Slash",
                        1, 0),
                        new EquipItemEffect(sword)
                        )));
            return sword;
        }

        /// <summary>
        /// A standard mace
        /// </summary>
        /// <returns></returns>
        public GameElement Mace()
        {
            var sword = Weapon("mace");
            sword.SetData(
                new ASCIIStyle("↑"),
                new PrefabStyle("Mace"),
                new QuickAttack(1, DamageType.Base, 2),
                new ItemActions(
                    new WindUpAction("WindUp_mace",
                        new AOEAttackActionFactory(new IEffect[]
                        {
                            new SFXImpactEffect(),
                            new KnockbackEffect(Vector.UnitX, 3),
                            new DamageEffect(2)
                        },
                        "Slash",
                        1, 0),
                        new EquipItemEffect(sword)
                        )));
            return sword;
        }

        /// <summary>
        /// A standard spear
        /// </summary>
        /// <returns></returns>
        public GameElement Spear()
        {
            var sword = Weapon("spear");
            sword.SetData(
                new ASCIIStyle("↑"),
                new PrefabStyle("Spear"),
                new QuickAttack(1, DamageType.Base, 1),
                new ItemActions(
                    new WindUpAction("WindUp_spear",
                        new RangedAOEAttackActionFactory(2,new IEffect[]
                        {
                            new SFXImpactEffect(),
                            new KnockbackEffect(Vector.UnitX, 2),
                            new DamageEffect(1)
                        },
                        "SpearStab",
                        2, 0, 1, 0),
                        new EquipItemEffect(sword)
                        )));
            return sword;
        }

        /// <summary>
        /// A bow
        /// </summary>
        /// <returns></returns>
        public GameElement Bow()
        {
            var sword = Weapon("bow");
            sword.SetData(
                new ASCIIStyle(")"),
                new PrefabStyle("Bow"),
                //new QuickAttack(1, DamageType.Base, 1),
                new ItemActions(
                    new WindUpAction("WindUp_bow",
                        new RangedAOEAttackActionFactory(6, new IEffect[]
                        {
                            new SFXImpactEffect(),
                            new KnockbackEffect(Vector.UnitX, 1),
                            new DamageEffect(1)
                        },
                        "SpearStab",
                        0, 0)
                        )));
            return sword;
        }


        public GameElement HealthPotion()
        {
            var item = Potion("health potion");
            item.SetData(
                new ASCIIStyle("¡"),
                new ConsumableItem(3));
            // Item has to be set consumable before creating the use item action
            item.SetData(
                new ItemActions(
                    new UseItemAction("Drink", item, new HealEffect()))); //TODO
            return item;
        }

        public GameElement Poison()
        {
            var item = Potion("poison");
            item.SetData(
                new ASCIIStyle("¡"),
                new ConsumableItem(3));
            // Item has to be set consumable before creating the use item action
            item.SetData(
                new ItemActions(
                    new UseItemAction("Drink", item, new ApplyStatusEffect(new Poisoned())))); //TODO
            return item;
        }
    }
}
