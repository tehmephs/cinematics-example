using System;

namespace Assets.Scripts.Graph.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class HideNodeInContextMenuAttribute : Attribute
	{
		public HideNodeInContextMenuAttribute()
		{
		}
	}
}
