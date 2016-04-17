using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

// taken from http://forum.unity3d.com/threads/c-serialization-playerprefs-mystery.72156/
public class PlayerPrefsSerializer
{
    static BinaryFormatter bf = new BinaryFormatter();
    // serializableObject is any struct or class marked with [Serializable]
    public static void Save(string prefKey, object serializableObject)
    {
        try
        {
            MemoryStream memoryStream = new MemoryStream();
            bf.Serialize(memoryStream, serializableObject);
            string tmp = System.Convert.ToBase64String(memoryStream.ToArray());
            PlayerPrefs.SetString(prefKey, tmp);
        }
        catch (Exception e)
        {
            Debug.LogError("Error in PlayerPrefsSerializer.Save - exception thrown:");
            Debug.LogError(e.Message);
            Debug.LogError(e.StackTrace);
        }
    }

    public static object Load<T>(string prefKey)
    {
        try
        { 
            if (!PlayerPrefs.HasKey(prefKey))
                return default(T);

            string serializedData = PlayerPrefs.GetString(prefKey);
            MemoryStream dataStream = new MemoryStream(System.Convert.FromBase64String(serializedData));

            T deserializedObject = (T)bf.Deserialize(dataStream);

            return deserializedObject;
        }
        catch(Exception e)
        {
            Debug.LogError("Error in PlayerPrefsSerializer.Load - exception thrown:");
            Debug.LogError(e.Message);
            Debug.LogError(e.StackTrace);
        }
        return null;
    }
}