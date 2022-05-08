using System;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class ChangeClothes : EditorWindow
{
    //素体
    public GameObject model;
    //服
    public GameObject[] clothes = new GameObject[0];
    //アクセサリー
    public GameObject[] accessories = new GameObject[0];
    //アクセサリー割当先
    public string[] allocations;

    [MenuItem("Window/Editor extention/ChangeClothes")]
    private static void ShowWindow()
    {
        ChangeClothes window = GetWindow<ChangeClothes>();
        window.titleContent = new GUIContent("着せ替えエディター");
    }

    private void OnGUI()
    {
        //素体GameObject入力欄表示
        model = EditorGUILayout.ObjectField("素体" , model, typeof(GameObject), true) as GameObject;
        //服とアクセサリー入力欄表示
        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty clothesProperty = so.FindProperty("clothes");
        SerializedProperty accessoriesProperty = so.FindProperty("accessories");
        SerializedProperty allocationsProperty = so.FindProperty("allocations");
        GUILayout.Label("服");
        EditorGUILayout.PropertyField(clothesProperty, true);
        using (new GUILayout.HorizontalScope())
        {
            using (new GUILayout.VerticalScope())
            {
                GUILayout.Label("アクセサリー");
                EditorGUILayout.PropertyField(accessoriesProperty, true);
            }
            using (new GUILayout.VerticalScope())
            {
                GUILayout.Label("アクセサリー割り当て先");
                EditorGUILayout.PropertyField(allocationsProperty, true);
                Array.Resize(ref allocations, accessories.Length);
            }
        }
        so.ApplyModifiedProperties();
        if (GUILayout.Button("着せ替える"))
        {
            DressUp();
        }
    }

    void DressUp()
    {
        //素体の子オブジェクト取得
        var parts = model.GetComponentsInChildren<Transform>();
        //服のリスト
        var clothesParts = new List<Transform>();
        //服の子オブジェクト取得
        foreach(var c in clothes)
        {
           foreach(var t in c.GetComponentsInChildren<Transform>())
           {
                clothesParts.Add(t);
           }
        }
        //着せる
        foreach (var c in clothesParts)
        {
            foreach(var p in parts)
            {
                //素体パーツの名前と服の名前比較
                if(c.name == p.name)
                {
                    //名前が一致した場合は親子関係にする
                    c.SetParent(p);
                }
            }
            //どこにも割り当てられなかった場合
            if(c.parent == null)
            {
                //素体のルートオブジェクト直下に配置
                c.SetParent(model.transform);
            } else if(c.parent.name != c.name)
            {
                c.SetParent(model.transform);
            }
        }
        //アクセサリー装着
        for(int i = 0; i < accessories.Length; i++)
        {
            foreach (var p in parts)
            {
                if (allocations[i] == p.name)
                {
                    if(accessories[i] != null)
                    accessories[i].transform.SetParent(p);
                }
            }
        }
    }
}
