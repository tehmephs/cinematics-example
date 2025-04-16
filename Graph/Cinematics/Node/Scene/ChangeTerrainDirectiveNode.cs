using System.Collections;

using Assets.Scripts.Data;
using Assets.Scripts.Graph.Attributes;
using UnityEngine;

using _CGRAPH = Constants.ScriptableObjects.Graph.Cinematics;

namespace Assets.Scripts.Graph.Cinematics
{
	[CreateNodeMenu(_CGRAPH.Path.ChangeTerrain, _CGRAPH.SortOrder.ChangeTerrain)]
	[NodeTint(_CGRAPH.Tint.ChangeTerrain), NodeWidth(_CGRAPH.Width.ChangeTerrain), NodeDefaultName(_CGRAPH.DefaultName.ChangeTerrain)]
	public class ChangeTerrainDirectiveNode : SceneDirectiveNode
	{
		[Space(5)]

		[Tooltip("Sets an arbitrary speed value that dictates how fast the terrain scrolls.  At a certain threshold, the player object will start to play a wind particle system.")]
		public float speed;

		[Tooltip("The amount of time it takes to reach the target speed.")]
		[Min(0)]
		public float time;

		[Tooltip("When checked, this directive will wait until the terrain speed is finished it's transition before continuing to the next directive.  Otherwise this will tell the terrain update to happen in the background and continue through directives without any delay.")]
		public bool await;

		private string DebugDisplayText => this.await ? " and awaiting the transition" : string.Empty;

		public override IEnumerator ExecuteDirective(ICinematicContext context)
		{
			context.LogDebug(this, $"Changing terrain scroll speed to {this.speed} over {this.time} seconds{this.DebugDisplayText}.");
			GameData.Instance.TriggerGlobalEvent(EventIdentity.UpdateTerrain, this.speed, this.time);

			if (await)
			{
				yield return new WaitForSeconds(this.time);
			}
		}
	}
}
