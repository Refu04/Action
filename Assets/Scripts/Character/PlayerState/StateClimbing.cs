using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateClimbing : PlayerStateBase
{
    public override void OnEnter(PlayerCore owner, PlayerStateBase prevState)
    {
        //ŠR“o‚èŠJŽn
        owner.Anim.SetBool("IsClimbing", true);
        owner.Rb.useGravity = false;
        owner.Col.enabled = false;
    }

    public override void OnUpdate(PlayerCore owner)
    {
        //ŠR“o‚èI—¹
        var posOffset = owner.IsRight ? 0.3f : -0.3f;
        if (owner.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            owner.Anim.SetBool("IsClimbing", false);
            owner.transform.position = new Vector3(owner.transform.position.x + posOffset, owner.transform.position.y + 1.5f, owner.transform.position.z);
            owner.Rb.useGravity = true;
            owner.Col.enabled = true;
            owner.ChangeState(owner.StateStanding);
        }
    }

}
