using System.Collections;
using Assets.Scripts.Attributes;
using Assets.Scripts.Graph.Attributes;
using _CGRAPH = Constants.ScriptableObjects.Graph.Cinematics;

namespace Assets.Scripts.Graph.Cinematics
{
	[CreateNodeMenu(_CGRAPH.Path.Dialogue, _CGRAPH.SortOrder.Dialogue)]
	[NodeTint(_CGRAPH.Tint.Dialogue), NodeWidth(_CGRAPH.Width.Dialogue), NodeDefaultName(_CGRAPH.DefaultName.Dialogue)]
	public class DialogueDirectiveNode : SceneDirectiveNode
	{
		[AlwaysExpand]
		public DialogueEvent dialogue;

		public override IEnumerator ExecuteDirective(ICinematicContext context)
			=> UXManager.Instance.RunDialogue(this.dialogue, context);
	}
}
