using UnityEngine;

public class WeaponsManager : MonoBehaviour
{
    public enum WeaponType
    {
        SINGLE,
        BURST,
        AUTO,
        MULTI
    }

    [SerializeField] WeaponSO weaponData;
    [SerializeField] WeaponsClass weapon;
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
            //else if(weapon is)
            WeaponSetup();
        }
    }

    private void WeaponSetup()
    {
        weapon.weaponBaseData = weaponData;
        weapon.Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            weapon.Fire(transform.forward);
        }
        if (Input.GetMouseButtonDown(1))
        {
            StartCoroutine(weapon.Reload());
        }
    }
}
