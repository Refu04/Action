using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    //�X�e�[�g�̃C���X�^���X
    public PlayerStateBase StateStanding { get; set; } = new StateStanding();
    public PlayerStateBase StateMoving { get; set; } = new StateMoving();

    //���݂̃X�e�[�g
    protected PlayerStateBase currentState;
    //�A�j���[�^�[�R���g���[���[
    [SerializeField]
    private Animator anim;
    public Animator Anim => anim;
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
}
