using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

public class Dummy : MonoBehaviour, IDamageApplicable
{
    public int HP;
    public Rigidbody rb;
    public Animator anim;

    public void ApplyDamage(int damage)
    {
        HP -= damage;
        KnockBack().Forget();
    }

    public async UniTask KnockBack()
    {
        anim.SetTrigger("Damage");
        await UniTask.DelayFrame(1);
        anim.SetTrigger("Damage");
    }
}
