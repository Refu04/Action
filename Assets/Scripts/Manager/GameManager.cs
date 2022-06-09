using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;

public class GameManager : SingletonGameObject<GameManager>
{
    //仮でボタン押したらステージセレクトSceneに遷移
    [SerializeField]
    private Button _startButton;

    void Start()
    {
        _startButton.OnClickAsObservable()
            .Subscribe(_ => LoadScene("StageSelect"));
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
