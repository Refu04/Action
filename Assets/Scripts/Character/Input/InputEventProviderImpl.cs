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

    //???????????????????????????????????????????
    //??????
    [SerializeField]
    private float[] acceptTime;
    //????????
    [SerializeField]
    private float[] inputStartTime;
    //????????
    private float comboTimeCount;

    private void Start()
    {
        //attackNum.Subscribe(_ => comboTimeCount = 0);
    }

    private void Update()
    {
        //?W?????v????
        isJumping.Value = Input.GetKeyDown(KeyCode.Space);
        //?????X?L??????
        moveSkill.Value = Input.GetKeyDown(KeyCode.E);
        //???????????????l?????m????
        move.SetValueAndForceNotify(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")));
        isAttacking.Value = Input.GetMouseButtonDown(0);
        ////????
        //if(Input.GetMouseButtonDown(0) && attackNum.Value < acceptTime.Length - 1)
        //{
        //    if(attackNum.Value == 0)
        //    {
        //        attackNum.Value++;
        //    } else
        //    {
        //        if(comboTimeCount > inputStartTime[attackNum.Value])
        //        {
        //            attackNum.Value++;
        //        }
        //    }
            
        //}

        ////????????????????
        //if(attackNum.Value > 0)
        //{
        //    comboTimeCount += Time.deltaTime;
        //    if(comboTimeCount >= acceptTime[AttackNum.Value])
        //    {
        //        comboTimeCount = 0;
        //        attackNum.Value = 0;
        //    }
        //}
    }
}
