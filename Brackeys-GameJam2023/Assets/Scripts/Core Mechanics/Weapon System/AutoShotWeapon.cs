using System.Collections;
using UnityEngine;

public class AutoShotWeapon : WeaponsClass
{
    public AutoShotSO weaponData;
    public override void Init(Transform muzzle)
    {
        weaponData = weaponBaseData as AutoShotSO;
        weaponData.canContinuallyFire = true;
        this.muzzle = muzzle;
        currentAmmo = weaponData.magazineSize;
        state = WeaponState.READY;
        waitBetweenBullets = new WaitForSeconds(1f/weaponData.fireRate);
        reloadWait = new WaitForSeconds(weaponData.reloadTime);
    }
    public override void Fire()
    {
        if(state == WeaponState.READY)
        {
            firingContinually = true;
            StartCoroutine(AutoFire());
        }
    }

    private IEnumerator AutoFire()
    {
        while(firingContinually)
        {
            currentAmmo--;
            PropelBullet();
            if(currentAmmo < 0)
            {
                state = WeaponState.EMPTY;
                yield break;
            }
            yield return waitBetweenBullets;
        }
    }

    public override void FireContinually()
    {
        if (firingContinually) return;

        firingContinually = true;
        //Debug.Log("called " + weaponData.name);
        Fire();
        return;
    }

    public override void StopFiring()
    {
        firingContinually = false;
    }
}
