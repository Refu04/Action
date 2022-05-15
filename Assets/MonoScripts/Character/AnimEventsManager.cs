using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventsManager : MonoBehaviour
{
    private PlayerCore player;
    private void Start()
    {
        player = transform.root.GetComponent<PlayerCore>();
    }
    private void EndClimb()
    {
        
        transform.position = player.Anim.rootPosition;
        player.Anim.SetBool("IsClimbing", false);
    }
}
