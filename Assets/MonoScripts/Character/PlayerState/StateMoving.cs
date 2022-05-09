using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ړ���
public class StateMoving : PlayerStateBase
{
    public override void OnEnter(PlayerCore owner, PlayerStateBase prevState)
    {
        
    }

    public override void OnUpdate(PlayerCore owner)
    {
        //�X�s�[�h�̎擾
        var speed = owner.InputEventProvider.MoveDirection.Value.x;
        //�A�j���[�^�[�ɃX�s�[�h�l���Z�b�g����
        owner.Anim.SetFloat("Speed", Mathf.Abs(speed));
        //���E�̌����̐؂�ւ�
        if(speed > 0)
        {
            owner.transform.rotation = Quaternion.Euler(0, 180, 0);
        } else if(speed < 0)
        {
            owner.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        //�X�s�[�h���O�ɂȂ�����StateStanding�ɑJ�ڂ���
        if(Mathf.Abs(speed) <= 0)
        {
            owner.ChangeState(owner.StateStanding);
        }
    }
}
