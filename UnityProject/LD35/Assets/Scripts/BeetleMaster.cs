using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeetleMaster : Singleton<BeetleMaster>
{
    [System.NonSerialized]
    public List<BeetleBase> m_beetles = new List<BeetleBase>();

    public void BeetleCreated(BeetleBase beetle)
    {
        if(!m_beetles.Contains(beetle))
        {
            m_beetles.Add(beetle);
        }
    }

    public void BeetleDied(BeetleBase beetle)
    {
        m_beetles.Remove(beetle);

        if(beetle.gameObject.tag == "Bot" && beetle.m_photonView.isMine)
        {
            //spawn a new beetle
            SpawnBot();
        }
    }

    float m_roomWidth = 50;
    float m_roomHeight = 50;
    public void SpawnBot()
    {
        PhotonNetwork.InstantiateSceneObject("BeetleBotPrefab", new Vector3(Random.Range(-m_roomWidth / 2, m_roomWidth / 2), Random.Range(-m_roomHeight / 2, m_roomHeight / 2), 0), Quaternion.identity, 0, null);
    }

    public BeetleBase FindClosestEnemy(BeetleBase originBeetle)
    {
        BeetleBase closestBeetle = null;
        float closestDistance = float.MaxValue;
        for (int beetleIndex = 0; beetleIndex < m_beetles.Count; beetleIndex++)
        {
            BeetleBase otherBeetle = m_beetles[beetleIndex];
            if(otherBeetle != null && otherBeetle != originBeetle)
            {
                float distance = Vector3.Distance(originBeetle.transform.position, otherBeetle.transform.position);
                if(distance < closestDistance)
                {
                    closestBeetle = otherBeetle;
                    closestDistance = distance;
                }
            }
        }

        return closestBeetle;
    }
}
