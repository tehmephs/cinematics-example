using System;

using Assets.Scripts.Attributes;
using UnityEngine;

using MDPT = Assets.Scripts.Data.MusicDirectiveParameterType;

namespace Assets.Scripts.Data
{
	[Serializable]
	public struct MusicDirectiveParameters
	{
		private const string _STR_PARAM_TYPE = "paramType";

		[Tooltip("The type of action to take regarding the music.  Skip types will immediately jump blocks.  Play through will play each block from start to finish until it reaches the destination block and then will operate automatically from there.")]
		public MDPT paramType;

		[Tooltip("Sets the new music script to play.")]
		[ShowWhen(_STR_PARAM_TYPE, MDPT.NewMusic)]
		public Music music;

		[Tooltip("Specifies a block index to skip to or play through to.  When used with a new music track, determines which block index to start on.")]
		[ShowWhen(_STR_PARAM_TYPE, MDPT.NewMusic, MDPT.SkipToBlockNumber, MDPT.PlayThroughToBlockNumber)]
		[Min(0)]
		public int index;

		[Tooltip("Specifies a number of blocks to skip or play through")]
		[ShowWhen(_STR_PARAM_TYPE, MDPT.SkipBlocks, MDPT.PlayThroughBlocks)]
		[Min(1)]
		public int number;

		[Tooltip("Offsets the wait time.")]
		[ShowWhen(_STR_PARAM_TYPE, MDPT.WaitForNextBlock)]
		public float offset;
	}
}
