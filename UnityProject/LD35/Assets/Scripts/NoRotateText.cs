using UnityEngine;
using System.Collections;

public class NoRotateText : MonoBehaviour
{
    void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }    
}
