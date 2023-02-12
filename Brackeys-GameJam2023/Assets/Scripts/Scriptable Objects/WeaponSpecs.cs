using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireType
{
    SINGLE,
    AUTO,
    BURST,
    MULTI_PROJECTILE
}

[CreateAssetMenu(fileName = "WeaponSpec", menuName = "Weapon Specification", order = 2)]
public class WeaponSpecs : ScriptableObject
{
    public float damage;
    public float magazineSize;
    public FireType fireType;

}
