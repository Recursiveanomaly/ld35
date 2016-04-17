using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class RebuildStaticData
{
    [MenuItem("Tools/Rebuild Static Data")]
    private static void RunRebuildStaticData()
    {
        Debug.Log("Starting Rebuilding Static Data");

        // Body Parts
        LoadExcelDataIntoSerializedBinary<LegsStaticData, LegPartDef>(LegsStaticData.c_definitionPath, LegsStaticData.c_resourcePath, LegsStaticData.c_sheetName);
        Debug.Log("Loaded Leg Defs Static Data");
        LoadExcelDataIntoSerializedBinary<ThoracesStaticData, ThoraxPartDef>(ThoracesStaticData.c_definitionPath, ThoracesStaticData.c_resourcePath, ThoracesStaticData.c_sheetName);
        Debug.Log("Loaded Thorax Defs Static Data");
        LoadExcelDataIntoSerializedBinary<AbdomensStaticData, AbdomenPartDef>(AbdomensStaticData.c_definitionPath, AbdomensStaticData.c_resourcePath, AbdomensStaticData.c_sheetName);
        Debug.Log("Loaded Abdomen Defs Static Data");
        LoadExcelDataIntoSerializedBinary<HeadsStaticData, HeadPartDef>(HeadsStaticData.c_definitionPath, HeadsStaticData.c_resourcePath, HeadsStaticData.c_sheetName);
        Debug.Log("Loaded Head Defs Static Data");
        LoadExcelDataIntoSerializedBinary<BeetleNamesStaticData, BeetleNameDef>(BeetleNamesStaticData.c_definitionPath, BeetleNamesStaticData.c_resourcePath, BeetleNamesStaticData.c_sheetName);
        Debug.Log("Loaded Beetle Name Defs Static Data");

        AssetDatabase.Refresh();
        Debug.Log("Finished Rebuilding Static Data");
    }

    private static void LoadExcelDataIntoSerializedBinary<T, S>(string defPath, string resourcePath, string sheetName) 
        where T : StaticDataDictionary<S>, new() 
        where S : StaticDataDef, new()
    {
        Dictionary<int, S> staticDataDictionary = ExcelReader.LoadStaticData<S>(Application.dataPath + defPath, sheetName);

        TextAsset textAsset = null;
        Object loadedAsset = AssetDatabase.LoadAssetAtPath("Assets/Resources/" + resourcePath, typeof(TextAsset));
        if (loadedAsset != null && loadedAsset is TextAsset)
        {
            textAsset = loadedAsset as TextAsset;
        }
        else
        {
            textAsset = new TextAsset();
        }

        T staticData = new T();
        staticData.OverwriteAllStaticData(staticDataDictionary);
        string binaryData = BinarySerializer.Serialize(staticData);

        File.WriteAllText(Application.dataPath + "/Resources/" + resourcePath + ".txt", binaryData);
    }
}
