using System;
using System.Collections.Generic;
using ShadedGames.Scripts.Grid_System;

namespace ShadedGames.Scripts.Utils
{
    /// <summary>
    /// Handles the type of connection is on this direction
    /// </summary>
    [Serializable]
    public class Direction
    {
        public List<ConnectionType> path;

        public Direction(List<ConnectionType> path)
        {
            this.path = path;
        }
    }
}