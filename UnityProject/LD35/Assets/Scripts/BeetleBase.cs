using UnityEngine;
using System.Collections;
using System;

public class BeetleBase : MonoBehaviour
{
    public int m_health = 3;
    public int m_damage = 1;
    public float m_damageInvulnerabilityTime = 1;

    public float m_bounceAfterBeingHit = 1f;
    public float m_bounceAfterHit = 1f;

    [System.NonSerialized]
    public PhotonView m_photonView;
    [System.NonSerialized]
    public Rigidbody2D m_body;

    void Awake()
    {
        m_photonView = GetComponent<PhotonView>();
        m_body = GetComponent<Rigidbody2D>();
    }

    float m_timeSinceDamage = 0;

    public void ApplyDamage(BeetleBase attackingBeetle)
    {
        if (attackingBeetle == null) return;

        // add some bounce
        Vector3 attackerForceVector = Vector3.zero;
        Vector3 defenderForceVector = Vector3.zero;

        Vector3 vectorFromAttack = (transform.position - attackingBeetle.transform.position).normalized;
        attackerForceVector = -vectorFromAttack * attackingBeetle.m_bounceAfterBeingHit;
        defenderForceVector = vectorFromAttack * m_bounceAfterBeingHit;

        attackingBeetle.m_body.velocity = Vector3.zero;
        attackingBeetle.m_body.AddForce(attackerForceVector);
        m_body.velocity = Vector3.zero;
        m_body.AddForce(defenderForceVector);

        if (m_timeSinceDamage + m_damageInvulnerabilityTime < Time.time)
        {
            // tell all networked clients that damage has been applied
            m_photonView.RPC("RPC_ApplyDamage", PhotonTargets.AllBuffered, attackingBeetle.m_damage);
            m_timeSinceDamage = Time.time;
        }
    }

    [PunRPC]
    public void RPC_ApplyDamage(int damage)
    {
        m_timeSinceDamage = Time.time;
        m_health -= damage;
        if (m_health <= 0)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
