using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputAbility : MonoBehaviour
{
    public abstract void Fire(Vector3 target);
    public abstract void FireContinually(bool fireFromEnemy, Transform target);
    public abstract void ReloadAll();
    public abstract void StopFiring();
}
