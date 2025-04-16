using Assets.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Util;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor
{
	[CustomPropertyDrawer(typeof(ActorAnimationList))]
	public class ActorAnimationListPropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var optionsProp = property.FindPropertyRelative(nameof(ActorAnimationList.animations));
			var selectedProp = property.FindPropertyRelative(nameof(ActorAnimationList.selectedAnimation));

			var orientationProp = property.FindPropertyRelative(nameof(ActorAnimationList.orientation));
			var boolProp = property.FindPropertyRelative(nameof(ActorAnimationList.on));

			var options = new List<ActorAnimation>();
			for (int i = 0; i < optionsProp.arraySize; i++)
			{
				options.Add(optionsProp.GetArrayElementAtIndex(i).objectReferenceValue as ActorAnimation);
			}

			string[] names = options.Select(o => o?.name)
									.WithoutNulls()
									.ToArray();

			int[] instanceIds = options.Select(o => o?.GetInstanceID())
									   .WithoutNulls()
									   .Cast<int>()
									   .ToArray();

			int currentIndex = Array.IndexOf(instanceIds, selectedProp.objectReferenceValue?.GetInstanceID());
			if (currentIndex < 0)
				currentIndex = 0;

			int selectedIndex = EditorGUILayout.Popup(new GUIContent("Animation"), currentIndex, names);
			if (selectedIndex >= 0 && selectedIndex < options.Count)
			{
				selectedProp.objectReferenceValue = EditorUtility.InstanceIDToObject(instanceIds[selectedIndex]);
				ActorAnimation selectedAnimation = options[selectedIndex];

				if (selectedAnimation.isDirectional)
				{
					EditorGUILayout.PropertyField(orientationProp);
				}

				if (selectedAnimation.parameterType == AnimatorControllerParameterType.Bool)
				{
					EditorGUILayout.PropertyField(boolProp);
				}
			}

			// property.serializedObject.ApplyModifiedProperties();
		}
	}
}
