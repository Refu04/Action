using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;


[System.Serializable]
public class AttackEvent : UnityEvent<PlayerCore> { }

[System.Serializable]
public class AttackBase
{
    public float inputStartTime;
    public float acceptTime;
    public AttackEvent OnAttack;
}

public class PlayerCore : MonoBehaviour
{
    //?X?e?[?g???C???X?^???X
    public PlayerStateBase StateStanding { get; set; } = new StateStanding();
    public PlayerStateBase StateMoving { get; set; } = new StateMoving();
    public PlayerStateBase StateJumping { get; set; } = new StateJumping();
    public PlayerStateBase StateClimbing { get; set; } = new StateClimbing();
    public PlayerStateBase StateBlinking { get; set; } = new StateBlinking();
    public PlayerStateBase StateWallSliding { get; set; } = new StateWallSliding();
    public PlayerStateBase StateAttacking { get; set; } = new StateAttacking();
    //???????X?e?[?g
    private PlayerStateBase currentState;
    //?A?j???[?^?[?R???g???[???[
    [SerializeField]
    private Animator anim;
    public Animator Anim => anim;

    [SerializeField]
    private Rigidbody rb;
    public Rigidbody Rb => rb;

    //??????????
    [SerializeField]
    private BoxCollider col;
    public BoxCollider Col => col;

    //????
    private bool isRight;
    public bool IsRight => isRight;

    //?e???p?????[?^
    //?W?????v?X?s?[?h
    [SerializeField]
    private float jumpSpeed;
    public float JumpSpeed => jumpSpeed;

    //Raycast?????v???C??????????????????
    [SerializeField]
    private float characterHeightOffset;
    //???n???????g?????C??
    [SerializeField]
    private LayerMask groundMask;
    public LayerMask GroundMask => groundMask;
    RaycastHit hit;

    //?????X?L??????????
    private int moveSkillCount;
    public int MoveSkillCount
    {
        get { return moveSkillCount; }
        set { moveSkillCount = value; }
    }
    //???n????
    private readonly ReactiveProperty<bool> isGrounded = new ReactiveProperty<bool>();
    public IReadOnlyReactiveProperty<bool> IsGrounded => isGrounded;
    //?????C?x???g
    public IInputEventProvider InputEventProvider { get; set; }

    

    [SerializeField]
    public AttackBase[] attackDataList = new AttackBase[3];

    void Start()
    {
        //?????X?e?[?g??????
        currentState = StateStanding;
        currentState.OnEnter(this, null);
        //?????C?x???g??????
        InputEventProvider = GetComponent<IInputEventProvider>();
        //???n????????
        //?????X?L???g?p???????Z?b?g
        isGrounded
            .Where(x => x)
            .Subscribe(_ => moveSkillCount = 0);
    }

    void Update()
    {
        //???????X?e?[?g??Update?????o??
        currentState.OnUpdate(this);
        //?X?e?[?g???????????s??????
        //???n????
        CheckGrounded();
        //????????
        if (Mathf.Floor(transform.localEulerAngles.y) == 90)
        {
            isRight = true;
        }
        else if (Mathf.Floor(transform.localEulerAngles.y) == 270)
        {
            isRight = false;
        }
        //Animator??JumpSpeed?p?????[?^???????x???l???????U??
        anim.SetFloat("JumpSpeed", rb.velocity.y);
    }

    //?X?e?[?g??????
    public void AssignState(PlayerStateBase stateStanding, PlayerStateBase stateMoving)
    {
        StateStanding = stateStanding;
        StateMoving = stateMoving;
    }

    //?X?e?[?g???X
    public void ChangeState(PlayerStateBase nextState)
    {
        //?O???X?e?[?g?I??????????
        currentState.OnExit(this, nextState);
        //?????X?e?[?g?J?n????????
        nextState.OnEnter(this, currentState);
        currentState = nextState;
        if(nextState == StateBlinking)
        {
            moveSkillCount++;
        }
    }

    private void CheckGrounded()
    {
        isGrounded.Value = Physics.BoxCast(
            transform.position - new Vector3(0, characterHeightOffset, 0),
            new Vector3(0.1f, 0.1f, 1f),
            Vector3.down,
            out hit,
            transform.rotation,
            0.01f,
            groundMask);
        anim.SetBool("isGrounded", isGrounded.Value);
        
    }

    private void OnDrawGizmos()
    {
        if (isGrounded.Value)
        {
            Gizmos.DrawWireCube(transform.position + Vector3.down * hit.distance, new Vector3(0.1f, 0.1f, 1f));
        }
    }

    
}