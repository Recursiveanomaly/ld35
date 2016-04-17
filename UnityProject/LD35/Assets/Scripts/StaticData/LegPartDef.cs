using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class LegPartDef : StaticDataDef
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
public class LegsStaticData : StaticDataDictionary<LegPartDef>
{
    public const string c_resourcePath = "StaticData/LegsStaticData";
    public const string c_definitionPath = "/StaticDefinitions/BodyPartDefs.xlsx";
    public const string c_sheetName = "LegPartDefs";
}
