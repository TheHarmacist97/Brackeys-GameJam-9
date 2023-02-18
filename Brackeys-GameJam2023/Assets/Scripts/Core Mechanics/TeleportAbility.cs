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

    private void OnEnable() => StartCoroutine(Initialise());

    private void OnDisable()
    {
        QuickTimeEvent.instance.hijackStarted -= StartedHijacking;
        QuickTimeEvent.instance.hijackComplete -= CompletedHijacking;
    }

    private IEnumerator Initialise()
    {
        yield return null;  
        QuickTimeEvent.instance.hijackStarted += StartedHijacking;
        QuickTimeEvent.instance.hijackComplete += CompletedHijacking;
        controller = GetComponent<CharacterController>();
    }
    public override void Fire(Vector3 target)
    {
        if(canTeleport)
        {
            StartCoroutine(Teleport(transform.forward * 5f));
        }
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
    private IEnumerator Teleport(Vector3 vector3)
    {
        canTeleport= false;        
        transform.position = transform.position + transform.forward * range;
        yield return new WaitForSeconds(coolDown);
        canTeleport= true;
    }
    private void CompletedHijacking(bool result)
    {
        canTeleport = result;
    }

    private void StartedHijacking()
    {
        canTeleport = false;
    }    
}
