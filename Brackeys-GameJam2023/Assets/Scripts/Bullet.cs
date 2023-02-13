using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class Bullet:MonoBehaviour
{
    public BulletSO bulletData;

    private void Update()
    {
        transform.Translate(bulletData.speed * Time.deltaTime * transform.forward);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);    
    }
}
