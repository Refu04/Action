using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//シングルトンなクラスに継承させる
public class SingletonGameObject<T> : MonoBehaviour where T : SingletonGameObject<T>
{
    private static T mInstance;

    public static T Instance
    {
        get
        {
            return mInstance;
        }
    }

    void Awake()
    {
        //自分のインスタンスが登録されているかチェック
        if (mInstance == null)
        {
            //無ければ破壊されないようにし、登録
            DontDestroyOnLoad(this.gameObject);
            mInstance = this as T;
            mInstance.Init();
        }
        else
        {
            //既にインスタンスがあれば破壊する
            Destroy(this.gameObject);
        }
    }

    //派生クラス用のAwake
    protected virtual void Init()
    {

    }
}