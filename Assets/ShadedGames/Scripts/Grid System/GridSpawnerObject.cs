using Grid_System.Building_System;
using UnityEngine;

namespace ShadedGames.Scripts.Grid_System
{
    public class GridSpawnerObject
    {
        private Grid<GridSpawnerObject> grid;
        private int x;
        private int z;
        private Cell cell;
        private GameObject cellPrefab;


        public GridSpawnerObject(Grid<GridSpawnerObject> grid, int x, int z)
        {
            this.grid = grid;
            this.x = x;
            this.z = z;
            cell = null;
            cellPrefab = null;

        }


        public GameObject GetCellPrefab() => cellPrefab;

        public void SetPlacedCell(Cell placeable)
        {
            cell = placeable;
//            cellPrefab = cell.gameObject;
            grid.TriggerGridObjectChanged(x, z);
            //Debug.Log($"is cell null? {cell == null}");
        }

        public void SetPlacedCellPrefab(GameObject cellPrefabInstance)
        {
            this.cellPrefab = cellPrefabInstance;
        }


        public void ClearPlacedCell()
        {
            cell = null;
            grid.TriggerGridObjectChanged(x, z);
        }

        public Cell GetPlacedCell()
        {
            return cell;
        }

        public bool CanBuild() => cell == null;


        public override string ToString()
        {
            return $"{x}{z}";
        }
    }
}