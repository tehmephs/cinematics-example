using System;
using UnityEngine;

namespace Assets.Scripts.Attributes
{
	/// <summary>
	/// For use with scriptable object properties, this will consume the double-click event that would normally open the scriptable object asset.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, Inherited = true)]
	public class DisableDoubleClickAttribute : PropertyAttribute
	{
		public DisableDoubleClickAttribute() { }
	}
}