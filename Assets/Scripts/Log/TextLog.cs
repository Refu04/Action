using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TextLog : MonoBehaviour
{
    //�@���O�o�͐�e�L�X�g
    [SerializeField]
    private Text logText;
    //���O�̃��X�g
    private List<string> logs;
    //�@���O��ۑ����鐔
    [SerializeField]
    private int maxLogNum = 10;
    //�@�c�̃X�N���[���o�[
    [SerializeField]
    private Scrollbar verticalScrollbar;
    private StringBuilder logTextStringBuilder;
    //���g�̃C���X�^���X�����J
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
        //���O�e�L�X�g�̒ǉ�
        logs.Add(text);
        //���O�̍ő�ۑ����𒴂�����Â����O���폜
        if (logs.Count > maxLogNum)
        {
            logs.RemoveRange(0, logs.Count - maxLogNum);
        }
        //���O�e�L�X�g�̕\��
        ViewLogText();
    }

    //�@���O�e�L�X�g�̕\��
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
