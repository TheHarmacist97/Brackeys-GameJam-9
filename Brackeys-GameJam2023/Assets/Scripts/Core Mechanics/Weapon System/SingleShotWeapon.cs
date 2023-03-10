using System.Collections;
using UnityEngine;

public class SingleShotWeapon : WeaponsClass
{
    public SingleShotSO weaponData;

    public override void Init(Transform muzzle)
    {
        this.muzzle = muzzle;
        weaponData = weaponBaseData as SingleShotSO;
        currentAmmo = weaponData.magazineSize;
        state = WeaponState.READY;
        waitBetweenBullets = new WaitForSeconds(weaponData.minimumTimeBetweenShots);
        reloadWait = new WaitForSeconds(weaponData.reloadTime);
    }
    public override void Fire(Vector3 target)
    {
        if (targetTransform == null) return;
        targetTransform.position = target;
        if (state == WeaponState.EMPTY)
        {
            StartCoroutine(Reload());
        }
        else if (state == WeaponState.READY)
        {
            currentAmmo--;
            PropelBullet();
            StartCoroutine(ShotLimiter());
        }
    }

    public IEnumerator ShotLimiter()
    {
        state = WeaponState.NEXT_WAIT;
        yield return waitBetweenBullets;
        state = currentAmmo > 0 ? WeaponState.READY : WeaponState.EMPTY;
    }

    public override void FireContinually(bool callFromEnemy, Transform target)
    {
        this.targetTransform = target;
        if (!callFromEnemy && !weaponData.canContinuallyFire) return;
        if (firingContinually) return;

        firingContinually = true;
        StartCoroutine(CycleFire());
    }

    protected override IEnumerator CycleFire()
    {
        while (firingContinually)
        {
            Fire(targetTransform.position);
            yield return waitBetweenBullets;
        }
    }

    public override void StopFiring()
    {
        firingContinually = false;
    }
}
