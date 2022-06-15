using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGManager : MonoBehaviour
{
    //背景画像
    [SerializeField]
    private Image[] BGImages;
    //背景画像スクロールスピード
    [SerializeField]
    private float[] scrollSpeed;
    //カメラ位置
    private Vector3 prevPos;

    void Start()
    {
        //カメラ位置初期化
        prevPos = Camera.main.transform.position;
    }

    void Update()
    {
        //カメラが横方向に動いていれば
        if (/*Mathf.Floor(prevPos.x * 100) / 100 != Mathf.Floor(Camera.main.transform.position.x * 100) / 100*/ prevPos.x != Camera.main.transform.position.x)
        {
            //左に動いている場合
            if(prevPos.x >= Camera.main.transform.position.x)
            {
                //背景移動処理
                for (int i = 0; i < BGImages.Length; i++)
                {
                    BGImages[i].transform.Translate(scrollSpeed[i], 0, 0);
                    if(BGImages[i].rectTransform.localPosition.x >= 1920)
                    {
                        BGImages[i].rectTransform.localPosition = new Vector3(-1920, 0, 0);
                    }
                }
                
            }
            //右に動いている場合
            else
            {
                //背景移動処理
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
