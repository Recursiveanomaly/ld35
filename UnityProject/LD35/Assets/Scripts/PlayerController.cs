using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float m_forwardSpeed;
    public float m_backwordSpeed;
    public float m_turnSpeed;
    public float m_jumpForce;
    public float m_jumpCooldown;

    Rigidbody2D m_body;
    PhotonView m_photonView;
    public BeetleBase m_beetleBase;
    
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
        if (m_photonView.isMine == false)
        {
            return;
        }

        UpdateMovement();
    }

    float m_lastJumpTime = 0;

    void UpdateMovement()
    {
        Vector2 movementVector = Vector3.zero;
        Vector2 forceVector = Vector3.zero;
        Vector3 eulerRotation = transform.localRotation.eulerAngles;

        // rotate using left/right
        if (Input.GetAxisRaw("Horizontal") > 0.5f)
        {
            eulerRotation.z -= m_turnSpeed;
            m_beetleBase.PlayTurnLeftAnimation();
        }
        else if (Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            eulerRotation.z += m_turnSpeed;
            m_beetleBase.PlayTurnRightAnimation();
        }

        // forward motion
        if (m_body.velocity.magnitude < m_forwardSpeed)
        {
            if (Input.GetAxisRaw("Vertical") > 0.5f)
            {
                movementVector.y = m_forwardSpeed;
            }
            else if (Input.GetAxisRaw("Vertical") < -0.5f)
            {
                movementVector.y = -m_backwordSpeed;
            }
        }

        // jump!
        if(Input.GetButtonDown("Jump") && m_lastJumpTime + m_jumpCooldown < Time.time)
        {
            forceVector.y += m_jumpForce;
            m_lastJumpTime = Time.time;
            m_beetleBase.PlayJumpAnimation();
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
