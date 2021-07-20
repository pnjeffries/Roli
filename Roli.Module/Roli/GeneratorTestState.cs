using Nucleus.Game;
using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roli
{
    /// <summary>
    /// A state used to debug the dungeon generator
    /// </summary>
    public class GeneratorTestState : RoliGameState
    {
        private DungeonArtitect _Artitect;

        private int _Frame = 0;

        public override void StartUp()
        {
            // Initialise state, map and stage:
            int mapX = 32;
            int mapY = 32;
            var stage = new MapStage();
            var map = new SquareCellMap<MapCell>(mapX, mapY);
            stage.Map = map;
            Stage = stage;

            //RNG = new Random(1);

            _Artitect = DesignDungeon(map, true);

            ShowGenerationStage(0);
        }

        public void ShowGenerationStage(int frame)
        {
            if (frame >= _Artitect.Snapshots.Count) frame = _Artitect.Snapshots.Count - 1;
            if (frame < 0) frame = 0;
            _Frame = frame;

            var blueprint = _Artitect.Snapshots[frame];
            var polys = blueprint.Contour(cell => cell == CellGenerationType.Void || cell == CellGenerationType.Door);
            for (int i = 0; i < polys.Count; i++)
            {
                // Bevel
                var poly = polys[i];
                poly.Clean();
                polys[i] = poly.Bevel(0.1, Angle.FromDegrees(10));
            }
            Stage.Borders = new CurveCollection();
            Stage.Borders.AddRange(polys);
        }

        public override void InputRelease(InputFunction input, Vector direction)
        {
            if (input == InputFunction.Right)
            {
                ShowGenerationStage(_Frame + 1);
            }
            else if (input == InputFunction.Left)
            {
                ShowGenerationStage(_Frame - 1);
            }
        }
    }
}
