using System;
using UnityEngine;
using Utils;

namespace ShadedGames.Scripts.Grid_System

{
    /// <summary>
    ///  Should This Grid Class support infinitely generated grid?
    /// </summary>
    /// 
    [Serializable]
    public class Grid<TGridObject>
    {
        public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;

        public class OnGridObjectChangedEventArgs : EventArgs
        {
            public int x;
            public int y;
            public int z;
        }

        private int height;
        private int width;
        private float cellSize;
        private Vector3 originPosition;
        private TGridObject[,] gridArray;
        private GameObject worldTextParent;


        //Debug


        public Grid(int width, int height, float cellSize, Vector3 originPosition,
            Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.originPosition = originPosition;

            gridArray = new TGridObject[width, height];

            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    gridArray[x, y] = createGridObject(this, x, y);
                }
            }
        }

        public void DebugLine()
        {
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.green, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.green, 100f);
                }
            }

            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.green, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.green, 100f);
        }

        public void DebugText()
        {   worldTextParent = GameObject.Find("DebugWorldTextParent");
            TextMesh[,] debugTextArray = new TextMesh[width, height];

            OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) =>
            {
                debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
            };
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    debugTextArray[x, y] = Utility.CreateWorldText(gridArray[x,y]?.ToString(), true, null,
                        GetCellMidPoint(x, y), 30, Color.white, TextAnchor.MiddleCenter);

                    debugTextArray[x,y].transform.parent = worldTextParent.transform;
                }
            }
        }

        public void ClearTGridObjectArray()
        {
            if (gridArray == null || gridArray.Length == 0)
            {
                Debug.Log("Grid Array is Empty!");
            }
            else
            {
                Array.Clear(gridArray, 0, gridArray.Length);
                worldTextParent = GameObject.Find("DebugWorldTextParent");
                for (int i = 0; i < worldTextParent.transform.childCount; i++)
                {
                    GameObject.DestroyImmediate(worldTextParent.transform.GetChild(i).gameObject);
                }
            }




        }

        // position of the cell in world space NOTE THAT THIS WILL BE IN THE Z AXIS

        // This Should place ANY GAMEOBJECT in the center of a cell. 


        public int GetWidth() => width;
        public int GetHeight() => height;
        public float GetCellSize() => cellSize;

        // Might Change this to a full 3D Grid
        public Vector3 GetWorldPosition(int x, int y) => new Vector3(x, 0, y) * cellSize + originPosition;
        // This just return the midpoint of the coordinates/ Hard coded FOR NOW, formula probably cell size/2
        public Vector3 GetNodeMidPointViaWorldPosition(int x, int y) { 
    return new Vector3(x+5, 0, y+5);
        }

        public Vector3 GetCellMidPoint(int x, int y) =>
            GetWorldPosition(x, y) + new Vector3(cellSize, 0, cellSize) * .5f;

        // Get the X Y value of the cell via WorldPosition
        public void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
            y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
        }

        public void GetXZ(Vector3 worldPosition, out int x, out int z)
        {
            x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
            z = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);
        }


        public void SetGridObject(int x, int y, TGridObject value)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                gridArray[x, y] = value;
                if (OnGridObjectChanged != null)
                    OnGridObjectChanged(this,
                        new OnGridObjectChangedEventArgs
                            { x = x, y = y }); // this just means that if something changed, change the cell grid value
            }
        }

        public bool ValidateCoordinates(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < width && y < height) return true;
            return false;

        }

        /// <summary>
        /// If The cell has changed, trigger an event to notify a changed
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void TriggerGridObjectChanged(int x, int y)
        {
            if (OnGridObjectChanged != null)
                OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
        }

        /// <summary>
        /// Set Value using wordlPosition
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <param name="value"></param>
        public void SetGridObject(Vector3 worldPosition, TGridObject value)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            SetGridObject(x, y, value);
        }

        public TGridObject GetGridObject(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                return gridArray[x, y];
            }

            return default;
        }

        public TGridObject GetGridObject(Vector3 worldPosition)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            return GetGridObject(x, y);
        }
        
        public Vector2Int ValidateGridPosition(Vector2Int gridPosition) {
            return new Vector2Int(
                Mathf.Clamp(gridPosition.x, 0, width - 1),
                Mathf.Clamp(gridPosition.y, 0, height - 1)
            );
        }

        public TGridObject GetGridObjectWithRawCoordinates(int x,int y)
        {
            //Debug.Log($"GridObject Initial Coordinates and results: {x} {y} {GetGridObject(x, y)} {gridArray.Length}");          
            return GetGridObject(x, y);
        }

        // WORLD POSITION TO GRID POSITION
    }
}