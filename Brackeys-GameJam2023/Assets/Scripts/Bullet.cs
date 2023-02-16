using UnityEngine;

[System.Serializable]
public class Bullet : MonoBehaviour
{
    public bool ignore = true;
    public float speed;
    public int damage;
    private void Start()
    {
        Destroy(gameObject, 3f);
    }
    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.forward, transform);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (ignore)
        {
            ignore = false;
            return;
        }
        if (collider.gameObject.TryGetComponent<IDamageable>(out IDamageable damage))
        {
            damage.TakeDamage(this.damage);
        }
        Destroy(gameObject);
    }
}
