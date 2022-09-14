using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SaveGame()
    {
        GameData data = new GameData();
        string saveData = JsonUtility.ToJson(data);
        string path = Application.persistentDataPath + "/save.txt";
        File.Delete(path);
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(saveData);
        writer.Close();
    }

    public static GameData LoadGame()
    {
        string savefile = Application.persistentDataPath + "/save.txt";
        StreamReader reader = new StreamReader(savefile);
        string saveData = reader.ReadToEnd();
        GameData data = JsonUtility.FromJson<GameData>(saveData);
        reader.Close();
        return data;
    }
}
