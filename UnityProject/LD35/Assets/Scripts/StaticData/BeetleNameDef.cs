using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class BeetleNameDef : StaticDataDef
{
    public string m_commonName
    {
        get; set;
    }

    public string m_scientificName
    {
        get; set;
    }
}

[Serializable]
public class BeetleNamesStaticData : StaticDataDictionary<BeetleNameDef>
{
    public const string c_resourcePath = "StaticData/BeetleNamesStaticData";
    public const string c_definitionPath = "/StaticDefinitions/BeetleNameDefs.xlsx";
    public const string c_sheetName = "beetleNameDefs";
}
