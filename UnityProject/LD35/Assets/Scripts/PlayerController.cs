using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float m_speed;
    public float m_turnSpeed;

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
        Vector3 eulerRotation = transform.localRotation.eulerAngles;

        if (Input.GetAxisRaw("Horizontal") > 0.5f)
        {
            eulerRotation.z -= m_turnSpeed;
        }
        else if (Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            eulerRotation.z += m_turnSpeed;
        }
        
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

        // rotate the movement vector to the forward facing direction
        movementVector = Quaternion.Euler(eulerRotation) * movementVector;

        transform.localRotation = Quaternion.Euler(eulerRotation);
        m_body.velocity = movementVector;
    }
}
