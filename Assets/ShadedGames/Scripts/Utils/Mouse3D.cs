using UnityEngine;

namespace ShadedGames.Scripts.Utils
{

    /// <summary>
    /// Singleton Behaviour
    /// </summary>
    public class Mouse3D : MonoBehaviour
    {
        public static Mouse3D Instance { get; private set; }

        [SerializeField] protected bool mouseHold = false;

        [SerializeField] private LayerMask mouseColliderLayerMask = new LayerMask();

        public bool GetMouseState() => mouseHold;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, mouseColliderLayerMask))
            {
                transform.position = raycastHit.point;
            }

            if (Input.GetMouseButtonUp(0))
            {
                mouseHold = false;
            }
            if (Input.GetMouseButtonDown(0))
            {
                mouseHold = true;
            }
        }

        public static Vector3 GetMouseWorldPosition() => Instance.GetMouseWorldPosition_Instance();

        private Vector3 GetMouseWorldPosition_Instance()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, mouseColliderLayerMask))
            {
                return raycastHit.point;
            }
            else
            {
                return Vector3.zero;
            }
        }
        public static Cell GetCellOnMouseWorldPosition() => Instance.GetCellOnMouseWorldPosition_Instance();
        private Cell GetCellOnMouseWorldPosition_Instance()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, mouseColliderLayerMask))
            {
                return raycastHit.transform.gameObject.GetComponent<Cell>();
            }
            else
            {
                return null;
            }
        }
    }
}