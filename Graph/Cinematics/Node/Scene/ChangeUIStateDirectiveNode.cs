using System;
using System.Collections;

using Assets.Scripts.Attributes;
using Assets.Scripts.Data;
using Assets.Scripts.Graph.Attributes;
using UnityEngine;

using _CGRAPH = Constants.ScriptableObjects.Graph.Cinematics;

namespace Assets.Scripts.Graph.Cinematics
{
	[CreateNodeMenu(_CGRAPH.Path.ChangeUI, _CGRAPH.SortOrder.ChangeUI)]
	[NodeTint(_CGRAPH.Tint.ChangeUI), NodeWidth(_CGRAPH.Width.ChangeUI), NodeDefaultName(_CGRAPH.DefaultName.ChangeUI)]
	public class ChangeUIStateDirectiveNode : SceneDirectiveNode
	{
		private const float _F_ACTION_AWAIT_TIME = 2.25f;
		private const string _STR_PARAM_TYPE = "targetType";

		[Space(5)]

		[NodeEnum, Tooltip("The type of UI component to change the state of")]
		public UIStateTargetType targetType;

		[Tooltip("Enables or disables the target UI component")]
		[ShowWhen(_STR_PARAM_TYPE, UIStateTargetType.Letterbox, UIStateTargetType.Fullscreen, UIStateTargetType.Status, UIStateTargetType.StageClear)]
		public bool enable;

		[Tooltip("The amount of time for the UI state directive to take place over, if applicable.")]
		[ShowWhen(_STR_PARAM_TYPE, ShowWhenOperator.None, UIStateTargetType.Action, UIStateTargetType.ExitDialogue)]
		public float time;

		[Tooltip("Sets the color for screen components, if applicable.")]
		[ShowWhen(_STR_PARAM_TYPE, UIStateTargetType.Letterbox, UIStateTargetType.Fullscreen, UIStateTargetType.Flash)]
		[ColorUsage(showAlpha: false)]
		public Color color;

		[Tooltip("Will wait until the transition finishes before continuing onto the next directive.  If unchecked, the UI state change will happen in the background and the cinematic will continue with it's directives without any delay.")]
		public bool await;

		private string ActionDebugText => this.enable ? "Enabling" : "Disabling";
		private string AwaitDebugText => this.await ? "awaiting" : "not awaiting";

		public override IEnumerator ExecuteDirective(ICinematicContext context)
		{
			if (this.targetType == UIStateTargetType.Action)
			{
				this.time = _F_ACTION_AWAIT_TIME;
			}

			yield return this.DoTargetTypeTransition(context);

			if (this.await)
			{
				context.LogDebug(this, $"Configuration is awaiting the UI state change for {this.time} seconds...");
				yield return new WaitForSeconds(this.time);
				context.LogDebug(this, $"UI State change await ended.");
			}
		}

		private IEnumerator DoTargetTypeTransition(ICinematicContext context)
			=> this.targetType switch
			{
				UIStateTargetType.Status => this.DoStatus(context),
				UIStateTargetType.StageClear => this.DoStageClear(context),
				UIStateTargetType.Letterbox => this.DoLetterbox(context),
				UIStateTargetType.AlertBlink => this.DoAlertBlink(context),
				UIStateTargetType.Flash => this.DoFlash(context),
				UIStateTargetType.Fullscreen => this.DoFullscreen(context),
				UIStateTargetType.Action => this.DoAction(context),
				UIStateTargetType.ExitDialogue => this.DoExitDialogue(context),
				_ => throw new Exception($"ChangeUIStateDirectiveNode::DoTargetTypeTransition - No switch case provided for `{this.targetType}`")
			};

		private IEnumerator DoStatus(ICinematicContext context)
		{
			context.LogDebug(this, $"{this.ActionDebugText} status UI over {this.time} seconds.");
			UXManager.Instance.ShowStatus(this.enable, this.time);
			yield return null;
		}

		private IEnumerator DoStageClear(ICinematicContext context)
		{
			context.LogDebug(this, $"{this.ActionDebugText} stage clear UI over {this.time} seconds.");
			UXManager.Instance.ShowStageClear(this.enable, this.time);
			yield return null;
		}

		private IEnumerator DoLetterbox(ICinematicContext context)
		{
			context.LogDebug(this, $"{this.ActionDebugText} letterbox UI over {this.time} seconds with color `{this.color}`");
			UXManager.Instance.SetLetterboxMode(this.enable, this.time, this.color);
			yield return null;
		}

		private IEnumerator DoAlertBlink(ICinematicContext context)
		{
			context.LogDebug(this, $"Triggering the alert dialog animation to play for {this.time} seconds.");
			UXManager.Instance.ShowAlertDialog(this.time);
			yield return null;
		}

		private IEnumerator DoFlash(ICinematicContext context)
		{
			context.LogDebug(this, $"Triggering a screen flash over {this.time} seconds with color `{this.color}`");
			UXManager.Instance.SetFullscreen(true, 0, this.color);

			yield return null;

			UXManager.Instance.SetFullscreen(false, this.time, this.color);
		}

		private IEnumerator DoFullscreen(ICinematicContext context)
		{
			context.LogDebug(this, $"{this.ActionDebugText} fullscreen UI element over {this.time} seconds with color `{this.color}`");
			UXManager.Instance.SetFullscreen(this.enable, this.time, this.color);
			yield return null;
		}

		private IEnumerator DoAction(ICinematicContext context)
		{
			context.LogDebug(this, $"Triggering the action dialog animation");
			UXManager.Instance.PlayActionDialog();
			yield return null;
		}

		private IEnumerator DoExitDialogue(ICinematicContext context)
		{
			context.LogDebug(this, $"Closing dialogue UI and {this.AwaitDebugText}");

			if (this.await)
				yield return UXManager.Instance.CloseDialogue();
			else
				UXManager.Instance.CloseDialogue();
		}
	}
}
