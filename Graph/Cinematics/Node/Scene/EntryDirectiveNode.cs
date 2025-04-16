using Assets.Scripts.Graph.Attributes;
using System.Collections;
using _CGRAPH = Constants.ScriptableObjects.Graph.Cinematics;

namespace Assets.Scripts.Graph.Cinematics
{
	[DisallowMultipleNodes, HideNodeInContextMenu]
	[NodeWidth(_CGRAPH.Width.Entry), NodeTint(_CGRAPH.Tint.Entry), NodeDefaultName(_CGRAPH.DefaultName.Entry)]
	public sealed class EntryDirectiveNode : DirectiveNode
	{
		[Output(ShowBackingValue.Never, ConnectionType.Override)]
		public SceneDirectiveNode to;

		public bool IsFaulted => this.Next == null;

		public override DirectiveNode Previous => null;
		public override DirectiveNode Next => this.GetNextNodeByField(nameof(this.to));

		public override IEnumerator ExecuteDirective(ICinematicContext context)
		{
			context.LogDebug(this, "Cinematic graph starting...");
			yield return null;
		}
	}
}
