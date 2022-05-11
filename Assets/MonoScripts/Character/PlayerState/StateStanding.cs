using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateStanding : PlayerStateBase
{
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
    }
}
