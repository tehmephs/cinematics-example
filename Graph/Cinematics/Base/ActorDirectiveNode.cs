using System.Collections;
using System.Linq;

using Assets.Scripts.Data;
using Assets.Scripts.Util;

using UnityEngine;
using XNode;

namespace Assets.Scripts.Graph.Cinematics
{
	public abstract class ActorDirectiveNode : DirectiveNode
	{
		[Input(ShowBackingValue.Never, ConnectionType.Override)]
		public ActorDirectiveNode from;

		[Output(ShowBackingValue.Never, ConnectionType.Override)]
		[Space(_F_PORT_HEIGHT)]
		public ActorDirectiveNode to;

		public override bool IsActorNode => true;
		public override DirectiveNode Next => this.GetNextNodeByField(nameof(this.to));
		public override DirectiveNode Previous => this.GetPreviousNodeByField(nameof(this.from));

		public bool ActionIsComplete { get; private set; }

		protected abstract IEnumerator OnExecuteDirective(IActor subject, ICinematicContext context);

		public virtual void UpdateActor(ActorId actor) => this.GetNext<ActorDirectiveNode>()?.UpdateActor(actor);

		public void ResetState() => this.ActionIsComplete = false;

		public IEnumerator ExecuteDirective(IActor subject, ICinematicContext context)
		{
			yield return this.OnExecuteDirective(subject, context);
			this.ActionIsComplete = true;
		}

		public override sealed IEnumerator ExecuteDirective(ICinematicContext context)
		{
			Debug.LogWarning($"ActorDirectiveNode::ExecuteDirective - Do not use the standard execution method for actor directives.  Use the method signature which accepts an IActor as the subject instead.");
			yield return null;
		}

		protected override void Init()
		{
			base.Init();
			this.UpdateActorConnection();
		}

		public override void OnCreateConnection(NodePort from, NodePort to)
		{
			if (!from.ValueType.DerivesFromAny<ActorId, DirectiveNode>() || from.ValueType.DerivesFrom<SceneDirectiveNode>())
			{
				this.from.ClearConnections();
			}
			else
			{
				base.OnCreateConnection(from, to);
				this.UpdateActorConnection();
			}
		}

		public override void OnRemoveConnection(NodePort port)
		{
			base.OnRemoveConnection(port);
			this.UpdateActorConnection();
		}

		public void UpdateActorConnection()
		{
			var actor = this.GetConnectedActor();
			this.UpdateActor(actor);
		}

		protected ActorId GetConnectedActor()
		{
			var previousConnection = this.GetInputPort(nameof(this.from))
										 .GetConnections()
										 .FirstOrDefault();

			if (previousConnection != null)
			{
				//this.GetInputValue<ActorId>(nameof(this.from));

				if (previousConnection.ValueType.Equals(typeof(ActorDirectiveNode)))
				{
					return (previousConnection.node as ActorDirectiveNode).GetConnectedActor();
				}
				else if (previousConnection.node.GetType().DerivesFrom<IDynamicActorList>())
				{
					int index = 0;

					IDynamicActorList node = previousConnection.node as IDynamicActorList;

					foreach (var output in previousConnection.node.DynamicOutputs)
					{
						if (output.Equals(previousConnection))
						{
							var actor = node.ActorList[index];
							return actor;
						}

						index++;
					}
				}
			}

			return null;
		}
	}
}
