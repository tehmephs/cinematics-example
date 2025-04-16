using System.Collections;
using Assets.Scripts.Graph.Attributes;
using SerializeReferenceEditor;
using UnityEngine;

using _CGRAPH = Constants.ScriptableObjects.Graph.Cinematics;

namespace Assets.Scripts.Graph.Cinematics
{
	[CreateNodeMenu(_CGRAPH.Path.Custom, _CGRAPH.SortOrder.Custom)]
	[NodeTint(_CGRAPH.Tint.Custom), NodeWidth(_CGRAPH.Width.Custom), NodeDefaultName(_CGRAPH.DefaultName.Custom)]
	public class CustomScriptDirectiveNode : SceneDirectiveNode
	{
		[Space(8)]

		[Tooltip("Accepts a custom script that connects to scene-driven nodes.  The directive can run advanced custom script code using the normal directive pipeline.")]
		[SerializeReference, SR]
		public SceneCinematicScript script;

		public int NodeWidth => this.script?.NodeWidth ?? _CGRAPH.Width.Custom;

		public override IEnumerator ExecuteDirective(ICinematicContext context)
		{
			if (this.script != null)
			{
				context.LogDebug(this, $"Running custom script `{this.script.GetType().Name}");
				yield return this.script.Run(context);
			}
			else
			{
				Debug.LogWarning($"CustomScriptDirectiveNode::ExecuteDirective - script reference was null, skipping directive");
			}
		}

		private void OnValidate()
		{
			if (this.script != null)
			{
				this.script.Validate();
			}
		}
	}
}
