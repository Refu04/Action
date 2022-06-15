using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class InputEventProviderImpl : MonoBehaviour, IInputEventProvider
{
    public IReadOnlyReactiveProperty<Vector3> MoveDirection => move;
    public IReadOnlyReactiveProperty<bool> IsJumping => isJumping;
    public IReadOnlyReactiveProperty<bool> MoveSkill => moveSkill;
    public IReadOnlyReactiveProperty<bool> IsAttacking => isAttacking;

    private readonly ReactiveProperty<Vector3> move = new ReactiveProperty<Vector3>();
    private readonly ReactiveProperty<bool> isJumping = new ReactiveProperty<bool>();
    private readonly ReactiveProperty<bool> moveSkill = new ReactiveProperty<bool>();
    private readonly ReactiveProperty<bool> isAttacking = new ReactiveProperty<bool>();
    

    private void Update()
    {
        //?W?????v????
        isJumping.Value = Input.GetKeyDown(KeyCode.Space);
        //?????X?L??????
        moveSkill.Value = Input.GetKeyDown(KeyCode.E);
        //???????????????l?????m????
        move.SetValueAndForceNotify(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")));
        isAttacking.Value = Input.GetMouseButtonDown(0);
    }
}
