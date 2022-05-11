using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public interface IInputEventProvider
{
    //�ړ�����
    IReadOnlyReactiveProperty<Vector3> MoveDirection { get; }
    //�W�����v
    IReadOnlyReactiveProperty<bool> IsJumping { get; }
}
