using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateStanding : PlayerStateBase
{
    public override void OnEnter(PlayerCore owner, PlayerStateBase prevState)
    {
        Debug.Log("StateStanding");
        owner.Anim.SetFloat("JumpSpeed", 0);
    }

    public override void OnUpdate(PlayerCore owner)
    {
        //移動キーが押されていればStateMovingに遷移する
        if (Mathf.Abs(owner.InputEventProvider.MoveDirection.Value.x) > 0)
        {
            owner.ChangeState(owner.StateMoving);
        }
        //スピードの取得
        var speed = owner.InputEventProvider.MoveDirection.Value.x;
        //アニメーターにスピード値をセットする
        owner.Anim.SetFloat("Speed", Mathf.Abs(speed));

        //ステート遷移処理
        //ジャンプしたらStateJumpingに遷移する
        if (owner.InputEventProvider.IsJumping.Value)
        {
            owner.ChangeState(owner.StateJumping);
        }
        //移動スキルボタンが押されたらState***に遷移
        if (owner.InputEventProvider.MoveSkill.Value && owner.MoveSkillCount < 1)
        {
            owner.ChangeState(owner.StateBlinking);
        }
    }
}
