using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class InputEventProviderImpl : MonoBehaviour, IInputEventProvider
{
    public IReadOnlyReactiveProperty<Vector3> MoveDirection => move;
    public IReadOnlyReactiveProperty<bool> IsJumping => isJumping;

    private readonly ReactiveProperty<Vector3> move = new ReactiveProperty<Vector3>();
    private readonly ReactiveProperty<bool> isJumping = new ReactiveProperty<bool>();
    

    private void Update()
    {
        isJumping.Value = Input.GetKeyDown(KeyCode.Space);
        //ˆÚ“®‘€ì‚Ì“ü—Í’l‚ğ’Ê’m‚·‚é
        move.SetValueAndForceNotify(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")));
    }
}
