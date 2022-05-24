using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerStateBase
{
    // �X�e�[�g���J�n�������ɌĂ΂��
    public virtual void OnEnter(PlayerCore owner, PlayerStateBase prevState) { }
    // ���t���[���Ă΂��
    public virtual void OnUpdate(PlayerCore owner) { }
    // �X�e�[�g���I���������ɌĂ΂��
    public virtual void OnExit(PlayerCore owner, PlayerStateBase nextState) { }
}
