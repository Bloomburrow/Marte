using UnityEditor;
using UnityEngine;

namespace Attributes
{
    [CustomPropertyDrawer(typeof(Indent))]
    public class IndentEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int lastIndentLevel = EditorGUI.indentLevel;

            EditorGUI.indentLevel = ((Indent)attribute).indentLevel;

            label.text = "└ " + label.text;

            EditorGUI.PropertyField(position, property, label);

            EditorGUI.indentLevel = lastIndentLevel;
        }
    }
}
