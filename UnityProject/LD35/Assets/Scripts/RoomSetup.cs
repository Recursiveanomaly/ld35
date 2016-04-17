using UnityEngine;
using System.Collections;

public class RoomSetup : MonoBehaviour 
{
    float m_roomWidth = 30;
    float m_roomHeight = 30;

    void OnJoinedRoom()
    {
        if( PhotonNetwork.isMasterClient == false )
        {
            return;
        }

        PhotonNetwork.InstantiateSceneObject("BeetleBotPrefab", new Vector3(Random.Range(-m_roomWidth / 2, m_roomWidth / 2), Random.Range(-m_roomHeight / 2, m_roomHeight / 2), 0), Quaternion.identity, 0, null);
        PhotonNetwork.InstantiateSceneObject("BeetleBotPrefab", new Vector3(Random.Range(-m_roomWidth / 2, m_roomWidth / 2), Random.Range(-m_roomHeight / 2, m_roomHeight / 2), 0), Quaternion.identity, 0, null);
        PhotonNetwork.InstantiateSceneObject("BeetleBotPrefab", new Vector3(Random.Range(-m_roomWidth / 2, m_roomWidth / 2), Random.Range(-m_roomHeight / 2, m_roomHeight / 2), 0), Quaternion.identity, 0, null);
        PhotonNetwork.InstantiateSceneObject("BeetleBotPrefab", new Vector3(Random.Range(-m_roomWidth / 2, m_roomWidth / 2), Random.Range(-m_roomHeight / 2, m_roomHeight / 2), 0), Quaternion.identity, 0, null);
        PhotonNetwork.InstantiateSceneObject("BeetleBotPrefab", new Vector3(Random.Range(-m_roomWidth / 2, m_roomWidth / 2), Random.Range(-m_roomHeight / 2, m_roomHeight / 2), 0), Quaternion.identity, 0, null);
        PhotonNetwork.InstantiateSceneObject("BeetleBotPrefab", new Vector3(Random.Range(-m_roomWidth / 2, m_roomWidth / 2), Random.Range(-m_roomHeight / 2, m_roomHeight / 2), 0), Quaternion.identity, 0, null);
    }
}
