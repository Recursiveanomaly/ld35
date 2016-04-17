using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class BinarySerializer
{
    static BinaryFormatter bf = new BinaryFormatter();
    // serializableObject is any struct or class marked with [Serializable]
    public static string Serialize(object serializableObject)
    {
        try
        {
            MemoryStream memoryStream = new MemoryStream();
            bf.Serialize(memoryStream, serializableObject);
            string tmp = System.Convert.ToBase64String(memoryStream.ToArray());
            return tmp;
        }
        catch (Exception e)
        {
            Debug.LogError("Error in BinarySerializer.Serialize - exception thrown:");
            Debug.LogError(e.Message);
            Debug.LogError(e.StackTrace);
        }
        return string.Empty;
    }

    public static T Deserialize<T>(string serializedData)
    {
        try
        {
            MemoryStream dataStream = new MemoryStream(System.Convert.FromBase64String(serializedData));
            T deserializedObject = (T)bf.Deserialize(dataStream);
            return deserializedObject;
        }
        catch (Exception e)
        {
            Debug.LogError("Error in BinarySerializer.Deserialize - exception thrown:");
            Debug.LogError(e.Message);
            Debug.LogError(e.StackTrace);
        }
        return default(T);
    }
}
