using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(StatsData))]
public class StatsDataEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property.FindPropertyRelative("statsData"), label);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("statsData"), label);
    }
}
