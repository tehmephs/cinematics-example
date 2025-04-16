using Assets.Scripts.Graph.Cinematics;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace Assets.Scripts.Editor.Graph
{
	[CustomNodeEditor(typeof(MacroDirectiveNode))]
	public class MacroDirectiveNodeEditor : NodeEditor
	{
		//public override void OnHeaderGUI()
		//{
		//	base.OnHeaderGUI();

		//	Event e = Event.current;
		//	Rect bodyRect = GUILayoutUtility.GetLastRect();

		//	if (e.type == EventType.MouseDown && e.clickCount == 2 && bodyRect.Contains(e.mousePosition))
		//	{
		//		this.DrilldownToMacro();
		//		e.Use();
		//	}
		//}

		public override void OnBodyGUI()
		{
			base.OnBodyGUI();

			// Enable double clicking on the body of the 
			float pad = EditorGUIUtility.singleLineHeight;

			Event e = Event.current;
			Rect bodyRect = GUILayoutUtility.GetLastRect();
			Rect fullRect = new Rect(bodyRect.x, bodyRect.y - pad, bodyRect.width, bodyRect.height + pad);

			if (e.type == EventType.MouseDown && e.clickCount == 2 && fullRect.Contains(e.mousePosition))
			{
				this.DrilldownToMacro();
				e.Use();
			}

			GUILayout.Space(10);

			var node = this.target as MacroDirectiveNode;

			if (node.macro != null)
			{
				if (GUILayout.Button($"Drilldown"))
				{
					CinematicGraphEditor.Drilldown(node.macro);
				}
			}
		}

		private void DrilldownToMacro()
		{
			var node = this.target as MacroDirectiveNode;

			if (node != null)
			{
				if (node.macro != null)
				{
					CinematicGraphEditor.Drilldown(node.macro);
				}
			}
			else
			{
				Debug.LogWarning($"MacroDirectiveNodeEditor::OpenGraphTab - Target cast as a null value, the editor did not open.");
			}
		}
	}
}
