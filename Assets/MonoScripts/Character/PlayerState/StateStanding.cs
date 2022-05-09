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
        //ˆÚ“®ƒL[‚ª‰Ÿ‚³‚ê‚Ä‚¢‚ê‚ÎStateMoving‚É‘JˆÚ‚·‚é
        if (Mathf.Abs(owner.InputEventProvider.MoveDirection.Value.x) > 0)
        {
            owner.ChangeState(owner.StateMoving);
        }
    }
}
