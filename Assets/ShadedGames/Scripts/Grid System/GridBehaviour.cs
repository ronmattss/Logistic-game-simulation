using System;
using System.Collections.Generic;
using Grid_System.Building_System;
using ShadedGames.Scripts.Grid_System.Building_System;
using ShadedGames.Scripts.Utils;
using UnityEngine;
using Utils;

namespace ShadedGames.Scripts.Grid_System
{
    /// <summary>
    /// GridBehaviour / TODO: Rename it
    /// This will be 
    /// </summary>
    public class GridBehaviour : MonoBehaviour
    {
        // Start is called before the first frame update

        public static GridBehaviour Instance { get; private set; } // Singleton

        public event EventHandler OnSelectedChanged;
        //public event EventHandler OnObjectPlaced;


        [SerializeField] private List<ScriptablePlaceableObject> placeableObjectsList;
        private ScriptablePlaceableObject scriptablePlaceableObject;
        private ScriptablePlaceableObject.Dir direction;
        private Grid<GridSpawnerObject> grid;

        void Awake()
        {
            Instance = this;
            // grid = new Grid<GridSpawnerObject>(20, 10, 10f, Vector3.zero,
            //     (Grid<GridSpawnerObject> g, int x, int y) => new GridSpawnerObject(g, x, y));
            grid.DebugLine();
            grid.DebugText();

            scriptablePlaceableObject = null;
        }

        private void Update()
        {
            // if (Input.GetMouseButtonDown(0) && scriptablePlaceableObject != null)
            // {
            //     Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
            //     grid.GetXZ(mousePosition, out int x, out int z);
            //     Debug.Log($"{x} {z}");
            //     Vector2Int placedObjectOrigin = new Vector2Int(x, z);
            //     placedObjectOrigin = grid.ValidateGridPosition(placedObjectOrigin);
            //
            //     List<Vector2Int> gridPositionList =
            //         scriptablePlaceableObject.GetGridPositionList(placedObjectOrigin, direction);
            //
            //     // debug Test if can Build
            //     bool canBuild = true;
            //     foreach (var gridPosition in gridPositionList)
            //     {
            //         if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
            //         {
            //             canBuild = false;
            //             break;
            //         }
            //     }
            //
            //     if (canBuild)
            //     {
            //         Vector2Int rotationOffset =
            //             scriptablePlaceableObject.GetRotationOffset(direction); // rotate with offset
            //         Vector3 placedObjectWorldPosition =
            //             grid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y) +
            //             new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
            //
            //         global::Grid_System.Building_System.Cell cell = global::Grid_System.Building_System.Cell.Create(placedObjectWorldPosition, placedObjectOrigin,
            //             direction, scriptablePlaceableObject);
            //
            //         foreach (var gridPosition in gridPositionList)
            //         {
            //             grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(cell);
            //         }
            //
            //         OnObjectPlaced?.Invoke(this, EventArgs.Empty);
            //     }
            //     else
            //     {
            //         Debug.Log("You can't place an object here");
            //         Utility.CreateWorldTextPopup("You can't place an object here", mousePosition);
            //     }
            // }
            //
            // if (Input.GetMouseButtonDown(1))
            // {
            //     Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
            //     if (grid.GetGridObject(mousePosition) != null)
            //     {
            //         global::Grid_System.Building_System.Cell cell = grid.GetGridObject(mousePosition).GetPlacedObject();
            //         if (cell != null)
            //         {
            //             cell.DestroySelf();
            //
            //             List<Vector2Int> gridPositionList = cell.GetGridPositionList();
            //             foreach (Vector2Int gridPosition in gridPositionList)
            //             {
            //                 grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
            //             }
            //         }
            //     }
            // }

            if (Input.GetKeyDown(KeyCode.R))
            {
                direction = ScriptablePlaceableObject.GetNextDir(direction);
            }


            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                scriptablePlaceableObject = placeableObjectsList[0];
                RefreshSelectedObjectType();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                scriptablePlaceableObject = placeableObjectsList[1];
                RefreshSelectedObjectType();
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                scriptablePlaceableObject = placeableObjectsList[2];
                RefreshSelectedObjectType();
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                scriptablePlaceableObject = placeableObjectsList[3];
                RefreshSelectedObjectType();
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                scriptablePlaceableObject = placeableObjectsList[4];
                RefreshSelectedObjectType();
            }

            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                scriptablePlaceableObject = placeableObjectsList[5];
                RefreshSelectedObjectType();
            }

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                DeselectObjectType();
            }
        }

        private void DeselectObjectType()
        {
            scriptablePlaceableObject = null;
            RefreshSelectedObjectType();
        }

        private void RefreshSelectedObjectType()
        {
            OnSelectedChanged?.Invoke(this, EventArgs.Empty);
        }

        public Vector3 GetMouseWorldSnappedPosition()
        {
            Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
            grid.GetXZ(mousePosition, out int x, out int z);

            if (scriptablePlaceableObject != null)
            {
                Vector2Int rotationOffset = scriptablePlaceableObject.GetRotationOffset(direction);
                Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z) +
                                                    new Vector3(rotationOffset.x, 0, rotationOffset.y) *
                                                    grid.GetCellSize();
                return placedObjectWorldPosition;
            }
            else
            {
                return mousePosition;
            }
        }

        public Quaternion GetPlacedObjectRotation()
        {
            if (scriptablePlaceableObject != null)
            {
                return Quaternion.Euler(0, scriptablePlaceableObject.GetRotationAngle(direction), 0);
            }
            else
            {
                return Quaternion.identity;
            }
        }

        public ScriptablePlaceableObject GetPlacedObjectTypeSO()
        {
            return scriptablePlaceableObject;
        }
    }
    
}