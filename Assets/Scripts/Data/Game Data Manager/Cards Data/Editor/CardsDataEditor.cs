using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CardsData))]
public class CardsDataEditor : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.PropertyField(position, property.FindPropertyRelative("cardsData"), label);
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("cardsData"), label);
	}
}