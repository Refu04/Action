using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    //�X�e�[�g�̃C���X�^���X
    public PlayerStateBase StateStanding { get; set; } = new StateStanding();
    public PlayerStateBase StateMoving { get; set; } = new StateMoving();
    public PlayerStateBase StateJumping { get; set; } = new StateJumping();

    //���݂̃X�e�[�g
    private PlayerStateBase currentState;
    //�A�j���[�^�[�R���g���[���[
    [SerializeField]
    private Animator anim;
    public Animator Anim => anim;

    [SerializeField]
    private Rigidbody rb;
    public Rigidbody Rb => rb;

    //����
    private bool isRight;
    public bool IsRight => isRight;

    //�e��p�����[�^
    //�W�����v�X�s�[�h
    [SerializeField]
    private float jumpSpeed;
    public float JumpSpeed => jumpSpeed;

    //Raycast���̃v���C���̍�����␳����
    [SerializeField]
    private float characterHeightOffset;
    //�ڒn����Ɏg�����C��
    [SerializeField]
    LayerMask groundMask;
    RaycastHit hit;

    //���n����
    private bool isGrounded;
    public bool IsGrounded => isGrounded;
    //���̓C�x���g
    public IInputEventProvider InputEventProvider { get; set; }
    void Start()
    {
        //�����X�e�[�g�̐ݒ�
        currentState = StateStanding;
        currentState.OnEnter(this, null);
        //���̓C�x���g�̎擾
        InputEventProvider = GetComponent<IInputEventProvider>();
    }
    
    void Update()
    {
        //���݂̃X�e�[�g��Update�Ăяo��
        currentState.OnUpdate(this);
        //�X�e�[�g�Ɋւ�炸�s������
        //���n����
        CheckGrounded();
        if (transform.localEulerAngles.y == 180)
        {
            isRight = true;
        } else if (transform.localEulerAngles.y == 0)
        {
            isRight = false;
        }
    }

    //�X�e�[�g�̊���
    public void AssignState(PlayerStateBase stateStanding, PlayerStateBase stateMoving)
    {
        StateStanding = stateStanding;
        StateMoving = stateMoving;
    }

    //�X�e�[�g�ύX
    public void ChangeState(PlayerStateBase nextState)
    {
        //�O�̃X�e�[�g�I�����̏���
        currentState.OnExit(this, nextState);
        //���̃X�e�[�g�J�n���̏���
        nextState.OnEnter(this, currentState);
        currentState = nextState;
        
    }

    private void CheckGrounded()
    {
        isGrounded = Physics.BoxCast(
            transform.position - new Vector3(0, characterHeightOffset, 0),
            new Vector3(1f, 0.2f, 1f),
            Vector3.down,
            out hit,
            transform.rotation,
            0.01f,
            groundMask);
        anim.SetBool("isGrounded", isGrounded);
    }

    private void OnDrawGizmos()
    {
        if (isGrounded)
        {
            Gizmos.DrawWireCube(transform.position + Vector3.down * hit.distance, new Vector3(1f, 0.2f, 1f));
        }
    }
}
