using UnityEditor;
using UnityEngine;

namespace Attributes
{
	[CustomPropertyDrawer(typeof(CardPicker))]
	public class CardPickerEditor : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.propertyType == SerializedPropertyType.Integer)
			{
				property.intValue = (int)(CardPicker.Card)EditorGUI.EnumPopup(position, label.text, (CardPicker.Card)property.intValue);
			}
			else
				EditorGUI.LabelField(position, label.text, "CardDropDownEditor/OnGUI/Card drop down only works for int");
		}
	}
}

