using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Linq;
using PhysicsSettings = Reallusion.Import.WeightMapper.PhysicsSettings;
using ColliderSettings = Reallusion.Import.ColliderManager.ColliderSettings;

namespace Reallusion.Import
{
	[CustomEditor(typeof(WeightMapper))]
	public class WeightMapperEditor : Editor
	{
		private WeightMapper weightMapper;
		private ColliderManager colliderManager;
		
		const float LABEL_WIDTH = 80f;
		const float GUTTER = 40f;
		const float BUTTON_WIDTH = 160f;

		private void OnEnable()
		{
			// Method 1
			weightMapper = (WeightMapper)target;
			colliderManager = weightMapper.GetComponentInParent<ColliderManager>();					
		}

		public override void OnInspectorGUI()
		{
			// Draw default inspector after button...
			base.OnInspectorGUI();

			OnClothInspectorGUI();
		}

		public void OnClothInspectorGUI()
		{
			Color background = GUI.backgroundColor;

			GUILayout.Space(10f);

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUI.backgroundColor = Color.Lerp(background, Color.white, 0.25f);
			if (GUILayout.Button("Rebuild Constraints", GUILayout.Width(BUTTON_WIDTH)))
			{
				weightMapper.ApplyWeightMap(false);
			}
			GUI.backgroundColor = background;
			GUILayout.FlexibleSpace();
			if (!EditorApplication.isPlaying)
			{
				EditorGUI.BeginDisabledGroup(!PhysicsSettingsStore.TryFindSettingsObject(out string foundSettingsGuid));
				GUI.backgroundColor = Color.Lerp(background, Color.yellow, 0.25f);
				if (GUILayout.Button("Recall Settings", GUILayout.Width(BUTTON_WIDTH)))
				{
					PhysicsSettingsStore.RecallClothSettings(weightMapper);
				}
				GUI.backgroundColor = background;
				EditorGUI.EndDisabledGroup();
			}
			else
			{
				GUI.backgroundColor = Color.Lerp(background, Color.red, 0.25f);
				if (GUILayout.Button("Save Settings", GUILayout.Width(BUTTON_WIDTH)))
				{
					PhysicsSettingsStore.SaveClothSettings(weightMapper);
				}
				GUI.backgroundColor = background;
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			GUILayout.Space(10f);

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUI.backgroundColor = Color.Lerp(background, Color.cyan, 0.25f);
			if (GUILayout.Button("Apply to Prefab", GUILayout.Width(BUTTON_WIDTH)))
			{
				UpdatePrefab(weightMapper);
			}
			GUILayout.FlexibleSpace(); 
			GUI.backgroundColor = Color.Lerp(background, Color.green, 0.25f);
			if (GUILayout.Button("Collider Manager", GUILayout.Width(BUTTON_WIDTH)))
			{
				Selection.activeObject = colliderManager;
			}
			GUI.backgroundColor = background;
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

		public void UpdatePrefab(Object component)
		{
			GameObject prefabRoot = PrefabUtility.GetOutermostPrefabInstanceRoot(component);
			if (prefabRoot)
			{
				// save prefab asset
				PrefabUtility.ApplyPrefabInstance(prefabRoot, InteractionMode.UserAction);
			}
		}
	}
}





