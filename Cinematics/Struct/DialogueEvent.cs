using System;
using Assets.Scripts.Attributes;
using Assets.Scripts.Data;
using UnityEngine;
using UnityEngine.Localization;

[Serializable]
public struct DialogueEvent
{
	private const string _STR_USE_TITLE = "useTitle";

	[Serializable]
	public struct DialogueActorConfig
	{
		[Tooltip("The actor ID to poll for a portrait")]
		public ActorId actor;

		[Tooltip("The portrait expression sprite cell to display.  If the sprite is not defined, no portrait will appear.")]
		public DialogueExpression expression;

		[Tooltip("Darkens the portrait sprite to distinguish that the actor is not talking.")]
		public bool darken;

		public bool IsConfigured => this.actor != null;
	}

	[Serializable]
	public struct DialogueActorConfigurations
	{
		[AlwaysExpand, ShowWhen("IsLeftConfigEnabled", ShowWhenOperator.Method, EnableOrDisable = true)]
		public DialogueActorConfig left;

		[AlwaysExpand, ShowWhen("IsRightConfigEnabled", ShowWhenOperator.Method, EnableOrDisable = true)]
		public DialogueActorConfig right;
	}

	[Header("Generic Configuration"), Space(5)]

	[Tooltip("Tells the dialogue controller to flip the banner and title backdrop.  The banner and title will also come in from the opposite screen side.")]
	public bool reverse;

	[Tooltip("Tells the dialogue controller to use the title banner instead of one of the actor configurations.  When `reverse` is checked, the left actor config is disabled.  Otherwise, the right config is disabled.")]
	public bool useTitle;

	[Tooltip("Tells the dialogue controller to clear the dialogue UI after this event is finished.")]
	public bool exitEvent;

	[Space(5), Header("Speaker Configuration"), Space(5)]

	[Tooltip("Specify the localized string to show the title bar.  If this is empty, the title bar will not appear.")]
	[ShowWhen(_STR_USE_TITLE, true)]
	public LocalizedString title;

	[Space(8)]

	public DialogueActorConfigurations actorConfig;

	[Space(5), Header("Dialogue Text"), Space(5)]

	[Tooltip("Specify the localized string to show as the banner text.")]
	public LocalizedString text;

	public bool IsLeftConfigEnabled() => !this.useTitle || !this.reverse;
	public bool IsRightConfigEnabled() => !this.useTitle || this.reverse;
}