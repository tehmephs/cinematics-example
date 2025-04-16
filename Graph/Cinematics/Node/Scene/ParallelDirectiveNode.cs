using System.Collections;

using Assets.Scripts.Data;
using Assets.Scripts.Graph.Attributes;

using UnityEngine;
using XNode;
using _CGRAPH = Constants.ScriptableObjects.Graph.Cinematics;

namespace Assets.Scripts.Graph.Cinematics
{
	[CreateNodeMenu(_CGRAPH.Path.Parallel, _CGRAPH.SortOrder.Parallel)]
	[NodeTint(_CGRAPH.Tint.Parallel), NodeWidth(_CGRAPH.Width.Parallel), NodeDefaultName(_CGRAPH.DefaultName.Parallel)]
	public class ParallelDirectiveNode : SceneDirectiveNode, IDynamicActorList
	{
		[Space(8)]

		[Output(dynamicPortList = true), Tooltip("Creates a parallel directive chain for the declared actors in the scene.  Each actor will run their directives parallel to one another.  It's recommended that at some point all parallel directives end on a `Wait For Actors` node")]
		public ActorId[] actors;

		public ActorId[] ActorList => this.actors;

		public override IEnumerator ExecuteDirective(ICinematicContext context)
		{
			for (int i = 0; i < this.actors.Length; i++)
			{
				var port = this.GetPort($"{nameof(this.actors)} {i}");
				var nextActorNode = this.GetConnectedNode<ActorDirectiveNode>(port);

				var actor = context.GetActor(this.actors[i]);

				if (actor != null)
				{
					context.LogDebug(this, actor, $"Starting actor directive chain");
					actor.RunDirective(nextActorNode, context);
				}
			}

			yield return null;
		}

		public override object GetValue(NodePort port)
		{
			if (port.IsDynamic)
			{
				int parsedIndex;
				string portIndexString = port.fieldName
											 .Replace(nameof(this.actors), string.Empty)
											 .Trim();

				if (int.TryParse(portIndexString, out parsedIndex))
				{
					if (parsedIndex >= 0 && parsedIndex < this.actors.Length)
						return this.actors[parsedIndex];
				}
			}

			return base.GetValue(port);
		}

		private void OnValidate()
		{
			for (int i = 0; i < this.actors.Length; i++)
			{
				var port = this.GetPort($"{nameof(this.actors)} {i}");
				var nextActorNode = this.GetConnectedNode<ActorDirectiveNode>(port);

				nextActorNode?.UpdateActor(this.actors[i]);
			}
		}
	}
}
