using ShadedGames.Scripts.Grid_System;

namespace ShadedGames.Scripts.Wave_Function
{
    public class EdgeFilter
    {

        private readonly ConnectionType _filterType;

        private readonly bool _isInclusive; // dk what is this

        private readonly int _edgeDirection; // direction of the edge [bottom right top left]

        // EdgeFilter Constructor
        public EdgeFilter(int edgeDirection, ConnectionType filterType, bool isInclusive)
        {
            _edgeDirection = edgeDirection;
            _filterType = filterType;
            _isInclusive = isInclusive;
        }
        
        //returns true if a module matches this type of filter
        public bool CheckModule(WaveFunctionNode cell)
        {
            var edge = (_edgeDirection + 2) % 4; // this is a weird equation to get the edge direction but it works
            var match = cell.GetConnections()[edge] == _filterType; // does this module's edge match with the filter type?

            return _isInclusive ? !match : match;
        }


    }
}