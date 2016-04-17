using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class BotController : MonoBehaviour
{
    enum eBotState
    {
        Idle,
        Attack,
    }

    public BeetleBase m_beetleBase;
    public float m_findTargetDistance = 10;
    Rigidbody2D m_body;
    float m_stateUpdateTime = 3;
    float m_timeUntilStateCheck;

    eBotState m_currentState = eBotState.Idle;
    BeetleBase m_currentTarget;

    List<BeetleBase> m_nearbyEnemies = new List<BeetleBase>();

    void Awake()
    {
        m_body = GetComponent<Rigidbody2D>();
        // we don't want all the bots updating at the same time
        m_timeUntilStateCheck = m_stateUpdateTime + UnityEngine.Random.Range(0, m_stateUpdateTime);
    }

    public bool IsLocalControl()
    {
        return m_beetleBase.m_photonView.isMine;
    }

    public void AddNearbyEnemy(BeetleBase enemy)
    {
        // don't add ourselves
        if (m_beetleBase == enemy) return;

        // dont double add
        if (m_nearbyEnemies.Contains(enemy)) return;

        m_nearbyEnemies.Add(enemy);
    }

    void Update()
    {
        if (!IsLocalControl()) return;

        m_timeUntilStateCheck -= Time.deltaTime;
        if(m_timeUntilStateCheck <= 0)
        {
            UpdateState();
            m_timeUntilStateCheck = m_stateUpdateTime;
        }

        UpdateCurrentState();
    }

    // this is run every m_stateUpdateTime and does computation intense stuff like looking for closest enemy
    // this is the only place we switch states
    void UpdateState()
    {
        m_currentState = eBotState.Idle;
        m_currentTarget = null;

        BeetleBase closestEnemy = BeetleMaster.Instance.FindClosestEnemy(m_beetleBase);
        if(closestEnemy != null)
        {
            // we have a close enemy, is it close enough?
            if(Vector3.Distance(transform.position, closestEnemy.transform.position) < m_findTargetDistance)
            {
                // new target
                m_currentState = eBotState.Attack;
                m_currentTarget = closestEnemy;
            }
        }
    }

    // this is run every frame, and just continues to do the same thing as was previously decided
    // it doesn't switch to new states
    void UpdateCurrentState()
    {
        switch (m_currentState)
        {
            case eBotState.Idle:
                RandomIdleMovement();
                break;
            case eBotState.Attack:
                //MoveTorwardsTarget();
                RandomIdleMovement();
                break;
        }
    }
    
    float m_jumpWaitAI = 3f;
    float m_jumpDelayMax = 3f;

    float m_targetIdleRotation = 0;
    float m_waitTimeAfterIdleRotate = 0;
    float m_maxWaitTimeAfterIdleRotate = 5f;
    float m_idleTurnSlow = 0.25f;

    void RandomIdleMovement()
    {
        Vector2 forceVector = Vector3.zero;
        Vector3 eulerRotation = transform.localRotation.eulerAngles;

        if (m_beetleBase.m_jumpCooldown + m_beetleBase.m_lastJumpTime + m_jumpWaitAI < Time.time)
        {
            // jump
            forceVector.y += m_beetleBase.m_jumpForce;
            m_beetleBase.m_lastJumpTime = Time.time;
            m_jumpWaitAI = UnityEngine.Random.Range(0, m_jumpDelayMax);
            m_beetleBase.PlayJumpAnimation();
        }

        // rotate randomly
        if (Mathf.Abs(eulerRotation.z - m_targetIdleRotation) < 10f)
        {
            m_waitTimeAfterIdleRotate -= Time.deltaTime;
            if (m_waitTimeAfterIdleRotate < 0)
            {
                // new target
                m_targetIdleRotation = UnityEngine.Random.Range(0, 360);
                m_waitTimeAfterIdleRotate = UnityEngine.Random.Range(0, m_maxWaitTimeAfterIdleRotate);
            }
        }
        else
        {
            if (eulerRotation.z < m_targetIdleRotation)
            {
                eulerRotation.z += m_beetleBase.m_turnSpeed * Time.deltaTime * m_idleTurnSlow;
            }
            else
            {
                eulerRotation.z -= m_beetleBase.m_turnSpeed * Time.deltaTime * m_idleTurnSlow;
            }
        }

        // rotate the movement and force vector to the forward facing direction
        Quaternion facingRotationQuaternion = Quaternion.Euler(eulerRotation);
        forceVector = facingRotationQuaternion * forceVector;

        transform.localRotation = Quaternion.Euler(eulerRotation);
        m_body.AddForce(forceVector);
    }

    void MoveTorwardsTarget()
    {

    }
}
