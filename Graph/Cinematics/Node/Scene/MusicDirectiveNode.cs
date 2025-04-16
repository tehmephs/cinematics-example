using System.Collections;

using Assets.Scripts.Attributes;
using Assets.Scripts.Data;
using Assets.Scripts.Graph.Attributes;
using UnityEngine;

using _CGRAPH = Constants.ScriptableObjects.Graph.Cinematics;

namespace Assets.Scripts.Graph.Cinematics
{
	[CreateNodeMenu(_CGRAPH.Path.Music, _CGRAPH.SortOrder.Music)]
	[NodeTint(_CGRAPH.Tint.Music), NodeWidth(_CGRAPH.Width.Music), NodeDefaultName(_CGRAPH.DefaultName.Music)]
	public class MusicDirectiveNode : SceneDirectiveNode
	{
		[Space(8)]

		[AlwaysExpand]
		public MusicDirectiveParameters parameters;

		private MusicDirector Director => MusicDirector.Instance;

		public override IEnumerator ExecuteDirective(ICinematicContext context)
		{
			switch (this.parameters.paramType)
			{
				case MusicDirectiveParameterType.NewMusic:
					this.Director.PlayMusic(this.parameters.music, this.parameters.index); break;
				case MusicDirectiveParameterType.SkipBlocks:
					this.Director.SkipBlocks(this.parameters.number); break;
				case MusicDirectiveParameterType.SkipToBlockNumber:
					this.Director.SkipToBlockIndex(this.parameters.index); break;
				case MusicDirectiveParameterType.SkipToNextBlock:
					this.Director.SkipToNextBlock(); break;
				case MusicDirectiveParameterType.SkipToNextLoop:
					this.Director.SkipToNextLoop(); break;
				case MusicDirectiveParameterType.PlayThroughBlocks:
					this.Director.PlayThroughBlocks(this.parameters.number); break;
				case MusicDirectiveParameterType.PlayThroughToBlockNumber:
					this.Director.PlayThroughToBlockIndex(this.parameters.index); break;
				case MusicDirectiveParameterType.PlayThroughToNextBlock:
					this.Director.PlayThroughToNextBlock(); break;
				case MusicDirectiveParameterType.PlayThroughToNextLoop:
					this.Director.PlayThroughToNextLoop(); break;
				case MusicDirectiveParameterType.WaitForNextBlock:
					double val = this.Director.TimeRemainingInCurrentClip + this.parameters.offset;
					yield return new WaitForSeconds((float)val);
					break;

			}

			yield return null;
		}
	}
}
