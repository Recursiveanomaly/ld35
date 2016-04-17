using UnityEngine;
using System.Collections;
using System;
using Spine.Unity;

public class BeetleBase : MonoBehaviour
{
    public int m_health = 3;
    public int m_damage = 1;
    public float m_damageInvulnerabilityTime = 1;

    public float m_bounceAfterBeingHit = 1f;
    public float m_bounceAfterHit = 1f;

    public SkeletonAnimation m_skeleton;

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

    public void PlayJumpAnimation()
    {
        if (m_skeleton != null && m_skeleton.state != null)
        {
            m_skeleton.state.SetAnimation(0, "jump", false);
            m_skeleton.state.AddAnimation(0, "idle", true, 0);
        }
    }

    public void PlayTurnLeftAnimation()
    {
        if (m_skeleton != null && m_skeleton.state != null && m_skeleton.AnimationName != "turnLeft")
        {
            m_skeleton.state.SetAnimation(0, "turnLeft", false);
            m_skeleton.state.AddAnimation(0, "idle", true, 0);
        }
    }

    public void PlayTurnRightAnimation()
    {
        if (m_skeleton != null && m_skeleton.state != null && m_skeleton.AnimationName != "turnRight")
        {
            m_skeleton.state.SetAnimation(0, "turnRight", false);
            m_skeleton.state.AddAnimation(0, "idle", true, 0);
        }
    }

    public void ApplyDamage(BeetleBase attackingBeetle)
    {
        if (attackingBeetle == null) return;

        // add some bounce
        Vector3 attackerForceVector = Vector3.zero;
        Vector3 defenderForceVector = Vector3.zero;

        Vector3 vectorFromAttack = (transform.position - attackingBeetle.transform.position).normalized;
        attackerForceVector = -vectorFromAttack * attackingBeetle.m_bounceAfterBeingHit;
        defenderForceVector = vectorFromAttack * m_bounceAfterBeingHit;

        attackingBeetle.Bounce(attackerForceVector);
        Bounce(defenderForceVector);

        if (m_timeSinceDamage + m_damageInvulnerabilityTime < Time.time)
        {
            // tell all networked clients that damage has been applied
            try
            {
                m_photonView.RPC("RPC_ApplyDamage", PhotonTargets.AllBuffered, attackingBeetle.m_damage);
            }
            catch { }
            m_timeSinceDamage = Time.time;
        }
    }

    public void Bounce(Vector3 force)
    {
        // tell network clients to bounce, or us if we own this object
        if (m_photonView.isMine)
        {
            RPC_Bounce(force);
        }
        else
        {
            //RPC_Bounce(force);
            try
            {
                m_photonView.RPC("RPC_Bounce", m_photonView.owner, force);
            }
            catch { }
        }
    }

    float m_timeSinceBounce = 0;
    float m_bounceCooldown = 0.2f;

    [PunRPC]
    public void RPC_Bounce(Vector3 force)
    {
        if (m_timeSinceBounce + m_bounceCooldown < Time.time)
        {
            m_bounceCooldown = Time.time;
            m_body.velocity = Vector3.zero;
            m_body.AddForce(force);
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
