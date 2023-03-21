using System.Collections;
using UnityEngine;

public enum WeaponState
{
    READY,
    NEXT_WAIT,
    EMPTY,
    RELOADING
}

public abstract class WeaponsClass : MonoBehaviour
{
    public bool firingContinually;
    public int currentAmmo;
    public WeaponState state;
    public WeaponSO weaponBaseData;
    public Transform muzzle;
    public Transform targetTransform;

    public WaitForSeconds reloadWait;
    public WaitForSeconds waitBetweenBullets;
    public abstract void Init(Transform muzzle);
    public abstract void Fire(Vector3 target);
    public abstract void FireContinually(bool callFromEnemy, Transform target);
    protected Transform bulletParent;
    protected virtual IEnumerator CycleFire()
    {
        yield break;
    }
    protected virtual void PropelBullet()
    {
        Instantiate(weaponBaseData.bullet, muzzle.position, Quaternion.LookRotation(targetTransform.position - muzzle.position));
    }
    public abstract void StopFiring();
    public virtual IEnumerator Reload()
    {
        if (state == WeaponState.NEXT_WAIT)
        {
            Debug.Log("Reloaded during shot wait");
            yield break;
        }
        if (state == WeaponState.RELOADING)
        {
            Debug.Log("Already reloading");
            yield break;
        }
        state = WeaponState.RELOADING;
        yield return reloadWait;
        currentAmmo = weaponBaseData.magazineSize;
        state = WeaponState.READY;
    }
}
