using System;
using UnityEngine;
using UnityEngine.UI;
using FunctionUpdater = ShadedGames.Scripts.Utils.FunctionUpdater;

namespace Utils
{
    /// <summary>
    /// Base Utility class for common things used in Unity
    /// </summary>
    public static class Utility
    {
        private static readonly Vector3 Vector3zero = Vector3.zero;
        private static readonly Vector3 Vector3one = Vector3.one;
        private static readonly Vector3 Vector3yDown = new Vector3(0, -1);

        public const int sortingOrderDefault = 5000;
        
        private static Transform cachedCanvasTransform;
        public static Transform GetCanvasTransform() {
            if (cachedCanvasTransform == null) {
                Canvas canvas = MonoBehaviour.FindObjectOfType<Canvas>();
                if (canvas != null) {
                    cachedCanvasTransform = canvas.transform;
                }
            }
            return cachedCanvasTransform;
        }
        
        // Creates a Text Mesh in the World and constantly updates it
        public static FunctionUpdater CreateWorldTextUpdater(Func<string> GetTextFunc, Vector3 localPosition, Transform parent = null) {
            TextMesh textMesh = CreateWorldText(GetTextFunc(),true, parent,localPosition);
            return FunctionUpdater.Create(() => {
                textMesh.text = GetTextFunc();
                return false;
            }, "WorldTextUpdater");
        }
        
        public static void CreateWorldTextPopup(string text, Vector3 localPosition) {
            CreateWorldTextPopup(null, text, localPosition, 20, Color.white, localPosition + new Vector3(0, 10), 1f);
        }
        
        // Create a Text Popup in the World
        public static void CreateWorldTextPopup(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, Vector3 finalPopupPosition, float popupTime) {
            TextMesh textMesh = CreateWorldText(true,parent, text, localPosition, fontSize, color, TextAnchor.LowerLeft, TextAlignment.Left, sortingOrderDefault);
            Transform transform = textMesh.transform;
            Vector3 moveAmount = (finalPopupPosition - localPosition) / popupTime;
            FunctionUpdater.Create(delegate () {
                transform.position += moveAmount * Time.deltaTime;
                popupTime -= Time.deltaTime;
                if (popupTime <= 0f) {
                    UnityEngine.Object.Destroy(transform.gameObject);
                    return true;
                } else {
                    return false;
                }
            }, "WorldTextPopup");
        }

        // Create Text Updater in UI
        public static FunctionUpdater CreateUITextUpdater(Func<string> GetTextFunc, Vector2 anchoredPosition) {
            Text text = DrawTextUI(GetTextFunc(), anchoredPosition,  20, GetDefaultFont());
            return FunctionUpdater.Create(() => {
                text.text = GetTextFunc();
                return false;
            }, "UITextUpdater");
        }
        
        public static Font GetDefaultFont() {
            return Resources.GetBuiltinResource<Font>("Arial.ttf");
        }
        
        public static Text DrawTextUI(string textString, Vector2 anchoredPosition, int fontSize, Font font) {
            return DrawTextUI(textString, GetCanvasTransform(), anchoredPosition, fontSize, font);
        }
        public static Text DrawTextUI(string textString, Transform parent, Vector2 anchoredPosition, int fontSize, Font font) {
            GameObject textGo = new GameObject("Text", typeof(RectTransform), typeof(Text));
            textGo.transform.SetParent(parent, false);
            Transform textGoTrans = textGo.transform;
            textGoTrans.SetParent(parent, false);
            textGoTrans.localPosition = Vector3zero;
            textGoTrans.localScale = Vector3one;

            RectTransform textGoRectTransform = textGo.GetComponent<RectTransform>();
            textGoRectTransform.sizeDelta = new Vector2(0,0);
            textGoRectTransform.anchoredPosition = anchoredPosition;

            Text text = textGo.GetComponent<Text>();
            text.text = textString;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.alignment = TextAnchor.MiddleLeft;
            if (font == null) font = GetDefaultFont();
            text.font = font;
            text.fontSize = fontSize;

            return text;
        }

        
        public static TextMesh CreateWorldText(string text, bool withZ, Transform parent = null,
            Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null,
            TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left,
            int sortingOrder = sortingOrderDefault)
        {
            if (color == null) color = Color.white;
            return CreateWorldText(withZ, parent, text, localPosition, fontSize, (Color)color, textAnchor,
                textAlignment, sortingOrder);
        }

        // Create Text in the world 
        public static TextMesh CreateWorldText(bool withZ, Transform parent, string text, Vector3 localPostion,
            int fontSize,
            Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
        {
            GameObject gameObject = new GameObject("Generated_Word_Text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);

            if (withZ)
            {
                transform.localPosition = new Vector3(localPostion.x,0,localPostion.z);
                transform.rotation = Quaternion.Euler(90,0,0);
            }


            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }


        public static Vector3 GetMouseWorldPosition3D() => GetMousePosition();

        static Vector3 GetMousePosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            ///TODO: Create LAYER MASK
            if (!Physics.Raycast(ray, out RaycastHit raycastHit, 999)) return Vector3.zero;
            Debug.Log(raycastHit.point);
            return raycastHit.point;
        }


        // Get Mouse World Position
        public static Vector3 GetMouseWorldPosition()
        {
            Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
            vec.z = 0f;
            return vec;
        }

        public static Vector3 GetMouseWorldPositionWithZ()
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
        {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }
    }
}