using UnityEngine;
using System.Collections;
using Spine.Unity;

public class DummyBug : MonoBehaviour
{
    public SkeletonAnimation m_skeleton;

    [ReadOnly]
    public int m_headDefID;
    [ReadOnly]
    public int m_thoraxDefID;
    [ReadOnly]
    public int m_abdomenDefID;
    [ReadOnly]
    public int m_legDefID;

    [ReadOnly]
    public int m_nameDefID;

    HeadPartDef head;
    ThoraxPartDef thorax;
    AbdomenPartDef abdomen;
    LegPartDef leg;
    BeetleNameDef nameDef;

    public void SetRandomParts()
    {
        head = StaticData.Instance.m_heads.GetRandomStaticDef(out m_headDefID);
        if (head != null) SetBodyPart(BeetleBase.eBodyPartType.kHead, head.m_assetName);

        thorax = StaticData.Instance.m_thoraces.GetRandomStaticDef(out m_thoraxDefID);
        if (thorax != null) SetBodyPart(BeetleBase.eBodyPartType.kThorax, thorax.m_assetName);

        abdomen = StaticData.Instance.m_abdomens.GetRandomStaticDef(out m_abdomenDefID);
        if (abdomen != null) SetBodyPart(BeetleBase.eBodyPartType.kAbdomen, abdomen.m_assetName);

        leg = StaticData.Instance.m_legs.GetRandomStaticDef(out m_legDefID);
        if (leg != null) SetBodyPart(BeetleBase.eBodyPartType.kLeg, leg.m_assetName);


        nameDef = StaticData.Instance.m_beetleNames.GetRandomStaticDef(out m_nameDefID);
    }


    public string GetCommonName()
    {
        if (nameDef == null) return "Boll Weevil";
        return nameDef.m_commonName;
    }
    public string GetScientificName()
    {
        if (nameDef == null) return "Anthonomus grandis grandis";
        return nameDef.m_scientificName;
    }

    public string GetDamageStatDesc()
    {
        float range = m_maxClampDamage - m_minClampDamage;
        float statAboveMin = m_damage - m_minClampDamage;
        if(statAboveMin > (range / 3) * 2)
        {
            return "Strong";
        }
        else if (statAboveMin > (range / 3))
        {
            return "Average";
        }
        return "Weak";
    }
    public string GetHealthStatDesc()
    {
        float range = m_maxClampMaxHealth - m_minClampMaxHealth;
        float statAboveMin = m_maxHealth - m_minClampMaxHealth;
        if (statAboveMin > (range / 3) * 2)
        {
            return "Strong";
        }
        else if (statAboveMin > (range / 3))
        {
            return "Average";
        }
        return "Weak";
    }
    public string GetTurnSpeedStatDesc()
    {
        float range = m_maxClampTurnSpeed - m_minClampTurnSpeed;
        float statAboveMin = m_turnSpeed - m_minClampTurnSpeed;
        if (statAboveMin > (range / 3) * 2)
        {
            return "Strong";
        }
        else if (statAboveMin > (range / 3))
        {
            return "Average";
        }
        return "Weak";
    }
    public string GetJumpSpeedStatDesc()
    {
        float range = m_maxClampJumpForce - m_minClampJumpForce;
        float statAboveMin = m_jumpForce - m_minClampJumpForce;
        if (statAboveMin > (range / 3) * 2)
        {
            return "Strong";
        }
        else if (statAboveMin > (range / 3))
        {
            return "Average";
        }
        return "Weak";
    }
    public string GetJumpCooldownStatDesc()
    {
        float range = m_maxClampJumpCooldown - m_minClampJumpCooldown;
        float statAboveMin = m_jumpCooldown - m_minClampJumpCooldown;
        if (statAboveMin > (range / 3) * 2)
        {
            return "Strong";
        }
        else if (statAboveMin > (range / 3))
        {
            return "Average";
        }
        return "Weak";
    }
    public string GetBackupSpeedStatDesc()
    {
        float range = m_maxClampBackSpeed - m_minClampBackSpeed;
        float statAboveMin = m_backSpeed - m_minClampBackSpeed;
        if (statAboveMin > (range / 3) * 2)
        {
            return "Strong";
        }
        else if (statAboveMin > (range / 3))
        {
            return "Average";
        }
        return "Weak";
    }

    public float m_backSpeed
    {
        get
        {
            if (head != null && thorax != null && abdomen != null && leg != null)
            {
                return Mathf.Clamp(head.m_backSpeed + thorax.m_backSpeed + abdomen.m_backSpeed + leg.m_backSpeed, m_minClampBackSpeed, m_maxClampBackSpeed);
            }
            return m_minClampBackSpeed;
        }
    }
    public float m_turnSpeed
    {
        get
        {
            if (head != null && thorax != null && abdomen != null && leg != null)
            {
                return Mathf.Clamp(head.m_turnSpeed + thorax.m_turnSpeed + abdomen.m_turnSpeed + leg.m_turnSpeed, m_minClampTurnSpeed, m_maxClampTurnSpeed);
            }
            return m_minClampTurnSpeed;
        }
    }
    public float m_jumpForce
    {
        get
        {
            if (head != null && thorax != null && abdomen != null && leg != null)
            {
                return Mathf.Clamp(head.m_jumpForce + thorax.m_jumpForce + abdomen.m_jumpForce + leg.m_jumpForce, m_minClampJumpForce, m_maxClampJumpForce);
            }
            return m_minClampJumpForce;
        }
    }
    public float m_jumpCooldown
    {
        get
        {
            if (head != null && thorax != null && abdomen != null && leg != null)
            {
                return Mathf.Clamp(head.m_jumpCooldown + thorax.m_jumpCooldown + abdomen.m_jumpCooldown + leg.m_jumpCooldown, m_minClampJumpCooldown, m_maxClampJumpCooldown);
            }
            return m_minClampJumpCooldown;
        }
    }
    public int m_maxHealth
    {
        get
        {
            if (head != null && thorax != null && abdomen != null && leg != null)
            {
                return Mathf.Clamp(head.m_health + thorax.m_health + abdomen.m_health + leg.m_health, m_minClampMaxHealth, m_maxClampMaxHealth);
            }
            return m_minClampMaxHealth;
        }
    }
    public int m_damage
    {
        get
        {
            if (head != null && thorax != null && abdomen != null && leg != null)
            {
                return Mathf.Clamp(head.m_damage + thorax.m_damage + abdomen.m_damage + leg.m_damage, m_minClampDamage, m_maxClampDamage);
            }
            return m_minClampDamage;
        }
    }

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

    void SetBodyPart(BeetleBase.eBodyPartType bodyPartType, string assetName)
    {
        if (m_skeleton == null) return;
        switch (bodyPartType)
        {
            case BeetleBase.eBodyPartType.kHead:
                m_skeleton.Skeleton.SetAttachment("headSlot", assetName);
                break;
            case BeetleBase.eBodyPartType.kThorax:
                m_skeleton.Skeleton.SetAttachment("thoraxSlot", assetName);
                break;
            case BeetleBase.eBodyPartType.kAbdomen:
                m_skeleton.Skeleton.SetAttachment("abdomenSlot", assetName);
                break;
            case BeetleBase.eBodyPartType.kLeg:
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
}
