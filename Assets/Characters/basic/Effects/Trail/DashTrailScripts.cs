using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTrailScripts : MonoBehaviour
{
    public bool emitting = false;
    
    private TrailRenderer[] _children;
    // Start is called before the first frame update
    void Start()
    {
        _children = GetComponentsInChildren<TrailRenderer>();
        ChangeEmitting();
    }

    void OnValidate(){
        if(_children != null) ChangeEmitting();
    }

    // Toggle Event
    void OnToggle(bool? flag){
        if(flag == null){
            emitting = !emitting;
        } else {
            emitting = (bool)flag;
        }
        ChangeEmitting();
    }

    void ChangeEmitting(){
        foreach(TrailRenderer child in _children){
            child.emitting = emitting;
        }
    }
}
