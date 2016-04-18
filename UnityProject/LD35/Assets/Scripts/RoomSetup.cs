using UnityEngine;
using System.Collections;

public class RoomSetup : MonoBehaviour 
{
    float m_roomWidth = 50;
    float m_roomHeight = 50;
    int m_numBots = 20;

    void OnJoinedRoom()
    {
        if( PhotonNetwork.isMasterClient == false )
        {
            return;
        }

        for(int i = 0; i < m_numBots; i++)
        {
            PhotonNetwork.InstantiateSceneObject("BeetleBotPrefab", new Vector3(Random.Range(-m_roomWidth / 2, m_roomWidth / 2), Random.Range(-m_roomHeight / 2, m_roomHeight / 2), 0), Quaternion.identity, 0, null);
        }
    }
}
