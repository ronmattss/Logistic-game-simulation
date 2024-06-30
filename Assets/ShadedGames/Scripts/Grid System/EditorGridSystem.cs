using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProjectAssets.Scripts.Util;
using ShadedGames.Scripts.Astar;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace ShadedGames.Scripts.Grid_System
{
    /// <summary>
    /// TODO: Separate Grid System used in Editor and IN GAME
    /// </summary>
    /*[ExecuteInEditMode]*/
    public class EditorGridSystem :MonoBehaviour
    {
        private static EditorGridSystem _instance;
        
        [SerializeField] private Grid<GridNodeOjbect> grid;  // This is the GRID that will be used by this system


        [SerializeField] private GameObject blankCellPrefab;
        [SerializeField] private int width = 10;
        [SerializeField] private int height = 10;
        [SerializeField] private float cellSize = 10f;
        [SerializeField] private List<GameObject> generatedObjects = new List<GameObject>();
        [SerializeField] private List<Cell> cellGrid = new List<Cell>(); // List of GameObject of the Grid
        [SerializeField] private GameObject parentGridGameObject;


        // EDIT MODE Stuff
        [SerializeField] private GameObject storedGridObjects;
        private StoredCells storedGridData;

        public bool editMode = false;
        public bool mapEditMode = false;
        public bool deleteMode = false;

        public GameObject currentlySelectedGameobject;

        public void SetGridWidthAndHeight(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
        public Grid<GridNodeOjbect> GetGrid() => grid;
        public int GetGridWidth() => this.width;

        public int GetGridSize() => width * height;
        public List<Cell> GetCellGrid() => cellGrid;


        public Cell GetCellOnGrid(Vector3 worldPosition)
        {
            return grid.GetGridObject(worldPosition).GetPlacedCell();
        }
        public Cell GetCellOnGridWithRawCoordinates(int x, int y)
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
                    Debug.Log(GetCellOnGrid(x, y).transform.name);
                    GetCellOnGrid(x, y).EditorModeSetNeighbor();
                }
            }
        }
        private void PlaceBlankCellGameObject(int x, int y)
        {
            var placedCell = Instantiate(blankCellPrefab, grid.GetCellMidPoint(x, y), Quaternion.identity);

            placedCell.transform.name = $"{x}{y}";
            placedCell.transform.parent = parentGridGameObject.transform;
            // Initialize Field Nodes

            var currentCell = placedCell.GetComponent<Cell>();

            currentCell.SetWorldPosition(grid.GetWorldPosition(x, y));

            currentCell.GetNode().InstantiateFieldNode(grid.GetCellMidPoint(x, y), x, y, 0); // Instantiate Field Node for A* // Changed World Position to MID POINT

            var currentGridObject = grid.GetGridObject(x, y);

            currentGridObject.SetPlacedCell(currentCell);

            currentGridObject.SetPlacedCellPrefab(placedCell);
            cellGrid.Add(currentCell);

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
            storedGridData.cellGrid = new List<Cell>(cellGrid);
            storedGridData.cellGrid.RemoveAll(c => c == null); // cleans up the list
        }
        void GenerateGrid()
        {
            storedGridObjects = GameObject.Find("storedGridObjects");
            storedGridData = storedGridObjects.GetComponent<StoredCells>();

            grid = new Grid<GridNodeOjbect>(width, height, cellSize, Vector3.zero,
                (Grid<GridNodeOjbect> g, int x, int y) => new GridNodeOjbect(g, x, y));
            storedGridData.grid = grid;
            storedGridData.grid.DebugProperties();

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


        // Update is called once per frame

        public static EditorGridSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<EditorGridSystem>();
                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject();
                        _instance = singletonObject.AddComponent<EditorGridSystem>();
                        singletonObject.name = typeof(EditorGridSystem).ToString() + " (Editor Singleton)";

                    //    DontDestroyOnLoad(singletonObject);
                    }
                }
                return _instance;
            }
        }

        public void EditCellNode()
        {
            // Get if Edit Mode
            // Get Type of Edit Mode
            // Depending on the Mode Edit or Delete the Cell / Node
            // Get Cell on the Grid
            // Edit Cell/ Node
            // Check Neighboring Cells and Edit

        }

        public void CheckEditMode()
        {
            if (mapEditMode)
            {
                return;
            }
            deleteMode = false;
            editMode = false;
        }

        public void OnEditMode()
        {
            // edit mode 
            //EDITS if node is connected or not, walkable or placeable. NOTE THAT DELETED NODE ARE JUST NODES THAT ARE NOT CONNECTED TO ANYTHING 
        }

        public void OnDeleteMode()
        {
            // delete mode 
        }



        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                _instance = this;
            }
        }

/*        void Update()
        {
         //   storedGridData.grid.DebugProperties();
         //   grid.DebugProperties();
            if(editMode)
            {
                Debug.Log("Updates: ");
                currentlySelectedGameobject = Selection.activeTransform.gameObject;
            }

        }*/
/*        private void OnEnable()
        {
            EditorApplication.update += Update;
        }
        private void OnDisable()
        {
            EditorApplication.update -= Update;
        }*/
    }
}