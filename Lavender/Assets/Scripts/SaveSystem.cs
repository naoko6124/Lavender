using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public static class SaveSystem
{
    public static void Save(SaveData data, int slot)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save" + slot + ".sav";
        Debug.Log(path);
        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static SaveData Load(int slot)
    {
        string path = Application.persistentDataPath + "/save" + slot + ".sav";
        if (!File.Exists(path))
            return null;

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
        SaveData sd = formatter.Deserialize(stream) as SaveData;
        stream.Close();
        return sd;
    }
}
