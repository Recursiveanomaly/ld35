using UnityEngine;
using System.Collections;

public class RoomSetup : MonoBehaviour 
{
    int m_numBots = 20;

    void OnJoinedRoom()
    {
        if( PhotonNetwork.isMasterClient == false )
        {
            return;
        }

        for(int i = 0; i < m_numBots; i++)
        {
            BeetleMaster.Instance.SpawnBot();
        }
    }
}
