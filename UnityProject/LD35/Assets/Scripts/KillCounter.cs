using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KillCounter : Singleton<KillCounter>
{
    public RectTransform m_killIndicator;

    public void AddKill()
    {
        RectTransform newKill = GameObject.Instantiate<RectTransform>(m_killIndicator);
        newKill.SetParent(transform, false);

        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    public void ClearKills()
    {
        foreach(var child in gameObject.transform.GetComponentsInChildren<Image>())
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
