using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class StageManager : MonoBehaviour
{
    [SerializeField]
    private Button[] _stageSelectButton;

    void Start()
    {
        SubscribeStageSelectButton();
    }

    void SubscribeStageSelectButton()
    {
        _stageSelectButton[0].OnClickAsObservable()
            .Subscribe(_ => GameManager.Instance.LoadScene("Stage1"));
        _stageSelectButton[1].OnClickAsObservable()
            .Subscribe(_ => GameManager.Instance.LoadScene("Stage2"));

    }
}
