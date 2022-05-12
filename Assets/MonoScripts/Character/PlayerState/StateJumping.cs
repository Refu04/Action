using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class StateJumping : PlayerStateBase
{
    private CancellationTokenSource cts;
    private RaycastHit hit;
    private RaycastHit hitTest;

    public override void OnEnter(PlayerCore owner, PlayerStateBase prevState)
    {
        //�ڒn���Ă��邩
        if(owner.IsGrounded)
        {
            //�W�����v
            owner.Rb.velocity += Vector3.up * owner.JumpSpeed;
        }
        //CancellationTokenSource�̐���
        cts = new CancellationTokenSource();
        //���n����J�n
        CheckGrounded(owner, cts.Token).Forget();
        //�W�����v�������d�͂�ς���
        owner.Rb.useGravity = false;
        //�d�͂�������
        AddGravity(owner, cts.Token).Forget();
        Debug.Log("StateJumping");
    }

    public override void OnUpdate(PlayerCore owner)
    {
        //Animator��JumpSpeed�p�����[�^�ɉ����x�̒l������U��
        owner.Anim.SetFloat("JumpSpeed", owner.Rb.velocity.y);
        //�R�߂܂蔻��
        var startHeightOffset = 4f;
        var armLength = 1f;
        Debug.DrawRay(owner.transform.position + new Vector3(0, startHeightOffset, 0), -owner.transform.right * 3, Color.red);
        if (Physics.Raycast(owner.transform.position + new Vector3(0, startHeightOffset, 0), -owner.transform.right, out hit, 3f))
        {
            if (!Physics.Raycast(new Vector3(hit.point.x - 0.1f, hit.point.y + armLength, owner.transform.position.z), -owner.transform.right, out hitTest, 1f))
            {
                Debug.DrawRay(new Vector3(hit.point.x - 0.1f, hit.point.y + armLength, owner.transform.position.z), -owner.transform.right * 1, Color.red);

                owner.Anim.SetBool("isCliming", true);
                owner.transform.position = new Vector3(
                    hit.point.x,
                    hit.collider.transform.position.y + hit.collider.transform.localScale.y / 2,
                    hit.point.z
                );
                owner.Rb.velocity = Vector3.zero;
            }
        }


    }

    public override void OnExit(PlayerCore owner, PlayerStateBase nextState)
    {
        //�d�͂̐ݒ��߂�
        owner.Rb.useGravity = true;
        //UniTask���L�����Z������
        cts.Cancel();
    }

    private async UniTask CheckGrounded(PlayerCore owner, CancellationToken token)
    {
        //�W�����v����ɒ��n������s��Ȃ��悤�ɒx�点��
        await UniTask.Delay(100);
        while(true)
        {
            await UniTask.Yield(PlayerLoopTiming.Update, token);
            //���n������StateStanding�ɑJ�ڂ���
            if (owner.IsGrounded)
            {
                
                owner.ChangeState(owner.StateStanding);
                break;
            }
        }
    }

    private async UniTask AddGravity(PlayerCore owner, CancellationToken token)
    {
        while(true)
        {
            //�X�V�^�C�~���O��FixedUpdate�ɍ��킹��
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate, token);
            //�d�͂�������
            owner.Rb.AddForce(new Vector3(0, -38f, 0), ForceMode.Acceleration);
        }
    }
    
}
