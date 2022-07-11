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
                //ã¸§ŒÀ
                if (rb.velocity.y >= 14)
                {
                    rb.velocity = new Vector3(rb.velocity.x, 14f, 0f);
                }
            }
            
        }
    }
}
