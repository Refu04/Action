using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private PlayerCore player;
    private BoxCollider col;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerCore>();
        col = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if(player.Rb.velocity.y < 0.1f && transform.position.y + 0.3f < player.transform.position.y)
        {
            col.enabled = true;
        } else
        {
            col.enabled = false;
        }
    }
}
