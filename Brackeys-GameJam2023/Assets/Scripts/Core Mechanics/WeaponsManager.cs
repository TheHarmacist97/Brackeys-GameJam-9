using UnityEngine;

public class WeaponsManager : MonoBehaviour
{
    [HideInInspector]
    public WeaponsClass weapon;
    [SerializeField] WeaponSO weaponData;
    [SerializeField] Transform muzzle;
    void Start()
    {
        if (weaponData != null)
        {
            if (weaponData is SingleShotSO)
            {
                weapon = gameObject.AddComponent<SingleShotWeapon>();
            }
            else if (weaponData is BurstShotSO)
            {
                weapon = gameObject.AddComponent<BurstShotWeapon>();
            }
            else if (weaponData is AutoShotSO)
            {
                weapon = gameObject.AddComponent<AutoShotWeapon>();
            }
            WeaponSetup();
        }
    }

    private void WeaponSetup()
    {
        weapon.weaponBaseData = weaponData;
        weapon.Init(muzzle);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            weapon.Fire();
        }
        if(Input.GetMouseButton(0))
        {
            weapon.FireContinually();
        }
        if(Input.GetMouseButtonUp(0))
        {
            weapon.StopFiring();
        }
        if (Input.GetMouseButtonDown(1))
        {
            StartCoroutine(weapon.Reload());
        }
    }
}
