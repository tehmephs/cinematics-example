using Assets.Scripts.Attributes;
using UnityEngine;
using UnityEditor;

namespace Assets.Scripts.Editor
{
	[CustomPropertyDrawer(typeof(AlwaysExpandAttribute))]
	public class AlwaysExpandAttributePropertyDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			float totalHeight = 0f;
			SerializedProperty prop = property.Copy();
			SerializedProperty endProperty = prop.GetEndProperty();

			bool enterChildren = true;
			while (prop.NextVisible(enterChildren) && !SerializedProperty.EqualContents(prop, endProperty))
			{
				totalHeight += EditorGUI.GetPropertyHeight(prop, true) + EditorGUIUtility.standardVerticalSpacing;
				enterChildren = false;
			}

			return totalHeight;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty prop = property.Copy();
			SerializedProperty endProperty = prop.GetEndProperty();

			position.height = EditorGUIUtility.singleLineHeight;

			bool enterChildren = true;
			while (prop.NextVisible(enterChildren) && !SerializedProperty.EqualContents(prop, endProperty))
			{
				float height = EditorGUI.GetPropertyHeight(prop, true);
				Rect fieldRect = new Rect(position.x, position.y, position.width, height);
				EditorGUI.PropertyField(fieldRect, prop, true);
				position.y += height + EditorGUIUtility.standardVerticalSpacing;

				enterChildren = false;
			}
		}
	}
}