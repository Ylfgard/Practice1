using UnityEngine;
using MapSystem.TileLayer;
using UnityEditor;

namespace CustomProperty
{
    [CustomPropertyDrawer(typeof(TileRule))]
    public class TileRuleUIE : PropertyDrawer
    {
        TileRule display;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUIStyle style = GUI.skin.toggle;
            position = new Rect(position.x, position.y, 18, 18);
            switch (property.enumValueIndex)
            {
                case 0:
                    display = TileRule.Different;
                    break;

                case 1:
                    display = TileRule.Same;
                    break;

                case 2:
                    display = TileRule.Both;
                    break;

                default:
                    Debug.LogError("Unrecognized Option");
                    break;
            }

            display = (TileRule)EditorGUI.EnumPopup(position, "", display, style);

            position = new Rect(position.x + 2, position.y, 16, 18);

            switch (display)
            {
                case TileRule.Same:
                    EditorGUI.LabelField(position, "X");
                    break;

                case TileRule.Different:
                    EditorGUI.LabelField(position, " ");
                    break;

                case TileRule.Both:
                    EditorGUI.LabelField(position, "0");
                    break;

                default:
                    Debug.LogError("Unrecognized Option");
                    break;
            }

            property.enumValueIndex = (int)display;
        }
    }
}

