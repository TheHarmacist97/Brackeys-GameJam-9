using System.Collections;
using UnityEngine;

public class BurstShotWeapon : WeaponsClass
{
    public BurstShotSO weaponData;
    private WaitForSeconds waitBetweenBullets;
    private WaitForSeconds waitBetweenTriggerPulls;
    public override void Fire()
    {
        if (state == WeaponState.READY)
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
            Debug.Log("Fired " + currentAmmo);
            fireEvent?.Invoke();
            if(currentAmmo == 0)
            {
                break;
            }
            yield return waitBetweenBullets;
            
        }
        Debug.Log("Limiting");
        yield return waitBetweenTriggerPulls;
        Debug.Log("Done limiting");
        state = currentAmmo > 0 ? WeaponState.READY : WeaponState.EMPTY;
    }

    public override void Init()
    {
        weaponData = weaponBaseData as BurstShotSO;
        currentAmmo = weaponData.magazineSize;
        waitBetweenBullets = new WaitForSeconds(weaponData.fireRate);
        waitBetweenTriggerPulls = new WaitForSeconds(weaponData.minimumTimeBetweenBursts);
    }
}
