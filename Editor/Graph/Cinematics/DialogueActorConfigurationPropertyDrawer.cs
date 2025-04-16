using UnityEditor;
using UnityEngine;


namespace Assets.Scripts.Editor.Graph
{
	[CustomPropertyDrawer(typeof(DialogueEvent.DialogueActorConfigurations))]
	public class DialogueActorConfigurationPropertyDrawer : PropertyDrawer
	{
		private const float _F_RECT_PADDING = 2f;
		private const float _F_RECT_PADDING2X = 4f;
		private const string _STR_LABEL_LEFT = "Left";
		private const string _STR_LABEL_RIGHT = "Right";

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			// Remove all indentation
			int indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			// Get left and right properties
			SerializedProperty leftProp = property.FindPropertyRelative(_STR_LABEL_LEFT.ToLower());
			SerializedProperty rightProp = property.FindPropertyRelative(_STR_LABEL_RIGHT.ToLower());

			// Get the rectangble half-width for spacing with additional padding
			float halfWidth = position.width / _F_RECT_PADDING - _F_RECT_PADDING;

			// Calculate rectangle space for the left side label and fields
			Rect leftLabel = new Rect(position.x, position.y, halfWidth, EditorGUIUtility.singleLineHeight);
			Rect leftRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, halfWidth, EditorGUIUtility.singleLineHeight);

			// Calculate rectangle space for the right side label and fields
			Rect rightLabel = new Rect(position.x + halfWidth + _F_RECT_PADDING2X, position.y, halfWidth, EditorGUIUtility.singleLineHeight);
			Rect rightRect = new Rect(position.x + halfWidth + _F_RECT_PADDING2X, position.y + EditorGUIUtility.singleLineHeight, halfWidth, EditorGUIUtility.singleLineHeight);

			// Create a bold and centered font style for the overhead labels
			GUIStyle boldFont = new GUIStyle { fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter, normal = new GUIStyleState { textColor = GUI.color } };

			// Draw the left label (and mark it readonly if it should be)
			GUI.enabled = this.GetEnabled(leftProp, false);
			EditorGUI.LabelField(leftLabel, new GUIContent(_STR_LABEL_LEFT), boldFont);
			EditorGUI.PropertyField(leftRect, leftProp, new GUIContent(_STR_LABEL_LEFT), true);

			// Draw the right label (and mark it readonly if it should be)
			GUI.enabled = this.GetEnabled(rightProp, true);
			EditorGUI.LabelField(rightLabel, new GUIContent(_STR_LABEL_RIGHT), boldFont);
			EditorGUI.PropertyField(rightRect, rightProp, new GUIContent(_STR_LABEL_RIGHT), true);

			// Reset the enabled flag for future property drawers.
			GUI.enabled = true;

			// Reset the indent
			EditorGUI.indentLevel = indent;

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			// Fetch the left and right property again
			SerializedProperty leftProp = property.FindPropertyRelative(_STR_LABEL_LEFT.ToLower());
			SerializedProperty rightProp = property.FindPropertyRelative(_STR_LABEL_RIGHT.ToLower());

			// Calculate both panel heights with additional single line space
			float leftHeight = EditorGUI.GetPropertyHeight(leftProp, true) + EditorGUIUtility.singleLineHeight;
			float rightHeight = EditorGUI.GetPropertyHeight(rightProp, true) + EditorGUIUtility.singleLineHeight;

			// Return the higher height value of the two so both fields are always visible and aligned.
			return Mathf.Max(leftHeight, rightHeight);
		}

		/// <summary>
		/// Determines if the property should be readonly or not, based on "useTitle" and "reverse" boolean values.
		/// </summary>
		/// <param name="property">The serialized property to evaluate</param>
		/// <param name="isReverseEnabled">The expected value of the "reverse" checkbox to check for to determine if this property should be enabled</param>
		/// <returns></returns>
		private bool GetEnabled(SerializedProperty property, bool isReverseEnabled)
		{
			bool title = property.serializedObject.FindProperty("dialogue.useTitle").boolValue;
			bool reverse = property.serializedObject.FindProperty("dialogue.reverse").boolValue;

			// If the title bar isn't in use, or if the given `isReverseEnabled` value matches, then enable this field
			// For the left field, if the title is enabled AND reversed, then the left field is disabled
			// For the right field, if the title is enabled AND NOT reversed, then the right field is disabled
			return !title || reverse.Equals(isReverseEnabled);
		}
	}
}
