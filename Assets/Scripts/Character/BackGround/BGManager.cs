using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGManager : MonoBehaviour
{
    //�w�i�摜
    [SerializeField]
    private Image[] BGImages;
    //�w�i�摜�X�N���[���X�s�[�h
    [SerializeField]
    private float[] scrollSpeed;
    //�J�����ʒu
    private Vector3 prevPos;

    void Start()
    {
        //�J�����ʒu������
        prevPos = Camera.main.transform.position;
    }

    void Update()
    {
        //�J�������������ɓ����Ă����
        if (/*Mathf.Floor(prevPos.x * 100) / 100 != Mathf.Floor(Camera.main.transform.position.x * 100) / 100*/ prevPos.x != Camera.main.transform.position.x)
        {
            //���ɓ����Ă���ꍇ
            if(prevPos.x >= Camera.main.transform.position.x)
            {
                //�w�i�ړ�����
                for (int i = 0; i < BGImages.Length; i++)
                {
                    BGImages[i].transform.Translate(scrollSpeed[i], 0, 0);
                    if(BGImages[i].rectTransform.localPosition.x >= 1920)
                    {
                        BGImages[i].rectTransform.localPosition = new Vector3(-1920, 0, 0);
                    }
                }
                
            }
            //�E�ɓ����Ă���ꍇ
            else
            {
                //�w�i�ړ�����
                for (int i = 0; i < BGImages.Length; i++)
                {
                    BGImages[i].transform.Translate(-scrollSpeed[i], 0, 0);
                    if (BGImages[i].rectTransform.localPosition.x <= -1920)
                    {
                        var diff = BGImages[i].rectTransform.localPosition.x + 1920;
                        BGImages[i].rectTransform.localPosition = new Vector3(1920 + diff, 0, 0);
                    }
                }
            }
            
            prevPos = Camera.main.transform.position;
        }
        
    }
}
