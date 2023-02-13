using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class Bullet:MonoBehaviour
{
    
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);    
    }
}
