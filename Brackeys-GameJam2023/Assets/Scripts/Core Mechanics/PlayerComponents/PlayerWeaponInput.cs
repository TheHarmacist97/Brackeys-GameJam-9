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
            else if (Input.GetMouseButton(0))
            {
                weaponManager.FireContinually(false);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                weaponManager.StopFiring();
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                weaponManager.ReloadAll();
            }
        }
    }
}
