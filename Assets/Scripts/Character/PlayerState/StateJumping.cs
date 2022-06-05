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

    public override void OnEnter(PlayerCore owner, PlayerStateBase prevState)
    {
        //�ڒn���Ă��邩
        if(owner.IsGrounded.Value)
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
        //�R�߂܂蔻��
        var startHeightOffset = 1.2f;
        var armLength = 0.5f;
        var rayOffset = owner.IsRight ? -0.1f : 0.1f;
        var posOffset = owner.IsRight ? -0f : 0f;
        //��ӂ肩��ray���΂�
        Debug.DrawRay(owner.transform.position + new Vector3(0, startHeightOffset, 0), -owner.transform.right * armLength, Color.red);
        if (Physics.Raycast(owner.transform.position + new Vector3(0, startHeightOffset, 0), -owner.transform.right, out hit, armLength))
        {
            //���̏�ӂ肩��ray���΂�
            if (!Physics.Raycast(new Vector3(owner.transform.position.x, hit.point.y + armLength + 0.5f, owner.transform.position.z), -owner.transform.right, 1f))
            {
                Debug.DrawRay(new Vector3(owner.transform.position.x, hit.point.y + armLength + 0.5f, owner.transform.position.z), -owner.transform.right * 1, Color.red);
                //2�{�ڂ�ray�����ɂ�������Ȃ���ΊR�߂܂�
                 owner.transform.position = new Vector3(
                    hit.point.x + posOffset,
                    hit.collider.transform.position.y + hit.collider.transform.localScale.y / 2 - 1.5f,
                    hit.point.z
                );
                //�����x�𖳂���
                owner.Rb.velocity = Vector3.zero;
                //StateCliming�Ɉڍs
                owner.ChangeState(owner.StateClimbing);
            }
        }
        //���X�e�[�g�ɕ�����
        //�ǃX���C�h����
        var slide = Physics.Raycast(owner.transform.position + new Vector3(0, startHeightOffset + 0.1f, 0), owner.transform.forward, 0.2f);
        //�ǃX���C�h���̏���
        if (slide)
        {
            //�E�������Ă��鎞
            if(owner.IsRight)
            {
                //�E�ɓ��͂��Ă����ꍇ
                if(owner.InputEventProvider.MoveDirection.Value.x > 0)
                {
                    owner.Anim.SetBool("IsWallSliding", true);
                    if (owner.Rb.velocity.y < 0)
                    {
                        owner.Rb.velocity += new Vector3(0, 50f * Time.deltaTime, 0);
                    }
                }
                
            }
            //���������Ă���Ƃ�
            else
            {
                //���ɓ��͂��Ă����ꍇ
                if (owner.InputEventProvider.MoveDirection.Value.x < 0)
                {
                    owner.Anim.SetBool("IsWallSliding", true);
                    if (owner.Rb.velocity.y < 0)
                    {
                        owner.Rb.velocity += new Vector3(0, 50f * Time.deltaTime, 0);
                    }
                }
            }
        } else
        {
            owner.Anim.SetBool("IsWallSliding", false);
        }
        //�ړ��X�L���{�^���������ꂽ��State***�ɑJ��
        if(owner.InputEventProvider.MoveSkill.Value && owner.MoveSkillCount < 1)
        {
            owner.ChangeState(owner.StateBlinking);
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
        while (true)
        {
            await UniTask.Yield(PlayerLoopTiming.Update, token);
            //���n������StateStanding�ɑJ�ڂ���
            if (owner.IsGrounded.Value)
            {
                owner.Anim.SetFloat("JumpSpeed", 0);
                owner.Anim.SetBool("IsWallSliding", false);
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
