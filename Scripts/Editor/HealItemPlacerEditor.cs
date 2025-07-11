using EFK2.HealthSystem;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace EFK2.Edit
{
	[CustomEditor(typeof(HealItemPlacer))]
	public class HealItemPlacerEditor : Editor
	{
		private const string OccupiedPointsName = "_occupiedPoints";

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			DrawDictionary();
		}

		private void DrawDictionary()
		{
			HealItemPlacer healItemPlacer = target as HealItemPlacer;

			Type type = healItemPlacer.GetType();

			FieldInfo fieldInfo = type.GetField(OccupiedPointsName, BindingFlags.NonPublic | BindingFlags.Instance);

			Dictionary<Transform, HealthItemCollectable> occupiedPoints = (Dictionary<Transform, HealthItemCollectable>)fieldInfo.GetValue(healItemPlacer);

			if (occupiedPoints != null)
			{
				foreach (var entry in occupiedPoints)
				{
					EditorGUILayout.LabelField(entry.Key.ToString(), entry.Value is null ? "null" : entry.Value.ToString());
				}
			}
		}
	}
}
#endif