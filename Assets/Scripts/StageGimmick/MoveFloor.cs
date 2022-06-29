using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MoveFloor : MonoBehaviour
{
    [SerializeField]
    private float movingWidth;
    [SerializeField]
    private float movingHeight;
    private bool isOnPlayer;
    private void Start()
    {
        isOnPlayer = false;
    }

    private void FixedUpdate()
    {
        transform.Translate((Mathf.Sin(Time.time) / 100) * movingWidth, (Mathf.Sin(Time.time) / 100) * movingHeight, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player" && !isOnPlayer)
        {
            var emptyObj = new GameObject();
            collision.transform.parent = emptyObj.transform;
            emptyObj.transform.parent = transform;
            isOnPlayer = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            collision.transform.parent = null;
            isOnPlayer = false;
        }
    }
}