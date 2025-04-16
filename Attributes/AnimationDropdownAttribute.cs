using System;
using UnityEngine;

namespace Assets.Scripts.Attributes
{
	/// <summary>
	/// For use with struct fields, this will ensure the property drawer is always expanded, and will disable the default foldout behavior.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, Inherited = true)]
	public class AnimationDropdownAttribute : PropertyAttribute
	{
		public AnimationDropdownAttribute() { }
	}
}