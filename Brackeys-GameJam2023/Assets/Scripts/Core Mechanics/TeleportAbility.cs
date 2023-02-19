using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class TeleportAbility : InputAbility
{
    private readonly float range = 5f;

    private float coolDown = 5f;
    private bool canTeleport = true;
    private CharacterController controller;
    private Material[] materials;
    private void OnEnable() => Initialise();

    private void OnDisable()
    {
        QuickTimeEvent.Instance.hijackStarted -= StartedHijacking;
        QuickTimeEvent.Instance.hijackComplete -= CompletedHijacking;
    }

    private void Awake()
    {
        materials = transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<Renderer>().sharedMaterials;
    }

    private void Initialise()
    {
        QuickTimeEvent.Instance.hijackStarted += StartedHijacking;
        QuickTimeEvent.Instance.hijackComplete += CompletedHijacking;
        //GameManager.Instance.PlayerDeathEvent += TeleportBack;
        controller = GetComponent<CharacterController>();
    }
    public override void Fire(Vector3 target)
    {
        Vector3 destination;
        if (canTeleport)
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, range, GameConfig.Constants.GROUND_TAG_INDEX))
            {
                destination = hit.point;
            }
            else
            {
                destination = transform.position + transform.forward * range;
            }
            StartCoroutine(Teleport(destination));
        }
    }

    public IEnumerator ChangeMaterialProperty(float target, float time, params Material[] materials)
    {
        float elapsedTime = 0f;
        float start = materials[0].GetFloat("_TeleportValue");
        while (elapsedTime < time)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            foreach (Material mat in materials)
            {
                mat.SetFloat("_TeleportValue", Mathf.Lerp(start, target, elapsedTime / time));
            }
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
        yield return StartCoroutine(ChangeMaterialProperty(1f, 0.2f, materials));
        canTeleport = false;
        transform.position = destination;
        StartCoroutine(ChangeMaterialProperty(0f, 0.75f, materials[0]));
        yield return StartCoroutine(ChangeMaterialProperty(0f, coolDown, materials[1]));
        canTeleport = true;
    }

    public void TeleportBack()
    {
        Vector3 destination;

        if (Physics.Raycast(transform.position, transform.up, out RaycastHit hit, range, GameConfig.Constants.GROUND_TAG_INDEX))
        {
            destination = hit.point;
        }
        else
        {
            destination = transform.position + transform.up * range;
        }
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
