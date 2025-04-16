using System.Collections;

using Assets.Scripts.Graph.Attributes;
using UnityEngine;

using _CGRAPH = Constants.ScriptableObjects.Graph.Cinematics;

namespace Assets.Scripts.Graph.Cinematics
{
	[CreateNodeMenu(_CGRAPH.Path.Activate, _CGRAPH.SortOrder.Activate)]
	[NodeTint(_CGRAPH.Tint.Activate), NodeWidth(_CGRAPH.Width.Activate), NodeDefaultName(_CGRAPH.DefaultName.Activate)]
	public class ActivateDirectiveNode : ActorDirectiveNode
	{
		[Space(5)]

		[Tooltip("The child object path to follow to a game object which will be activated or deactivated.  If this is left blank, the `_Visibility` property on it's material shader will be changed instead (if the main parent gets deactivated everything stops working, so it's only aesthetic at the root level)")]
		public string path;

		[Tooltip("When checked, the game object will be set to active, otherwise it will be set to inactive.")]
		public bool activate;

		protected override IEnumerator OnExecuteDirective(IActor subject, ICinematicContext context)
		{
			if (!string.IsNullOrEmpty(this.path))
			{
				var target = subject.transform.Find(this.path);
				if (target != null)
				{
					target.gameObject.SetActive(this.activate);
				}
				else
				{
					Debug.LogWarning($"ActivateDirection::Execute - directive failed to find a child object at `{this.name}`.  The activate directive was discarded.");
				}
			}
			else
			{
				subject.SetVisible(this.activate);
			}

			yield return null;
		}
	}
}
