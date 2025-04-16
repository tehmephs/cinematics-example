using UnityEngine;

namespace Assets.Scripts.Graph.Cinematics
{
	public abstract class SceneDirectiveNode : DirectiveNode
	{
		[Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.InheritedAny)]
		public SceneDirectiveNode from;

		[Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.InheritedAny)]
		[Space(_F_PORT_HEIGHT)]
		public SceneDirectiveNode to;

		public override DirectiveNode Next => this.GetNextNodeByField(nameof(this.to));
		public override DirectiveNode Previous => this.GetPreviousNodeByField(nameof(this.from));
	}
}
