using UnityEngine;
using System.Collections;

public class RoomSetup : MonoBehaviour 
{
    void OnJoinedRoom()
    {
        if( PhotonNetwork.isMasterClient == false )
        {
            return;
        }

        PhotonNetwork.InstantiateSceneObject("BeetleBotPrefab", new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0), Quaternion.identity, 0, null);
        PhotonNetwork.InstantiateSceneObject("BeetleBotPrefab", new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0), Quaternion.identity, 0, null);
        PhotonNetwork.InstantiateSceneObject("BeetleBotPrefab", new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0), Quaternion.identity, 0, null);
        PhotonNetwork.InstantiateSceneObject("BeetleBotPrefab", new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0), Quaternion.identity, 0, null);
        PhotonNetwork.InstantiateSceneObject("BeetleBotPrefab", new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0), Quaternion.identity, 0, null);
        PhotonNetwork.InstantiateSceneObject("BeetleBotPrefab", new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0), Quaternion.identity, 0, null);
    }
}
