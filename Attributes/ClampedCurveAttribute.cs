using System;
using UnityEngine;

namespace Assets.Scripts.Attributes
{
	/// <summary>
	/// By default clamps an AnimationCurve property on both the X and Y axis representing time and value as a 0-1 function in both directions.
	/// The width and height clamps of the rectangle can be overriden through the alternate constructor
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, Inherited = true)]
	public class ClampedCurveAttribute : PropertyAttribute
	{
		public Rect bounds;
		public int height;

		/// <summary>
		/// Clamps the AnimationCurve editor on both axises using 0-1 representation
		/// </summary>
		/// <param name="height">Adjusts the line height for the property drawer</param>
		public ClampedCurveAttribute(int height = 1) : this(1, 1, height) { }

		/// <summary>
		/// Sets a specific rectangle width and height for the curve chart.
		/// </summary>
		/// <param name="x">The width scale of the curve chart</param>
		/// <param name="y">The height scale of the curve chart</param>
		/// <param name="height">The line height value</param>
		public ClampedCurveAttribute(float x, float y, int height = 1)
		{
			this.bounds = new Rect(0, 0, x, y);
			this.height = height;
		}
	}
}