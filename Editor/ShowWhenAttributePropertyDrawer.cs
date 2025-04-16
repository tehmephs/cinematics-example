using System.Linq;

using Assets.Scripts.Attributes;
using Assets.Scripts.Util;
using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace Assets.Scripts.Editor
{
	[CustomPropertyDrawer(typeof(ShowWhenAttribute))]
	public class ShowWhenAttributePropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			ShowWhenAttribute condHAtt = (ShowWhenAttribute)attribute;
			bool show = GetAttributeResult(condHAtt, property);

			if (show)
			{
				EditorGUI.PropertyField(position, property, label, true);
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			ShowWhenAttribute condHAtt = (ShowWhenAttribute)attribute;
			bool show = GetAttributeResult(condHAtt, property);

			if (show)
			{
				return EditorGUI.GetPropertyHeight(property, label);
			}
			else
			{
				return -EditorGUIUtility.standardVerticalSpacing;
			}
		}

		private bool GetAttributeResult(ShowWhenAttribute attr, SerializedProperty property)
		{
			bool show = true;

			if (attr.Operator == ShowWhenOperator.Method)
			{
				object methodTarget = null;
				MethodInfo targetMethodInfo = null;

				string[] elements = property.propertyPath.Split(".");

				if (elements.Length > 1)
				{
					string parentPath = string.Join(".", elements.Take(elements.Length - 1));
					SerializedProperty parent = property.serializedObject.FindProperty(parentPath);
					targetMethodInfo = parent.boxedValue
											 .GetType()
											 .GetMethod(attr.FieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

					methodTarget = parent.boxedValue;
				}
				else
				{
					targetMethodInfo = property.serializedObject
											   .targetObject
											   .GetType()
											   .GetMethod(attr.FieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
					methodTarget = property.serializedObject.targetObject;
				}

				if (targetMethodInfo != null)
				{
					show = (bool)targetMethodInfo.Invoke(methodTarget, null);
				}
				else
				{
					Debug.LogWarning($"ShowWhenAttributePropertyDrawer::GetAttributeResult - Could not find a method at {attr.FieldName}.  The result was false");
				}
			}
			else
			{
				string propertyPath = property.propertyPath;

				var propertyPathArr = propertyPath.Split('.');
				string lastElement = propertyPathArr.Last().Replace(property.name, attr.FieldName);
				string conditionPath = string.Join('.', propertyPathArr.Take(propertyPathArr.Length - 1).Include(lastElement));
				SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

				if (sourcePropertyValue != null)
				{
					SerializedPropertyType propertyType = sourcePropertyValue.propertyType;

					return this.GetAttributeResult(attr, sourcePropertyValue, propertyType);
				}
				else
				{
					Debug.LogWarning($"ShowWhenAttributePropertyDrawer::GetAttributeResult - Attempting to use a ConditionalHideAttribute but no matching SourcePropertyValue found in object: {attr.FieldName}");
				}
			}

			return show;
		}

		private bool GetAttributeResult(ShowWhenAttribute attr, SerializedProperty propertyValue, SerializedPropertyType type)
			=> type switch
			{
				SerializedPropertyType.Enum => this.GetEnumResult(attr, propertyValue),
				SerializedPropertyType.Boolean => this.GetBoolResult(attr, propertyValue),
				_ => throw new System.Exception($"ShowWhenPropertyDrawer::GetAttributeResult - No property evaluation available for type {type}")
			};

		private bool GetBoolResult(ShowWhenAttribute attr, SerializedProperty propertyValue) =>
			attr.Operator switch
			{
				ShowWhenOperator.Any => attr.Values.Any(val => propertyValue.boolValue == (bool)val),
				ShowWhenOperator.None => attr.Values.All(val => propertyValue.boolValue != (bool)val),
				ShowWhenOperator.Flag => attr.Values.All(val => (propertyValue.boolValue & (bool)val) == (bool)val),
				_ => false
			};

		private bool GetEnumResult(ShowWhenAttribute attr, SerializedProperty propertyValue)
			=> attr.Operator switch
			{
				ShowWhenOperator.Any => attr.Values.Any(val => propertyValue.intValue == (int)val),
				ShowWhenOperator.None => attr.Values.All(val => propertyValue.intValue != (int)val),
				ShowWhenOperator.Flag => attr.Values.All(val => (propertyValue.enumValueFlag & (int)val) == (int)val),
				_ => false
			};
	}
}