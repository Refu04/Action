using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class InputEventProviderImpl : MonoBehaviour, IInputEventProvider
{
    public IReadOnlyReactiveProperty<Vector3> MoveDirection => move;
    public IReadOnlyReactiveProperty<bool> IsJumping => isJumping;
    public IReadOnlyReactiveProperty<bool> MoveSkill => moveSkill;

    private readonly ReactiveProperty<Vector3> move = new ReactiveProperty<Vector3>();
    private readonly ReactiveProperty<bool> isJumping = new ReactiveProperty<bool>();
    private readonly ReactiveProperty<bool> moveSkill = new ReactiveProperty<bool>();


    private void Update()
    {
        //ジャンプ操作
        isJumping.Value = Input.GetKeyDown(KeyCode.Space);
        //移動スキル発動
        moveSkill.Value = Input.GetKeyDown(KeyCode.E);
        //移動操作の入力値を通知する
        move.SetValueAndForceNotify(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")));
    }
}
