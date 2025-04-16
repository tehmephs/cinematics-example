using Assets.Scripts.Graph.Attributes;
using System.Collections;
using UnityEngine;
using _CGRAPH = Constants.ScriptableObjects.Graph.Cinematics;

namespace Assets.Scripts.Graph.Cinematics
{
	[CreateNodeMenu(_CGRAPH.Path.WaitForActors, _CGRAPH.SortOrder.WaitForActors)]
	[NodeTint(_CGRAPH.Tint.WaitForActors), NodeDefaultName(_CGRAPH.DefaultName.WaitForActors)]
	public sealed class WaitForActorsDirectiveNode : SceneDirectiveNode
	{
		[Input(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.InheritedAny)]
		public ActorDirectiveNode actors;

		public override IEnumerator ExecuteDirective(ICinematicContext context)
		{
			float time = Time.realtimeSinceStartup;

			var actorNodesPending = this.GetPreviousNodesbyField<ActorDirectiveNode>(nameof(this.actors));

			if (actorNodesPending.Length > 0)
			{
				context.LogDebug(this, $"Processor is awaiting {actorNodesPending.Length} parallel actions to finish...");

				for (int i = 0; i < actorNodesPending.Length; i++)
				{
					while (!actorNodesPending[i].ActionIsComplete)
					{
						yield return null;
					}
				}

				context.LogDebug(this, $"Wait ended after {Time.realtimeSinceStartup - time} seconds...");
			}
			else
			{
				Debug.LogWarning($"WaitForActorsDirectiveNode::ExecuteDirective - Wait for actors node has no actor directives plugged into it, make sure actor directives end their connection on the `actors` field or this node moves on without evaluating.");
			}
		}
	}
}
