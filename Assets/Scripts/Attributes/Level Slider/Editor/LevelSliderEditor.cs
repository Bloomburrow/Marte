using UnityEditor;
using UnityEngine;

namespace Attributes
{
    [CustomPropertyDrawer(typeof(LevelSlider))]
    public class LevelSliderEditor : PropertyDrawer
    {
        #region Variables & Properties

        float standardHorizontalSpacing = 14f;
        float labelWidth = 48f;

        #endregion

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.Vector2Int)
            {
				position = EditorGUI.PrefixLabel(position, label);

				float indentHorizontalSpacing = EditorGUI.indentLevel * standardHorizontalSpacing;

                float x = (float)property.vector2IntValue.x;

                if (x <= 0)
                    x = 1;

                x = EditorGUI.IntField(new Rect(position.x - indentHorizontalSpacing - 2f, position.y, indentHorizontalSpacing + labelWidth, position.height), (int)x);

                float y = (float)property.vector2IntValue.y;

                if(y <= 0)
                    y = 1;

                y = EditorGUI.IntField(new Rect(position.x - indentHorizontalSpacing - 2f + position.width - labelWidth, position.y, indentHorizontalSpacing + labelWidth, position.height), (int)y);

                x = GUI.HorizontalSlider(new Rect(position.x + labelWidth + standardHorizontalSpacing, position.y, position.width + indentHorizontalSpacing + -labelWidth - standardHorizontalSpacing - labelWidth - standardHorizontalSpacing * 2f, position.height), x, 1f, y, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb, GUIStyle.none);

                property.vector2IntValue = new Vector2Int((int)x, (int)y);
            }
            else
                EditorGUI.LabelField(position, label.text, "LevelSliderEditor/OnGUI/Level slider only works for Vector2Int");
        }
    }
}
