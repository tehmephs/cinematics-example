using UnityEngine;
using UnityEditor;
using Assets.Scripts.Attributes;

namespace Assets.Scripts.Editor
{
	[CustomPropertyDrawer(typeof(DisableDoubleClickAttribute))]
	public class DisableDoubleClickPropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			EditorGUI.LabelField(position, label);
			EditorGUI.ObjectField(position, property);

			if (Event.current.type == EventType.MouseDown && Event.current.clickCount == 2 && position.Contains(Event.current.mousePosition))
			{
				Event.current.Use();
			}

			EditorGUI.EndProperty();
		}
	}
}