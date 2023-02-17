using System.Collections.Generic;
using UnityEngine;

public class WeaponsManager:InputAbility
{
    [HideInInspector]
    public List<WeaponsClass> weapons;
    private CharacterData characterData;
    public void Init(WeaponSO[] weaponsArray, Transform[] muzzlesArray)
    {
        for (int i = 0; i < muzzlesArray.Length; i++)
        {
            SetupWeapon(weaponsArray[i]);
            weapons[i].weaponBaseData = weaponsArray[i];
            weapons[i].Init(muzzlesArray[i]);
        }
    }

    private void Awake()
    {
        characterData = GetComponent<Character>().data;
        Init(characterData.weaponsData.ToArray(), characterData.muzzles.ToArray());
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

    private void Update()
    {
        foreach(WeaponsClass weapon in weapons)
        {
            weapon.target = characterData.target.position;
        }
    }


    public override void ReloadAll()
    {
        foreach (WeaponsClass weapon in weapons)
        {
            StartCoroutine(weapon.Reload());
        }
    }

    public override void StopFiring()
    {
        foreach (WeaponsClass weapon in weapons)
        {
            weapon.StopFiring();
        }
    }

    public override void FireContinually(bool fireFromEnemy, Vector3 target)
    {
        foreach (WeaponsClass weapon in weapons)
        {
            weapon.FireContinually(fireFromEnemy, target);
        }
    }

    public override void Fire(Vector3 target)
    {
        foreach (WeaponsClass weapon in weapons)
        {
            weapon.Fire(target);
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
