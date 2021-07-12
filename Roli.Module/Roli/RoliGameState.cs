using Nucleus.Game;
using Nucleus.Geometry;
using Nucleus.Rendering;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Roli
{
    /// <summary>
    /// A main-line game state for Roli
    /// </summary>
    public class RoliGameState : RLState
    {
        #region Properties

        private CurveCollection _WallLines = null;
        
        /// <summary>
        /// Curves representing the border between walkable and solid space
        /// </summary>
        public CurveCollection WallLines
        {
            get { return _WallLines; }
            set { ChangeProperty(ref _WallLines, value); }
        }

        #endregion

        #region Methods

        public override void StartUp()
        {
            // Initialise state, map and stage:
            int mapX = 32;
            int mapY = 32;
            var stage = new MapStage();
            var map = new SquareCellMap<MapCell>(mapX, mapY);
            stage.Map = map;
            Stage = stage;

            var generator = GenerateLevel(map);

            var blueprint = generator.Blueprint;
            BuildDungeon(blueprint);

            var creatures = new CreatureLibrary();
            // Create player character
            var hero = creatures.Hero();
            map[5, 6].PlaceInCell(hero);
            Elements.Add(hero);
            Controlled = hero;

            base.StartUp();
        }

        public DungeonArtitect GenerateLevel(SquareCellMap<MapCell> map, bool record = false)
        {
            int mapX = map.SizeX;
            int mapY = map.SizeY;

            var rooms = new RoomLibrary();
            // Create map:
            var generator = new DungeonArtitect(mapX, mapY);
            generator.RNG = RNG;
            generator.Templates.Add(rooms.StandardRoom());
            generator.Templates.Add(rooms.LargeRoom());
            generator.Templates.Add(rooms.Corridor());
            generator.Templates.Add(rooms.Cell());
            generator.Templates.Add(rooms.Exit());
            generator.RecordSnapshots = record;

            while (!generator.ExitPlaced)
            {
                generator.ClearBlueprint(mapX, mapY);
                generator.Generate(5, 4, rooms.StandardRoom(), CompassDirection.North);
            }

            return generator;
        }

        private void BuildDungeon(SquareCellMap<BlueprintCell> blueprint)
        {
            var map = Stage.Map;
            map.InitialiseCells();

            // Build dungeon from blueprint:
            Random rng = new Random();
            for (int i = 0; i < blueprint.CellCount; i++)
            {
                CellGenerationType cGT = blueprint[i].GenerationType;
                if (cGT.IsWall()) //|| cGT == CellGenerationType.Untouched)
                {
                    var wall = new GameElement("Wall");
                    wall.SetData(new ASCIIStyle("#"), new PrefabStyle("Wall"), new MapCellCollider(),
                        new VisionBlocker(), new Memorable(), new Inertia(true));
                    map[i].PlaceInCell(wall);
                    Elements.Add(wall);
                }
                else if (cGT == CellGenerationType.Door && rng.NextDouble() > 0.5)
                {
                    // Create door
                    /*var door = new ActiveElement("Door");
                    door.SetData(new ASCIIStyle("+"), new VisionBlocker(), new Memorable(), new Inertia(true));
                    map[i, j].PlaceInCell(door);
                    state.Elements.Add(door);*/
                }
                else if (cGT == CellGenerationType.Void)
                {
                    if (blueprint[i].Room.Template.RoomType == RoomType.Exit)
                    {
                        var exit = new ActiveElement("Exit");
                        exit.SetData(new ASCIIStyle(">"),
                            new Memorable(), new Inertia(true));
                        map[i].PlaceInCell(exit);
                        Elements.Add(exit);
                    }
                }
            }

            BuildWallOutline(blueprint);
        }

        public void BuildWallOutline(SquareCellMap<BlueprintCell> blueprint)
        {
            // Build (initial) wall lines:
            bool rocky = false;
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
                        rockyPoints[j] = rockyPoints[j] + new Vector(new Angle(RNG.NextDouble() * 2 * Math.PI)) * RNG.NextDouble() * 0.1;
                    }
                    poly = new PolyLine(true, rockyPoints);
                    poly.Clean();
                }
                polys[i] = poly.Bevel(0.1, Angle.FromDegrees(10));
            }
            WallLines = new CurveCollection();
            WallLines.AddRange(polys);
        }

    }

    #endregion
}

