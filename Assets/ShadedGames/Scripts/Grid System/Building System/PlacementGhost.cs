using ShadedGames.Scripts.Grid_System;
using ShadedGames.Scripts.Grid_System.Building_System;
using UnityEngine;

namespace Grid_System.Building_System
{
    public class PlacementGhost : MonoBehaviour
    {
        private Transform visual;
        private ScriptablePlaceableObject placeableObject;

        private void Start()
        {
            RefreshVisual();

            GridBehaviour.Instance.OnSelectedChanged += Instance_OnSelectedChanged;

        }
        
        private void LateUpdate() {
            Vector3 targetPosition = GridBehaviour.Instance.GetMouseWorldSnappedPosition();
            targetPosition.y = 1f;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);

            transform.rotation = Quaternion.Lerp(transform.rotation, GridBehaviour.Instance.GetPlacedObjectRotation(), Time.deltaTime * 15f);
        }

        private void Instance_OnSelectedChanged(object sender, System.EventArgs e) {
            RefreshVisual();
        }
        /// <summary>
        /// Refreshes the visual model
        /// </summary>
        private void RefreshVisual()
        {
            if (visual != null)
            {
                Destroy(visual.gameObject);
                visual = null;
            }

            ScriptablePlaceableObject scriptablePlaceableObject = GridBehaviour.Instance.GetPlacedObjectTypeSO();

            if (scriptablePlaceableObject != null)
            {
                visual = Instantiate(scriptablePlaceableObject.visual, Vector3.zero, Quaternion.identity);
                visual.parent = transform;
                visual.localPosition = Vector3.zero;
                visual.localEulerAngles = Vector3.zero;
                SetLayerRecursive(visual.gameObject, 11);
                
            }
        }
        
        private void SetLayerRecursive(GameObject targetGameObject, int layer) {
            targetGameObject.layer = layer;
            foreach (Transform child in targetGameObject.transform) {
                SetLayerRecursive(child.gameObject, layer);
            }
        }
    }
}