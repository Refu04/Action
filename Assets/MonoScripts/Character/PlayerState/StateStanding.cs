using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateStanding : PlayerStateBase
{
    public override void OnEnter(PlayerCore owner, PlayerStateBase prevState)
    {

    }

    public override void OnUpdate(PlayerCore owner)
    {
        //移動キーが押されていればStateMovingに遷移する
        if (Mathf.Abs(owner.InputEventProvider.MoveDirection.Value.x) > 0)
        {
            owner.ChangeState(owner.StateMoving);
        }
    }
}
