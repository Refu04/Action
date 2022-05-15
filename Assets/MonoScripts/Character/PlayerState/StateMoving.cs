using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//移動中
public class StateMoving : PlayerStateBase
{
    private float speed;
    public override void OnEnter(PlayerCore owner, PlayerStateBase prevState)
    {
        Debug.Log("StateMoving");
    }

    public override void OnUpdate(PlayerCore owner)
    {
        //スピードの取得
        var speed = owner.InputEventProvider.MoveDirection.Value.x;
        //アニメーターにスピード値をセットする
        owner.Anim.SetFloat("Speed", Mathf.Abs(speed));
        //左右の向きの切り替え
        if(speed > 0)
        {
            owner.transform.rotation = Quaternion.Euler(0, 180, 0);
        } else if(speed < 0)
        {
            owner.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        //移動処理
        owner.Rb.velocity = new Vector3(
            owner.InputEventProvider.MoveDirection.Value.x * 10f,
            owner.Rb.velocity.y,
            0);

        //ステート遷移処理
        //スピードが０になったらStateStandingに遷移する
        if (Mathf.Abs(speed) <= 0)
        {
            owner.ChangeState(owner.StateStanding);
        }
        //ジャンプしたらStateJumpingに遷移する
        if (owner.InputEventProvider.IsJumping.Value)
        {
            owner.ChangeState(owner.StateJumping);
        }
        //地面についていなかったらStateJumpingに遷移する
        if (!owner.IsGrounded)
        {
            owner.ChangeState(owner.StateJumping);
        }
    }
}
