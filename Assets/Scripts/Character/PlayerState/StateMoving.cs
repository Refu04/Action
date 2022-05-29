using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ړ���
public class StateMoving : PlayerStateBase
{
    private float speed;
    public override void OnEnter(PlayerCore owner, PlayerStateBase prevState)
    {
        Debug.Log("StateMoving");
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
            owner.transform.rotation = Quaternion.Euler(0, 90, 0);
        } else if(speed < 0)
        {
            owner.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        //�ړ�����
        owner.Rb.velocity = new Vector3(
            owner.InputEventProvider.MoveDirection.Value.x * 4f,
            owner.Rb.velocity.y,
            0);

        //�X�e�[�g�J�ڏ���
        //�X�s�[�h���O�ɂȂ�����StateStanding�ɑJ�ڂ���
        if (Mathf.Abs(speed) <= 0)
        {
            owner.ChangeState(owner.StateStanding);
        }
        //�W�����v������StateJumping�ɑJ�ڂ���
        if (owner.InputEventProvider.IsJumping.Value)
        {
            owner.ChangeState(owner.StateJumping);
        }
        //�n�ʂɂ��Ă��Ȃ�������StateJumping�ɑJ�ڂ���
        if (!owner.IsGrounded.Value)
        {
            owner.ChangeState(owner.StateJumping);
        }
        //�ړ��X�L���{�^���������ꂽ��State***�ɑJ��
        if (owner.InputEventProvider.MoveSkill.Value)
        {
            owner.ChangeState(owner.StateBlinking);
        }
    }
}
