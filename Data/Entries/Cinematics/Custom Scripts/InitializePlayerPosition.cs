using System.Collections;

using Assets.Scripts.Attributes;
using Assets.Scripts.Data;
using Assets.Scripts.Graph.Cinematics;
using SerializeReferenceEditor;
using UnityEngine;

[SRName("Initialize Player Position")]
public class InitializePlayerPosition : SceneCinematicScript
{
	//public override int NodeWidth => 400;
	//public override float LabelWidth => 120;

	[Header("Starting Position"), Space(5)]
	[AlwaysExpand, Tooltip("The world coordinate (by reference or by raw vector) from which the fly-in will begin from.  The player will be teleported to this point immediately and then perform the fly-in using the directed parameters.")]
	public DirectiveTarget start;

	[Tooltip("If checked, the drone will also fly-in with the player.")]
	public bool includeDrone = true;

	public override IEnumerator Run(ICinematicContext context)
	{
		var playerActor = context.GetActor(ActorIdentity.Player);
		var droneActor = context.GetActor(ActorIdentity.Drone);
		Vector3 position = this.start.GetPosition(playerActor, context);

		playerActor.transform.position = this.start.GetPosition(playerActor, context);

		if (includeDrone)
		{
			droneActor.transform.position = this.start.GetPosition(droneActor, context);
		}

		yield return null;
	}
}
