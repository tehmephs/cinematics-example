using System;
using System.Collections;
using System.Linq;
using XNode;

namespace Assets.Scripts.Graph.Cinematics
{
	[Serializable]
	public abstract class DirectiveNode : Node, INamedEntity
	{
		protected const float _F_PORT_HEIGHT = -18;

		public abstract DirectiveNode Previous { get; }
		public abstract DirectiveNode Next { get; }

		public virtual bool IsActorNode => false;

		public virtual IEnumerator ExecuteDirective(ICinematicContext context)
		{
			yield return null;
		}

		protected DirectiveNode GetConnectedNode(NodePort port)
		{
			var connection = port?.GetConnections()?.FirstOrDefault();
			return connection?.node as DirectiveNode;
		}

		protected DirectiveNode[] GetConnectedNodes(NodePort port)
			=> port.GetConnections()
				   .Select(con => (DirectiveNode)con.node)
				   .ToArray();

		protected TNode GetPrevious<TNode>()
			where TNode : DirectiveNode
				=> this.Previous is TNode ? (TNode)this.Previous : null;

		protected TNode GetNext<TNode>()
			where TNode : DirectiveNode
				=> this.Next is TNode ? (TNode)this.Next : null;

		protected DirectiveNode GetPreviousNodeByField(string fieldName) => this.GetConnectedNode(this.GetInputPort(fieldName));
		protected DirectiveNode[] GetPreviousNodesByField(string fieldName) => this.GetConnectedNodes(this.GetInputPort(fieldName));
		protected DirectiveNode GetNextNodeByField(string fieldName) => this.GetConnectedNode(this.GetOutputPort(fieldName));
		protected DirectiveNode[] GetNextNodesByField(string fieldName) => this.GetConnectedNodes(this.GetOutputPort(fieldName));

		protected TNode GetPreviousNodeByField<TNode>(string fieldName)
			where TNode : DirectiveNode
				=> (TNode)this.GetPreviousNodeByField(fieldName);

		protected TNode[] GetPreviousNodesbyField<TNode>(string fieldName)
			where TNode : DirectiveNode
				=> this.GetPreviousNodesByField(fieldName).Cast<TNode>().ToArray();

		protected TNode GetNextNodeByField<TNode>(string fieldName)
			where TNode : DirectiveNode
				=> (TNode)this.GetNextNodeByField(fieldName);

		protected TNode[] GetNextNodesByField<TNode>(string fieldName)
			where TNode : DirectiveNode
				=> this.GetNextNodesByField(fieldName).Cast<TNode>().ToArray();

		protected TNode GetConnectedNode<TNode>(NodePort port)
			where TNode : DirectiveNode
				=> (TNode)this.GetConnectedNode(port);
	}
}
