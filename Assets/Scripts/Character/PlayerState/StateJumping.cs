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
        //接地しているか
        if(owner.IsGrounded.Value)
        {
            //ジャンプ
            owner.Rb.velocity += Vector3.up * owner.JumpSpeed;
        }
        //前ステートがWallSlidingかどうか
        if(prevState == owner.StateWallSliding)
        {
            //壁ジャンプ
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
        //CancellationTokenSourceの生成
        cts = new CancellationTokenSource();
        //着地判定開始
        CheckGrounded(owner, cts.Token).Forget();
        //ジャンプ中だけ重力を変える
        owner.Rb.useGravity = false;
        //重力をかける
        AddGravity(owner, cts.Token).Forget();
        Debug.Log("StateJumping");
    }

    public override void OnUpdate(PlayerCore owner)
    {
        if(canMove)
        {
            //移動処理
            var xInput = owner.InputEventProvider.MoveDirection.Value.x;
            
            if (owner.Rb.velocity.x <= 4 && owner.Rb.velocity.x >= -4)
            {
                owner.Rb.velocity = new Vector3(xInput * 4, owner.Rb.velocity.y, 0);
                //振り返った時の処理
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
            //崖掴まり判定
            var startHeightOffset = 1.2f;
            var armLength = 0.5f;
            var rayOffset = owner.IsRight ? -0.1f : 0.1f;
            var posOffset = owner.IsRight ? 0.5f : -0.5f;
            //首辺りからrayを飛ばす
            Debug.DrawRay(owner.transform.position + new Vector3(0, startHeightOffset, 0), owner.transform.forward * armLength, Color.red);
            if (Physics.Raycast(owner.transform.position + new Vector3(0, startHeightOffset, 0), owner.transform.forward, out wallHit, armLength, owner.GroundMask))
            {
                //Staticでなければ掴まない
                //if(!wallHit.collider.gameObject.isStatic)
                //{
                //    return;
                //}
                //頭の上辺りからrayを飛ばす
                if (!Physics.Raycast(new Vector3(owner.transform.position.x, wallHit.point.y + armLength + 0.5f, owner.transform.position.z), owner.transform.forward, 1f))
                {
                    Debug.DrawRay(new Vector3(owner.transform.position.x, wallHit.point.y + armLength + 0.5f, owner.transform.position.z), owner.transform.forward * 1, Color.red);
                    //2本目のrayが何にも当たらなければ
                    //2本目のrayの到達地点から真下に向かって3本目のrayを出す
                    if (Physics.Raycast(new Vector3(owner.transform.position.x + posOffset, wallHit.point.y + armLength + 0.5f, owner.transform.position.z), Vector3.down, out cliffHit, armLength + 0.6f))
                    {
                        owner.transform.position = new Vector3(
                            wallHit.point.x,
                            cliffHit.point.y - 1.5f,
                            wallHit.point.z
                            );
                        //加速度を無くす
                        owner.Rb.velocity = Vector3.zero;
                        //StateClimingに移行
                        owner.ChangeState(owner.StateClimbing);
                    }


                }
            }
        }

        //移動スキルボタンが押されたらState***に遷移
        if (owner.InputEventProvider.MoveSkill.Value && owner.MoveSkillCount < 1)
        {
            owner.ChangeState(owner.StateBlinking);
        }
    }

    public override void OnExit(PlayerCore owner, PlayerStateBase nextState)
    {
        //重力の設定を戻す
        owner.Rb.useGravity = true;
        //止まる
        owner.Rb.velocity = Vector3.zero;
        //UniTaskをキャンセルする
        cts.Cancel();
    }
    

    private async UniTask CheckGrounded(PlayerCore owner, CancellationToken token)
    {
        //ジャンプ直後に着地判定を行わないように遅らせる
        await UniTask.Delay(100);
        while (true)
        {
            await UniTask.Yield(PlayerLoopTiming.Update, token);
            
            //着地したらStateStandingに遷移する
            if (owner.IsGrounded.Value)
            {
                owner.Anim.SetFloat("JumpSpeed", 0);
                owner.Anim.SetBool("IsWallSliding", false);
                owner.ChangeState(owner.StateStanding);
                break;
            }
            //壁スライド判定
            RaycastHit wallSlideHit;
            var slide = Physics.Raycast(owner.transform.position + new Vector3(0, 1.4f, 0), owner.transform.forward, out wallSlideHit, 0.2f, owner.GroundMask);
            
            //壁スライドステートに移行
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
            //更新タイミングをFixedUpdateに合わせる
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate, token);
            //重力をかける
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
