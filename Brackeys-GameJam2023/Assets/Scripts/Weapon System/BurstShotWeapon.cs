using System.Collections;
using UnityEngine;

public class BurstShotWeapon : WeaponsClass
{
    public BurstShotSO weaponData;
    private WaitForSeconds waitBetweenTriggerPulls;
    private WaitForSeconds continualFireWait;

    public override void Init(Transform muzzle)
    {
        this.muzzle = muzzle;
        weaponData = weaponBaseData as BurstShotSO;
        currentAmmo = weaponData.magazineSize;
        waitBetweenBullets = new WaitForSeconds(1f / weaponData.fireRate);
        waitBetweenTriggerPulls = new WaitForSeconds(weaponData.minimumTimeBetweenBursts);
        continualFireWait = new WaitForSeconds(weaponData.minimumTimeBetweenBursts + (weaponData.burstsPerTriggerPull / weaponData.fireRate));
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
            StartCoroutine(Burst());
        }
    }

    private IEnumerator Burst()
    {
        state = WeaponState.NEXT_WAIT;
        for (int i = 0; i < weaponData.burstsPerTriggerPull; i++)
        {
            currentAmmo--;
            PropelBullet();
            if (currentAmmo == 0)
            {
                break;
            }
            yield return waitBetweenBullets;

        }
        yield return waitBetweenTriggerPulls;
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

    public override void StopFiring()
    {
        firingContinually = false;
    }

    protected override IEnumerator CycleFire()
    {
        while (firingContinually)
        {
            Fire(targetTransform.position);
            yield return continualFireWait;
        }
    }
}
