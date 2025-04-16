using UnityEngine;
using UnityEditor;
using Assets.Scripts.Attributes;

namespace Assets.Scripts.Editor
{
	[CustomPropertyDrawer(typeof(ClampedCurveAttribute))]
	public class ClampedCurveAttributePropertyDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			ClampedCurveAttribute boundedCurve = (ClampedCurveAttribute)attribute;
			return EditorGUIUtility.singleLineHeight * boundedCurve.height;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			ClampedCurveAttribute boundedCurve = (ClampedCurveAttribute)attribute;

			EditorGUI.BeginProperty(position, label, property);
			property.animationCurveValue = EditorGUI.CurveField(position, label, property.animationCurveValue, Color.white, boundedCurve.bounds);
			EditorGUI.EndProperty();
		}
	}
}