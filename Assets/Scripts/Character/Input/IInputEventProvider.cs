using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public interface IInputEventProvider
{
    //????????
    IReadOnlyReactiveProperty<Vector3> MoveDirection { get; }
    //?W?????v
    IReadOnlyReactiveProperty<bool> IsJumping { get; }
    //?????X?L??
    IReadOnlyReactiveProperty<bool> MoveSkill { get; }
    //
    IReadOnlyReactiveProperty<bool> IsAttacking { get; }
}
