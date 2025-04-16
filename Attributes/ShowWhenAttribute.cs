using System;
using UnityEngine;

namespace Assets.Scripts.Attributes
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
	public class ShowWhenAttribute : PropertyAttribute
	{
		public string FieldName = string.Empty;

		/// <summary>
		/// Instructs the property drawer to make the field readonly or editable instead of show/hide
		/// </summary>
		public bool EnableOrDisable = false;

		public ShowWhenOperator Operator;

		public object[] Values;

		/// <summary>
		/// Will configure a field to show or hide itself relative to other field's realtime values in the same struct or class.
		/// </summary>
		/// <param name="field">The name of the field to evaluate</param>
		/// <param name="values">The bool or enum values that will show the property if the target field matches any of the specified values</param>
		public ShowWhenAttribute(string field, params object[] values) : this(field, ShowWhenOperator.Any, values) { }

		/// <summary>
		/// An expanded field configuration that allows for evaluating a field in advanced ways
		/// </summary>
		/// <param name="field">The name of the field to evaluate, or if using `Method` operation, the name of the method to perform the evaluation conditions.</param>
		/// <param name="oper">The operation to evaluate with.  Any will check if any value matches one of the given values.  None will show the field when it doesn't match any of the given values.  Flag will do a logical AND to see if any of the given flags are in the flag value.  Method will run the named method to evaluate if the field should show or hide.</param>
		/// <param name="values">The values to match against the given operator.</param>
		public ShowWhenAttribute(string field, ShowWhenOperator oper, params object[] values)
		{
			this.FieldName = field;
			this.Values = values;
			this.Operator = oper;
		}
	}
}