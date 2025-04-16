using System;
using System.Collections.Generic;

using Assets.Scripts.Data;

[Serializable]
public struct ActorAnimationList
{
	public enum OrientationDirection
	{
		Left = -1,
		Neutral = 0,
		Right = 1
	}

	public List<ActorAnimation> animations;
	public ActorAnimation selectedAnimation;

	[NodeEnum]
	public OrientationDirection orientation;

	public bool on;

	public void PlayForActor(IActor actor, bool off = false) => this.selectedAnimation.PlayForActor(actor, this, off);
}
