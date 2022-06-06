using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBlinking : PlayerStateBase
{
    float time;
    public override void OnEnter(PlayerCore owner, PlayerStateBase prevState)
    {
        //�u�����N�J�n
        owner.Anim.SetBool("IsBlinking", true);
        owner.Rb.useGravity = false;
        //�^�C�}�[���Z�b�g
        time = 0;
        //�u�����N����
        //�ړ��l�����͂���Ă��邩
        if (owner.InputEventProvider.MoveDirection.Value.x == 0)
        {
            //�����Ă�������Ƀu�����N����
            if(owner.IsRight)
            {
                owner.Rb.velocity = new Vector3(10, 0, 0);
            } else
            {
                owner.Rb.velocity = new Vector3(-10, 0, 0);
            }
            
        }
        //���͂���Ă�������Ƀu�����N����
        else if(owner.InputEventProvider.MoveDirection.Value.x > 0)
        {
            owner.Rb.velocity = new Vector3(10, 0, 0);
        } else
        {
            owner.Rb.velocity = new Vector3(-10, 0, 0);
        }
    }

    public override void OnUpdate(PlayerCore owner)
    {
        time += Time.deltaTime;
        //�u�����N�I��
        if (time > 0.3f)
        {
            owner.Anim.SetBool("IsBlinking", false);
            owner.Rb.useGravity = true;
            owner.Rb.velocity = Vector3.zero;
            owner.ChangeState(owner.StateStanding);
        }
    }
}
