﻿using UnityEngine;
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

    [NonSerialized]
    public bool m_dead = false;

    [ReadOnly]
    public float m_fromPartsBackSpeed;
    public float m_overrideBackSpeed = -1;
    [ReadOnly]
    public float m_fromPartsTurnSpeed;
    public float m_overrideTurnSpeed = -1;
    [ReadOnly]
    public float m_fromPartsJumpForce;
    public float m_overrideJumpForce = -1;
    [ReadOnly]
    public float m_fromPartsJumpCooldown;
    public float m_overrideJumpCooldown = -1;
    [ReadOnly]
    public int m_fromPartsMaxHealth;
    public int m_overrideMaxHealth = -1;
    [ReadOnly]
    public int m_fromPartsDamage;
    public int m_overrideDamage = -1;

    private float m_minClampBackSpeed = 5;
    private float m_maxClampBackSpeed = 30;

    private float m_minClampTurnSpeed = 40;
    private float m_maxClampTurnSpeed = 200;

    private float m_minClampJumpForce = 700;
    private float m_maxClampJumpForce = 2000;

    private float m_minClampJumpCooldown = 0.4f;
    private float m_maxClampJumpCooldown = 1.75f;

    private int m_minClampMaxHealth = 25;
    private int m_maxClampMaxHealth = 75;

    private int m_minClampDamage = 5;
    private int m_maxClampDamage = 30;

    public float m_backSpeed
    {
        get
        {
            if (m_overrideBackSpeed != -1) return m_overrideBackSpeed;
            HeadPartDef head = GetHeadDef();
            ThoraxPartDef thorax = GetThoraxDef();
            AbdomenPartDef abdomen = GetAbdomenDef();
            LegPartDef leg = GetLegDef();
            if (head != null && thorax != null && abdomen != null && leg != null)
            {
               m_fromPartsBackSpeed = Mathf.Clamp(head.m_backSpeed + thorax.m_backSpeed + abdomen.m_backSpeed + leg.m_backSpeed, m_minClampBackSpeed, m_maxClampBackSpeed);
                return m_fromPartsBackSpeed;
            }
            m_fromPartsBackSpeed = -1;
            return 1;
        }
    }
    public float m_turnSpeed
    {
        get
        {
            if (m_overrideTurnSpeed != -1) return m_overrideTurnSpeed;
            HeadPartDef head = GetHeadDef();
            ThoraxPartDef thorax = GetThoraxDef();
            AbdomenPartDef abdomen = GetAbdomenDef();
            LegPartDef leg = GetLegDef();
            if (head != null && thorax != null && abdomen != null && leg != null)
            {
                m_fromPartsTurnSpeed = Mathf.Clamp(head.m_turnSpeed + thorax.m_turnSpeed + abdomen.m_turnSpeed + leg.m_turnSpeed, m_minClampTurnSpeed, m_maxClampTurnSpeed);
                return m_fromPartsTurnSpeed;
            }
            m_fromPartsTurnSpeed = -1;
            return 1;
        }
    }
    public float m_jumpForce
    {
        get
        {
            if (m_overrideJumpForce != -1) return m_overrideJumpForce;
            HeadPartDef head = GetHeadDef();
            ThoraxPartDef thorax = GetThoraxDef();
            AbdomenPartDef abdomen = GetAbdomenDef();
            LegPartDef leg = GetLegDef();
            if (head != null && thorax != null && abdomen != null && leg != null)
            {
                m_fromPartsJumpForce = Mathf.Clamp(head.m_jumpForce + thorax.m_jumpForce + abdomen.m_jumpForce + leg.m_jumpForce, m_minClampJumpForce, m_maxClampJumpForce);
                return m_fromPartsJumpForce;
            }
            m_fromPartsJumpForce = -1;
            return 1;
        }
    }
    public float m_jumpCooldown
    {
        get
        {
            if (m_overrideJumpCooldown != -1) return m_overrideJumpCooldown;
            HeadPartDef head = GetHeadDef();
            ThoraxPartDef thorax = GetThoraxDef();
            AbdomenPartDef abdomen = GetAbdomenDef();
            LegPartDef leg = GetLegDef();
            if (head != null && thorax != null && abdomen != null && leg != null)
            {
                m_fromPartsJumpCooldown = Mathf.Clamp(head.m_jumpCooldown + thorax.m_jumpCooldown + abdomen.m_jumpCooldown + leg.m_jumpCooldown, m_minClampJumpCooldown, m_maxClampJumpCooldown);
                return m_fromPartsJumpCooldown;
            }
            m_fromPartsJumpCooldown = -1;
            return 0.1f;
        }
    }
    public int m_maxHealth
    {
        get
        {
            if (m_overrideMaxHealth != -1) return m_overrideMaxHealth;
            HeadPartDef head = GetHeadDef();
            ThoraxPartDef thorax = GetThoraxDef();
            AbdomenPartDef abdomen = GetAbdomenDef();
            LegPartDef leg = GetLegDef();
            if (head != null && thorax != null && abdomen != null && leg != null)
            {
                m_fromPartsMaxHealth = Mathf.Clamp(head.m_health + thorax.m_health + abdomen.m_health + leg.m_health, m_minClampMaxHealth, m_maxClampMaxHealth);
                return m_fromPartsMaxHealth;
            }
            m_fromPartsMaxHealth = -1;
            return 1;
        }
    }
    public int m_damage
    {
        get
        {
            if (m_overrideDamage != -1) return m_overrideDamage;
            HeadPartDef head = GetHeadDef();
            ThoraxPartDef thorax = GetThoraxDef();
            AbdomenPartDef abdomen = GetAbdomenDef();
            LegPartDef leg = GetLegDef();
            if (head != null && thorax != null && abdomen != null && leg != null)
            {
                m_fromPartsDamage = Mathf.Clamp(head.m_damage + thorax.m_damage + abdomen.m_damage + leg.m_damage, m_minClampDamage, m_maxClampDamage);
                return m_fromPartsDamage;
            }
            m_fromPartsDamage = -1;
            return 1;
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
    public AudioClip m_deathClip;
    public AudioClip m_impactClip;
    AudioSource m_turnAudioSource;

    public SkeletonAnimation m_skeleton;

    [System.NonSerialized]
    public PhotonView m_photonView;
    [System.NonSerialized]
    public Rigidbody2D m_body;

    public string m_name;
    public TextMesh m_textMesh;

    [ReadOnly]
    public int m_headDefID;
    [ReadOnly]
    public int m_thoraxDefID;
    [ReadOnly]
    public int m_abdomenDefID;
    [ReadOnly]
    public int m_legDefID;

    public bool m_cocoon;
    public bool m_emerging;

    void Awake()
    {
        m_fromPartsBackSpeed = -1;
        m_fromPartsTurnSpeed = -1;
        m_fromPartsJumpForce = -1;
        m_fromPartsJumpCooldown = -1;
        m_fromPartsMaxHealth = -1;
        m_fromPartsDamage = -1;

        m_overrideBackSpeed = -1;
        m_overrideTurnSpeed = -1;
        m_overrideJumpForce = -1;
        m_overrideJumpCooldown = -1;   
        m_overrideMaxHealth = -1;
        m_overrideDamage = -1;

        m_photonView = GetComponent<PhotonView>();
        m_body = GetComponent<Rigidbody2D>();
        m_turnAudioSource = gameObject.AddComponent<AudioSource>();
    }


    [PunRPC]
    public void RPC_SetParts(string name, int headID, int thoraxID, int abdomenID, int legID)
    {
        if (m_photonView.isMine) return;
        m_name = name;
        m_headDefID = headID;
        m_thoraxDefID = thoraxID;
        m_abdomenDefID = abdomenID;
        m_legDefID = legID;
        ApplyDefs();
    }

    public void ApplyDefs()
    {
        m_textMesh.text = m_name;
        
        HeadPartDef headDef = GetHeadDef();
        if (headDef != null) SetBodyPart(eBodyPartType.kHead, headDef.m_assetName);

        ThoraxPartDef thoraxDef = GetThoraxDef();
        if (thoraxDef != null) SetBodyPart(eBodyPartType.kThorax, thoraxDef.m_assetName);

        AbdomenPartDef abdomenDef = GetAbdomenDef();
        if (abdomenDef != null) SetBodyPart(eBodyPartType.kAbdomen, abdomenDef.m_assetName);

        LegPartDef legDef = GetLegDef();
        if (legDef != null) SetBodyPart(eBodyPartType.kLeg, legDef.m_assetName);
    }

    void Start()
    {
        if (gameObject.tag == "Bot")
        {
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

            m_photonView.RPC("RPC_SetParts", PhotonTargets.All, m_name, m_headDefID, m_thoraxDefID, m_abdomenDefID, m_legDefID);
        }

        // read from the values to setup inspector, bleh
        float back = m_backSpeed;
        float turn = m_turnSpeed;
        float jumpForce = m_jumpForce;
        float jumpCooldown = m_jumpCooldown;
        int damage = m_damage;

        m_health = m_maxHealth;

        BeetleMaster.Instance.BeetleCreated(this);

        m_skeleton.state.SetAnimation(0, "cocoonIdle", true);
        m_cocoon = true;
        m_emerging = false;
    }

    public IEnumerator CR_EmergeFromCocoon(float randomizer)
    {
        m_emerging = true;
        m_skeleton.state.SetAnimation(0, "cocoonIdle", false);
        yield return new WaitForSeconds(randomizer);
        m_skeleton.state.SetAnimation(0, "cocoonEmerge", false);
        m_skeleton.state.AddAnimation(0, "idle", true, 0);
        m_photonView.RPC("RPC_PlayEmergeAnimation", PhotonTargets.All);
        yield return new WaitForSeconds(1);
        m_cocoon = false;
        m_emerging = false;
    }
    
    [PunRPC]
    public void RPC_PlayEmergeAnimation()
    {
        if (m_photonView.isMine) return;

        if (m_skeleton != null && m_skeleton.state != null && m_skeleton.AnimationName != "death")
        {
            m_skeleton.state.SetAnimation(0, "cocoonEmerge", false);
            m_skeleton.state.AddAnimation(0, "idle", true, 0);
        }

        m_cocoon = false;
    }

    void OnDestroy()
    {
        BeetleMaster.Instance.BeetleDied(this);
    }

    HeadPartDef GetHeadDef()
    {
        return StaticData.Instance.m_heads.GetStaticDef(m_headDefID);
    }

    ThoraxPartDef GetThoraxDef()
    {
        return StaticData.Instance.m_thoraces.GetStaticDef(m_thoraxDefID);
    }

    AbdomenPartDef GetAbdomenDef()
    {
        return StaticData.Instance.m_abdomens.GetStaticDef(m_abdomenDefID);
    }

    LegPartDef GetLegDef()
    {
        return StaticData.Instance.m_legs.GetStaticDef(m_legDefID);
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

    public void PlayDeathSFX()
    {
        //if (m_photonView == null || !m_photonView.isMine || gameObject.tag == "Bot") return;
        if (m_deathClip!= null)
        {
            AudioSource.PlayClipAtPoint(m_deathClip, transform.position, 1f);
            //m_audioSource.clip = m_jumpClip;
            //m_audioSource.Play();
        }
    }

    public void PlayImpactSFX()
    {
        //if (m_photonView == null || !m_photonView.isMine || gameObject.tag == "Bot") return;
        if (m_impactClip != null)
        {
            AudioSource.PlayClipAtPoint(m_impactClip, transform.position, 1f);
            //m_audioSource.clip = m_jumpClip;
            //m_audioSource.Play();
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

    float m_timeSinceDamage = 0;

    public void PlayHurtAnimation()
    {
        if (m_skeleton != null && m_skeleton.state != null && m_skeleton.AnimationName != "death")
        {
            m_skeleton.state.SetAnimation(1, "hurt", false);
            PlayImpactSFX();
        }
    }

    public void PlayDeathAnimation()
    {
        if (m_skeleton != null && m_skeleton.state != null)
        {
            m_skeleton.state.SetAnimation(0, "death", false);
            PlayDeathSFX();
        }
    }

    public void PlayJumpAnimation()
    {
        if (m_skeleton != null && m_skeleton.state != null && !m_cocoon && !m_emerging && m_skeleton.AnimationName != "death")
        {
            m_skeleton.state.SetAnimation(0, "jump", false);
            m_skeleton.state.AddAnimation(0, "idle", true, 0);
            PlayJumpSFX();
            m_photonView.RPC("RPC_PlayJumpAnimation", PhotonTargets.All);
        }
    }

    [PunRPC]
    public void RPC_PlayJumpAnimation()
    {
        if (m_photonView.isMine) return;

        if (m_skeleton != null && m_skeleton.state != null && m_skeleton.AnimationName != "death")
        {
            m_skeleton.state.SetAnimation(0, "jump", false);
            m_skeleton.state.AddAnimation(0, "idle", true, 0);
        }
        m_cocoon = false;
    }

    public void PlayTurnLeftAnimation()
    {
        if (m_skeleton != null && m_skeleton.state != null && !m_cocoon && !m_emerging && m_skeleton.AnimationName == "idle" && m_skeleton.AnimationName != "death")
        {
            m_skeleton.state.SetAnimation(0, "turnLeft", false);
            m_skeleton.state.AddAnimation(0, "idle", true, 0);
            PlayClockSpinSFX();
        }
    }

    public void PlayTurnRightAnimation()
    {
        if (m_skeleton != null && m_skeleton.state != null && !m_cocoon && !m_emerging && m_skeleton.AnimationName == "idle" && m_skeleton.AnimationName != "death")
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
                m_photonView.RPC("RPC_ApplyDamage", PhotonTargets.All, attackingBeetle.m_damage, attackingBeetle.m_photonView.instantiationId);
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
        if (m_timeSinceBounce + m_bounceCooldown < Time.time && !m_dead)
        {
            m_bounceCooldown = Time.time;
            m_body.velocity = Vector3.zero;
            m_body.AddForce(force);
        }
    }

    [PunRPC]
    public void RPC_ApplyDamage(int damage, int sourceID)
    {
        PlayHurtAnimation();
        m_timeSinceDamage = Time.time;
        m_health -= damage;
        if (m_health <= 0)
        {
            StartCoroutine(CR_Destroy());
            BeetleMaster.Instance.Kill(sourceID);
        }
    }

    IEnumerator CR_Destroy()
    {
        if (!m_dead)
        {
            PlayDeathAnimation();
        }
        m_body.velocity = Vector3.zero;
        m_dead = true;

        Collider2D collider = GetComponent<Collider2D>();
        if(collider != null)
        {
            collider.enabled = false;
        }
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach(var col in colliders)
        {
            col.enabled = false;
        }

        yield return new WaitForSeconds(3f);
        if (m_photonView.isMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
