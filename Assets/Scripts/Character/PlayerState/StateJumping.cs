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
    private RaycastHit wallHit;
    private RaycastHit cliffHit;
    private bool canMove = true;

    public override void OnEnter(PlayerCore owner, PlayerStateBase prevState)
    {
        //�ڒn���Ă��邩
        if(owner.IsGrounded.Value)
        {
            //�W�����v
            owner.Rb.velocity += Vector3.up * owner.JumpSpeed;
        }
        //�O�X�e�[�g��WallSliding���ǂ���
        if(prevState == owner.StateWallSliding)
        {
            //�ǃW�����v
            owner.Rb.velocity = Vector3.zero;
            owner.Rb.velocity += Vector3.up * owner.JumpSpeed;
            canMove = false;
            toggleCanMove(400, 0).Forget();
            if (owner.IsRight)
            {
                owner.Rb.velocity += Vector3.left * (owner.JumpSpeed / 3);
                owner.transform.rotation = Quaternion.Euler(0, -90, 0);
            } else
            {
                owner.Rb.velocity += Vector3.right * (owner.JumpSpeed / 3);
                owner.transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            
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
        if(canMove)
        {
            //�ړ�����
            var xInput = owner.InputEventProvider.MoveDirection.Value.x;
            
            if (owner.Rb.velocity.x <= 4 && owner.Rb.velocity.x >= -4)
            {
                owner.Rb.velocity = new Vector3(xInput * 4, owner.Rb.velocity.y, 0);
                //�U��Ԃ������̏���
                if (xInput > 0)
                {
                    owner.transform.rotation = Quaternion.Euler(0, 90, 0);
                    //owner.Rb.velocity = new Vector3(0, owner.Rb.velocity.y, 0);
                }
                else if (xInput < 0)
                {
                    owner.transform.rotation = Quaternion.Euler(0, -90, 0);
                    //owner.Rb.velocity = new Vector3(0, owner.Rb.velocity.y, 0);
                }
            }
            
        }
        
        if (owner.Rb.velocity.y < 0)
        {
            //�R�͂܂蔻��
            var startHeightOffset = 1.2f;
            var armLength = 0.5f;
            var rayOffset = owner.IsRight ? -0.1f : 0.1f;
            var posOffset = owner.IsRight ? 0.5f : -0.5f;
            //��ӂ肩��ray���΂�
            Debug.DrawRay(owner.transform.position + new Vector3(0, startHeightOffset, 0), owner.transform.forward * armLength, Color.red);
            if (Physics.Raycast(owner.transform.position + new Vector3(0, startHeightOffset, 0), owner.transform.forward, out wallHit, armLength, owner.GroundMask))
            {
                //Static�łȂ���Β͂܂Ȃ�
                //if(!wallHit.collider.gameObject.isStatic)
                //{
                //    return;
                //}
                //���̏�ӂ肩��ray���΂�
                if (!Physics.Raycast(new Vector3(owner.transform.position.x, wallHit.point.y + armLength + 0.5f, owner.transform.position.z), owner.transform.forward, 1f))
                {
                    Debug.DrawRay(new Vector3(owner.transform.position.x, wallHit.point.y + armLength + 0.5f, owner.transform.position.z), owner.transform.forward * 1, Color.red);
                    //2�{�ڂ�ray�����ɂ�������Ȃ����
                    //2�{�ڂ�ray�̓��B�n�_����^���Ɍ�������3�{�ڂ�ray���o��
                    if (Physics.Raycast(new Vector3(owner.transform.position.x + posOffset, wallHit.point.y + armLength + 0.5f, owner.transform.position.z), Vector3.down, out cliffHit, armLength + 0.6f))
                    {
                        owner.transform.position = new Vector3(
                            wallHit.point.x,
                            cliffHit.point.y - 1.5f,
                            wallHit.point.z
                            );
                        //�����x�𖳂���
                        owner.Rb.velocity = Vector3.zero;
                        //StateCliming�Ɉڍs
                        owner.ChangeState(owner.StateClimbing);
                    }


                }
            }
        }

        //�ړ��X�L���{�^���������ꂽ��State***�ɑJ��
        if (owner.InputEventProvider.MoveSkill.Value && owner.MoveSkillCount < 1)
        {
            owner.ChangeState(owner.StateBlinking);
        }
    }

    public override void OnExit(PlayerCore owner, PlayerStateBase nextState)
    {
        //�d�͂̐ݒ��߂�
        owner.Rb.useGravity = true;
        //�~�܂�
        owner.Rb.velocity = Vector3.zero;
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
            //�ǃX���C�h����
            RaycastHit wallSlideHit;
            var slide = Physics.Raycast(owner.transform.position + new Vector3(0, 1.4f, 0), owner.transform.forward, out wallSlideHit, 0.2f, owner.GroundMask);
            
            //�ǃX���C�h�X�e�[�g�Ɉڍs
            if (slide/* && wallSlideHit.transform.gameObject.isStatic*/)
            {
                
                if (owner.InputEventProvider.MoveDirection.Value.x != 0)
                {
                    owner.ChangeState(owner.StateWallSliding);
                }
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

    private async UniTask toggleCanMove(int beforeDelay, int afterDelay)
    {
        await UniTask.Delay(beforeDelay);
        if(canMove)
        {
            canMove = false;
        } else
        {
            canMove = true;
        }
        await UniTask.Delay(afterDelay);
    }
}
