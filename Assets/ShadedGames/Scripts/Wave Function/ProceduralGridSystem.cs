using System;
using System.Collections.Generic;
using ShadedGames.Scripts.Utils;
using ShadedGames.Scripts.Wave_Function;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;
using Random = UnityEngine.Random;

namespace ShadedGames.Scripts.Grid_System
{
    // This is WFC PGS
    public class ProceduralGridSystem : ProjectAssets.Scripts.Util.Singleton<ProceduralGridSystem>
    {
        private Grid<GridSpawnerObject> grid; // This is your grid, where ProceduralGridObject Can Spawn
        [SerializeField] private List<ScriptableModule> listOfPossibleObjects;
        [SerializeField] private WaveFunctionNode cell;
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private int width = 10;
        [SerializeField] private int height = 10;
        [SerializeField] private float cellSize = 10f;
        [SerializeField] private List<GameObject> generatedObjects = new List<GameObject>();

        public Heap<WaveFunctionNode> orderedCells;

        /*
         * Load all 
         * 
         */
        public List<ScriptableModule> GetListOfModules() => listOfPossibleObjects; //


        void Awake()
        {
            // place Grid
            // create a grid
            grid = new Grid<GridSpawnerObject>(width, height, cellSize, Vector3.zero,
                (Grid<GridSpawnerObject> g, int x, int y) => new GridSpawnerObject(g, x, y));
            orderedCells = new Heap<WaveFunctionNode>(width * height);

            PopulateGrid();
            Generate();
            grid.DebugLine();
            grid.DebugText();
        }

        // Now you have a grid for Procedural Grid Object
        // this contains a Cell where you can place in the Grid

        // For Testing Manual Placing
        // Place a Cell Prefab, then later populate it
        
        // Manual placement of Blocks
        public Grid<GridSpawnerObject> GetGrid() => grid;

        private void PlaceBlankCell(int x, int y)
        {
            var placedCell = Instantiate(cellPrefab, grid.GetCellMidPoint(x, y), Quaternion.identity);

            placedCell.transform.name = $"{x} {y} ";

            var currentCell = placedCell.GetComponent<WaveFunctionNode>();

            currentCell.SetWorldPosition(grid.GetWorldPosition(x, y));
            currentCell.SetModules(GetListOfModules());

            var currentGridObject = grid.GetGridObject(x, y);

            currentGridObject.SetPlacedCell(currentCell);

            currentGridObject.SetPlacedCellPrefab(placedCell);

            // add the cell to the heap
            orderedCells.Add(currentCell);
        }

        public void Generate()
        {
            // apply constraints before Generating and propagating
            while (orderedCells.Count > 0)
            {
                var currentCell = orderedCells.GetFirst();
                if (currentCell.GetPossibleModules().Count == 1)
                {
                    // currentCell.Collapse();
                    currentCell.SetModule(currentCell.GetPossibleModules()[0]);
                    orderedCells.RemoveFirst();
                }
                else
                {
                    currentCell.SetModule(
                        currentCell.GetPossibleModules()[Random.Range(0, currentCell.GetPossibleModules().Count)]);
                }
            }
            // apply Game Objects
            for (int x = 0; x < (grid.GetWidth()); x++)
            for (int y = 0; y < (grid.GetHeight()); y++)
            {
                try
                {
                    Debug.Log($"Current Cell {x} {y}");
                    var currentCell = grid.GetGridObject(x, y).GetPlacedCell();
                  var cellObject =  Instantiate(currentCell.GetModule().moduleGameObject, grid.GetCellMidPoint(x, y),
                        Quaternion.identity);
                  generatedObjects.Add(cellObject);
                }
                catch (NullReferenceException e)
                {
                    Debug.Log($"Cell at: {x} {y} throwing an error");
                    throw new NullReferenceException();
                }
            }
        }

        public WaveFunctionNode GetCellOnGrid(Vector3 worldPosition)
        {
            return grid.GetGridObject(worldPosition).GetPlacedCell();
        }

        public WaveFunctionNode GetCellOnGrid(int x, int y)
        {
            if (!grid.ValidateCoordinates(x, y)) return null;
            return grid.GetGridObject(x, y).GetPlacedCell() == null ? null : grid.GetGridObject(x, y).GetPlacedCell();
        }



        private void PopulateGrid()
        {
            for (int x = 0; x < grid.GetWidth(); x++)
            {
                for (int y = 0; y < grid.GetHeight(); y++)
                {
                    PlaceBlankCell(x, y);
                }
            }

            SetNeighbors();
        }
        
        private void ClearGrid()
        {
            for (var i = 0; i < generatedObjects.Count; i++)
            {
                var cellObject = generatedObjects[i];
                generatedObjects.Remove(cellObject);
                Destroy(cellObject);
            }

            generatedObjects = new List<GameObject>();
        }

        private void SetNeighbors()
        {
            for (int x = 0; x < grid.GetWidth(); x++)
            {
                for (int y = 0; y < grid.GetHeight(); y++)
                {
                    Debug.Log(GetCellOnGrid(x, y).transform.name);
                    GetCellOnGrid(x, y).SetNeighbor();
                }
            }
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                grid = new Grid<GridSpawnerObject>(width, height, cellSize, Vector3.zero,
                    (Grid<GridSpawnerObject> g, int x, int y) => new GridSpawnerObject(g, x, y));
                orderedCells = new Heap<WaveFunctionNode>(width * height);
                ClearGrid();
                PopulateGrid();
                Generate();
                grid.DebugLine();
                grid.DebugText();
            }
            if (Input.GetMouseButtonDown(0) && cell != null)
            {
                Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
                grid.GetXZ(mousePosition, out int x, out int z);
                Debug.Log($"{x} {z}");
                Vector2Int placedObjectOrigin = new Vector2Int(x, z);
                placedObjectOrigin = grid.ValidateGridPosition(placedObjectOrigin);

                /*// So here's the plan Manual WFC first
                // WFC works like this
                // When u place a Cell this what happens to the cell
                // first check if there are neighboring cells
                // >> If there is none then 
                // >> Randomly select a module 
                // else
                    check neighbors
                    then one by one remove possible modules until one is found
                    rinse repeat
                */
            }
        }
    }
}