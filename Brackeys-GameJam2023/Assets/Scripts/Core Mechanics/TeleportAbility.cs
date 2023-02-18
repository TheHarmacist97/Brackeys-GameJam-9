using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class TeleportAbility : InputAbility
{
    [SerializeField]
    private float range;
    [SerializeField]
    private float coolDown;
    private bool canTeleport = true;
    private CharacterController controller;

    private void OnEnable() => Initialise();

    private void Initialise()
    {
        controller = GetComponent<CharacterController>();
    }
    public override void Fire(Vector3 target)
    {
        if(canTeleport)
        {
            StartCoroutine(Teleport(transform.forward * 5f));
        }
    }

    public override void FireContinually(bool fireFromEnemy, Vector3 target)
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
    private IEnumerator Teleport(Vector3 vector3)
    {
        canTeleport= false;        
        transform.position = transform.position + transform.forward * range;
        yield return new WaitForSeconds(coolDown);
        canTeleport= true;
    }
}
