using System.Collections.Generic;
using UnityEngine;

public class WeaponsManager:MonoBehaviour
{
    [HideInInspector]
    public List<WeaponsClass> weapons;
    public void Init(WeaponSO[] weaponsArray, Transform[] muzzlesArray)
    {
        for (int i = 0; i < muzzlesArray.Length; i++)
        {
            SetupWeapon(weaponsArray[i]);
            weapons[i].weaponBaseData = weaponsArray[i];
            weapons[i].Init(muzzlesArray[i]);
        }
    }

    private void SetupWeapon(WeaponSO weaponData)
    {
        if (weaponData != null)
        {
            if (weaponData is SingleShotSO)
            {
                weapons.Add( gameObject.AddComponent<SingleShotWeapon>());
            }
            else if (weaponData is BurstShotSO)
            {
                weapons.Add(gameObject.AddComponent<BurstShotWeapon>());
            }
            else if (weaponData is AutoShotSO)
            {
                weapons.Add(gameObject.AddComponent<AutoShotWeapon>());
            }

        }
    }

    // Update is called once per frame
    

    public void ReloadAll()
    {
        foreach (WeaponsClass weapon in weapons)
        {
            StartCoroutine(weapon.Reload());
        }
    }

    public void StopFiring()
    {
        foreach (WeaponsClass weapon in weapons)
        {
            weapon.StopFiring();
        }
    }

    public void FireContinually(bool fireFromEnemy)
    {
        foreach (WeaponsClass weapon in weapons)
        {
            Debug.Log("called");
            weapon.FireContinually(fireFromEnemy);
        }
    }

    public void Fire()
    {
        foreach (WeaponsClass weapon in weapons)
        {
            weapon.Fire();
        }
    }

    private void OnDestroy()
    {
        foreach (WeaponsClass weapon in weapons)
        {
            Destroy(weapon);
        }
    }
}
