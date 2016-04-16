using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float m_speed;
    public float m_turnSpeed;
    public float m_jumpForce;

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
        
        // turn this off for now
        m_speed = 0;
        if (Input.GetAxisRaw("Vertical") > 0.5f)
        {
            movementVector.y = m_speed;
        }
        else if (Input.GetAxisRaw("Vertical") < -0.5f)
        {
            movementVector.y = -m_speed;
        }
        else
        {
            movementVector.y = 0;
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
