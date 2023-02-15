using System;
using System.Collections;
using UnityEngine;

public class SearchForJackSpots : MonoBehaviour
{
    public Collider[] jackInSpots;

    private int gotInRange;
    private bool alive = true;
    private bool isHit;
    private float timeBetweenPulses;
    private int currentPulses;

    private RaycastHit hit;
    private WaitForSeconds pulseWait;
    private Parasite parasite;
    private Transform mainCamTransform;
    private PlayerMovement pMovement;
    private Vector3 boxCastExtents;
    private bool startedHijacking;

    private void OnEnable()
    {
        alive = true;
    }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        parasite = GetComponent<Parasite>();    
        jackInSpots = new Collider[20];
        timeBetweenPulses = parasite.parasiteData.maxTime / parasite.parasiteData.maxPulses;
        pulseWait = new WaitForSeconds(timeBetweenPulses);
        boxCastExtents = Vector3.one * parasite.parasiteData.thickness;
        boxCastExtents.y *= 2;

        pMovement = GetComponent<PlayerMovement>();
        mainCamTransform = Camera.main.transform;
    }


    private void Start()
    {
        mainCamTransform = Camera.main.transform;   
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
            currentPulses++;
            yield return pulseWait;
            Pulse();
            if (currentPulses == parasite.parasiteData.maxPulses)
                alive = false;
        }
    }

    private void Pulse()
    {
        gotInRange = Physics.OverlapSphereNonAlloc(gameObject.transform.position, parasite.parasiteData.maxRange, jackInSpots, parasite.parasiteData.layer);
    }
    private void Update()
    {
        if (gotInRange > 0)
        {
            if(Input.GetKeyDown(KeyCode.E)&&!startedHijacking)
            {
                Debug.Log("Got casted");
                if(Physics.BoxCast(mainCamTransform.position, boxCastExtents, mainCamTransform.forward, out hit, mainCamTransform.rotation, parasite.parasiteData.maxRange))
                {
                    isHit = false;
                    if(hit.transform.CompareTag(GameConfig.Constants.JACK_TAG))
                    {
                        Debug.Log("Got jack "+hit.collider.name);
                        isHit = true;
                        startedHijacking = true;
                        StartCoroutine(JackParasite(hit.transform.position));
                    }
                }
            }
        }
    }

    private IEnumerator JackParasite(Vector3 target)
    {
        float elapsedTime = 0f;
        Vector3 startPos = transform.position;
        while(elapsedTime <= parasite.parasiteData.rate)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, target, elapsedTime / parasite.parasiteData.rate);
        }
        transform.forward = hit.normal;
    }
    private void OnDrawGizmos()
    {
        
        if (isHit)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(mainCamTransform.position, mainCamTransform.forward * hit.distance);
            Gizmos.DrawWireCube(mainCamTransform.position + mainCamTransform.forward * hit.distance, Vector3.one * parasite.parasiteData.thickness);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(mainCamTransform.position, mainCamTransform.forward * parasite.parasiteData.maxRange);
        }
    }
}

