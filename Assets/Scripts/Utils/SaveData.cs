using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveData
{
    public static string Load(string saveFile)
    {
        string combinedPath = CombineDataPath(saveFile);
        string json = "";
        if (File.Exists(combinedPath))
        {
            json = File.ReadAllText(combinedPath);
        }
        else
        {
            Debug.Log("Save file does not exist");
        }
        return json;
    }

    public static void Save(string saveFile, string json)
    {
        string combinedPath = CombineDataPath(saveFile);
        StreamWriter sw = File.CreateText(combinedPath);
        sw.Close();
        File.WriteAllText(combinedPath, json);
    }

    public static string CombineDataPath(string saveFile)
    {
        return Application.streamingAssetsPath + "/" + saveFile;
    }
}
