using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindArea : MonoBehaviour
{
    [SerializeField]
    private float force;
    [SerializeField]
    private bool x;

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            var rb = other.GetComponent<Rigidbody>();
            if(x)
            {
                rb.velocity = new Vector3(force, rb.velocity.y, 0);
            } else
            {
                rb.velocity = new Vector3(rb.velocity.x, force, 0);
            }
            
        }
    }
}
