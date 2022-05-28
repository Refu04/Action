using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerCore : MonoBehaviour
{
    //ステートのインスタンス
    public PlayerStateBase StateStanding { get; set; } = new StateStanding();
    public PlayerStateBase StateMoving { get; set; } = new StateMoving();
    public PlayerStateBase StateJumping { get; set; } = new StateJumping();
    public PlayerStateBase StateClimbing { get; set; } = new StateClimbing();
    public PlayerStateBase StateBlinking { get; set; } = new StateBlinking();

    //現在のステート
    private PlayerStateBase currentState;
    //アニメーターコントローラー
    [SerializeField]
    private Animator anim;
    public Animator Anim => anim;

    [SerializeField]
    private Rigidbody rb;
    public Rigidbody Rb => rb;

    //当たり判定
    [SerializeField]
    private BoxCollider col;
    public BoxCollider Col => col;

    //向き
    private bool isRight;
    public bool IsRight => isRight;

    //各種パラメータ
    //ジャンプスピード
    [SerializeField]
    private float jumpSpeed;
    public float JumpSpeed => jumpSpeed;

    //Raycast時のプレイヤの高さを補正する
    [SerializeField]
    private float characterHeightOffset;
    //接地判定に使うレイヤ
    [SerializeField]
    private LayerMask groundMask;
    public LayerMask GroundMask => groundMask;
    RaycastHit hit;

    //移動スキル発動回数
    private int moveSkillCount;
    public int MoveSkillCount => moveSkillCount;
    //着地判定
    private readonly ReactiveProperty<bool> isGrounded = new ReactiveProperty<bool>();
    public IReadOnlyReactiveProperty<bool> IsGrounded => isGrounded;
    //入力イベント
    public IInputEventProvider InputEventProvider { get; set; }
    void Start()
    {
        //初期ステートの設定
        currentState = StateStanding;
        currentState.OnEnter(this, null);
        //入力イベントの取得
        InputEventProvider = GetComponent<IInputEventProvider>();
        //着地時の処理
        //移動スキル使用回数リセット
        isGrounded
            .Where(x => x)
            .Subscribe(_ => moveSkillCount = 0);
    }

    void Update()
    {
        //現在のステートのUpdate呼び出し
        currentState.OnUpdate(this);
        //ステートに関わらず行う処理
        //着地判定
        CheckGrounded();
        //向き判定
        if (transform.localEulerAngles.y == 180)
        {
            isRight = true;
        }
        else if (transform.localEulerAngles.y == 0)
        {
            isRight = false;
        }
        //AnimatorのJumpSpeedパラメータに加速度の値を割り振る
        anim.SetFloat("JumpSpeed", rb.velocity.y);
    }

    //ステートの割当
    public void AssignState(PlayerStateBase stateStanding, PlayerStateBase stateMoving)
    {
        StateStanding = stateStanding;
        StateMoving = stateMoving;
    }

    //ステート変更
    public void ChangeState(PlayerStateBase nextState)
    {
        //前のステート終了時の処理
        currentState.OnExit(this, nextState);
        //次のステート開始時の処理
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
            new Vector3(0.3f, 0.2f, 1f),
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
            Gizmos.DrawWireCube(transform.position + Vector3.down * hit.distance, new Vector3(0.3f, 0.2f, 1f));
        }
    }

    
}