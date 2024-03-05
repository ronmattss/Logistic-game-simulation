using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ShadedGames.Scripts.Grid_System;


namespace ShadedGames.Scripts.Astar
{
    public class PathFinder : MonoBehaviour
    {
        public Transform seeker, target; // Seeker is the agent finidng a path, target is the destination
                                         // Start is called before the first frame update
        public GridSystem grid;
        void Start()
        {
            grid = this.GetComponent<GridSystem>();
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

        // PathRequest Get FieldNode From The Position of the Agent, AND GetFieldNode For the Destination ( Mouse Input, Struncture, Agent Position)
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
                    foreach (FieldNode neighbor in grid.GetNeighbors(currentNode.nodeParent))
                    {
                        if (!neighbor.isPlaceable || closedSet.Contains(neighbor))
                        {
                            continue;
                        }
                        int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor) + neighbor.movementPenalty;
                        if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                        {
                            neighbor.gCost = newMovementCostToNeighbor;
                            neighbor.hCost = GetDistance(neighbor, endNode);
                            neighbor.parent = currentNode;
                            //add neighbor to openset if it is already
                            if (!openSet.Contains(neighbor))
                            {
                                openSet.Add(neighbor);

                            }
                            else
                            {
                                openSet.UpdateItem(neighbor);
                            }

                        }
                    }

                }
            }
            if (pathSuccess)
            {
                waypoints = TracePath(startNode, endNode);

                pathSuccess = waypoints.Length > 0;

            }
            callback(new PathResult(waypoints, pathSuccess, request.callBack));
            /// TODO: CALLBACK PATHRESULT AND WAYPOINTS AND WHAT WILL BE RETURND

        }

        Vector3[] TracePath(FieldNode startNode, FieldNode endNode)
        {
            List<FieldNode> path = new List<FieldNode>();
            FieldNode currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            Vector3[] waypoints = SimplifyPath(path);
            Array.Reverse(waypoints);
            return waypoints;
        }


        Vector3[] SimplifyPath(List<FieldNode> path)
        {
            List<Vector3> waypoints = new List<Vector3>();
            Vector2 directionOld = Vector2.zero;
            //    waypoints.Add(path[0].worldPosition); // Modified final Node
            for (int i = 1; i < path.Count; i++)
            {
                Vector2 directionnew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
                if (directionnew != directionOld)
                {
                    waypoints.Add(path[i].worldPosition);       // Modified  OLD: [i]
                    directionOld = directionnew;
                }
            }
            return waypoints.ToArray();
        }
    }
}