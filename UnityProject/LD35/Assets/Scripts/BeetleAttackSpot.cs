using UnityEngine;
using System.Collections;

public class BeetleAttackSpot : MonoBehaviour
{
    [System.NonSerialized]
    public BeetleBase m_beetleBase;

    void Awake()
    {
        if (m_beetleBase == null)
        {
            m_beetleBase = GetComponent<BeetleBase>();
        }
        if (m_beetleBase == null)
        {
            m_beetleBase = GetComponentInParent<BeetleBase>();
        }
    }

    void OnCollison2D()
    {

    }
}
