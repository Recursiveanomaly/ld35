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

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(m_beetleBase == null || m_beetleBase.m_photonView == null)
        {
            return;
        }
        BeetleDamageSpot damageSpot = coll.collider.gameObject.GetComponent<BeetleDamageSpot>();
        if (damageSpot != null && damageSpot.m_beetleBase != null && !damageSpot.m_beetleBase.m_cocoon)
        {
            // we don't apply damage we caused
            if (!m_beetleBase.m_photonView.isMine)
            {
                // we didn't cause this damage
                if (damageSpot.m_beetleBase.m_photonView.isMine)
                {
                    // someone is applying damage to us
                    damageSpot.m_beetleBase.ApplyDamage(m_beetleBase);
                }
            }
            else if (m_beetleBase.m_photonView.tag == "Bot")
            {
                // this was caused by a bot
                if (damageSpot.m_beetleBase.m_photonView.isMine)
                {
                    // someone is applying damage to something we own
                    damageSpot.m_beetleBase.ApplyDamage(m_beetleBase);
                }
            }
            else if(damageSpot.m_beetleBase.tag == "Bot")
            {
                // this was done to a bot by a locally owned beetle
                damageSpot.m_beetleBase.ApplyDamage(m_beetleBase);
            }
        }
        else
        {
            // we didn't hit a damage spot, lets just bounce
            if (m_beetleBase.m_photonView.isMine)
            {
                Vector3 attackerForceVector = Vector3.zero;

                Vector3 vectorToAttack = (transform.position - coll.collider.transform.position).normalized;
                attackerForceVector = vectorToAttack * m_beetleBase.m_bounceAfterHit;

                m_beetleBase.Bounce(attackerForceVector);
            }
        }
    }
}
