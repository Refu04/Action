using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//移動中
public class StateMoving : PlayerStateBase
{
    public override void OnEnter(PlayerCore owner, PlayerStateBase prevState)
    {
        
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
        //スピードが０になったらStateStandingに遷移する
        if(Mathf.Abs(speed) <= 0)
        {
            owner.ChangeState(owner.StateStanding);
        }
    }
}
