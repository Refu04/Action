using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            var rb = other.GetComponent<Rigidbody>();
            rb.velocity = transform.up * 20;
        }
    }
}
