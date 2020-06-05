using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private static string saveFolder = "/saves/";
    private static string saveFormat = ".data";

    private static void CreateIfNotExist(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    public static void Save(PlayerData data, string userName)
    {
        string path = Application.persistentDataPath + saveFolder + userName + saveFormat;
        BinaryFormatter formatter = new BinaryFormatter();

        CreateIfNotExist(Application.persistentDataPath + saveFolder);

        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData Load(string userName)
    {
        string path = Application.persistentDataPath + saveFolder + userName + saveFormat;

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = (PlayerData)formatter.Deserialize(stream);
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogWarning("Player data not found!!");
            return new PlayerData();
        }

        
    }

    public static void Remove(string userName)
    {
        string path = Application.persistentDataPath + saveFolder + userName + saveFormat;

        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
