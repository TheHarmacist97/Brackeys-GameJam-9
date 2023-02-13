using System;
using System.Collections;
using UnityEngine;

public enum WeaponState
{
    READY,
    NEXT_WAIT,
    EMPTY,
    RELOADING
}

public abstract class WeaponsClass : MonoBehaviour
{
    public int currentAmmo;
    public WeaponState state;
    public WeaponSO weaponBaseData;
    public WaitForSeconds reloadWait;

    public delegate void FireEvent();
    public FireEvent fireEvent;
    public abstract void Fire();
    public abstract void Init();
    public virtual IEnumerator Reload()
    {
        if (state == WeaponState.NEXT_WAIT)
        {
            Debug.Log("Reloaded during shot wait");
            yield break;
        }
        if(state == WeaponState.RELOADING)
        {
            Debug.Log("Already reloading");
            yield break;
        }
        state = WeaponState.RELOADING;
        Debug.Log("reloading");
        yield return reloadWait;
        Debug.Log("reloaded");
        currentAmmo = weaponBaseData.magazineSize;
        state = WeaponState.READY;
    }
}
