using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using ShadedGames.Scripts.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ShadedGames.Scripts.Grid_System
{
    /// <summary>
    ///  Manual WFC modules
    ///  This Cell class is for WFC (will rewrite it as node class )
    /// </summary>
    /// 
    public class WaveFunctionNode : MonoBehaviour, IHeapItem<WaveFunctionNode>
    {
        private Vector3 worldPosition;
        private ConnectionType[] connectionType = new ConnectionType[4];
        [SerializeField] private List<ScriptableModule> possibleModules = new List<ScriptableModule>();
        [SerializeField] private ScriptableModule scriptableModule;

        [ItemCanBeNull]
        private Dictionary<CompassDirection, WaveFunctionNode> nodeNeighbors = new Dictionary<CompassDirection, WaveFunctionNode>();

        [SerializeField] private WaveFunctionNode[] neighborObjects = new WaveFunctionNode[4];
        private bool isCollapsed = false;
        [SerializeField] private Direction direction;


        // public Module SetupModule(Vector3 worldPosition, Vector2Int origin, ScriptableModule module)
        // {
        //     GameObject placedModule = Instantiate(module.moduleGameObject, worldPosition, Quaternion.identity);
        // }

        private void Awake()
        {
        }

        public List<ScriptableModule> GetPossibleModules() => possibleModules;

        public void SetModules(List<ScriptableModule> modules)
        {
            foreach (var module in modules)
            {
                possibleModules.Add(module);
            }
        }

        public ConnectionType[] GetConnections() => connectionType;

        // Connection per Direction
        // North South

        // 


        public void SetNeighbor()
        {
            int x, y;
            ProceduralGridSystem.Instance.GetGrid().GetXZ(worldPosition, out x, out y);
            neighborObjects[0] = ProceduralGridSystem.Instance.GetCellOnGrid(x, y + 1); // North
            neighborObjects[1] = ProceduralGridSystem.Instance.GetCellOnGrid(x + 1, y); // East
            neighborObjects[2] = ProceduralGridSystem.Instance.GetCellOnGrid(x, y - 1); // South
            neighborObjects[3] = ProceduralGridSystem.Instance.GetCellOnGrid(x - 1, y); // West
        }


        // Now we have neighbors it's time to Collapse
        // first we need to see if there are Cells that has been collapsed. meaning if there are uncollapsed filter, 
        // we disregard it at filtering
        // if all sides are uncollapsed just pick a random module
        public bool CheckNeighborsIfCollapsed()
        {
            // Check if all neighbors are uncollapsed
            return neighborObjects.Any(t => t.isCollapsed);
        }

        public void SetModule(ScriptableModule module)
        {
            possibleModules = new List<ScriptableModule> { module };
            ProceduralGridSystem.Instance.orderedCells.UpdateItem(this);

            Collapse();
           // Debug.Log($"Possible Modules Left{possibleModules.Count}");
            scriptableModule = possibleModules[0];
            Debug.Log($"this cell: {this.worldPosition} is null, Number of Modules{possibleModules.Count}");
            direction = new Direction(scriptableModule.connections.ToList());
        }

        public ScriptableModule GetModule() => scriptableModule;

        public void FilterCell(EdgeFilter filter)
        {
            if (possibleModules.Count == 1) return;
            var removingModules = new List<ScriptableModule>();

            for (var i = 0; i < possibleModules.Count; i++)
            {
                if (filter.CheckModule(possibleModules[i]))
                {
                    removingModules.Add(possibleModules[i]);
                }
            }

            // remove filtered modules
            for (var i = 0; i < removingModules.Count; i++)
            {
                RemoveModule(removingModules[i]);
            }
        }

        public void Collapse()
        {
            // check if the current cell fits to other "collapsed/ finished" neighboring cells
            for (var i = 0; i < neighborObjects.Length; i++)
            {
                // if neighbor is null or neighbor[i].possibleModules is Greater than 1
                if (neighborObjects[i] == null || neighborObjects[i].possibleModules.Count > 1) continue;

                if (possibleModules[0].connections[i] != neighborObjects[i].possibleModules[0].connections[(i + 2) % 4])
                {
                    Debug.LogError(
                        $"Setting module {possibleModules[0]} would not fit already set neighbour {neighborObjects[i].gameObject}!",
                        gameObject);
                }
            }

            //Propagate changes to neighbors
            for (var i = 0; i < neighborObjects.Length; i++)
            {
                if (neighborObjects[i] == null) continue;

                neighborObjects[i].FilterCell(new EdgeFilter(i, possibleModules[0].connections[i], true));
            }
        }

        public void RemoveModule(ScriptableModule module)
        {
            // remove a module from list of possible modules
            possibleModules.Remove(module);

            // update the cell on the heap
            ProceduralGridSystem.Instance.orderedCells.UpdateItem(this);

            for (var i = 0; i < neighborObjects.Length; i++)
            {
                // only check if has a neighbor on this side
                if (neighborObjects[i] == null) continue;

                var edgeType = module.connections[i];
                var lastWithEdgeType = true;

                // search other possible modules for same edge type
                for (var j = 0; j < possibleModules.Count; j++)
                {
                    if (possibleModules[j].connections[i] == edgeType)
                    {
                        lastWithEdgeType = false;
                        break;
                    }
                }

                if (lastWithEdgeType)
                {
                    // populate edge changes to neighbor cell
                    var edgeFilter = new EdgeFilter(i, edgeType, false);
                    neighborObjects[i].FilterCell(edgeFilter);
                }
            }
        }


        // world Position is not the MidPoint Position of the Prefab?
        public void SetWorldPosition(Vector3 worldPos)
        {
            worldPosition = worldPos;
        }

        public Vector3 GetWorldPosition() => worldPosition;

        public int CompareTo(WaveFunctionNode other)
        {
            var compare = possibleModules.Count.CompareTo(other.possibleModules.Count);
            if (compare == 0)
            {
                var r = Random.Range(1, 3);
                return r == 1 ? -1 : 1;
            }

            return -compare;
        }

        public int HeapIndex { get; set; }
    }
}