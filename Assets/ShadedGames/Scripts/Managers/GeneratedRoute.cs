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
            public List<Node> nodeWaypoint = new List<Node>();

            public GeneratedRoute(string v, List<Node> waypointsToAdd)
            {
                routeName = v;
                nodeWaypoint = waypointsToAdd;
            }
        }
    

}