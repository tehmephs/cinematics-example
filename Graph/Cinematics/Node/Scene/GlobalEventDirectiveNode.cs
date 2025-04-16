using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Data;
using Assets.Scripts.Graph.Attributes;
using UnityEngine;

using _CGRAPH = Constants.ScriptableObjects.Graph.Cinematics;

namespace Assets.Scripts.Graph.Cinematics
{
	[CreateNodeMenu(_CGRAPH.Path.GlobalEvent, _CGRAPH.SortOrder.GlobalEvent)]
	[NodeTint(_CGRAPH.Tint.GlobalEvent), NodeWidth(_CGRAPH.Width.GlobalEvent), NodeDefaultName(_CGRAPH.DefaultName.GlobalEvent)]
	public class GlobalEventDirectiveNode : SceneDirectiveNode
	{
		[Space(8)]

		[Tooltip("The global event to trigger.")]
		public EventId eventId;

		[Tooltip("Passes the specified arguments to the global event.  Reference `Global Events` documentation for each global event to see what arguments are accepted or expected.")]
		public List<GlobalEventArgument> arguments;

		public override IEnumerator ExecuteDirective(ICinematicContext context)
		{
			object[] argValues = this.arguments.Select(arg => arg.GetArgumentValue())
											   .ToArray();

			context.LogDebug(this, $"Triggering global event `{this.eventId.name}` with arguments: [{string.Join(", ", argValues)}]");

			GameData.Instance.TriggerGlobalEvent(this.eventId, argValues);
			yield return null;
		}
	}
}
