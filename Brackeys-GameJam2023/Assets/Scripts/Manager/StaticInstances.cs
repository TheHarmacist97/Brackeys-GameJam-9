using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class StaticInstances<T> : MonoBehaviour where T: MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get 
        {
            if(_instance==null)
            {
                _instance = GameObject.FindObjectOfType<T>();
                if(_instance==null)
                {
                    _instance = new GameObject(typeof(T).Name).AddComponent<T>();
                }
            }
            return _instance; 
        }
    }
    protected virtual void Awake()
    {
        if (_instance != null)
            Destroy(this);
    }
}
