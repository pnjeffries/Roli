using Nucleus.Game;
using Nucleus.Geometry;
using Nucleus.Geometry.Cell_Maps;
using Nucleus.Maths;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roli
{
    public class RoomLibrary
    {
        public FeatureLibrary Features { get; set; } = new FeatureLibrary();

        public RoomTemplate StandardRoom()
        {
            return new RoomTemplate("room", RoomType.Room, 3,4,3,4) { MaxConnections = 2 };
        }

        public RoomTemplate LargeRoom()
        {
            return new RoomTemplate("large room", RoomType.Room, 4, 8, 4, 8) { MaxConnections = 4 };
        }

        public RoomTemplate StoreRoom()
        {
            return new RoomTemplate("storeroom", RoomType.Room, 4, 8, 4, 8) { MaxConnections = 2,
            Features = new FeatureSetOutCollection()
            {
                new FeatureSetOut(Features.Crate, new Interval(0.25, 0.5))
            }
            };
        }

        public RoomTemplate Cell()
        {
            return new RoomTemplate("cell", RoomType.Room, 3, 4, 3, 4) { MaxConnections = 1 , LockChance = 0.25};
        }

        public RoomTemplate Corridor()
        {
            return new RoomTemplate("corridor", RoomType.Circulation, 1, 1, 2, 8)
            {
                SproutTries = 10,
                MaxConnections = 6
            };
        }

        public RoomTemplate Hall()
        {
            return new RoomTemplate("hall", RoomType.Circulation, 2, 3, 2, 8)
            {
                SproutTries = 10,
                MaxConnections = 6
            };
        }

        public RoomTemplate Entry()
        {
            return new RoomTemplate("entry", RoomType.Entry, 1, 1, 1, 1)
            { MaxConnections = 1 };
        }

        public RoomTemplate Exit()
        {
            return new RoomTemplate("exit", RoomType.Exit, 1, 1, 1, 1)
            { MaxConnections = 1 , LockChance = 0.25};
        }

        public RoomTemplate Crevice()
        {
            return new RoomTemplate("crevice", RoomType.Room, 1, 1, 1, 3);
        }

        public RoomTemplate Cavern()
        {
            return new RoomTemplate("cavern", RoomType.Circulation, 2, 10, 2, 10) { MaxConnections = 8,
            Features = new FeatureSetOutCollection()
            {
                new FeatureSetOut(Features.Wall, new Interval(0, 2), 
                ConditionalCellMask.Empty,
                new AdjacencyCellMask(cell => cell is GameMapCell gCell && gCell.HasContentsWithData<Outlined, GameMapCell>(), 2)) // Corners
            }
            };
        }
    }
}
