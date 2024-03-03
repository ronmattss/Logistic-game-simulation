using System.Collections.Generic;
using System;

namespace ShadedGames.Scripts.Managers
{


 
        // Start is called before the first frame update
        // in-game there will be lists of COURIERs that can request paths 
        // for now u select an Agent,

        [Serializable]
        public class GeneratedRoute
        {
            public string routeName;
            public Node destination; // Nearest Node to the destination
            public Node startingPoint; // Agent current Location Node
            public List<Node> nodeWaypoint = new List<Node>();

            // Starting Node
            // Destination Node
            // Path Generated ? by Pathfinding Algo
            // Generated Route will be fed to the AgentRouteManager
            //

            public GeneratedRoute(string v, List<Node> waypointsToAdd)
            {
                routeName = v;
                nodeWaypoint = waypointsToAdd;
            }
        }
    

}