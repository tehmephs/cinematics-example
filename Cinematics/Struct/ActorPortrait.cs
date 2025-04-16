using System;
using UnityEngine;

[Serializable]
public struct ActorPortrait
{
	[Tooltip("This portrait will be used if a sprite is not defined for a particular expression but is referred to.")]
	public Sprite fallback;

	[Tooltip("This portrait sprite represents the most neutral profile expression that is desired for the actor.  Example: neutral pose, hands in pocket, neutral facial expression or mild contentment.")]
	public Sprite rest;

	[Tooltip("This portrait sprite represents the actor portraying confidence or arrogance.  Example: finger raised in the air, closed eyes, a smirk that says `I know exactly what i'm talking about`")]
	public Sprite smug;

	[Tooltip("This portrait sprite represents the actor portraying calm happiness.  Go for a rested smile for this pose.  Example: neutral pose, arms on hips or hands in a fist with a smile.")]
	public Sprite happy;

	[Tooltip("This portrait sprite represents the actor's concern about something.  This is not an extreme expression, but mild uncertainty or disdain.  Example: Posture slouching a bit, or a mild frown, eyes should depict concern.")]
	public Sprite worry;

	[Tooltip("This portrait sprite represents the actor's disgust with someone or something.  This should be an expression of intense displeasure with the circumstances.  Example: The actor just smelled something awful or saw something disgusting that made them reel, either morally or physically.")]
	public Sprite disgust;

	[Tooltip("This portrait sprite represents the actor's surprise with someone or something.  This should be an expression of intense shock in reaction to unexpected circumstances or information.  Example: The actor is not expecting this at all, posture is leaning backwards as if flinching at something being thrown at them, rounded mouth or widened eyes.  Shouldn't be angry or disgusted, nor overly happy, but just shocked.")]
	public Sprite surprise;

	[Tooltip("This portrait sprite represents the actor's intense rage at someone or something.  This should be an expression of intense anger and resentment.  Example: Balled fists, leaning forward posture.  Eyebrows sharply angled, actor should look ready to start a serious fist fight.")]
	public Sprite rage;

	[Tooltip("This portrait sprite represents the actor's fear.  This should portray the actor being intensely afraid of something, or grossed out by it.  Example: The actor is very uncomfortable and concerned, or they just had a foul smelling plate of food put in front of them.")]
	public Sprite afraid;

	[Tooltip("This portrait sprite represents the actor's nausea or displeasure with their surroundings. Example: The anime trope with vertical lines and a blue shadow over their eyes.  Hand to mouth as if they're about to throw up perhaps, or just a look of utterly being creeped out.")]
	public Sprite sick;
}
