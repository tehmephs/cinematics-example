using Assets.Scripts.Data;
using Assets.Scripts.Graph.Cinematics;
using UnityEngine;

public interface IActor : ISceneEntity<ActorId>
{
	bool Busy { get; }
	bool HasAnimator { get; }
	bool HasMaterial { get; }
	bool HasRenderer { get; }

	Material GetMaterial();
	Sprite GetPortrait(DialogueExpression expression);

	void RunDirective(ActorDirectiveNode node, ICinematicContext context);

	void SetAnimationStateBool(string name, bool value);
	void SetAnimationStateFloat(string name, float value);
	void SetAnimationStateInt(string name, int value);
	void SetAnimationStateTrigger(string name);
	void SetAnimationStatePredefined(ActorAnimationList anim);

	void SetVisible(bool hidden);
}
