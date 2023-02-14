using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName = "Bullet", menuName = "BulletType", order = 3)]
public class BulletSO : ScriptableObject
{
    public enum BulletType
    {
       HITSCAN,
       PROJECTILE,
       PROJECTILE_WITH_GRAVITY
    }
    public GameObject bulletPrefab;
    public BulletType type;
    public float speed;
    public float damage;
    
}
