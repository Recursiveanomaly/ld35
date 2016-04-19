using UnityEngine;
using System.Collections;

public class RoomSetup : MonoBehaviour 
{
    int m_numBots = 17;

    void OnJoinedRoom()
    {
        MetamorphosisPanel.Instance.Show();

        if ( PhotonNetwork.isMasterClient == false )
        {
            return;
        }

        for(int i = 0; i < m_numBots; i++)
        {
            BeetleMaster.Instance.SpawnBot();
        }
    }
}
