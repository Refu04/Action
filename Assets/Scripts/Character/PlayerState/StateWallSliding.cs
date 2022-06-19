using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateWallSliding : PlayerStateBase
{
    public override void OnEnter(PlayerCore owner, PlayerStateBase prevState)
    {
        owner.Rb.useGravity = false;
        owner.Rb.velocity = Vector3.zero;
        owner.Anim.SetBool("IsWallSliding", true);
        owner.MoveSkillCount = 0;
        Debug.Log("StateWallSliding");
    }

    public override void OnUpdate(PlayerCore owner)
    {
        if (owner.IsRight && owner.InputEventProvider.MoveDirection.Value.x > 0 || !owner.IsRight && owner.InputEventProvider.MoveDirection.Value.x < 0)
        {
            owner.transform.Translate(0, -0.2f * Time.deltaTime, 0);
        }
        else
        {
            owner.ChangeState(owner.StateStanding);
        }

        if (owner.InputEventProvider.IsJumping.Value)
        {
            owner.ChangeState(owner.StateJumping);
        }
    }

    public override void OnExit(PlayerCore owner, PlayerStateBase nextState)
    {
        owner.Rb.useGravity = true;
        owner.Anim.SetBool("IsWallSliding", false);
    }
}
