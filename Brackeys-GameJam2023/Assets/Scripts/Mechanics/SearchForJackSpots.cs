using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchForJackSpots : MonoBehaviour
{
    public Collider[] enemyColliders;

    private bool alive = true;
    private bool startedHijacking;
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

    
    public void ResetParasite()
    {
        alive = true;
        startedHijacking = false;
        hijackedSuccessfully = false;
        currentPulses = 0;
        characterTargeted = null;
        StartCoroutine(PulseCoroutine());
    }

    private void OnEnable()
    {
        ResetParasite();
        Subscribe();
    }

    private void Subscribe()
    {
        alive = true;
        QuickTimeEvent.Instance.hijackComplete += StopPulsing;
    }

    private void OnDisable()
    {
        QuickTimeEvent.Instance.hijackComplete -= StopPulsing;
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
    }


    private void Start()
    {
        alive = true;
        QuickTimeEvent.Instance.hijackComplete += StopPulsing;
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
            yield return pulseWait;
            if (!startedHijacking)
            {
                Debug.Log("Pulse minus");
                currentPulses++;
            }
            Pulse();
            if (currentPulses == parasite.parasiteData.maxPulses)
                alive = false;
            if (hijackedSuccessfully) yield break;
        }
        Death();
    }

    private void StopPulsing(bool result)
    {
        hijackedSuccessfully = result;
        enabled = !result;
        if(!result)
        {
            StopAllCoroutines();
            Death();
        }
    }

    private void Death()
    {
        if (!alive&&!hijackedSuccessfully)
        {
            GameManager.Instance.KillPlayer();
            Destroy(gameObject);
        }
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
        if (gotInRange > 0&&!startedHijacking)
        {
            if (Physics.BoxCast(mainCamTransform.position, boxCastExtents
                , mainCamTransform.forward, out hit, mainCamTransform.rotation, data.maxRange, data.enemyLayer))
            {
                TargetedUI.Instance.SetPosition(hit.transform.position + (Vector3.up * 1.5f));
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("Got casted");
                    Debug.Log("Got jack " + hit.collider.name);
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
        QuickTimeEvent.Instance.StartQTEWrapper(characterTargeted);
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

}

