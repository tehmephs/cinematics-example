using System;

namespace Assets.Scripts.Graph.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class NodeDefaultNameAttribute : Attribute
	{
		public string DefaultName { get; private set; }

		public NodeDefaultNameAttribute(string defaultName)
		{
			this.DefaultName = defaultName;
		}
	}
}
