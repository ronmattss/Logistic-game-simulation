using System.Collections.Generic;
using UnityEngine;

namespace ShadedGames.Scripts.Grid_System.Building_System
{
    public class PlaceableObject :MonoBehaviour
    {

        private ScriptablePlaceableObject placeableObject;
        private Vector2Int origin;
        private ScriptablePlaceableObject.Dir direction;
        
        public static PlaceableObject Create(Vector3 worldPosition, Vector2Int origin, ScriptablePlaceableObject.Dir direction,
            ScriptablePlaceableObject placedObject)
        {
            Transform placedObjectTransform = Instantiate(placedObject.prefab, worldPosition, Quaternion.Euler(0, placedObject.GetRotationAngle(direction), 0));
            PlaceableObject placed = placedObjectTransform.GetComponent<PlaceableObject>();
            placed.Setup(placedObject, origin, direction);
            return placed;
        }


        private void Setup(ScriptablePlaceableObject scriptablePlaceableObject, Vector2Int origin,
            ScriptablePlaceableObject.Dir direction)
        {
            this.placeableObject = scriptablePlaceableObject;
            this.origin = origin;
            this.direction = direction;
        }


        public List<Vector2Int> GetGridPositionList()
        {
            return placeableObject.GetGridPositionList(origin, direction);
        }
        
        public void DestroySelf() {
            Destroy(gameObject);
        }

        public override string ToString() {
            return placeableObject.nameString;
        }




    }
}