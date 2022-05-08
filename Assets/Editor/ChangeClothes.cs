using System;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class ChangeClothes : EditorWindow
{
    //�f��
    public GameObject model;
    //��
    public GameObject[] clothes = new GameObject[0];
    //�A�N�Z�T���[
    public GameObject[] accessories = new GameObject[0];
    //�A�N�Z�T���[������
    public string[] allocations;

    [MenuItem("Window/Editor extention/ChangeClothes")]
    private static void ShowWindow()
    {
        ChangeClothes window = GetWindow<ChangeClothes>();
        window.titleContent = new GUIContent("�����ւ��G�f�B�^�[");
    }

    private void OnGUI()
    {
        //�f��GameObject���͗��\��
        model = EditorGUILayout.ObjectField("�f��" , model, typeof(GameObject), true) as GameObject;
        //���ƃA�N�Z�T���[���͗��\��
        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty clothesProperty = so.FindProperty("clothes");
        SerializedProperty accessoriesProperty = so.FindProperty("accessories");
        SerializedProperty allocationsProperty = so.FindProperty("allocations");
        GUILayout.Label("��");
        EditorGUILayout.PropertyField(clothesProperty, true);
        using (new GUILayout.HorizontalScope())
        {
            using (new GUILayout.VerticalScope())
            {
                GUILayout.Label("�A�N�Z�T���[");
                EditorGUILayout.PropertyField(accessoriesProperty, true);
            }
            using (new GUILayout.VerticalScope())
            {
                GUILayout.Label("�A�N�Z�T���[���蓖�Đ�");
                EditorGUILayout.PropertyField(allocationsProperty, true);
                Array.Resize(ref allocations, accessories.Length);
            }
        }
        so.ApplyModifiedProperties();
        if (GUILayout.Button("�����ւ���"))
        {
            DressUp();
        }
    }

    void DressUp()
    {
        //�f�̂̎q�I�u�W�F�N�g�擾
        var parts = model.GetComponentsInChildren<Transform>();
        //���̃��X�g
        var clothesParts = new List<Transform>();
        //���̎q�I�u�W�F�N�g�擾
        foreach(var c in clothes)
        {
           foreach(var t in c.GetComponentsInChildren<Transform>())
           {
                clothesParts.Add(t);
           }
        }
        //������
        foreach (var c in clothesParts)
        {
            foreach(var p in parts)
            {
                //�f�̃p�[�c�̖��O�ƕ��̖��O��r
                if(c.name == p.name)
                {
                    //���O����v�����ꍇ�͐e�q�֌W�ɂ���
                    c.SetParent(p);
                }
            }
            //�ǂ��ɂ����蓖�Ă��Ȃ������ꍇ
            if(c.parent == null)
            {
                //�f�̂̃��[�g�I�u�W�F�N�g�����ɔz�u
                c.SetParent(model.transform);
            } else if(c.parent.name != c.name)
            {
                c.SetParent(model.transform);
            }
        }
        //�A�N�Z�T���[����
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
