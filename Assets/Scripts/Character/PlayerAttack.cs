using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //攻撃判定コライダー
    [SerializeField]
    private BoxCollider attackCol;
    public BoxCollider AttackCol => attackCol;
    [System.Serializable]
    public class AttackEvent : UnityEvent<PlayerCore, int> { }
    [System.Serializable]
    public class AttackBase
    {
        public float inputStartTime;
        public float acceptTime;
        public Vector3 colliderPos;
        public Vector3 colliderSize;
        public int damage;
        public AttackEvent[] AttackEvents;
        public float[] TriggerTimes;
    }
    public int damage;
    [SerializeField]
    public AttackBase[] attackDataList = new AttackBase[3];

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.TryGetComponent<IDamageApplicable>(out var enemy))
        {
            enemy.ApplyDamage(damage);
        }
    }
}
