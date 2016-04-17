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
    float m_stateUpdateTime = 3;
    float m_timeUntilStateCheck;

    eBotState m_currentState = eBotState.Idle;
    BeetleBase m_currentTarget;

    List<BeetleBase> m_nearbyEnemies = new List<BeetleBase>();

    void Awake()
    {
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
                MoveTorwardsTarget();
                break;
        }
    }

    void RandomIdleMovement()
    {
        //if(m_beetleBase)
    }

    void MoveTorwardsTarget()
    {

    }
}
