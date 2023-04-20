using UnityEditor;
using MapSystem;
using UnityEngine;

namespace CustomProperty
{
    [CustomPropertyDrawer(typeof(TileChoiceRules))]
    public class TileChoiceRulesUIE : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property) * 4;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            Rect lablePosition = new Rect(position.x, position.y + position.height / 2, position.width, position.height);

            position = EditorGUI.PrefixLabel(lablePosition, label);
            
            float widthSize = position.width / 9;
            position.height /= 4;
            float hightSize = position.height;

            Rect pos1 = new Rect(position.x, position.y - hightSize, position.width, position.height);
            Rect pos2 = new Rect(position.x + widthSize, position.y - hightSize, position.width, position.height);
            Rect pos3 = new Rect(position.x + widthSize * 2, position.y - hightSize, position.width, position.height);

            SerializedProperty topLeft = property.FindPropertyRelative("_topLeft");
            SerializedProperty top = property.FindPropertyRelative("_top");
            SerializedProperty topRight = property.FindPropertyRelative("_topRight");

            EditorGUI.PropertyField(pos1, topLeft, new GUIContent(""));
            EditorGUI.PropertyField(pos2, top, new GUIContent(""));
            EditorGUI.PropertyField(pos3, topRight, new GUIContent(""));


            pos1 = new Rect(position.x, position.y, position.width, position.height);
            pos2 = new Rect(position.x + widthSize * 2, position.y, position.width, position.height);

            SerializedProperty left = property.FindPropertyRelative("_left");
            SerializedProperty right = property.FindPropertyRelative("_right");

            EditorGUI.PropertyField(pos1, left, new GUIContent(""));
            EditorGUI.PropertyField(pos2, right, new GUIContent(""));


            pos1 = new Rect(position.x, position.y + hightSize, position.width, position.height);
            pos2 = new Rect(position.x + widthSize, position.y + hightSize, position.width, position.height);
            pos3 = new Rect(position.x + widthSize * 2, position.y + hightSize, position.width, position.height);

            SerializedProperty bottomLeft = property.FindPropertyRelative("_bottomLeft");
            SerializedProperty bottom = property.FindPropertyRelative("_bottom");
            SerializedProperty bottomRight = property.FindPropertyRelative("_bottomRight");

            EditorGUI.PropertyField(pos1, bottomLeft, new GUIContent(""));
            EditorGUI.PropertyField(pos2, bottom, new GUIContent(""));
            EditorGUI.PropertyField(pos3, bottomRight, new GUIContent(""));


            pos1 = new Rect(position.x, position.y - hightSize * 2, position.width, position.height);
            SerializedProperty tile = property.FindPropertyRelative("_tile");
            EditorGUI.PropertyField(pos1, tile, new GUIContent(""));

            EditorGUI.EndProperty();
        }
    }
}