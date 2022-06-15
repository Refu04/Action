using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

public class StateAttacking : PlayerStateBase
{
    private float comboTimeCount;
    private int attackNum;
    private bool attack;
    public override void OnEnter(PlayerCore owner, PlayerStateBase prevState)
    {
        attackNum = 0;
        attack = false;
        comboTimeCount = 0;
        var cts = new CancellationTokenSource();
        OnAttack(owner, cts.Token).Forget();
        Debug.Log("StateAttacking");

    }

    public override void OnUpdate(PlayerCore owner)
    {
        comboTimeCount += Time.deltaTime;
        owner.Anim.SetInteger("AttackNum", attackNum);
    }

    public override void OnExit(PlayerCore owner, PlayerStateBase nextState)
    {

    }

    private async UniTask OnAttack(PlayerCore owner, CancellationToken ct)
    {
        owner.Anim.SetInteger("AttackNum", attackNum);
        await UniTask.DelayFrame(10);
        owner.attackDataList[attackNum].OnAttack.Invoke(owner);
        while (true)
        {
            if (attackNum < owner.attackDataList.Length - 1)
            {
                //æs“ü—ÍŽó•t
                if (owner.InputEventProvider.IsAttacking.Value && owner.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime > owner.attackDataList[attackNum].inputStartTime)
                {
                    attack = true;
                }
            }


            if (comboTimeCount >= owner.attackDataList[attackNum].acceptTime)
            {
                if (!attack)
                {
                    owner.Anim.SetInteger("AttackNum", -1);
                    owner.ChangeState(owner.StateStanding);
                    break;
                }
                attackNum++;
                attack = false;
                comboTimeCount = 0;

            }
            await UniTask.Yield();
        }
    }
}
