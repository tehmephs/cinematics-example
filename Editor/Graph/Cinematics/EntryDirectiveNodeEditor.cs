using System.IO;

using Assets.Scripts.Graph.Cinematics;
using UnityEngine;
using XNodeEditor;

namespace Assets.Scripts.Editor.Graph
{
	/// <summary>
	/// Special node editor for the Entry directive node that appears as the default in every cinematics graph
	/// </summary>
	[CustomNodeEditor(typeof(EntryDirectiveNode))]
	public class EntryDirectiveNodeEditor : NodeEditor
	{
		private const string _STR_ARROW_IMAGE = "./Assets/_Game/UI/image/graph/arrow_right.png";

		public override void OnBodyGUI()
		{
			// Load the green arrow icon as a texture
			// TODO: Find a better way to do this with Resources.Load
			var arrowTexture = new Texture2D(150, 100); //Resources.Load<Texture2D>(_STR_ARROW_IMAGE);
			var data = File.ReadAllBytes(_STR_ARROW_IMAGE);
			arrowTexture.LoadImage(data);

			// Draw the port field with the arrow as the label
			var node = this.target as EntryDirectiveNode;
			NodeEditorGUILayout.PortField(new GUIContent(arrowTexture), node.GetOutputPort(nameof(node.to)));
		}
	}
}
