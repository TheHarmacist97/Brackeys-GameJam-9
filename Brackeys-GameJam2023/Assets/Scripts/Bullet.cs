using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class Bullet:MonoBehaviour
{
    public float speed;
    private void Start()
    {
       Destroy(gameObject, 3f);
    }
    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.forward, transform);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);    
    }
}
