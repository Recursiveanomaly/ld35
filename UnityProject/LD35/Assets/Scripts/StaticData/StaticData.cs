using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

public sealed class StaticData
{    
    public LegsStaticData m_legs;
    public ThoracesStaticData m_thoraces;
    public AbdomensStaticData m_abdomens;
    public HeadsStaticData m_heads;

    public BeetleNamesStaticData m_beetleNames;

    #region Singleton

    private StaticData()
    {
        m_legs = LoadStaticDataDictionary<LegsStaticData, LegPartDef>(LegsStaticData.c_resourcePath);
        m_thoraces = LoadStaticDataDictionary<ThoracesStaticData, ThoraxPartDef>(ThoracesStaticData.c_resourcePath);
        m_abdomens = LoadStaticDataDictionary<AbdomensStaticData, AbdomenPartDef>(AbdomensStaticData.c_resourcePath);
        m_heads = LoadStaticDataDictionary<HeadsStaticData, HeadPartDef>(HeadsStaticData.c_resourcePath);

        m_beetleNames = LoadStaticDataDictionary<BeetleNamesStaticData, BeetleNameDef>(BeetleNamesStaticData.c_resourcePath);
    }

    public static StaticData Instance { get { return Nested.instance; } }

    private class Nested
    {
        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Nested()
        {
        }

        internal static readonly StaticData instance = new StaticData();
    }
    #endregion

    private static T LoadStaticDataDictionary<T, S>(string resourcePath)
        where T : StaticDataDictionary<S>, new()
        where S : StaticDataDef, new()
    {
        T staticDataDictionary;
        TextAsset textAsset = Resources.Load<TextAsset>(resourcePath);
        if (textAsset != null && textAsset.text != string.Empty)
        {
            staticDataDictionary = BinarySerializer.Deserialize<T>(textAsset.text);
        }
        else
        {
            Debug.LogError("Error in StaticData.LoadStaticDataDictionary - unable to load " + resourcePath + " static data");
            staticDataDictionary = new T();
        }
        return staticDataDictionary;
    }
}

[Serializable]
public abstract class StaticDataDictionary<T> where T : StaticDataDef, new()
{
    public Dictionary<int, T> m_defs = new Dictionary<int, T>();

    [NonSerialized]
    int previousKeyUsed = 0;
    protected int GetNextKey()
    {
        while (m_defs.ContainsKey(previousKeyUsed)) previousKeyUsed++;
        return previousKeyUsed;
    }

    public void AddStaticDef(T def, int id = -1)
    {
        if (id != -1 && m_defs.ContainsKey(id))
        {
            Debug.LogError("Error in StaticData.AddStaticDef - specified key already exists in table");
        }

        if (id == -1)
        {
            id = GetNextKey();
        }
        def.m_ID = id;
        m_defs.Add(id, def);
    }

    public T GetStaticDef(int id)
    {
        if (m_defs.ContainsKey(id))
        {
            return m_defs[id];
        }
        return null;
    }

    public void OverwriteAllStaticData(Dictionary<int, T> newDefs)
    {
        m_defs = newDefs;
    }
}

[Serializable]
public class StaticDataDef
{
    public string Serialize()
    {
        StringBuilder serializedText = new StringBuilder();
        Type staticDefType = this.GetType();
        PropertyInfo[] propertyInfos = staticDefType.GetProperties();
        for (int propertyIndex = 0; propertyIndex < propertyInfos.Length; propertyIndex++)
        {
            object data = propertyInfos[propertyIndex].GetValue(this, null);
            if (data.GetType().IsEnum)
            {
                serializedText.Append((int)data);
            }
            else
            {
                serializedText.Append(data);
            }
            serializedText.Append(",");
        }
        return serializedText.ToString();
    }

    public static T Deserialize<T>(string serializedText) where T : StaticDataDef, new()
    {
        Type staticDefType = typeof(T);
        PropertyInfo[] propertyInfos = staticDefType.GetProperties();

        T deserializedDef = new T();
        string[] values = serializedText.Split(',');
        for (int propertyIndex = 0; propertyIndex < propertyInfos.Length && propertyIndex < values.Length; propertyIndex++)
        {
            string data = values[propertyIndex];
            if (!string.IsNullOrEmpty(data))
            {
                Type propertyType = propertyInfos[propertyIndex].PropertyType;
                if (propertyType == typeof(int) || propertyType.IsEnum)
                {
                    int deserializedValue;
                    if (int.TryParse(data, out deserializedValue))
                    {
                        propertyInfos[propertyIndex].SetValue(deserializedDef, deserializedValue, null);
                    }
                }
                else if (propertyType == typeof(string))
                {
                    propertyInfos[propertyIndex].SetValue(deserializedDef, data, null);
                }
                else
                {
                    Debug.Log("Error in StaticData.Deserialize - don't know what to do with type: " + propertyType + ", value: " + data);
                }
            }
        }
        return deserializedDef;
    }

    public int m_ID
    {
        get; set;
    }
}