﻿using Nucleus.Game;
using Nucleus.Geometry;
using Nucleus.Logs;
using Nucleus.Rendering;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Roli
{
    /// <summary>
    /// A main-line game state for Roli
    /// </summary>
    public class RoliGameState : RLState
    {
        #region Methods

        public override void StartUp()
        {
            var stages = GenerateAllLevels();
            Stage = stages.First();

            Log.WriteScripted("Welcome", Controlled);

            base.StartUp();
        }

        public IList<MapStage> GenerateAllLevels()
        {
            var result = new List<MapStage>();

            var stageLib = new StageLibrary();
            var styles = stageLib.MainLevels();

            // Generate stages in reverse, so we know who to connect to:
            StageExit exit = null;
            int maxLevel = styles.Count;
            for (int i = 0; i < maxLevel; i++)
            {
                var style = styles[maxLevel - i - 1];
                var stage = GenerateStage(style, exit, out int entryIndex);
                stage.Name = "Floor " + (maxLevel - i) + ": " + style.Name;
                result.Insert(0, stage);
                exit = new StageExit(stage, entryIndex);
            }

            var creatures = new CreatureLibrary();
            // Create player character
            var hero = creatures.Hero();
            Controlled = hero;
            result[0].AddElement(hero, exit.CellIndex);

            return result;
        }

        public MapStage GenerateStage(RoliStageStyle style, StageExit exit, out int entryIndex)
        {
            // Initialise state, map and stage:
            int mapX = 24;
            int mapY = 24;
            var stage = new MapStage();
            var map = new SquareCellMap<MapCell>(mapX, mapY);
            stage.Map = map;

            var generator = DesignDungeon(map, style, exit != null);

            var blueprint = generator.Blueprint;
            entryIndex = BuildDungeon(blueprint, stage, style, exit);

            return stage;
        }

        public DungeonArtitect DesignDungeon(SquareCellMap<MapCell> map, RoliStageStyle style, bool includeExit, bool record = false)
        {
            int mapX = map.SizeX;
            int mapY = map.SizeY;

            var rooms = new RoomLibrary();
            // Create map:
            var generator = new DungeonArtitect(mapX, mapY, style);
            generator.RNG = RNG;
            generator.RecordSnapshots = record;

            while (!generator.ExitPlaced)
            {
                generator.ClearBlueprint(mapX, mapY);
                if (!includeExit) generator.ExitPlaced = true; //Do not include exit
                generator.Generate(rooms.Entry());
            }

            return generator;
        }

        private int BuildDungeon(SquareCellMap<BlueprintCell> blueprint, MapStage stage, RoliStageStyle style, StageExit exit)
        {
            int result = 0;
            var map = stage.Map as SquareCellMap<MapCell>;
            map.InitialiseCells();

            var features = new FeatureLibrary();
            var creatures = new CreatureLibrary();
            var items = new ItemLibrary();
            var enemies = style.Creatures;
            var allEnemies = creatures.AllEnemies;
            var allItems = items.AllItems;
            // Build dungeon from blueprint:
            Random rng = new Random();
            for (int i = 0; i < blueprint.CellCount; i++)
            {
                CellGenerationType cGT = blueprint[i].GenerationType;
                if (cGT.IsWall()) //|| cGT == CellGenerationType.Untouched)
                {
                    stage.AddElement(features.Wall(), i);
                }
                else if (cGT == CellGenerationType.Door && rng.NextDouble() < style.DoorChance)
                {
                    // Create door
                    stage.AddElement(features.Door(), i);
                }
                else if (cGT == CellGenerationType.Void)
                {
                    if (blueprint[i].Room.Template.RoomType == RoomType.Exit)
                    {
                        stage.AddElement(features.Exit(exit), i);
                    }
                    else if (blueprint[i].Room.Template.RoomType == RoomType.Entry)
                    {
                        // TODO
                        //stage.AddElement(features.Entrance(), i);
                        result = i;
                    }
                    else
                    {
                        if (rng.NextDouble() < 0.1)
                        {
                            var enemyTable = enemies;
                            if (rng.NextDouble() < 0.08) //Out-of-depth chance
                            {
                                enemyTable = allEnemies;
                            }
                            var func = enemyTable.Roll(RNG);
                            var element = func.Invoke();
                            stage.AddElement(element, i);
                        }
                        else if (rng.NextDouble() < 0.025)
                        {
                            var func = allItems.Roll(RNG);
                            var element = func.Invoke();
                            stage.AddElement(element, i);
                        }
                    }
                }
            }

            stage.Borders = BuildWallOutline(blueprint, style);
            return result;
        }

        public CurveCollection BuildWallOutline(SquareCellMap<BlueprintCell> blueprint, RoliStageStyle style)
        {
            // Build (initial) wall lines:
            bool rocky = style.Roughness > 0;
            var polys = blueprint.Contour(cell => cell.GenerationType == CellGenerationType.Void || cell.GenerationType == CellGenerationType.Door);
            for (int i = 0; i < polys.Count; i++)
            {
                // Bevel
                var poly = polys[i];
                poly.Clean();
                if (rocky)
                {
                    poly = poly.Bevel(0.5, Angle.FromDegrees(10));
                    var rockyPoints = poly.Divide((int)(poly.Length / 0.5));
                    for (int j = 0; j < rockyPoints.Length; j++)
                    {
                        rockyPoints[j] = rockyPoints[j] + new Vector(new Angle(RNG.NextDouble() * 2 * Math.PI)) * RNG.NextDouble() * style.Roughness;
                    }
                    poly = new PolyLine(true, rockyPoints);
                    poly.Clean();
                }
                polys[i] = poly.Bevel(0.1, Angle.FromDegrees(10));
            }
            var result = new CurveCollection();
            result.AddRange(polys);
            return result;
        }

    }

    #endregion
}

