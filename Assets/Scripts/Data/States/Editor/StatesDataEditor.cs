using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(StatesData))]
public class StatesDataEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property.FindPropertyRelative("statesData"), label);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("statesData"), label);
    }
}

