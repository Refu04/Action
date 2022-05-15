using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerStateBase
{
    // ステートを開始した時に呼ばれる
    public virtual void OnEnter(PlayerCore owner, PlayerStateBase prevState) { }
    // 毎フレーム呼ばれる
    public virtual void OnUpdate(PlayerCore owner) { }
    // ステートを終了した時に呼ばれる
    public virtual void OnExit(PlayerCore owner, PlayerStateBase nextState) { }
}
