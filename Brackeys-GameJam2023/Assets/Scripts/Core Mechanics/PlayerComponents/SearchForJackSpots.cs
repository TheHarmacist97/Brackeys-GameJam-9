using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SearchForJackSpots : MonoBehaviour
{
    public Collider[] enemyColliders;

    private bool isHit;
    private bool alive = true;
    private bool startedHijacking;
    private bool hijackingResultOut;
    private bool hijackedSuccessfully;
    private int gotInRange;
    private int currentPulses;
    private float timeBetweenPulses;

    private Parasite parasite;
    private ParasiteData data;

    private Transform mainCamTransform;
    private WaitForSeconds pulseWait;
    private Vector3 boxCastExtents;
    private RaycastHit hit;

    private Character characterTargeted;
    private PlayerMovement pMovement;
    private CharacterController characterController;
    private CinemachineFreeLook freeLook;
    private readonly float rate = 4f;

    private void OnEnable()
    {
        StartCoroutine(Subscribe());
    }

    private IEnumerator Subscribe()
    {
        yield return new WaitUntil(()=>QuickTimeEvent.instance!=null);
        alive = true;
        QuickTimeEvent.instance.hijackComplete += StopPulsing;
    }

    private void OnDisable()
    {
        QuickTimeEvent.instance.hijackComplete -= StopPulsing;
    }
    private void Awake()
    {
        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
        yield return null;
        parasite = GetComponent<Parasite>();
        mainCamTransform = Camera.main.transform;
        pMovement = GetComponent<PlayerMovement>();
        characterController = GetComponent<CharacterController>();
        freeLook = GetComponent<Character>().TppCinemachineCamera;
        data = parasite.parasiteData;

        enemyColliders = new Collider[20];
        timeBetweenPulses = data.maxTime / data.maxPulses;
        pulseWait = new WaitForSeconds(timeBetweenPulses);
        boxCastExtents = Vector3.one * data.boxCastThickness;
        boxCastExtents.y *= 2;
    }


    private void Start()
    {
        alive = true;
        QuickTimeEvent.instance.hijackComplete += StopPulsing;
        Invoke(nameof(PlayerCharacterDeath), 2f);
    }

    public void PlayerCharacterDeath()
    {
        StartCoroutine(PulseCoroutine());
    }

    private IEnumerator PulseCoroutine()
    {
        while (alive)
        {
            if (!startedHijacking)
            {
                currentPulses++;
            }
            yield return pulseWait;
            Pulse();
            if (currentPulses == parasite.parasiteData.maxPulses)
                alive = false;
        }
        Death();
    }

    private void StopPulsing(bool result)
    {
        alive = result;
        if(alive)
        {
            StopCoroutine(PulseCoroutine());
        }
    }

    private void Death()
    {
        GameManager.Instance.HackCharacter(null);
        Destroy(gameObject);
    }

    private void Pulse()
    {
        gotInRange = Physics.OverlapSphereNonAlloc(gameObject.transform.position,
            data.maxRange, enemyColliders, data.enemyLayer);
        for (int i = 0; i < gotInRange; i++)
        {
            Collider coll = enemyColliders[i];
            if (coll.TryGetComponent(out Enemy enemy))
            {
                enemy.StunEnemy();
            }
        }
    }

    private void Update()
    {
        if (gotInRange > 0)
        {
            if (Input.GetKeyDown(KeyCode.E) && !startedHijacking)
            {
                Debug.Log("Got casted");
                isHit = false;
                if (Physics.BoxCast(mainCamTransform.position, boxCastExtents
                    , mainCamTransform.forward, out hit, mainCamTransform.rotation, data.maxRange, data.enemyLayer))
                {
                    Debug.Log("Got jack " + hit.collider.name);
                    isHit = true;
                    startedHijacking = true;
                    StartJack();
                }
            }
        }
    }

    private void StartJack()
    {
        characterTargeted = hit.transform.GetComponent<Character>();
        Transform parent = characterTargeted.data.jackInSpot;
        transform.SetPositionAndRotation(parent.position, parent.rotation);
        transform.parent = parent;

        characterController.enabled = false;
        pMovement.enabled = false;
        QuickTimeEvent.instance.StartQTEWrapper(characterTargeted);
    }


    private IEnumerator ShootUp()
    {
        pMovement.enabled = false;
        characterController.enabled = false;
        float elapsedTime = 0f;
        Vector3 target = transform.position + Vector3.up * 25f;
        Vector3 startPos = transform.position;
        freeLook.LookAt = hit.transform;
        freeLook.Follow = null;
        while (elapsedTime <= rate)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, target, elapsedTime / rate);
        }
        StartCoroutine(Hijack());
    }

    private IEnumerator Hijack()
    {
        Vector3 start = hit.transform.position + (Vector3.up * 25f);
        Transform parent = hit.transform.GetComponent<Character>().data.jackInSpot;
        Vector3 target = parent.position;
        float elapsedTime = 0f;
        while (elapsedTime <= rate)
        {
            yield return null;
            elapsedTime += 3f * Time.deltaTime;
            transform.position = Vector3.Lerp(start, target, elapsedTime / rate);
        }
        freeLook.LookAt = transform;
        freeLook.Follow = transform;
        transform.rotation = parent.localRotation;
        transform.parent = parent;
    }


    private void OnDrawGizmos()
    {

        if (isHit)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(mainCamTransform.position, mainCamTransform.forward * hit.distance);
            Gizmos.DrawWireCube(mainCamTransform.position + mainCamTransform.forward * hit.distance, Vector3.one * parasite.parasiteData.boxCastThickness);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(mainCamTransform.position, mainCamTransform.forward * parasite.parasiteData.maxRange);
        }
    }
}

