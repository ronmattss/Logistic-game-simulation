using System;
using UnityEngine;

namespace ShadedGames.Scripts.Grid_System
{
    // this will be the Module that contains the 
    [CreateAssetMenu(fileName = "NewModule", menuName = "ShadedGames/WFC/Module", order = 0)]
    [Serializable]
    public class ScriptableModule : ScriptableObject
    {
        public GameObject moduleGameObject;

        [Header("Connections: North,East,South,West")]
        public ConnectionType[] connections = new ConnectionType[4];
        // this will be a Cell Object 


        // Place Cell Logic Here e.g checking neighboring cells for the generator
    }

    public enum ConnectionType
    {
        Blocked,
        Open,
        None // This means that no connection can be made?
    }

    public enum CompassDirection
    {
        North,
        East,
        South,
        West
    }
}