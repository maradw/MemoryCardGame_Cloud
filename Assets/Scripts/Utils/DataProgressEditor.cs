using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class DataProgressEditor : EditorWindow
{
    
    public ProgressData progressData;
    [MenuItem("Window/Progress Data Editor")]
    static void Init() 
    {
        EditorWindow.GetWindow(typeof(DataProgressEditor)).Show();
    }
	private void OnGUI()
	{
		if (progressData != null)
		{
			SerializedObject serializedObject = new SerializedObject(this);
			SerializedProperty serializedProperty = serializedObject.FindProperty("progressData");
			EditorGUILayout.PropertyField(serializedProperty, true);
			serializedObject.ApplyModifiedProperties();

			if (GUILayout.Button("Save data"))
			{
				SaveGameData();
			}
		}

		if (GUILayout.Button("Load data"))
		{
			LoadGameData();
		}

		if (GUILayout.Button("Create new data"))
		{
			CreateNewData();
		}
	}

	private void LoadGameData()
	{
		string filePath = EditorUtility.OpenFilePanel("Select localization data file", Application.streamingAssetsPath, "json");

		if (!string.IsNullOrEmpty(filePath))
		{
			string dataAsJson = File.ReadAllText(filePath);

			progressData = JsonUtility.FromJson<ProgressData>(dataAsJson);
		}
	}

	private void SaveGameData()
	{
		string filePath = EditorUtility.SaveFilePanel("Save localization data file", Application.streamingAssetsPath, "", "json");

		if (!string.IsNullOrEmpty(filePath))
		{
			string dataAsJson = JsonUtility.ToJson(progressData);
			File.WriteAllText(filePath, dataAsJson);
		}
	}

	private void CreateNewData()
	{
		progressData = new ProgressData();
	}
	

}
