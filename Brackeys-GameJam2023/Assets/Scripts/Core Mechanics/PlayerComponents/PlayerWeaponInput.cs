using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWeaponInput : MonoBehaviour
{
    private WeaponsManager weaponManager;
    private Transform targetTransform;

    private void OnEnable()
    {
        weaponManager = GetComponent<WeaponsManager>();
        targetTransform = GetComponent<Character>().data.target;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = targetTransform.position;
        if(weaponManager != null) 
        { 
            if (Input.GetMouseButtonDown(0))
            {
                weaponManager.Fire(pos);
            }
            else if (Input.GetMouseButton(0))
            {
                weaponManager.FireContinually(false, targetTransform);
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
