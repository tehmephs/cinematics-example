using Assets.Scripts.Graph.Attributes;
using System.Collections;
using UnityEngine;

using _CGRAPH = Constants.ScriptableObjects.Graph.Cinematics;

namespace Assets.Scripts.Graph.Cinematics
{
	[CreateNodeMenu(_CGRAPH.Path.WaitActor, _CGRAPH.SortOrder.WaitActor)]
	[NodeTint(_CGRAPH.Tint.WaitActor), NodeWidth(_CGRAPH.Width.WaitActor), NodeDefaultName(_CGRAPH.DefaultName.WaitActor)]
	public class WaitActorDirectiveNode : ActorDirectiveNode
	{
		[Space(5)]

		[Tooltip("The duration in seconds to wait.")]
		public float time;

		protected override IEnumerator OnExecuteDirective(IActor actor, ICinematicContext context)
		{
			context.LogDebug(this, actor, $"{actor.name} is waiting for {this.time} seconds...");

			yield return new WaitForSeconds(this.time);

			context.LogDebug(this, actor, $"{actor.name} stopped waiting");
		}
	}
}
