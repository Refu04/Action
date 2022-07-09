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
                    rb.AddForce(force, rb.velocity.y, 0);
                
            } else
            {
                rb.AddForce(rb.velocity.x, force, 0);
                //óéâ∫êßå¿
                //if (rb.velocity.y <= -8f)
                //{
                //    rb.velocity = new Vector3(0f, -8f, 0f);
                //}
                //è„è∏êßå¿
                if (rb.velocity.y >= 14)
                {
                    rb.velocity = new Vector3(0f, 14f, 0f);
                }
            }
            
        }
    }
}
