using Nucleus.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roli
{
    public class RoomLibrary
    {
        public RoomTemplate StandardRoom()
        {
            return new RoomTemplate(RoomType.Room, 3,4,3,4) { MaxConnections = 2 };
        }

        public RoomTemplate LargeRoom()
        {
            return new RoomTemplate(RoomType.Room, 4, 8, 4, 8) { MaxConnections = 4 };
        }

        public RoomTemplate Cell()
        {
            return new RoomTemplate(RoomType.Room, 3, 4, 3, 4) { MaxConnections = 1 };
        }

        public RoomTemplate Corridor()
        {
            return new RoomTemplate(RoomType.Circulation, 1, 1, 4, 8)
            {
                SproutTries = 10,
                MaxConnections = 6
            };
        }

        public RoomTemplate Exit()
        {
            return new RoomTemplate(RoomType.Exit, 1, 1, 1, 1)
            { MaxConnections = 1 };
        }
    }
}
