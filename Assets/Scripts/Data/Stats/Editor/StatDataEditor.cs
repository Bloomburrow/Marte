using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(StatData))]
public class StatDataEditor : PropertyDrawer
{
    #region Variables & Properties

    Vector2 standardSpacing = new Vector2(14f, 16f);
    float labelWidth = 48f;

    #endregion

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position.height = standardSpacing.y + 2f;

        SerializedProperty name = property.FindPropertyRelative("name");

        property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, name.stringValue);

        if (property.isExpanded)
        {
            position.y += standardSpacing.y + 2f;

            SerializedProperty isMoment = property.FindPropertyRelative("isMoment");

            string[] types = { "Int", "Moment" };

            isMoment.boolValue = Convert.ToBoolean(EditorGUI.Popup(position, "Type", Convert.ToInt32(isMoment.boolValue), types));

            SerializedProperty value = property.FindPropertyRelative("value");

            if (isMoment.boolValue)
            {
                position.y += standardSpacing.y + 2f;

                int momentOptionsLength = Enum.GetNames(typeof(Moment)).Length;

                if (value.floatValue > momentOptionsLength)
                    value.floatValue = momentOptionsLength;

                name.stringValue = "Moment";

                value.floatValue = (float)(Moment)EditorGUI.EnumPopup(position, name.stringValue, (Moment)(int)value.floatValue);
            }
            else
            {
                position.y += standardSpacing.y + 2f;

                name.stringValue = EditorGUI.TextField(position, "Name", name.stringValue);

                position.y += standardSpacing.y + 2f;

                SerializedProperty increasePerLevel = property.FindPropertyRelative("increasePerLevel");

                increasePerLevel.boolValue = EditorGUI.Toggle(position, "Value Increase Per Level", increasePerLevel.boolValue);

                position.y += standardSpacing.y + 2f;

                Rect prefixLabePosition = EditorGUI.PrefixLabel(position, new GUIContent("Value"));

                value.floatValue = EditorGUI.FloatField(new Rect(prefixLabePosition.x, prefixLabePosition.y, labelWidth, prefixLabePosition.height), value.floatValue);

                if (increasePerLevel.boolValue)
                {
                    EditorGUI.LabelField(new Rect(prefixLabePosition.x + labelWidth, prefixLabePosition.y, labelWidth / 3f, prefixLabePosition.height), " + ");

                    SerializedProperty valueIncreasePerLevel = property.FindPropertyRelative("valueIncreasePerLevel");

                    valueIncreasePerLevel.floatValue = EditorGUI.FloatField(new Rect(prefixLabePosition.x + labelWidth + (labelWidth / 3f), prefixLabePosition.y, labelWidth, prefixLabePosition.height), valueIncreasePerLevel.floatValue);

                    EditorGUI.LabelField(new Rect(prefixLabePosition.x + labelWidth + (labelWidth / 3f) + labelWidth, prefixLabePosition.y, prefixLabePosition.width - labelWidth - (labelWidth / 3f) - labelWidth, prefixLabePosition.height), " per level ");
                }
            }
        } 
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = standardSpacing.y + 2f;

        if (property.isExpanded)
        {
            height += standardSpacing.y + 2f;

            height += standardSpacing.y + 2f;

            height += standardSpacing.y + 2f;

            if (!property.FindPropertyRelative("isMoment").boolValue)
                height += standardSpacing.y + 2f;
        }

        return height;
    }
}
