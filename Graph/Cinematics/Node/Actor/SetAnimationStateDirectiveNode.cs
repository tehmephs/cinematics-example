using System;
using System.Collections;
using System.Linq;

using Assets.Scripts.Attributes;
using Assets.Scripts.Data;
using Assets.Scripts.Graph.Attributes;

using UnityEngine;

using _CGRAPH = Constants.ScriptableObjects.Graph.Cinematics;

namespace Assets.Scripts.Graph.Cinematics
{
	[CreateNodeMenu(_CGRAPH.Path.SetAnimState, _CGRAPH.SortOrder.SetAnimState)]
	[NodeTint(_CGRAPH.Tint.SetAnimState), NodeWidth(_CGRAPH.Width.SetAnimState), NodeDefaultName(_CGRAPH.DefaultName.SetAnimState)]
	public class SetAnimationStateDirectiveNode : ActorDirectiveNode
	{
		private const string _STR_HAS_ACTOR = "hasValidActorConnection";

		[SerializeField, HideInInspector]
		private bool hasValidActorConnection = false;

		[Space(10)]

		[Tooltip("Toggles manual configuration mode.  By default, you can select from a list of defined animation state objects that are tied to the actor being routed to this node.  If this box is checked, you'll need to input the raw parameters to alter the state machine.")]
		[ShowWhen(_STR_HAS_ACTOR, true)]
		public bool manual;

		[Tooltip("The type of parameter to change")]
		[ShowWhen("ShowManualList", ShowWhenOperator.Method)]
		[NodeEnum]
		public AnimatorControllerParameterType paramType = AnimatorControllerParameterType.Trigger;

		[Tooltip("The name of the parameter to change")]
		[ShowWhen("ShowManualList", ShowWhenOperator.Method)]
		public string parameter;

		[Tooltip("The value to give the target parameter")]
		[ShowWhen("ShowIntegerField", ShowWhenOperator.Method)]
		public int intValue;

		[Tooltip("The value to give the target parameter")]
		[ShowWhen("ShowBoolField", ShowWhenOperator.Method)]
		public bool boolValue;

		[Tooltip("The value to give the target parameter")]
		[ShowWhen("ShowFloatField", ShowWhenOperator.Method)]
		public float floatValue;

		[ShowWhen("ShowAutoList", ShowWhenOperator.Method)]
		public ActorAnimationList animation;

		protected override IEnumerator OnExecuteDirective(IActor subject, ICinematicContext context)
			=> this.manual ? this.DoExecuteDirective(subject, context)
						   : this.DoExecuteAsActorAnim(subject, context);

		private IEnumerator DoExecuteDirective(IActor subject, ICinematicContext context)
			=> this.paramType switch
			{
				AnimatorControllerParameterType.Float => this.DoAnimStateFloat(subject, context),
				AnimatorControllerParameterType.Int => this.DoAnimStateInt(subject, context),
				AnimatorControllerParameterType.Bool => this.DoAnimStateBool(subject, context),
				AnimatorControllerParameterType.Trigger => this.DoAnimStateTrigger(subject, context),
				_ => throw new Exception($"SetAnimationStateDirectiveNode::DoExecuteDirective - No switch case provided for `{this.paramType}`")
			};

		private IEnumerator DoAnimStateFloat(IActor subject, ICinematicContext context)
		{
			context.LogDebug(this, subject, $"Changing animation state {this.parameter} to `{this.floatValue}`");
			subject.SetAnimationStateFloat(this.parameter, this.floatValue);
			yield return null;
		}

		private IEnumerator DoAnimStateInt(IActor subject, ICinematicContext context)
		{
			context.LogDebug(this, subject, $"Changing animation state {this.parameter} to `{this.intValue}`");
			subject.SetAnimationStateInt(this.parameter, this.intValue);
			yield return null;
		}

		private IEnumerator DoAnimStateBool(IActor subject, ICinematicContext context)
		{
			context.LogDebug(this, subject, $"Changing animation state {this.parameter} to `{this.boolValue}`");
			subject.SetAnimationStateBool(this.parameter, this.boolValue);
			yield return null;
		}

		private IEnumerator DoAnimStateTrigger(IActor subject, ICinematicContext context)
		{
			context.LogDebug(this, subject, $"Changing animation state trigger {this.parameter}");
			subject.SetAnimationStateTrigger(this.parameter);
			yield return null;
		}

		private IEnumerator DoExecuteAsActorAnim(IActor subject, ICinematicContext context)
		{
			this.animation.selectedAnimation.PlayForActor(subject, this.animation);
			yield return null;
		}

		private bool ShowIntegerField() => !this.ShowAutoList() && this.paramType == AnimatorControllerParameterType.Int;
		private bool ShowFloatField() => !this.ShowAutoList() && this.paramType == AnimatorControllerParameterType.Float;
		private bool ShowBoolField() => !this.ShowAutoList() && this.paramType == AnimatorControllerParameterType.Bool;
		private bool ShowAutoList() => !this.manual && this.hasValidActorConnection;
		private bool ShowManualList() => this.manual || !this.hasValidActorConnection;

		public override void UpdateActor(ActorId actor)
		{
			if (actor != null)
			{
				this.animation.animations = actor.animationStates;
				this.hasValidActorConnection = this.animation.animations.Any();
			}
			else
			{
				this.hasValidActorConnection = false;
			}

			base.UpdateActor(actor);
		}
	}
}
