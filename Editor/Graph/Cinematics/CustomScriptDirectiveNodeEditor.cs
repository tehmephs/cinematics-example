using Assets.Scripts.Graph.Cinematics;
using UnityEditor;
using XNodeEditor;

namespace Assets.Scripts.Editor.Graph
{
	[CustomNodeEditor(typeof(CustomScriptDirectiveNode))]
	public class CustomScriptDirectiveNodeEditor : NodeEditor
	{
		public override void OnBodyGUI()
		{
			var node = this.target as CustomScriptDirectiveNode;
			float originalLabelWidth = EditorGUIUtility.labelWidth;

			EditorGUIUtility.labelWidth = node.script?.LabelWidth ?? EditorGUIUtility.labelWidth;
			base.OnBodyGUI();
			EditorGUIUtility.labelWidth = originalLabelWidth;
		}

		public override int GetWidth()
		{
			var node = this.target as CustomScriptDirectiveNode;
			return node.NodeWidth;
		}
	}

	[CustomNodeEditor(typeof(ActorCustomScriptDirectiveNode))]
	public class ActorCustomScriptDirectiveNodeEditor : NodeEditor
	{
		public override void OnBodyGUI()
		{
			var node = this.target as ActorCustomScriptDirectiveNode;
			float originalLabelWidth = EditorGUIUtility.labelWidth;

			EditorGUIUtility.labelWidth = node.script?.LabelWidth ?? EditorGUIUtility.labelWidth;
			base.OnBodyGUI();
			EditorGUIUtility.labelWidth = originalLabelWidth;
		}

		public override int GetWidth()
		{
			var node = this.target as ActorCustomScriptDirectiveNode;
			return node.NodeWidth;
		}
	}
}
