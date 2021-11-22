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
                        Sword, Axe, Mace, Spear, Bow, Scythe,
                        HealthPotion, InvincibilityPotion, StrengthPotion, SpeedTonic,
                        Shield,
                        Coins, Arrows // Poison,
                   );
            }
        } 

        /// <summary>
        /// Generate a new item element by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GameElement ByName(string name)
        {
            name = name.Replace('_', ' ');
            foreach (var create in AllItems)
            {
                // This is a bit inefficient, but Mono throws a wobbly
                // if we try to access the Method name itself
                var element = create.Key.Invoke();
                if (element.Name == name) return element;
            }
            return null;
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

        public GameElement Key(string name, string keyCode)
        {
            var result = new StaticElement(name);
            result.SetData(
                new PickUp(),
                new LogDescription("<color=#FF00FD>", "</color>"),
                new ASCIIStyle("ϙ"),
                new Key(keyCode));
            return result;
        }

        /// <summary>
        /// Create a resource pickup to represent the given resource quantity
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public GameElement Resource(Resource resource)
        {
            return resource.CreatePickup();
        }

        /// <summary>
        /// A shield
        /// </summary>
        /// <returns></returns>
        public GameElement Shield()
        {
            var result = new StaticElement("shield");
            result.SetData(
                new PickUp(),
                new LogDescription("<color=#FF00FD>", "</color>"),
                new ASCIIStyle("○"),
                new ItemActions(
                    new UseItemAction("defend", result, new ApplyStatusEffect(new Shielded())))
                );
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
                        "Attack_sword",
                        "Slash",
                        1, 0),
                        new EquipItemEffect(sword)
                        )));
            return sword;
        }

        /// <summary>
        /// A standard axe
        /// </summary>
        /// <returns></returns>
        public GameElement Axe()
        {
            var weapon = Weapon("axe");
            weapon.SetData(
                new ASCIIStyle("↑"),
                new PrefabStyle("Axe"),
                new QuickAttack(2, DamageType.Base, 1),
                new ItemActions(
                    new WindUpAction("WindUp_axe",
                        new AOEAttackActionFactory(new IEffect[]
                        {
                            new SFXImpactEffect(),
                            new KnockbackEffect(Vector.UnitX, 0),
                            new DamageEffect(4)
                        },
                        "Attack_axe",
                        "Slash",
                        1, 0),
                        new EquipItemEffect(weapon)
                        )));
            return weapon;
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
                        "Attack_mace",
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
                //new QuickAttack(1, DamageType.Base, 1),
                new ItemActions(
                    new WindUpAction("WindUp_spear",
                        new RangedAOEAttackActionFactory(2,new IEffect[]
                        {
                            new SFXImpactEffect(),
                            new KnockbackEffect(Vector.UnitX, 1),
                            new DamageEffect(2)
                        },
                        "Attack_spear",
                        "SpearStab",
                        0,0),//2, 0, 1, 0),
                        new EquipItemEffect(sword)
                        )));
            return sword;
        }

        /// <summary>
        /// A scythe
        /// </summary>
        /// <returns></returns>
        public GameElement Scythe()
        {
            var weapon = Weapon("scythe");
            weapon.SetData(
                new ASCIIStyle("↑"),
                new PrefabStyle("Scythe"),
                //new QuickAttack(1, DamageType.Base, 1),
                new ItemActions(
                    new WindUpAction("WindUp_scythe",
                        new AOEAttackActionFactory(new IEffect[]
                        {
                            new SFXImpactEffect(),
                            new KnockbackEffect(Vector.UnitX, 1),
                            new DamageEffect(2)
                        },
                        "Attack_scythe",
                        "Sweep",
                        1, -1, 1, 0, 1, 1),//2, 0, 1, 0),
                        new EquipItemEffect(weapon)
                        )));
            return weapon;
        }

        /// <summary>
        /// A bow
        /// </summary>
        /// <returns></returns>
        public GameElement Bow()
        {
            var weapon = Weapon("bow");
            weapon.SetData(
                new ASCIIStyle(")"),
                new PrefabStyle("Bow"),
                //new ConsumableItem(6),
                //new QuickAttack(1, DamageType.Base, 1),
                new ItemActions(
                    new WindUpAction("WindUp_bow", new Resource(ResourceTypes.Arrows,1),
                        new RangedAOEAttackActionFactory(6, new IEffect[]
                        {
                            new SFXImpactEffect(),
                            new KnockbackEffect(Vector.UnitX, 1),
                            new DamageEffect(2)
                        },
                        new IEffect[]
                        {
                            new ConsumeResourceEffect(new Resource(ResourceTypes.Arrows, 1))
                        },
                        "Attack_bow",
                        "SpearStab",
                        0, 0)
                        )));
            return weapon;
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

        public GameElement InvincibilityPotion()
        {
            var item = Potion("invincibility potion");
            item.SetData(
                new ASCIIStyle("¡"),
                new ConsumableItem(3));
            // Item has to be set consumable before creating the use item action
            item.SetData(
                new ItemActions(
                    new UseItemAction("Drink", item, new ApplyStatusEffect(new Invincible()), new SFXEffect(SFXKeywords.Heal)))); //TODO
            return item;
        }

        public GameElement SpeedTonic()
        {
            var item = Potion("speed tonic");
            item.SetData(
                new ASCIIStyle("¡"),
                new ConsumableItem(3));
            // Item has to be set consumable before creating the use item action
            item.SetData(
                new ItemActions(
                    new UseItemAction("Drink", item, new ApplyStatusEffect(new Quickened()), new SFXEffect(SFXKeywords.Heal)))); //TODO
            return item;
        }

        public GameElement StrengthPotion()
        {
            var item = Potion("strength potion");
            item.SetData(
                new ASCIIStyle("¡"),
                new ConsumableItem(3));
            // Item has to be set consumable before creating the use item action
            item.SetData(
                new ItemActions(
                    new UseItemAction("Drink", item, new ApplyStatusEffect(new Strong()), new SFXEffect(SFXKeywords.Heal)))); //TODO
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

        public GameElement Arrows()
        {
            return Resource(new Resource(ResourceTypes.Arrows, 6));
        }

        public GameElement Coins()
        {
            return Resource(new Resource(ResourceTypes.Coins, 10));
        }
    }
}
