using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class AirTrain : MonoBehaviour
{
    [SerializeField]
    private Vector3[] destinations;
    [SerializeField]
    private float speed;
    private int destNum;
    private Vector3 prevPos;
    private Vector3 initPos;
    private bool isOnPlayer;
    private bool start;

    // Start is called before the first frame update
    void Start()
    {
        destNum = 0;
        prevPos = transform.position;
        initPos = transform.position;
        PlayerCore.OnDead.Subscribe(_ =>
        {
            destNum = 0;
            transform.position = initPos;
            prevPos = transform.position;
            start = false;
        });
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!start)
        {
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, prevPos + destinations[destNum], speed);
        //if(Vector3.Distance(transform.position, prevPos + destinations[destNum]) < 0.1f)
        //{
        //    destNum++;
        //    prevPos = transform.position;
        //    if(destNum == destinations.Length)
        //    {
        //        destNum = 0;
        //        transform.position = initPos;
        //        prevPos = transform.position;
        //    }
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player" && !isOnPlayer)
        {
            var emptyObj = new GameObject();
            collision.transform.parent = emptyObj.transform;
            emptyObj.transform.parent = transform;
            isOnPlayer = true;
        }
        start = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.parent = null;
            isOnPlayer = false;
        }
    }
}
