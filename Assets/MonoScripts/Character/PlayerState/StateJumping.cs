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
        //接地しているか
        if(owner.IsGrounded)
        {
            //ジャンプ
            owner.Rb.velocity += Vector3.up * owner.JumpSpeed;
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
        //AnimatorのJumpSpeedパラメータに加速度の値を割り振る
        owner.Anim.SetFloat("JumpSpeed", owner.Rb.velocity.y);
        //崖捕まり判定
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
        //重力の設定を戻す
        owner.Rb.useGravity = true;
        //UniTaskをキャンセルする
        cts.Cancel();
    }

    private async UniTask CheckGrounded(PlayerCore owner, CancellationToken token)
    {
        //ジャンプ直後に着地判定を行わないように遅らせる
        await UniTask.Delay(100);
        while(true)
        {
            await UniTask.Yield(PlayerLoopTiming.Update, token);
            //着地したらStateStandingに遷移する
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
            //更新タイミングをFixedUpdateに合わせる
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate, token);
            //重力をかける
            owner.Rb.AddForce(new Vector3(0, -38f, 0), ForceMode.Acceleration);
        }
    }
    
}
