using Assets.Scripts.Graph.Attributes;
using UnityEngine;
using _CGRAPH = Constants.ScriptableObjects.Graph.Common;

namespace Assets.Scripts.Graph
{
	[CreateNodeMenu(_CGRAPH.Comment_Path, _CGRAPH.Comment_SortOrder)]
	[NodeTint(_CGRAPH.Comment_Tint), NodeDefaultName(_CGRAPH.Comment_DefaultName)]
	public class CommentNode : UtilityNode
	{
		[HideInInspector]
		[Min(200)]
		public float width = 200;

		[HideInInspector, ColorUsage(showAlpha: false)]
		public Color commentColor = new Color(0.2f, 0.2f, 0.2f);

		[HideInInspector, ColorUsage(showAlpha: false)]
		public Color textColor = Color.white;

		[HideInInspector]
		public string comment = "Double click to change comment and size...";
	}
}
