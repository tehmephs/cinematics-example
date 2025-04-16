using System;
using System.Linq;
using System.Reflection;

using Assets.Scripts.Graph.Attributes;
using Assets.Scripts.Util;
using UnityEngine;
using XNode;

using _CPATH = Constants.ScriptableObjects.Path;
using _CSORT = Constants.ScriptableObjects.SortOrder;

namespace Assets.Scripts.Graph.Cinematics
{
	[CreateAssetMenu(fileName = "Cinematics Graph", menuName = _CPATH.Cinematics, order = _CSORT.CinematicGraph)]
	[RequireNode(typeof(EntryDirectiveNode))]
	public class CinematicsGraph : NodeGraph
	{
		private EntryDirectiveNode entryNode;

		[SerializeField, Tooltip("Will display debug logging from directives when this is checked.")]
		private bool enableDebugLogging = false;

		public EntryDirectiveNode EntryNode
		{
			get
			{
				if (this.entryNode == null || this.entryNode.IsFaulted)
				{
					this.entryNode = this.GetRootNode();
				}

				return this.entryNode;
			}
		}

		public bool IsDebugLoggingEnabled => this.enableDebugLogging;

		private void Awake()
		{
			this.entryNode = this.GetRootNode();
		}

		public EntryDirectiveNode GetRootNode()
		{
			foreach (var node in this.nodes)
			{
				if (node is EntryDirectiveNode)
					return node as EntryDirectiveNode;
			}

			Debug.LogWarning($"CinematicsGraph::GetRootNode - Couldn't find entry node in graph `{this.name}`");
			return null;
		}

		public void ResetAllNodeStates()
		{
			foreach (var node in this.nodes.OfType<ActorDirectiveNode>())
			{
				node.ResetState();
			}
		}

		public override Node AddNode(Type type)
		{
			if (type.DerivesFromAny<DirectiveNode, UtilityNode>())
			{
				var newNode = base.AddNode(type);

				var attr = type.GetCustomAttribute<NodeDefaultNameAttribute>();

				if (attr != null)
					newNode.name = attr.DefaultName;

				return newNode;
			}
			else
			{
				Debug.LogError($"CinematicsGraph::AddNode - Only DirectiveNode types can be used in this graph.");
				return null;
			}
		}
	}
}
