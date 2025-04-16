using System;
using System.Collections.Generic;
using Assets.Scripts.Graph.Attributes;
using Assets.Scripts.Graph.Cinematics;
using Assets.Scripts.Util;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace Assets.Scripts.Editor.Graph
{
	[CustomNodeGraphEditor(typeof(CinematicsGraph))]
	public class CinematicGraphEditor : NodeGraphEditor
	{
		private const float _F_DRILLDOWN_PAD = 4f;
		private const float _F_DRILLDOWN_CRUMB = 10 + _F_DRILLDOWN_PAD;

		private static readonly List<int> _graphTrail = new List<int>();
		private static bool _isDrillingDown = false;

		#region Overrides

		public override void OnGUI()
		{
			this.DrawBreadcrumbs();
			base.OnGUI();
		}

		public override string GetPortTooltip(NodePort port)
		{
			return null;
		}

		public override string GetNodeMenuName(Type type)
		{
			if (!type.HasCustomAttribute<HideNodeInContextMenuAttribute>())
			{
				return base.GetNodeMenuName(type);
			}

			return null;
		}

		public override void OnOpen()
		{
			if (!_isDrillingDown)
			{
				_graphTrail.Clear();
			}

			_isDrillingDown = false;
			base.OnOpen();

			var graph = NodeEditorWindow.current.graph as CinematicsGraph;
			NodeEditorWindow.current.titleContent = new GUIContent(graph.name);
		}

		#endregion

		/// <summary>
		/// Tells the graph editor that the user is drilling down into a sub-cinematic (macro) and to log the breadcrumb trail step
		/// </summary>
		/// <param name="graphToOpen">The sub-cinematic graph to open</param>
		public static void Drilldown(CinematicsGraph graphToOpen)
		{
			_isDrillingDown = true;

			var comingFromGraph = NodeEditorWindow.current.graph as CinematicsGraph;
			int sourceInstanceId = comingFromGraph.GetInstanceID();
			int targetInstanceId = graphToOpen.GetInstanceID();

			if (!_graphTrail.Contains(targetInstanceId))
			{
				if (!_graphTrail.Contains(sourceInstanceId))
				{
					_graphTrail.Add(sourceInstanceId);
				}

				NodeEditorWindow.Open(graphToOpen);
			}
			else
			{
				Drillup(graphToOpen);
			}
		}

		/// <summary>
		/// Tells the graph editor to return to a graph that's in the breadcrumb trail currently, removing any subsequent trail graphs depending on the order in which the drilldowns happen
		/// This will also be called if the user attempts to drilldown to a macro that is already part of the current breadcumb trail and otherwise would become an endless loop.
		/// </summary>
		/// <param name="graphToOpen">The target graph to open</param>
		/// <param name="index">The index at which it sits in the current breadcrumb trail</param>
		public static void Drillup(CinematicsGraph graphToOpen)
		{
			int targetInstanceId = graphToOpen.GetInstanceID();

			if (_graphTrail.Contains(targetInstanceId))
			{
				_isDrillingDown = true;

				var targetIndex = _graphTrail.IndexOf(targetInstanceId);
				_graphTrail.RemoveRange(targetIndex, _graphTrail.Count - targetIndex);
				NodeEditorWindow.Open(graphToOpen);
			}
			else
			{
				Debug.LogWarning($"CinematicGraphEditor::Drillup - called on a graph that was not already in the breadcrumb list.  Nothing happened.");
			}

		}

		private void DrawBreadcrumbs()
		{
			if (_graphTrail.Count == 0)
				return;

			GUILayout.BeginArea(new Rect(10, 10, 1000, 30));
			GUILayout.BeginHorizontal();

			for (int i = 0; i < _graphTrail.Count; i++)
			{
				var graph = EditorUtility.InstanceIDToObject(_graphTrail[i]) as CinematicsGraph;

				Rect buttonRect = GUILayoutUtility.GetRect(new GUIContent(graph.name), EditorStyles.linkLabel)
												  .PadRect(_F_DRILLDOWN_PAD);


				EditorGUIUtility.AddCursorRect(buttonRect, MouseCursor.Link);

				if (GUI.Button(buttonRect, graph.name, EditorStyles.toolbarButton))
				{
					Drillup(graph);
					break;
				}

				if (i < _graphTrail.Count - 1)
				{
					GUILayout.Space(_F_DRILLDOWN_PAD * 2);
					GUILayout.Label(">", GUILayout.Width(_F_DRILLDOWN_CRUMB));
				}
			}

			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}

	}
}
