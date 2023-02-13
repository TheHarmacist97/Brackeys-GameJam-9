using System.Collections;
using UnityEngine;

public class SingleShotWeapon : WeaponsClass
{
    public SingleShotSO weaponData;
    private WaitForSeconds waitBetweenShots;

    public override void Init()
    {
        weaponData = weaponBaseData as SingleShotSO;
        currentAmmo = weaponData.magazineSize;
        state = WeaponState.READY;
        waitBetweenShots = new WaitForSeconds(weaponData.minimumTimeBetweenShots);
        reloadWait = new WaitForSeconds(weaponData.reloadTime);

        Debug.Log(weaponData.minimumTimeBetweenShots);
    }
    public override void Fire(Vector3 target)
    {
        if (state == WeaponState.READY)
        {
            currentAmmo--;
            Debug.Log("Fired " + currentAmmo);
            StartCoroutine(ShotLimiter());
        }
    }

    public IEnumerator ShotLimiter()
    {
        state = WeaponState.NEXT_WAIT;
        Debug.Log("limiting");
        yield return waitBetweenShots;
        Debug.Log("limited shot");
        state = currentAmmo > 0 ? WeaponState.READY : WeaponState.EMPTY;
    }

}
