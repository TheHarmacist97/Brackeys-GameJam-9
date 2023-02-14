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

    public WaitForSeconds reloadWait;
    public  WaitForSeconds waitBetweenBullets;
    public abstract void Init(Transform muzzle);
    public abstract void Fire();
    public abstract void FireContinually();
    protected virtual IEnumerator CycleFire()
    {
        yield break;
    }
    protected virtual void PropelBullet()
    {
        Debug.Log(weaponBaseData.bullet.bulletPrefab!=null);
        Instantiate(weaponBaseData.bullet.bulletPrefab, muzzle.position, muzzle.rotation);

    }
    public abstract void StopFiring();
    public virtual IEnumerator Reload()
    {
        if (state == WeaponState.NEXT_WAIT)
        {
            Debug.Log("Reloaded during shot wait");
            yield break;
        }
        if(state == WeaponState.RELOADING)
        {
            Debug.Log("Already reloading");
            yield break;
        }
        state = WeaponState.RELOADING;
        Debug.Log("reloading");
        yield return reloadWait;
        Debug.Log("reloaded");
        currentAmmo = weaponBaseData.magazineSize;
        state = WeaponState.READY;
    }
}
