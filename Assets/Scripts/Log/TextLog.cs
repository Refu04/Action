using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TextLog : MonoBehaviour
{
    //　ログ出力先テキスト
    [SerializeField]
    private Text logText;
    //ログのリスト
    private List<string> logs;
    //　ログを保存する数
    [SerializeField]
    private int maxLogNum = 10;
    //　縦のスクロールバー
    [SerializeField]
    private Scrollbar verticalScrollbar;
    private StringBuilder logTextStringBuilder;
    //自身のインスタンスを公開
    public static TextLog Instance;
    // Start is called before the first frame update
    void Start()
    {
        logs = new List<string>();
        logTextStringBuilder = new StringBuilder();
        Instance = this;
    }

    public void AddLog(string text)
    {
        //ログテキストの追加
        logs.Add(text);
        //ログの最大保存数を超えたら古いログを削除
        if (logs.Count > maxLogNum)
        {
            logs.RemoveRange(0, logs.Count - maxLogNum);
        }
        //ログテキストの表示
        ViewLogText();
    }

    //　ログテキストの表示
    public void ViewLogText()
    {
        logTextStringBuilder.Clear();
        foreach(var log in logs)
        {
            logTextStringBuilder.Append(Environment.NewLine + log);
        }
        logText.text = logTextStringBuilder.ToString().TrimStart();
        verticalScrollbar.value = 0f;
    }
}
