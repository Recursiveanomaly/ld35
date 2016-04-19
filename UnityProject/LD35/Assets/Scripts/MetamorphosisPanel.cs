using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class MetamorphosisPanel : Singleton<MetamorphosisPanel>
{
    public DummyBug m_dummyBug;

    public Button m_rerollButton;
    public Button m_emergeButton;

    public Text m_commonName;
    public Text m_scientificName;

    public Text m_damageStatDesc;
    public Text m_healthStatDesc;
    public Text m_turnSpeedStatDesc;
    public Text m_jumpSpeedStatDesc;
    public Text m_jumpCooldownStatDesc;
    public Text m_backupSpeedStatDesc;

    public Text m_damageStatDesc1;
    public Text m_healthStatDesc1;
    public Text m_turnSpeedStatDesc1;
    public Text m_jumpSpeedStatDesc1;
    public Text m_jumpCooldownStatDesc1;
    public Text m_backupSpeedStatDesc1;

    public BeetleBase m_playerPrefab;

    public void Show()
    {
        MusicManager.Instance.PlayCocoonMusic();
        m_rerollsLeft = 3;
        m_rerollButtonText.text = "Reroll (" + m_rerollsLeft.ToString() + " left)";
        m_dummyBug.SetRandomParts();
        SetStats();
        gameObject.SetActive(true);
        EnableButtons();
    }

    public Text m_rerollButtonText;

    public int m_rerollsLeft = 3;

    IEnumerator CR_RollParts()
    {
        DisableButtons();
        for (int i = 0; i < 20; i++)
        {
            m_dummyBug.SetRandomParts();
            SetStats();
            yield return new WaitForSeconds(0.1f);
        }
        EnableButtons();
    }

    void SetStats()
    {
        m_commonName.text = m_dummyBug.GetCommonName();
        m_scientificName.text = m_dummyBug.GetScientificName();

        m_damageStatDesc.text = m_dummyBug.GetDamageStatDesc();
        m_healthStatDesc.text = m_dummyBug.GetHealthStatDesc();
        m_turnSpeedStatDesc.text = m_dummyBug.GetTurnSpeedStatDesc();
        m_jumpSpeedStatDesc.text = m_dummyBug.GetJumpSpeedStatDesc();
        m_jumpCooldownStatDesc.text = m_dummyBug.GetJumpCooldownStatDesc();
        m_backupSpeedStatDesc.text = m_dummyBug.GetBackupSpeedStatDesc();

        m_damageStatDesc1.text = m_dummyBug.GetDamageStatDesc();
        m_healthStatDesc1.text = m_dummyBug.GetHealthStatDesc();
        m_turnSpeedStatDesc1.text = m_dummyBug.GetTurnSpeedStatDesc();
        m_jumpSpeedStatDesc1.text = m_dummyBug.GetJumpSpeedStatDesc();
        m_jumpCooldownStatDesc1.text = m_dummyBug.GetJumpCooldownStatDesc();
        m_backupSpeedStatDesc1.text = m_dummyBug.GetBackupSpeedStatDesc();
    }

    public void RollClicked()
    {
        m_rerollsLeft--;
        m_rerollButtonText.text = "Reroll (" + m_rerollsLeft.ToString() + " left)";
        StartCoroutine(CR_RollParts());
    }

    public void EmergeClicked()
    {
        DisableButtons();
        StartCoroutine(CR_Emerge());
    }

    IEnumerator CR_Emerge()
    {

        KillCounter.Instance.ClearKills();

        GameObject player = PhotonNetwork.Instantiate(m_playerPrefab.gameObject.name, BeetleMaster.Instance.RandomSpawn(), Quaternion.identity, 0);
        //set stats
        BeetleBase beetleBase = player.GetComponent<BeetleBase>();
        beetleBase.m_headDefID = m_dummyBug.m_headDefID;
        beetleBase.m_thoraxDefID = m_dummyBug.m_thoraxDefID;
        beetleBase.m_abdomenDefID = m_dummyBug.m_abdomenDefID;
        beetleBase.m_legDefID = m_dummyBug.m_legDefID;
        beetleBase.m_name = m_dummyBug.GetScientificName();
        beetleBase.ApplyDefs();

        beetleBase.m_photonView.RPC("RPC_SetParts", PhotonTargets.All, beetleBase.m_name, beetleBase.m_headDefID, beetleBase.m_thoraxDefID, beetleBase.m_abdomenDefID, beetleBase.m_legDefID);

        yield return new WaitForSeconds(1);

        MusicManager.Instance.PlayThemeMusic();
        gameObject.SetActive(false);
    }

    void EnableButtons()
    {
        m_rerollButton.interactable = m_rerollsLeft > 0;
        m_emergeButton.interactable = true;
    }

    void DisableButtons()
    {
        m_rerollButton.interactable = false;
        m_emergeButton.interactable = false;
    }
}
