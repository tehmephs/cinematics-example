using System.Collections;
using Assets.Scripts.Graph.Attributes;
using SerializeReferenceEditor;
using UnityEngine;

using _CGRAPH = Constants.ScriptableObjects.Graph.Cinematics;

namespace Assets.Scripts.Graph.Cinematics
{
	[CreateNodeMenu(_CGRAPH.Path.ActorCustom, _CGRAPH.SortOrder.ActorCustom)]
	[NodeTint(_CGRAPH.Tint.ActorCustom), NodeWidth(_CGRAPH.Width.ActorCustom), NodeDefaultName(_CGRAPH.DefaultName.ActorCustom)]
	public class ActorCustomScriptDirectiveNode : ActorDirectiveNode
	{
		[Space(5)]

		[Tooltip("Accepts a custom script that connects to actor-driven nodes.  The directive can run advanced custom script code using the normal directive pipeline.")]
		[SerializeReference, SR]
		public ActorCinematicScript script;

		public int NodeWidth => this.script?.NodeWidth ?? _CGRAPH.Width.Custom;

		protected override IEnumerator OnExecuteDirective(IActor subject, ICinematicContext context)
		{
			if (this.script != null)
			{
				context.LogDebug(this, subject, $"Running custom script `{this.script.GetType().Name}` on actor `{subject.name}`");
				yield return this.script.Run(subject, context);
			}
			else
			{
				Debug.LogWarning($"ActorCustomScriptDirectiveNode::ExecuteDirective - script reference was null, skipping directive");
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
