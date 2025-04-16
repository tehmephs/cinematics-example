using System.Collections;

using Assets.Scripts.Graph.Attributes;
using UnityEngine;

using _CGRAPH = Constants.ScriptableObjects.Graph.Cinematics;

namespace Assets.Scripts.Graph.Cinematics
{
	[CreateNodeMenu(_CGRAPH.Path.WaitScene, _CGRAPH.SortOrder.WaitScene)]
	[NodeTint(_CGRAPH.Tint.WaitScene), NodeWidth(_CGRAPH.Width.WaitScene), NodeDefaultName(_CGRAPH.DefaultName.WaitScene)]
	public class WaitSceneDirectiveNode : SceneDirectiveNode
	{
		[Space(5)]

		[Tooltip("The duration in seconds to wait.")]
		public float time;

		[Tooltip("When checked, the duration will happen in real time (the delay will not be affected by time scaling)")]
		public bool realtime;

		public override IEnumerator ExecuteDirective(ICinematicContext context)
		{
			if (this.realtime)
			{
				context.LogDebug(this, $"Processor is waiting for {this.time} realtime seconds...");
				yield return new WaitForSecondsRealtime(this.time);
			}
			else
			{
				context.LogDebug(this, $"Processor is waiting for {this.time} scaled seconds...");
				yield return new WaitForSeconds(this.time);
			}

			context.LogDebug(this, $"Processor finished waiting");
		}
	}
}
