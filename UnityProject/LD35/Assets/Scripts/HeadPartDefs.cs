using UnityEngine;
using System.Collections;

[System.Serializable]
public class HeadPartDefs
{
    public int m_ID
    {
        get; set;
    }

    public string name
    {
        get; set;
    }

    public string description
    {
        get; set;
    }

    public string assetName
    {
        get; set;
    }

    public int health
    {
        get; set;
    }

    public int damage
    {
        get; set;
    }

    public int turnSpeed
    {
        get; set;
    }

    public int jumpForce
    {
        get; set;
    }
}
