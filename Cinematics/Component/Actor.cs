using System;
using System.Collections;

using Assets.Scripts.Data;
using Assets.Scripts.Graph.Cinematics;
using UnityEngine;

public class Actor : IdentityComponent<ActorId>, IActor
{
	private const float _F_DEFAULT_MARGIN = 0.05f;
	private const float _F_DEFAULT_TIME = 1;

	private Animator animator;
	private Material material;
	private Renderer actorRenderer;

	public ActorId Identity => this.identity;
	public Vector3 position => this.transform.position;


	private void Awake()
	{
		if (this.TryGetComponent(out this.actorRenderer))
		{
			this.HasRenderer = true;

			this.material = actorRenderer.material;
			this.HasMaterial = this.material != null;
		}

		this.HasAnimator = this.TryGetComponent(out this.animator);
	}


	#region IActor

	public bool Busy { get; private set; }
	public bool HasAnimator { get; private set; }
	public bool HasMaterial { get; private set; }
	public bool HasRenderer { get; private set; }

	public Material GetMaterial() => this.material;

	public void RunDirective(ActorDirectiveNode node, ICinematicContext context)
	{
		this.Busy = true;
		this.StartCoroutine(this.DoRunDirective(node, context));
	}

	public Sprite GetPortrait(DialogueExpression expression) => this.identity.GetPortrait(expression);

	public void SetAnimationStateBool(string name, bool value) => this.AssertAnimator(() => this.animator.SetBool(name, value));
	public void SetAnimationStateFloat(string name, float value) => this.AssertAnimator(() => this.animator.SetFloat(name, value));
	public void SetAnimationStateInt(string name, int value) => this.AssertAnimator(() => this.animator.SetInteger(name, value));
	public void SetAnimationStateTrigger(string name) => this.AssertAnimator(() => this.animator.SetTrigger(name));
	public void SetAnimationStatePredefined(ActorAnimationList anim) => this.AssertAnimator(() => anim.selectedAnimation.PlayForActor(this, anim));

	public void SetVisible(bool visible)
	{
		if (this.HasRenderer)
		{
			this.actorRenderer.enabled = visible;
		}
	}

	#endregion

	private void AssertAnimator(Action followupAction)
	{
		if (HasAnimator)
		{
			followupAction();
		}
		else
		{
			Debug.LogWarning($"Actor::AssertAnimator - attempted to set an animation state but the actor `{this.name}` has no animator");
		}
	}

	#region Coroutine

	private IEnumerator DoRunDirective(ActorDirectiveNode node, ICinematicContext context)
	{
		yield return node.ExecuteDirective(this, context);

		// Stop directives when a non-actor node is reached
		var nextNode = node.Next;
		bool canRunNextNode = nextNode?.IsActorNode ?? false;

		if (canRunNextNode)
		{
			yield return this.DoRunDirective((ActorDirectiveNode)nextNode, context);
		}
		else
		{
			context.LogDebug(node, this, "Ran out of directives, exiting busy state.");
			this.Busy = false;
		}
	}

	#endregion
}
