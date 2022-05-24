using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public interface IInputEventProvider
{
    //移動操作
    IReadOnlyReactiveProperty<Vector3> MoveDirection { get; }
    //ジャンプ
    IReadOnlyReactiveProperty<bool> IsJumping { get; }
    //移動スキル
    IReadOnlyReactiveProperty<bool> MoveSkill { get; }
}
