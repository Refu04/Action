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
        //接地しているか
        if(owner.IsGrounded.Value)
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
        //崖捕まり判定
        var startHeightOffset = 1.2f;
        var armLength = 0.5f;
        var rayOffset = owner.IsRight ? -0.1f : 0.1f;
        var posOffset = owner.IsRight ? -0f : 0f;
        //首辺りからrayを飛ばす
        Debug.DrawRay(owner.transform.position + new Vector3(0, startHeightOffset, 0), -owner.transform.right * armLength, Color.red);
        if (Physics.Raycast(owner.transform.position + new Vector3(0, startHeightOffset, 0), -owner.transform.right, out hit, armLength))
        {
            //頭の上辺りからrayを飛ばす
            if (!Physics.Raycast(new Vector3(owner.transform.position.x, hit.point.y + armLength + 0.5f, owner.transform.position.z), -owner.transform.right, 1f))
            {
                Debug.DrawRay(new Vector3(owner.transform.position.x, hit.point.y + armLength + 0.5f, owner.transform.position.z), -owner.transform.right * 1, Color.red);
                //2本目のrayが何にも当たらなければ崖捕まり
                 owner.transform.position = new Vector3(
                    hit.point.x + posOffset,
                    hit.collider.transform.position.y + hit.collider.transform.localScale.y / 2 - 1.5f,
                    hit.point.z
                );
                //加速度を無くす
                owner.Rb.velocity = Vector3.zero;
                //StateClimingに移行
                owner.ChangeState(owner.StateClimbing);
            }
        }
        //他ステートに分ける
        //壁スライド判定
        var slide = Physics.Raycast(owner.transform.position + new Vector3(0, startHeightOffset + 0.1f, 0), owner.transform.forward, 0.2f);
        //壁スライド中の処理
        if (slide)
        {
            //右を向いている時
            if(owner.IsRight)
            {
                //右に入力していた場合
                if(owner.InputEventProvider.MoveDirection.Value.x > 0)
                {
                    owner.Anim.SetBool("IsWallSliding", true);
                    if (owner.Rb.velocity.y < 0)
                    {
                        owner.Rb.velocity += new Vector3(0, 50f * Time.deltaTime, 0);
                    }
                }
                
            }
            //左を向いているとき
            else
            {
                //左に入力していた場合
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
        //移動スキルボタンが押されたらState***に遷移
        if(owner.InputEventProvider.MoveSkill.Value && owner.MoveSkillCount < 1)
        {
            owner.ChangeState(owner.StateBlinking);
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
