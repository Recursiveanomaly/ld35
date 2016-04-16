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

    void UpdateMovement()
    {
        Vector2 movementVector = Vector3.zero;
        Vector2 forceVector = Vector3.zero;
        Vector3 eulerRotation = transform.localRotation.eulerAngles;

        // rotate using left/right
        if (Input.GetAxisRaw("Horizontal") > 0.5f)
        {
            eulerRotation.z -= m_turnSpeed;
        }
        else if (Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            eulerRotation.z += m_turnSpeed;
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
        if(Input.GetButtonDown("Jump"))
        {
            forceVector.y += m_jumpForce;
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
