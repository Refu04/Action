using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBlinking : PlayerStateBase
{
    float time;
    public override void OnEnter(PlayerCore owner, PlayerStateBase prevState)
    {
        //ブリンク開始
        owner.Anim.SetBool("IsBlinking", true);
        owner.Rb.useGravity = false;
        //タイマーリセット
        time = 0;
        //ブリンク処理
        //移動値が入力されているか
        if (owner.InputEventProvider.MoveDirection.Value.x == 0)
        {
            //向いている方向にブリンクする
            if(owner.IsRight)
            {
                owner.Rb.velocity = new Vector3(10, 0, 0);
            } else
            {
                owner.Rb.velocity = new Vector3(-10, 0, 0);
            }
            
        }
        //入力されている方向にブリンクする
        else if(owner.InputEventProvider.MoveDirection.Value.x > 0)
        {
            owner.Rb.velocity = new Vector3(10, 0, 0);
        } else
        {
            owner.Rb.velocity = new Vector3(-10, 0, 0);
        }
    }

    public override void OnUpdate(PlayerCore owner)
    {
        time += Time.deltaTime;
        //ブリンク終了
        if (time > 0.3f)
        {
            owner.Anim.SetBool("IsBlinking", false);
            owner.Rb.useGravity = true;
            owner.Rb.velocity = Vector3.zero;
            owner.ChangeState(owner.StateStanding);
        }
    }
}
