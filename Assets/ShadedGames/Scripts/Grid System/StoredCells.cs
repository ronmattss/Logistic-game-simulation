using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadedGames.Scripts.Grid_System
{

    [ExecuteAlways]
public class StoredCells : MonoBehaviour
{
        public List<Cell> cellGrid = new List<Cell>(); // List of GameObject of the Grid
        public Grid<GridNodeOjbect> grid;  // This is the GRID that will be used by this system
                                            // Start is called before the first frame update
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
}
