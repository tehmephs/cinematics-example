using System.Collections;
using Assets.Scripts.Graph.Attributes;
using UnityEngine;

using _CGRAPH = Constants.ScriptableObjects.Graph.Cinematics;

namespace Assets.Scripts.Graph.Cinematics
{
	// TODO : Add custom editor for drill downs
	[CreateNodeMenu(_CGRAPH.Path.Macro, _CGRAPH.SortOrder.Macro)]
	[NodeTint(_CGRAPH.Tint.Macro), NodeWidth(_CGRAPH.Width.Macro), NodeDefaultName(_CGRAPH.DefaultName.Macro)]
	public class MacroDirectiveNode : SceneDirectiveNode
	{
		[Space(8)]

		[Tooltip("Runs a sub-cinematic macro.  Double click on the node to drill down to make changes to the macro graph.  A UI element at the top of the graph will bring you back to the parent cinematic.")]
		public CinematicsGraph macro;

		public override IEnumerator ExecuteDirective(ICinematicContext context)
		{
			var subContext = context.Clone(this.macro);

			context.LogDebug(this, $"Macro `{this.macro.name}` starting, cloned a sub context for the new graph.");
			yield return this.DoExecuteDirective(this.macro.EntryNode, subContext);
		}

		private IEnumerator DoExecuteDirective(DirectiveNode node, ICinematicContext context)
		{
			if (node != null)
			{
				yield return node.ExecuteDirective(context);
				yield return this.DoExecuteDirective(node.Next, context);
			}
			else
			{
				context.LogDebug(this, $"Macro `{this.macro.name}` ended, flushing sub context.");
				context.Flush();
				context = null;
			}
		}
	}
}
