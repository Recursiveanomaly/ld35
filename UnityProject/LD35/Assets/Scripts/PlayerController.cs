﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D m_body;
    PhotonView m_photonView;
    public BeetleBase m_beetleBase;

    Vector3 m_cameraOffset = new Vector3(0, 0, -20);

    void Awake()
    {
        m_body = GetComponent<Rigidbody2D>();
        m_photonView = GetComponent<PhotonView>();
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        if (m_photonView.isMine == false || m_beetleBase.m_dead)
        {
            return;
        }

        Camera.main.transform.position = transform.position + m_cameraOffset;

        UpdateMovement();
    }

    void UpdateMovement()
    {
        Vector2 movementVector = Vector3.zero;
        Vector2 forceVector = Vector3.zero;
        Vector3 eulerRotation = transform.localRotation.eulerAngles;

        // rotate using left/right
        if (Input.GetAxisRaw("Horizontal") > 0.5f)
        {
            eulerRotation.z -= m_beetleBase.m_turnSpeed * Time.deltaTime;
            m_beetleBase.PlayTurnRightAnimation();
        }
        else if (Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            eulerRotation.z += m_beetleBase.m_turnSpeed * Time.deltaTime;
            m_beetleBase.PlayTurnLeftAnimation();
        }
        else
        {
            m_beetleBase.StopSpinSFX();
        }

        // jump!
        if((Input.GetButton("Jump") || Input.GetAxisRaw("Vertical") > 0.5f) && m_beetleBase.m_lastJumpTime + m_beetleBase.m_jumpCooldown < Time.time)
        {
            forceVector.y += m_beetleBase.m_jumpForce;
            m_beetleBase.m_lastJumpTime = Time.time;
            m_beetleBase.PlayJumpAnimation();
        }
        else if (Input.GetAxisRaw("Vertical") < -0.5f)
        {
            movementVector.y = -m_beetleBase.m_backSpeed * Time.deltaTime;
        }

        // rotate the movement and force vector to the forward facing direction
        Quaternion facingRotationQuaternion = Quaternion.Euler(eulerRotation);
        movementVector = facingRotationQuaternion * movementVector;
        forceVector = facingRotationQuaternion * forceVector;

        transform.localRotation = Quaternion.Euler(eulerRotation);
        m_body.velocity += movementVector;
        m_body.AddForce(forceVector);
    }
}
