using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateStanding : PlayerStateBase
{
    public override void OnEnter(PlayerCore owner, PlayerStateBase prevState)
    {
        Debug.Log("StateStanding");
        owner.Anim.SetFloat("JumpSpeed", 0);
    }

    public override void OnUpdate(PlayerCore owner)
    {
        //?????L?[????????????????StateMoving???J??????
        if (Mathf.Abs(owner.InputEventProvider.MoveDirection.Value.x) > 0)
        {
            owner.ChangeState(owner.StateMoving);
        }
        //?X?s?[?h??????
        var speed = owner.InputEventProvider.MoveDirection.Value.x;
        //?A?j???[?^?[???X?s?[?h?l???Z?b?g????
        owner.Anim.SetFloat("Speed", Mathf.Abs(speed));

        //?X?e?[?g?J??????
        //?W?????v??????StateJumping???J??????
        if (owner.InputEventProvider.IsJumping.Value)
        {
            owner.ChangeState(owner.StateJumping);
        }
        //�n�ʂɂ��Ă��Ȃ�������StateJumping�ɑJ�ڂ���
        if (!owner.IsGrounded.Value)
        {
            owner.ChangeState(owner.StateJumping);
        }
        //?????X?L???{?^??????????????State***???J??
        if (owner.InputEventProvider.MoveSkill.Value && owner.MoveSkillCount < 1)
        {
            owner.ChangeState(owner.StateBlinking);
        }
        if(owner.InputEventProvider.IsAttacking.Value)
        {
            owner.ChangeState(owner.StateAttacking);
        }
    }
}
