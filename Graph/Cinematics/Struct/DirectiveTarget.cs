using Assets.Scripts.Attributes;
using Assets.Scripts.Data;
using System;
using UnityEngine;

namespace Assets.Scripts.Graph.Cinematics
{
	[Serializable]
	public struct DirectiveTarget
	{
		private const string _STR_TARGET_TYPE = "targetType";

		[NodeEnum, Tooltip("Determines where in world space to end the movement.  Actor will find the current position of the named actor.  Point will reference a static point in the scene, or Vector allows for an explicit coordinate definition.")]
		public CinematicActionTargetType targetType;

		[Tooltip("The actor to use as the destination point.")]
		[ShowWhen(_STR_TARGET_TYPE, CinematicActionTargetType.Actor)]
		public ActorId actor;

		[Tooltip("A point entity in the scene to use as the destination.")]
		[ShowWhen(_STR_TARGET_TYPE, CinematicActionTargetType.Point)]
		public PointId point;

		[Tooltip("When the target type is `Actor` or `Point`, an additional vector offset can be specified which will move to the adjusted point in the scene relative to the actor or point specified.  If used with `Vector` target, the destination will be a relative point offset from the subject's current position.")]
		public bool relative;

		[Tooltip("A raw point in world space.  If `relative` is checked, this acts as an offset from the specified point, or offset relative to the subject of this directive.")]
		[ShowWhen("IsRelativePositioningOrVector", Operator = ShowWhenOperator.Method)]
		public Vector3 vector;

		public Vector3 GetPosition(IActor subject, ICinematicContext context)
			=> this.targetType switch
			{
				CinematicActionTargetType.Vector => this.GetVectorPosition(subject, context),
				CinematicActionTargetType.Point => this.GetPointPosition(subject, context),
				CinematicActionTargetType.Actor => this.GetActorPosition(subject, context),
				_ => throw new Exception($"DirectiveTarget::GetTargetPosition - No switch case provided for `{this.targetType}`")
			};

		private Vector3 GetPointPosition(IActor subject, ICinematicContext context) => this.GetSceneEntityPosition(context.GetPoint(this.point), subject?.position ?? Vector3.zero);
		private Vector3 GetActorPosition(IActor subject, ICinematicContext context) => this.GetSceneEntityPosition(context.GetActor(this.actor), subject?.position ?? Vector3.zero);

		private Vector3 GetVectorPosition(IActor subject, ICinematicContext context)
		{
			if (this.relative)
			{
				return (subject?.position ?? Vector3.zero) + this.vector;
			}
			else
			{
				return this.vector;
			}
		}

		private Vector3 GetSceneEntityPosition(ISceneEntity entity, Vector3 fallback = default)
		{
			if (entity != null)
			{
				return this.relative ? entity.position + this.vector : entity.position;
			}
			else
			{
				Debug.LogWarning($"MoveDirectiveNode::GetSceneEntityPosition - No entity exists in the scene referencing the identity `{entity.name}`.  The fallback value {fallback} was returned.");
				return fallback;
			}
		}

		private bool IsRelativePositioningOrVector() => this.relative || this.targetType == CinematicActionTargetType.Vector;
	}
}
