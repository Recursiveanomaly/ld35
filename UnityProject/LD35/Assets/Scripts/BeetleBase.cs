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

    public float m_forwardSpeed
    {
        get
        {
            return 0;
        }
    }
    public float m_backSpeed
    {
        get
        {
            return 60;
        }
    }
    public float m_turnSpeed
    {
        get
        {
            return Mathf.Max(3, GetHeadDef().m_turnSpeed + GetThoraxDef().m_turnSpeed + GetAbdomenDef().m_turnSpeed + GetLegDef().m_turnSpeed) * 60;
        }
    }
    public float m_jumpForce
    {
        get
        {
            return Mathf.Max(700, GetHeadDef().m_jumpForce + GetThoraxDef().m_jumpForce + GetAbdomenDef().m_jumpForce + GetLegDef().m_jumpForce);
        }
    }
    public float m_jumpCooldown
    {
        get
        {
            return 0.5f;
        }
    }
    public int m_maxHealth
    {
        get
        {
            return GetHeadDef().m_health + GetThoraxDef().m_health + GetAbdomenDef().m_health + GetLegDef().m_health;
        }
    }
    public int m_damage
    {
        get
        {
            return GetHeadDef().m_damage + GetThoraxDef().m_damage + GetAbdomenDef().m_damage + GetLegDef().m_damage;
        }
    }

    [NonSerialized]
    public float m_lastJumpTime = 0;
    public float m_damageInvulnerabilityTime = 1;
    public float m_health;

    public float m_bounceAfterBeingHit = 1f;
    public float m_bounceAfterHit = 1f;

    public AudioClip m_jumpClip;
    public AudioClip m_counterTurnClip;
    public AudioClip m_clockTurnClip;
    AudioSource m_turnAudioSource;

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
        m_turnAudioSource = gameObject.AddComponent<AudioSource>();

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

        m_health = m_maxHealth;
    }

    HeadPartDef GetHeadDef()
    {
        return StaticData.Instance.m_heads.GetStaticDef(m_headDefID);
    }

    ThoraxPartDef GetThoraxDef()
    {
        return StaticData.Instance.m_thoraces.GetStaticDef(m_headDefID);
    }

    AbdomenPartDef GetAbdomenDef()
    {
        return StaticData.Instance.m_abdomens.GetStaticDef(m_headDefID);
    }

    LegPartDef GetLegDef()
    {
        return StaticData.Instance.m_legs.GetStaticDef(m_headDefID);
    }

    public void PlayJumpSFX()
    {
        if (m_photonView == null || !m_photonView.isMine || gameObject.tag == "Bot") return;
        if (m_jumpClip != null)
        {
            AudioSource.PlayClipAtPoint(m_jumpClip, transform.position, 0.5f);
            //m_audioSource.clip = m_jumpClip;
            //m_audioSource.Play();
        }
    }

    public void PlayCounterSpinSFX()
    {
        if (m_photonView == null || !m_photonView.isMine || gameObject.tag == "Bot") return;
        if (m_counterTurnClip != null)
        {
            m_turnAudioSource.volume = 0.25f;
            m_turnAudioSource.clip = m_counterTurnClip;
            m_turnAudioSource.Play();
            m_stopColldown = float.MaxValue;
        }
    }

    public void PlayClockSpinSFX()
    {
        if (m_photonView == null || !m_photonView.isMine || gameObject.tag == "Bot") return;
        if (m_clockTurnClip != null && !m_turnAudioSource.isPlaying)
        {
            m_turnAudioSource.volume = 0.25f;
            m_turnAudioSource.clip = m_clockTurnClip;
            m_turnAudioSource.Play();
            m_stopColldown = float.MaxValue;
        }
    }

    float m_stopColldown = float.MaxValue;
    float m_stopColldownMax = 0.5f;
    public void StopSpinSFX()
    {
        if (m_photonView == null || !m_photonView.isMine || gameObject.tag == "Bot") return;
        if (m_stopColldown == float.MaxValue)
        {
            m_stopColldown = m_stopColldownMax;
        }
        if (m_turnAudioSource.isPlaying)
        {
            if (m_stopColldown < 0)
            {
                m_turnAudioSource.Stop();
            }
            else
            {
                m_stopColldown -= Time.deltaTime;
            }
        }
    }

    void Start()
    {
        BeetleMaster.Instance.BeetleCreated(this);
    }

    void OnDestroy()
    {
        BeetleMaster.Instance.BeetleDied(this);
    }

    float m_timeSinceDamage = 0;

    public void PlayJumpAnimation()
    {
        if (m_skeleton != null && m_skeleton.state != null)
        {
            m_skeleton.state.SetAnimation(0, "jump", false);
            m_skeleton.state.AddAnimation(0, "idle", true, 0);
            PlayJumpSFX();
        }
    }

    public void PlayTurnLeftAnimation()
    {
        if (m_skeleton != null && m_skeleton.state != null && m_skeleton.AnimationName != "turnLeft")
        {
            m_skeleton.state.SetAnimation(0, "turnLeft", false);
            m_skeleton.state.AddAnimation(0, "idle", true, 0);
            PlayClockSpinSFX();
        }
    }

    public void PlayTurnRightAnimation()
    {
        if (m_skeleton != null && m_skeleton.state != null && m_skeleton.AnimationName != "turnRight")
        {
            m_skeleton.state.SetAnimation(0, "turnRight", false);
            m_skeleton.state.AddAnimation(0, "idle", true, 0);
            PlayCounterSpinSFX();
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
