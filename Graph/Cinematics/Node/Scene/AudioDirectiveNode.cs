using System;
using System.Collections;

using Assets.Scripts.Attributes;
using Assets.Scripts.Data;
using Assets.Scripts.Graph.Attributes;
using UnityEngine;

using _CGRAPH = Constants.ScriptableObjects.Graph.Cinematics;

namespace Assets.Scripts.Graph.Cinematics
{
	[CreateNodeMenu(_CGRAPH.Path.Audio, _CGRAPH.SortOrder.Audio)]
	[NodeTint(_CGRAPH.Tint.Audio), NodeWidth(_CGRAPH.Width.Audio), NodeDefaultName(_CGRAPH.DefaultName.Audio)]
	public class AudioDirectiveNode : SceneDirectiveNode
	{
		private const string _STR_FOLEY = "isFoley";

		[Space(5)]

		[Tooltip("If checked, will expect a Foley script instead of just an individual audio clip.")]
		public bool isFoley;

		[Tooltip("Plays the foley script at the given position.")]
		[ShowWhen(_STR_FOLEY, true)]
		public Foley foley;

		[Tooltip("Sets the event time for the foley script.")]
		[ShowWhen(_STR_FOLEY, true)]
		[Min(0)]
		public float eventTime;

		[Tooltip("Plays the given audio clip at the target position.")]
		[ShowWhen(_STR_FOLEY, false)]
		public AudioClip clip;

		[Tooltip("Tells the audio clip to start at the given offset")]
		[ShowWhen(_STR_FOLEY, false)]
		[Min(0)]
		public float offset;

		[Tooltip("Adjusts the relative output volume of the audio.")]
		[ShowWhen(_STR_FOLEY, false)]
		[Range(0, 1)]
		public float volume = 1;

		[AlwaysExpand]
		public DirectiveTarget target;

		private void OnValidate()
		{
			if (this.target.targetType == CinematicActionTargetType.Vector && this.target.relative)
			{
				// TODO: Add some kind of warning symbol to faulted nodes?
				Debug.LogWarning($"AudioDirectiveNode::OnValidate - Avoid usage of the `relative` option with Vector targets on non-actor nodes.  Because no actor is specified, the position becomes relative to Vector3.zero.");
			}
		}

		public override IEnumerator ExecuteDirective(ICinematicContext context)
		{
			var position = this.target.GetPosition(null, context);

			if (this.isFoley)
			{
				context.LogDebug(this, $"Playing foley script `{this.foley.name}` with event time {this.eventTime} at position {position}");
				AudioDirector.Instance.PlayFoley(this.foley, position, this.eventTime);
			}
			else
			{
				context.LogDebug(this, $"Playing audio clip `{this.clip.name}` with offset {this.offset} and volume setting {this.volume} at position {position}");
				AudioDirector.Instance.PlayAudioAtPosition(this.clip, position, this.volume, this.offset);
			}

			yield return null;
		}
	}
}
