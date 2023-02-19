
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
                _instance = FindObjectOfType<T>();
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
        if (_instance != null && _instance != this as T)
        {
            Debug.Log("Duplicate found");
            Destroy(this.gameObject);
        }
        else
            _instance = this as T;
    }
}
