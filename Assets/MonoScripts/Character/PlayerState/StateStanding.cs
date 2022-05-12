using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateStanding : PlayerStateBase
{
    public override void OnEnter(PlayerCore owner, PlayerStateBase prevState)
    {
        Debug.Log("StateStanding");
    }

    public override void OnUpdate(PlayerCore owner)
    {
        //�ړ��L�[��������Ă����StateMoving�ɑJ�ڂ���
        if (Mathf.Abs(owner.InputEventProvider.MoveDirection.Value.x) > 0)
        {
            owner.ChangeState(owner.StateMoving);
        }
        //�X�s�[�h�̎擾
        var speed = owner.InputEventProvider.MoveDirection.Value.x;
        //�A�j���[�^�[�ɃX�s�[�h�l���Z�b�g����
        owner.Anim.SetFloat("Speed", Mathf.Abs(speed));

        //�X�e�[�g�J�ڏ���
        //�W�����v������StateJumping�ɑJ�ڂ���
        if (owner.InputEventProvider.IsJumping.Value)
        {
            owner.ChangeState(owner.StateJumping);
        }
    }
}
