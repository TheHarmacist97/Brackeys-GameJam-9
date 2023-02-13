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

    [HideInInspector]
    public WeaponsClass weapon;
    [SerializeField] WeaponSO weaponData;
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
            weapon.Fire();
        }
        if (Input.GetMouseButtonDown(1))
        {
            StartCoroutine(weapon.Reload());
        }
    }
}
