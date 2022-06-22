using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MoveFloor : MonoBehaviour
{
    [SerializeField]
    private float movingWidth;
    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(new Vector3(transform.position.x + (Mathf.Sin(Time.time) / 100) * movingWidth, transform.position.y, transform.position.z));
    }
}