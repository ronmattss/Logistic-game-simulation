using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace ShadedGames.Scripts.Astar
{
    public class PathFinder : MonoBehaviour
    {
        public Transform seeker, target; // Seeker is the agent finidng a path, target is the destination
                                         // Start is called before the first frame update
        void Start()
        {
            //   grid = this.GetComponent<FieldGrid>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        int GetDistance(FieldNode nodeA, FieldNode nodeB)
        {
            int xDistance = Mathf.Abs(nodeA.gridX - nodeB.gridX);
            int yDistance = Mathf.Abs(nodeA.gridY - nodeB.gridY);

            if (xDistance > yDistance)
                return 14 * yDistance + 10 * (xDistance - yDistance);
            return 14 * xDistance + 10 * (yDistance - xDistance);

        }

        public void FindPath(PathRequest request, Action<PathResult> callback)
        {
            // Finding a way how to instantiate THESE
            FieldNode startNode = new FieldNode(); // will change THIS
            FieldNode endNode = new FieldNode();

            Vector3[] waypoints = new Vector3[0];
            bool pathSuccess = false;
            if (startNode.isPlaceable && endNode.isPlaceable)
            {

                // CHANGE FIELDNODE size to a Variable
                PathFinderHeap<FieldNode> openSet = new PathFinderHeap<FieldNode>(100); // List for unoptimized version
                HashSet<FieldNode> closedSet = new HashSet<FieldNode>();

                openSet.Add(startNode);

                while (openSet.Count > 0)
                {
                    FieldNode currentNode = openSet.RemoveFirst();
                    closedSet.Add(currentNode);

                    if (currentNode == endNode)
                    {
                        // timer
                        Debug.Log(this.transform.name + " path Found ");
                        break;
                    }

                    // calculate gCost and hCost
                    // Need revised FieldNode and Grid


                }
            }
        }
    }
}