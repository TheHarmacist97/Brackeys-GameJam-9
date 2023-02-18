using System.Collections;
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
    private Material baseBodyMat;
    private Material lineBodyMat;
    private void OnEnable() => Initialise();

    private void OnDisable()
    {
        QuickTimeEvent.Instance.hijackStarted -= StartedHijacking;
        QuickTimeEvent.Instance.hijackComplete -= CompletedHijacking;
        GameManager.Instance.PlayerDeathEvent -= TeleportBack;
    }

    private void Initialise()
    {
        QuickTimeEvent.Instance.hijackStarted += StartedHijacking;
        QuickTimeEvent.Instance.hijackComplete += CompletedHijacking;
        GameManager.Instance.PlayerDeathEvent += TeleportBack;
        controller = GetComponent<CharacterController>();
    }
    public override void Fire(Vector3 target)
    {
        if (canTeleport)
        {
            StartCoroutine(Teleport(transform.position + transform.forward * 5f));
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
    private IEnumerator Teleport(Vector3 destination)
    {
        canTeleport = false;
        transform.position = destination;
        yield return new WaitForSeconds(coolDown);
        canTeleport = true;
    }

    public void TeleportBack()
    {
        Debug.Log("Called");
        Vector3 destination = transform.position + (transform.up * 3f);
        StartCoroutine(Teleport(destination));
    }

    private void CompletedHijacking(bool result)
    {
        canTeleport = false;
    }

    private void StartedHijacking()
    {
        canTeleport = false;
        StopCoroutine(Teleport(transform.forward * 5f));
        StopAllCoroutines();
    }
}
