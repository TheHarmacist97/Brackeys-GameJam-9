using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class TeleportAbility : InputAbility
{
    private CharacterController controller;
    private void OnEnable() => Initialise();

    private void Initialise()
    {
        controller = GetComponent<CharacterController>();
    }
    public override void Fire(Vector3 target)
    {
        Teleport(transform.forward * 5f);
    }

    public override void FireContinually(bool fireFromEnemy, Transform target)
    {
        //Does Nothing
    }

    public override void ReloadAll()
    {
        //Does Nothing
    }

    public override void StopFiring()
    {
        //Does Nothing
    }
    private void Teleport(Vector3 vector3)
    {

    }
}
