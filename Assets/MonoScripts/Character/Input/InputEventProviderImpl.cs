using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class InputEventProviderImpl : MonoBehaviour, IInputEventProvider
{
    public IReadOnlyReactiveProperty<Vector3> MoveDirection => move;

    private readonly ReactiveProperty<Vector3> move = new ReactiveProperty<Vector3>();

    private void Update()
    {
        //ˆÚ“®‘€ì‚Ì“ü—Í’l‚ğ’Ê’m‚·‚é
        move.SetValueAndForceNotify(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
    }
}
