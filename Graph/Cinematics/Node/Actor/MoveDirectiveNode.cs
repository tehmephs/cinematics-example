using System;
using System.Collections;

using Assets.Scripts.Attributes;
using Assets.Scripts.Data;
using Assets.Scripts.Graph.Attributes;

using UnityEngine;

using _CGRAPH = Constants.ScriptableObjects.Graph.Cinematics;

namespace Assets.Scripts.Graph.Cinematics
{
	[CreateNodeMenu(_CGRAPH.Path.Move, _CGRAPH.SortOrder.Move)]
	[NodeTint(_CGRAPH.Tint.Move), NodeWidth(_CGRAPH.Width.Move), NodeDefaultName(_CGRAPH.DefaultName.Move)]
	public class MoveDirectiveNode : ActorDirectiveNode
	{
		private const string _STR_MOVE_TYPE = "moveType";
		private const string _STR_TARGET_TYPE = "targetType";

		[Space(5)]

		[NodeEnum, Tooltip("Determines how to move the actor.  MoveTo will tell the actor to change it's world position using the given motion curve and duration.  LookAt will change the actor's rotation to look at a point in world space over the given time and motion curve.  TeleportTo will instantly warp the actor to the world coordinates given.")]
		public MovementDirectiveType moveType;

		[Space(5), Header("Target Parameters"), Space(5)]

		[AlwaysExpand, Tooltip("Encapsulates a model which represents how a position in world space is derived (either from a raw vector, an actor, or point)")]
		public DirectiveTarget target;

		[Space(5)]

		[Tooltip("The animation curve to support the motion.  The curve will be applied using 0-1 representing the value of `time` on the X axis, and 0-1 representing position from the actor's starting point to the destination point on the Y axis.")]
		[ShowWhen(_STR_MOVE_TYPE, MovementDirectiveType.MoveTo)]
		public MotionCurve motion;

		[Tooltip("The duration to apply the motion over.")]
		[ShowWhen(_STR_MOVE_TYPE, MovementDirectiveType.MoveTo, MovementDirectiveType.LookAt)]
		[Min(0)]
		public float time;

		protected override IEnumerator OnExecuteDirective(IActor actor, ICinematicContext context) => this.DoExecuteDirective(actor, context);

		private IEnumerator DoExecuteDirective(IActor actor, ICinematicContext context)
			=> this.moveType switch
			{
				MovementDirectiveType.MoveTo => this.DoMoveDirective(actor, context),
				MovementDirectiveType.TeleportTo => this.DoTeleportDirective(actor, context),
				MovementDirectiveType.LookAt => this.DoLookDirective(actor, context),
				_ => throw new Exception($"MoveDirectiveNode::DoExecuteDirective - No switch case provided for `{this.moveType}`")
			};

		private IEnumerator DoMoveDirective(IActor subject, ICinematicContext context)
		{
			// float elapsed = 0;

			Vector3 endingPoint = this.target.GetPosition(subject, context);
			Vector3 startingPoint = subject.position;
			//Vector3 distance = endingPoint - startingPoint;

			context.LogDebug(this, subject, $"Actor moving from {startingPoint} to {endingPoint} using motion `{this.motion.name}` over {this.time} seconds.");

			yield return this.motion.ApplyMotionToTransform(subject.transform, endingPoint, this.time);

			//while (elapsed < this.time)
			//{
			//	elapsed += Time.deltaTime;
			//	subject.transform.position = startingPoint + (distance * this.motion.curve.Evaluate(elapsed / this.time));
			//	yield return null;
			//}

			context.LogDebug(this, subject, $"Move finished!");
		}

		private IEnumerator DoTeleportDirective(IActor subject, ICinematicContext context)
		{
			var destination = this.target.GetPosition(subject, context);
			context.LogDebug(this, subject, $"Actor teleporting from {subject.position} to {destination} instantly.");
			subject.transform.position = destination;
			yield return null;
		}

		private IEnumerator DoLookDirective(IActor subject, ICinematicContext context)
		{
			float elapsed = 0;

			Vector3 targetPoint = this.target.GetPosition(subject, context);

			Quaternion startRotation = subject.transform.rotation;
			Quaternion targetRotation = Quaternion.LookRotation(targetPoint - subject.position, Vector3.up);

			context.LogDebug(this, subject, $"Actor rotating from {subject.transform.localRotation} to {targetRotation} over {this.time} seconds...");

			while (elapsed < this.time)
			{
				elapsed += Time.deltaTime;
				subject.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / this.time);
				// subject.transform.Rotate(new Vector3(0, degreesPerSecond * Time.deltaTime, 0));
				yield return null;
			}

			subject.transform.rotation = targetRotation;

			context.LogDebug(this, subject, $"Actor rotation snapped to {subject.transform.localRotation}");
		}
	}
}
