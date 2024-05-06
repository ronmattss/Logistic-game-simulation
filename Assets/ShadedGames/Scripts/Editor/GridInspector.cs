using ShadedGames.Scripts.Grid_System;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


namespace ShadedGames.Scripts
{

    [ExecuteAlways]
    [CustomEditor(typeof(EditorGridSystem))]
    public class GridInspector : Editor
    {
        public VisualTreeAsset m_InspectorXML;

        // Grid Text Fields
        public IntegerField integerField_GridWidth;
        public IntegerField integerField_GridHeight;
        public IntegerField integerField_CellSize;
        public Button button_GenerateNodeGrid;
        public Button button_RemoveNodeGrid;

        SerializedProperty parentGridObjectProp;
        private void OnEnable()
        {
            parentGridObjectProp = serializedObject.FindProperty("parentGridGameObject");
        }


        public override VisualElement CreateInspectorGUI()
        {
            VisualElement rootVisualElement = new VisualElement();
            m_InspectorXML.CloneTree(rootVisualElement);

            SerializedProperty parentGridGameObjectProp = serializedObject.FindProperty("parentGridGameObject");
            SerializedProperty blankCellPrefabProp = serializedObject.FindProperty("blankCellPrefab");

            PropertyField parentGridObjectField = new PropertyField(parentGridGameObjectProp, "Parent Grid Object");
            PropertyField blankCellPrefabField = new PropertyField(blankCellPrefabProp, "Blank Cell Object");

            GetGridFields(rootVisualElement);
            rootVisualElement.Add(parentGridObjectField);
            rootVisualElement.Add(blankCellPrefabField);





            return rootVisualElement;
        }


        public void GetGridFields(VisualElement rootElement)
        {
            integerField_GridWidth = rootElement.Q<IntegerField>("grid_width_field");
            integerField_GridHeight = rootElement.Q<IntegerField>("grid_height_field");
            integerField_CellSize = rootElement.Q<IntegerField>("grid_size_field");
            button_GenerateNodeGrid = rootElement.Q<Button>("generate_grid_button");
            button_RemoveNodeGrid = rootElement.Q<Button>("remove_grid_button");


            // register callback
            button_GenerateNodeGrid.RegisterCallback<MouseUpEvent>(evnt => {
                Debug.Log($"WxH: {integerField_GridWidth.text} {integerField_GridHeight.text}");
                EditorGridSystem.Instance.SetGridWidthAndHeight(int.Parse(integerField_GridWidth.text), int.Parse(integerField_GridHeight.text));
                EditorGridSystem.Instance.GenerateGridOnEditor();
            });
            button_RemoveNodeGrid.RegisterCallback<MouseUpEvent>((evt=> { EditorGridSystem.Instance.RemoveGeneratedGridOnEditor(); }));

        }



        void ClickTest()
        {
            Debug.Log("I am Clicked");
        }

    }

}