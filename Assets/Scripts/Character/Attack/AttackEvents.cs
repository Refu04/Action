using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackEvents : MonoBehaviour
{

    public void Test(PlayerCore owner, int attackNum)
    {
        Debug.Log(owner + "" + Time.time);
    }
    
    public void EnableAttackCollider(PlayerCore owner, int attackNum)
    {
        
        owner.PlayerAttack.AttackCol.enabled = true;
    }

    public void DisableAttackCollider(PlayerCore owner, int attackNum)
    {
        owner.PlayerAttack.AttackCol.enabled = false;
    }

    public void ChangeColliderSize(PlayerCore owner, int attackNum)
    {
        //ˆÊ’u‚Ì”½‰f
        owner.PlayerAttack.AttackCol.center = owner.PlayerAttack.attackDataList[attackNum].colliderPos;
        //ƒTƒCƒY‚Ì”½‰f
        owner.PlayerAttack.AttackCol.size = owner.PlayerAttack.attackDataList[attackNum].colliderSize;
    }
}
