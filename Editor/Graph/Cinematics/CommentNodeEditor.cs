using Assets.Scripts.Graph;
using Assets.Scripts.Util;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace Assets.Scripts.Editor.Graph
{
	/// <summary>
	/// Special node editor for the Entry directive node that appears as the default in every cinematics graph
	/// </summary>
	[CustomNodeEditor(typeof(CommentNode))]
	public class CommentNodeEditor : NodeEditor
	{
		private const float _F_MIN_WIDTH = 200;
		private const float _F_COMMENT_PADDING = 10;

		private bool isEditing = false;
		private float editWidth;

		private CommentNode Node => this.target as CommentNode;

		public override void OnBodyGUI()
		{
			CommentNode node = this.Node;

			GUIStyle wrapStyle = new GUIStyle(EditorStyles.label);
			wrapStyle.wordWrap = true;
			wrapStyle.normal.textColor = node.textColor;

			float labelHeight = wrapStyle.CalcHeight(new GUIContent(node.comment), node.width);

			var containerRect = new Rect(_F_COMMENT_PADDING, _F_COMMENT_PADDING + 20, node.width, labelHeight).PadRect(_F_COMMENT_PADDING);

			Event e = Event.current;
			if (e.type == EventType.MouseDown && e.clickCount == 2 && containerRect.Contains(e.mousePosition))
			{
				this.ToggleTextEditor();
				e.Use();
			}

			if (!this.isEditing)
			{
				EditorGUILayout.LabelField(node.comment, wrapStyle);
				// GUI.Label(containerRect, node.comment, wrapStyle);
			}
			else
			{
				SerializedProperty nodeColorProp = this.serializedObject.FindProperty("commentColor");
				SerializedProperty textColorProp = this.serializedObject.FindProperty("textColor");
				SerializedProperty widthProp = this.serializedObject.FindProperty("width");
				SerializedProperty commentProp = this.serializedObject.FindProperty("comment");

				GUIStyle textAreaStyle = new GUIStyle(EditorStyles.textArea);
				EditorGUILayout.PropertyField(widthProp);
				EditorGUILayout.PropertyField(nodeColorProp);
				EditorGUILayout.PropertyField(textColorProp);
				commentProp.stringValue = EditorGUILayout.TextArea(commentProp.stringValue, textAreaStyle);

				if (GUILayout.Button($"Save Comment"))
				{
					widthProp.floatValue = Mathf.Max(_F_MIN_WIDTH, widthProp.floatValue);

					this.serializedObject.ApplyModifiedProperties();
					this.ToggleTextEditor();
				}
			}
		}

		public override int GetWidth() => (int)this.Node.width;

		public override Color GetTint() => this.Node.commentColor.WithoutAlpha(1);

		private void ToggleTextEditor() => this.isEditing = !this.isEditing;
	}
}
