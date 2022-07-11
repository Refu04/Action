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
        owner.Rb.velocity = Vector3.zero;
        var cts = new CancellationTokenSource();
        OnAttack(owner, cts.Token).Forget();
        Debug.Log("StateAttacking");

    }

    public override void OnUpdate(PlayerCore owner)
    {
        comboTimeCount += Time.deltaTime;
        
    }

    public override void OnExit(PlayerCore owner, PlayerStateBase nextState)
    {

    }

    private async UniTask OnAttack(PlayerCore owner, CancellationToken ct)
    {
        await ChangeAttack(owner, ct);
        while (true)
        {
            //�R���{�ŏI�i�łȂ����
            if (attackNum < owner.PlayerAttack.attackDataList.Length - 1)
            {
                //���͎�t�J�n
                if (owner.InputEventProvider.IsAttacking.Value && comboTimeCount >= owner.PlayerAttack.attackDataList[attackNum].inputStartTime)
                {
                    attack = true;
                }
            }

            //��t���Ԃ��߂�����
            if (comboTimeCount >= owner.PlayerAttack.attackDataList[attackNum].acceptTime)
            {
                //���͂�����Ă��Ȃ�������I��
                if (!attack)
                {
                    owner.Anim.SetInteger("AttackNum", -1);
                    owner.ChangeState(owner.StateStanding);
                    break;
                }
                //���̍U����
                attackNum++;
                attack = false;
                comboTimeCount = 0;
                await ChangeAttack(owner, ct);
            }
            await UniTask.Yield();
        }
    }

    private async UniTask ChangeAttack(PlayerCore owner, CancellationToken ct)
    {
        //�A�j���[�V�����̔��f
        owner.Anim.SetInteger("AttackNum", attackNum);
        //�A�j���[�V�������؂�ւ��̂�҂�
        await UniTask.DelayFrame(1);
        //�_���[�W�̔��f
        owner.PlayerAttack.damage = owner.PlayerAttack.attackDataList[attackNum].damage;
        //���̍U����AttackEvents�̌Ăяo��
        for (int i = 0; i < owner.PlayerAttack.attackDataList[attackNum].AttackEvents.Length; i++)
        {
            CallAttackEvents(owner, i, ct).Forget();
        }
    }

    private async UniTask CallAttackEvents(PlayerCore owner,int eventNum, CancellationToken ct)
    {

        await UniTask.WaitUntil(() => owner.PlayerAttack.attackDataList[attackNum].TriggerTimes[eventNum] <= owner.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        owner.PlayerAttack.attackDataList[attackNum].AttackEvents[eventNum].Invoke(owner, attackNum);
        await UniTask.WaitUntil(() => 1f <= owner.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
    }
}
