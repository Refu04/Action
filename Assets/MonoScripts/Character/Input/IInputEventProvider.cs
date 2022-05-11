using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public interface IInputEventProvider
{
    //ˆÚ“®‘€ì
    IReadOnlyReactiveProperty<Vector3> MoveDirection { get; }
    //ƒWƒƒƒ“ƒv
    IReadOnlyReactiveProperty<bool> IsJumping { get; }
}
