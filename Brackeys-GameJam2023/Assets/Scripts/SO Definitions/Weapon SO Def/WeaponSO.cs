using UnityEngine;

public class WeaponSO:ScriptableObject 
{
    public float damage;
    public float reloadTime;
    public int magazineSize;
    public bool canContinuallyFire;
    public BulletSO bullet;
}
