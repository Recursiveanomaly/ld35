using UnityEngine;
using System.Collections;

[System.Serializable]
public class HeadPartDef
{
    public int m_ID
    {
        get; set;
    }

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
