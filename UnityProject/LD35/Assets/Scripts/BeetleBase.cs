using UnityEngine;
using System.Collections;
using System;
using Spine.Unity;

public class BeetleBase : MonoBehaviour
{
    public enum eBodyPartType
    {
        kHead,
        kThorax,
        kAbdomen,
        kLeg
    }

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

    public string m_name;
    public TextMesh m_textMesh;

    int m_headDefID;
    int m_thoraxDefID;
    int m_abdomenDefID;
    int m_legDefID;

    void Awake()
    {
        m_photonView = GetComponent<PhotonView>();
        m_body = GetComponent<Rigidbody2D>();

        // randomize the name
        int nameID;
        BeetleNameDef nameDef = StaticData.Instance.m_beetleNames.GetRandomStaticDef(out nameID);
        if (nameDef != null)
        {
            m_name = nameDef.m_scientificName;
        }
        else
        {
            m_name = "Caulophilus oryzae";
        }
        m_textMesh.text = m_name;

        // randomize body parts
        HeadPartDef headDef = StaticData.Instance.m_heads.GetRandomStaticDef(out m_headDefID);
        if (headDef != null) SetBodyPart(eBodyPartType.kHead, headDef.m_assetName);

        ThoraxPartDef thoraxDef = StaticData.Instance.m_thoraces.GetRandomStaticDef(out m_thoraxDefID);
        if (thoraxDef != null) SetBodyPart(eBodyPartType.kThorax, thoraxDef.m_assetName);

        AbdomenPartDef abdomenDef = StaticData.Instance.m_abdomens.GetRandomStaticDef(out m_abdomenDefID);
        if (abdomenDef != null) SetBodyPart(eBodyPartType.kAbdomen, abdomenDef.m_assetName);

        LegPartDef legDef = StaticData.Instance.m_legs.GetRandomStaticDef(out m_legDefID);
        if (legDef != null) SetBodyPart(eBodyPartType.kLeg, legDef.m_assetName);
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

    public void SetBodyPart(eBodyPartType bodyPartType, string assetName)
    {
        if (m_skeleton == null) return;
        switch (bodyPartType)
        {
            case eBodyPartType.kHead:
                m_skeleton.Skeleton.SetAttachment("headSlot", assetName);
                break;
            case eBodyPartType.kThorax:
                m_skeleton.Skeleton.SetAttachment("thoraxSlot", assetName);
                break;
            case eBodyPartType.kAbdomen:
                m_skeleton.Skeleton.SetAttachment("abdomenSlot", assetName);
                break;
            case eBodyPartType.kLeg:
                m_skeleton.Skeleton.SetAttachment("legBackLeftUpperSlot", assetName + "-legBL");
                m_skeleton.Skeleton.SetAttachment("legBackLeftLowerSlot", assetName + "-legBLd");

                m_skeleton.Skeleton.SetAttachment("legBackRightUpperSlot", assetName + "-legBR");
                m_skeleton.Skeleton.SetAttachment("legBackRightLowerSlot", assetName + "-legBRd");

                m_skeleton.Skeleton.SetAttachment("legMiddleLeftUpperSlot", assetName + "-legML");
                m_skeleton.Skeleton.SetAttachment("legMiddleLeftLowerSlot", assetName + "-legMLd");

                m_skeleton.Skeleton.SetAttachment("legMiddleRightUpperSlot", assetName + "-legMR");
                m_skeleton.Skeleton.SetAttachment("legMiddleRightLowerSlot", assetName + "-legMRd");

                m_skeleton.Skeleton.SetAttachment("legFrontLeftUpperSlot", assetName + "-legFL");
                m_skeleton.Skeleton.SetAttachment("legFrontLeftLowerSlot", assetName + "-legFLd");

                m_skeleton.Skeleton.SetAttachment("legFrontRightUpperSlot", assetName + "-legFR");
                m_skeleton.Skeleton.SetAttachment("legFrontRightLowerSlot", assetName + "-legFRd");
                break;
            default:
                break;
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
