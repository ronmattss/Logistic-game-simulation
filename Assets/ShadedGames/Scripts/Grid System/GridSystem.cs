using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProjectAssets.Scripts.Util;
using ShadedGames.Scripts.Astar;
using ShadedGames.Scripts.Grid_System;
using Unity.Mathematics;
using UnityEngine;

namespace ShadedGames.Scripts.Grid_System
{

    public class GridSystem : Singleton<GridSystem>
    {

        [SerializeField] private Grid<GridNodeOjbect> grid;  // This is the GRID that will be used by this system


        [SerializeField] private GameObject blankCellPrefab;
        [SerializeField] private int width = 10;
        [SerializeField] private int height = 10;
        [SerializeField] private float cellSize = 10f;
        [SerializeField] private List<GameObject> generatedObjects = new List<GameObject>();
        [SerializeField] private List<Cell> cellGrid = new List<Cell>();


        public Grid<GridNodeOjbect> GetGrid() => grid;
        public List<Cell> GetCellGrid() => cellGrid;


        public Cell GetCellOnGrid(Vector3 worldPosition)
        {
            return grid.GetGridObject(worldPosition).GetPlacedCell();
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
                    GetCellOnGrid(x, y).SetNeighbor();
                }
            }
        }


        private void PlaceBlankCellGameObject(int x, int y)
        {
            var placedCell = Instantiate(blankCellPrefab, grid.GetCellMidPoint(x, y), Quaternion.identity);

            placedCell.transform.name = $"{x} {y} ";
            placedCell.transform.parent = this.transform;

            // Initialize Field Nodes




            var currentCell = placedCell.GetComponent<Cell>();

            currentCell.SetWorldPosition(grid.GetWorldPosition(x, y));

            currentCell.GetNode().InstantiateFieldNode(grid.GetWorldPosition(x, y), x, y, 0); // Instantiate Field Node for A*

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
        }
        void Awake()
        {
            GenerateGrid();
            PopulateGridWithBlankGameObject();
            grid.DebugLine();
            grid.DebugText();
        }

        void GenerateGrid()
        {
            grid = new Grid<GridNodeOjbect>(width, height, cellSize, Vector3.zero,
                (Grid<GridNodeOjbect> g, int x, int y) => new GridNodeOjbect(g, x, y));

        }

        public List<FieldNode> GetNeighbors(Node node)
        {
            List<FieldNode> neighbors = new List<FieldNode>();
            //neighbors = node.GetNodeNeighbors().ToList<FieldNode>();

            for (int i = 0; i < node.GetNodeNeighbors().Length; i++)
            {
                neighbors.Add(node.GetNodeNeighbors()[i].GetFieldNode());
            }
            return neighbors;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}