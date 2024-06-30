using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProjectAssets.Scripts.Util;
using ShadedGames.Scripts.Astar;
using Unity.Mathematics;
using UnityEngine;

namespace ShadedGames.Scripts.Grid_System
{
    /// <summary>
    /// TODO: Separate Grid System used in Editor and IN GAME
    /// </summary>
    public class GridSystem : Singleton<GridSystem>
    {

        [SerializeField] private Grid<GridNodeOjbect> grid;  // This is the GRID that will be used by this system


        [SerializeField] private GameObject blankCellPrefab;
        [SerializeField] private int width = 10;
        [SerializeField] private int height = 10;
        [SerializeField] private float cellSize = 10f;
        [SerializeField] private List<GameObject> generatedObjects = new List<GameObject>();
        [SerializeField] private List<Cell> cellGrid = new List<Cell>();
        [SerializeField] private List<Cell> tempCellGrid = new List<Cell>();

        [SerializeField] private GameObject parentGridGameObject;
        [SerializeField] private StoredCells generalGridData;


        public Grid<GridNodeOjbect> GetGrid() => grid;
        public int GetGridSize() => width * height;
        public List<Cell> GetCellGrid() => cellGrid;

        void Awake()
        {
            GenerateGrid();
            PopulateGridWithBlankGameObject();
        }
        public Cell GetCellViaNameOnGridList(string cellCoordinateAsString)
        {
            foreach (var cell in cellGrid)
            {
                if(cellCoordinateAsString == cell.name)
                {  return cell; }
            }
            return null;
        }

        public Cell GetCellOnGrid(Vector3 worldPosition)
        {
            Debug.Log($"Vector Start: {worldPosition}");
            return grid.GetGridObject(worldPosition).GetPlacedCell();
        }
        public  Cell GetCellOnGridWithRawCoordinates(int x ,int y)
        {
           
            return grid.GetGridObjectWithRawCoordinates(x, y).GetPlacedCell();
        }
        public Cell GetCellOnGrid(int x, int y)
        {
            if (!grid.ValidateCoordinates(x, y)) return null;
            return grid.GetGridObject(x, y).GetPlacedCell() == null ? null : grid.GetGridObject(x, y).GetPlacedCell();
        }


        public Cell GetCellOnGrid(float x, float z)
        {
            
            if (!grid.ValidateCoordinates((int)math.floor(x), (int)math.floor(z))) return null;
            return grid.GetGridObject(new Vector3(x, 0, z)).GetPlacedCell();

        }

        // Here lies a whole set of functions that I know is bugged
        private void SetCellNeighbors()
        {
            for (int x = 0; x < grid.GetWidth(); x++)
            {
                for (int y = 0; y < grid.GetHeight(); y++)
                {
                //    Debug.Log(GetCellOnGrid(x, y).transform.name);
                    GetCellOnGrid(x, y).SetNeighbor();
                }
            }
        }

        // MODIFY TO accomodate 
        private void PlaceBlankCellGameObject(int x, int y)
        {
            var placedCell = Instantiate(blankCellPrefab, grid.GetCellMidPoint(x, y), Quaternion.identity);

            placedCell.transform.name = $"{x}{y}";
            placedCell.transform.parent = parentGridGameObject.transform;

            // Initialize Field Nodes




            var currentCell = placedCell.GetComponent<Cell>();

            currentCell.SetWorldPosition(grid.GetWorldPosition(x, y));

            currentCell.GetNode().InstantiateFieldNode(grid.GetCellMidPoint(x, y), x, y, 0); // Instantiate Field Node for A* // Changed World Position to MID POINT
            currentCell.GetNode().SetParentCell(currentCell);
            var currentGridObject = grid.GetGridObject(x, y);

            currentGridObject.SetPlacedCell(currentCell); // assign this CELL to the GRIDObject Cell

            currentGridObject.SetPlacedCellPrefab(placedCell);

            //tempCellGrid.Add(currentCell);
            // Remove All Nodes that are not Blocked Or Path

          

        }

        

        // Populate Grid with Cell GameObjects
        private void PopulateGridWithBlankGameObject()
        {
            for (int x = 0; x < grid.GetWidth(); x++)
            {
                for (int y = 0; y < grid.GetHeight(); y++)
                {
                    PlaceBlankCellGameObject(x, y);
                }
            }

            SetCellNeighbors();
        }


        void GenerateGrid()
        {
/*            //grid = EditorGridSystem.Instance.GetGrid();
            Debug.Log($"is the Origin Grid Array Null? {EditorGridSystem.Instance.GetGrid().GetGridArray() == null}");*/
            grid = new Grid<GridNodeOjbect>(width, height, cellSize, Vector3.zero,
                (Grid<GridNodeOjbect> g, int x, int y) => new GridNodeOjbect(g, x, y));

        }

        public List<FieldNode> GetNeighbors(Node node)
        {
            List<FieldNode> neighbors = new List<FieldNode>();
            //neighbors = node.GetNodeNeighbors().ToList<FieldNode>();

            for (int i = 0; i < node.GetNodeNeighbors().Length; i++)
            {
               
                if (node.GetNodeNeighbors()[i] == null)
                {
                 //   neighbors.Add(null);
                }
                else
                {
                   var nodeNeighbors = node.GetNodeNeighbors()[i];
                    neighbors.Add(nodeNeighbors.GetFieldNode());
              //  Debug.Log($"Node Direction: {i} World Position: {nodeNeighbors.GetFieldNode().worldPosition}");
                }
            }
            return neighbors;
        }



        public void GenerateGridOnEditor()
        {
            GenerateGrid();
            PopulateGridWithBlankGameObject();
            grid.DebugLine();
            grid.DebugText();
        }

        public void RemoveGeneratedGridOnEditor()
        {
            grid.ClearTGridObjectArray();
            for (int i = 0; i < parentGridGameObject.transform.childCount; i++)
            {
                GameObject.DestroyImmediate(parentGridGameObject.transform.GetChild(i).gameObject);
            }

            Debug.Log($"grid: {parentGridGameObject.transform.childCount}");
        }


        private void OnEnable()
        {
            DontDestroyOnLoad(gameObject);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}