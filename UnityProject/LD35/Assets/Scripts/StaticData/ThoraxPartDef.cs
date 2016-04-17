using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class ThoraxPartDefs : StaticDataDef
{
    public string m_name
    {
        get; set;
    }

    public string m_description
    {
        get; set;
    }

    public string m_assetName
    {
        get; set;
    }

    public int m_health
    {
        get; set;
    }

    public int m_damage
    {
        get; set;
    }

    public int m_turnSpeed
    {
        get; set;
    }

    public int m_jumpForce
    {
        get; set;
    }
}

[Serializable]
public class ThoracesStaticData : StaticDataDictionary<ThoraxPartDefs>
{
    public const string c_resourcePath = "StaticData/ThoracesStaticData";
    public const string c_definitionPath = "/StaticDefinitions/BodyPartDefs.xlsx";
    public const string c_sheetName = "ThoraxPartDefs";
}
