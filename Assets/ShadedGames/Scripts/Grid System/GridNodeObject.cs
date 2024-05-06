using System;
using UnityEngine;

namespace ShadedGames.Scripts.Grid_System
{
    [Serializable]
    public class GridNodeOjbect
    {
        private Grid<GridNodeOjbect> grid;
        private int x;
        private int z;
        private Cell cell;
        private GameObject cellPrefab; // GameObject Representation of the GridNodeObject


        public GridNodeOjbect(Grid<GridNodeOjbect> grid, int x, int z)
        {
            this.grid = grid;
            this.x = x;
            this.z = z;
            cell = null;
            cellPrefab = null;
        }

        public void SetPlacedCell(Cell placeable)
        {
            cell = placeable;
            grid.TriggerGridObjectChanged(x, z); // update the GRID
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
            Debug.Log($"is this cell null:{cell == null}");
            return cell;
        }
        public override string ToString()
        {
            return $"{x}{z}";
        }

        public GameObject GetCellPrefab() => cellPrefab;
        public bool CanBuild() => cell == null;

    }
}
