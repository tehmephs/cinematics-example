using System;

using UnityEngine;

namespace Assets.Scripts.Attributes
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true)]
	public class EnumArrayTitleAttribute : PropertyAttribute
	{
		public string Property { get; private set; }

		public EnumArrayTitleAttribute(string property)
		{
			this.Property = property;
		}
	}
}