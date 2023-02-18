using UnityEngine;

[System.Serializable]
public class Bullet : MonoBehaviour
{
    public enum BulletType
    {
        PROJECTILE,
        RAYCAST,
    }
    [SerializeField]private float speed;
    [SerializeField]private int damage;
    [SerializeField]private BulletType bulletType;
    [SerializeField]private float bulletRange;

    private void Start()
    {
        Destroy(gameObject, bulletRange/speed);
        if(bulletType == BulletType.RAYCAST)
        {
            if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, bulletRange))
            {
                TryDamage(hitInfo.collider);
            }
        }
    }
    private void Update()
    {
        if(bulletType== BulletType.PROJECTILE)
            transform.Translate(speed * Time.deltaTime * Vector3.forward, transform);
    }

    private void OnTriggerEnter(Collider collider)
    {
        //Debug.Log("Collided");
        TryDamage(collider);
        Destroy(gameObject);
    }

    private void TryDamage(Collider collider)
    {
        if (collider.gameObject.TryGetComponent<IDamageable>(out IDamageable damage))
        {
            //Debug.Log("damaged");
            damage.TakeDamage(this.damage);
        }
    }
}
