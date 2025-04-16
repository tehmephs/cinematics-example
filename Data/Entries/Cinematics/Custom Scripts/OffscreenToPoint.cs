using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.Attributes;
using Assets.Scripts.Data;
using Assets.Scripts.Graph.Cinematics;
using SerializeReferenceEditor;
using UnityEditor;
using UnityEngine;

[SRName("Player Offscreen Fly-In")]
public class OffscreenToPointCinematicScript : SceneCinematicScript
{
	private const string _STR_ACTOR_PLAYER_PATH = "Assets/_Game/Scripts/Data/Entries/Identity/Actor/Player.asset";

	public override int NodeWidth => 400;
	public override float LabelWidth => 120;

	[Space(5), Header("Destination Position"), Space(5)]
	[AlwaysExpand, Tooltip("The world coordinate (by reference or by raw vector) from which the fly-in will end.  The player will move over the given duration to the destination.")]
	public DirectiveTarget end;

	[Space(10), Header("Script Settings")]

	[Tooltip("Determines how long the fly-in will take.")]
	[Min(0)]
	public float duration;

	[Tooltip("Determines how long to wait before the fly-in occurs.  The teleport will still happen immediately.")]
	[Min(0)]
	public float delay;

	[Tooltip("The motion curve to use for the movement to the endpoint.")]
	public MotionCurve motion;

	[Tooltip("If checked, the drone will also fly-in with the player.")]
	public bool includeDrone = true;

	[Tooltip("The world space offset from the player to tag along from.")]
	[ShowWhen("includeDrone", true)]
	public Vector3 followOffset;

	[Tooltip("Determines if an animation will be used for the player fly-in or not.")]
	public bool useAnimationState;

	[Tooltip("Choose a player animation state to use for the fly-in")]
	[ShowWhen("useAnimationState", true)]
	public ActorAnimationList animation;

	public override IEnumerator Run(ICinematicContext context)
	{
		var playerActor = context.GetActor(ActorIdentity.Player);
		var droneActor = context.GetActor(ActorIdentity.Drone);
		Vector3 endPoint = this.end.GetPosition(playerActor, context);

		List<Transform> transformsToMove = new List<Transform>() { playerActor.transform };
		List<Vector3> destinations = new List<Vector3>() { endPoint };

		if (includeDrone)
		{
			transformsToMove.Add(droneActor.transform);
			destinations.Add(this.end.GetPosition(droneActor, context) + this.followOffset);
			droneActor.transform.position = playerActor.position + this.followOffset;
		}

		yield return null;

		if (this.useAnimationState)
		{
			this.animation.PlayForActor(playerActor);
		}

		yield return new WaitForSeconds(this.delay);

		yield return this.motion.ApplyMotionToTransforms(transformsToMove.ToArray(), destinations.ToArray(), this.duration);

		if (this.useAnimationState && this.animation.selectedAnimation.parameterType == AnimatorControllerParameterType.Bool)
		{
			this.animation.PlayForActor(playerActor, off: true);
		}

		yield return null;
	}

	protected override void OnValidate()
	{
		ActorId player = AssetDatabase.LoadAssetAtPath<ActorId>(_STR_ACTOR_PLAYER_PATH);

		if (player != null)
		{
			this.animation.animations = player.animationStates;
		}
		else
		{
			throw new System.Exception($"OffscreenToPoint::OnValidate - The player actor was expected at the path `{_STR_ACTOR_PLAYER_PATH}` but was not found.  Please update the code to point to the correct path.");
		}
	}
}
