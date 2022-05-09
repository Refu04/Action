using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    //ステートのインスタンス
    public PlayerStateBase StateStanding { get; set; } = new StateStanding();
    public PlayerStateBase StateMoving { get; set; } = new StateMoving();

    //現在のステート
    protected PlayerStateBase currentState;
    //アニメーターコントローラー
    [SerializeField]
    private Animator anim;
    public Animator Anim => anim;
    //入力イベント
    public IInputEventProvider InputEventProvider { get; set; }
    void Start()
    {
        //初期ステートの設定
        currentState = StateStanding;
        currentState.OnEnter(this, null);
        //入力イベントの取得
        InputEventProvider = GetComponent<IInputEventProvider>();
    }
    
    void Update()
    {
        //現在のステートのUpdate呼び出し
        currentState.OnUpdate(this);
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
    }
}
