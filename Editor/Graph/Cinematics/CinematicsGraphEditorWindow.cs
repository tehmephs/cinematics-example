//using System.Collections.Generic;

//using Assets.Scripts.Graph.Cinematics;
//using UnityEngine;
//using XNodeEditor;

//namespace Assets.Scripts.Editor.Graph
//{
//	public class CinematicsGraphEditorWindow : NodeEditorWindow
//	{
//		private int selectedTabIndex = 0;
//		private List<string> tabs = new List<string>();
//		private List<CinematicsGraph> openGraphs = new List<CinematicsGraph>();

//		/// <summary>
//		/// Opens a cinematics graph window
//		/// </summary>
//		/// <param name="graph">The graph to open</param>
//		public static void Open(CinematicsGraph graph)
//		{
//			var window = GetWindow<CinematicsGraphEditorWindow>();
//			window.titleContent = new GUIContent(graph.name);

//			if (window.graph != graph)
//			{
//				window.LoadGraph(graph);
//			}

//			window.Show();
//			window.graph = graph;
//		}

//		/// <summary>
//		/// Adds an additional tab to the current editor window with another graph view.
//		/// </summary>
//		/// <param name="graph">The graph to open a tab for</param>
//		public static void OpenTab(CinematicsGraph graph)
//		{
//			var window = GetWindow<CinematicsGraphEditorWindow>();

//			if (!window.openGraphs.Contains(graph))
//			{
//				window.openGraphs.Add(graph);
//				window.titleContent = new GUIContent(graph.name);
//				window.Show();
//			}
//			else
//			{
//				Debug.LogWarning($"CinematicsGraphEditorWindow::OpenTab - Cannot have more than one tab open for the same graph `{graph.name}`.");
//			}
//		}

//		public void LoadGraph(CinematicsGraph graph)
//		{
//			if (graph == null)
//			{
//				Debug.LogWarning("Tried to load a null CinematicsGraph.");
//				return;
//			}

//			// Tell xNode this is now the active graph and window
//			NodeEditorWindow.current = this;
//			this.graph = graph;

//			// Optional: Set title based on graph name
//			this.titleContent = new GUIContent($"Cinematics: {graph.name}");

//			// this.Repaint();
//			// Optional: Force the editor to repaint and center the view, etc.
//			// NodeEditor.RepaintClients();
//			NodeEditorWindow.current.Home(); // This centers the view on the graph
//		}


//		protected override void OnGUI()
//		{
//			this.selectedTabIndex = GUILayout.Toolbar(this.selectedTabIndex, this.tabs.ToArray());

//			if (this.openGraphs != null && this.selectedTabIndex < this.openGraphs.Count)
//			{
//				this.graph = this.openGraphs[this.selectedTabIndex];
//				base.OnGUI();
//			}
//		}
//	}
//}
