using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponInput : MonoBehaviour
{
    private WeaponsManager weaponManager;

    private void OnEnable()
    {
        weaponManager = GetComponent<WeaponsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(weaponManager != null) 
        { 
            if (Input.GetMouseButtonDown(0))
            {
                weaponManager.Fire();
            }
            if (Input.GetMouseButton(0))
            {
                weaponManager.FireContinually(false);
            }
            if (Input.GetMouseButtonUp(0))
            {
                weaponManager.StopFiring();
            }
            if (Input.GetMouseButtonDown(1))
            {
                weaponManager.ReloadAll();
            }
        }
    }
}
