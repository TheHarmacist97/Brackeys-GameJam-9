using System;
using System.Collections;
using UnityEngine;

public class SearchForJackSpots : MonoBehaviour
{
    [Range(0.1f, 2f)]public float thickness;
    public float maxRange;
    public float rate;
    public int maxPulses;
    public LayerMask layer;
    public Collider[] jackInSpots;

    private int gotInRange;
    private bool alive = true;
    private bool isHit;
    private float timeBetweenPulses;
    private int currentPulses;

    private RaycastHit hit;
    private WaitForSeconds pulseWait;
    [SerializeField] private Transform mainCamTransform;
    [SerializeField] private PlayerMovement pMovement;
    private Vector3 boxCastExtents;
    private bool startedHijacking;

    private void OnEnable()
    {
        alive = true;
    }

    private void Init(Transform cameraTransform, PlayerMovement movement)
    {
        mainCamTransform = cameraTransform;
        pMovement = movement;
    }

    private void Awake()
    {
        jackInSpots = new Collider[20];
        timeBetweenPulses = 100f / maxPulses;
        pulseWait = new WaitForSeconds(timeBetweenPulses);
        boxCastExtents = Vector3.one*thickness;
        boxCastExtents.y *= 2;
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
            if (currentPulses == maxPulses)
                alive = false;
        }
    }

    private void Pulse()
    {
        gotInRange = Physics.OverlapSphereNonAlloc(gameObject.transform.position, maxRange, jackInSpots, layer);
    }
    private void Update()
    {
        if (gotInRange > 0)
        {
            if(Input.GetKeyDown(KeyCode.E)&&!startedHijacking)
            {
                Debug.Log("Got casted");
                if(Physics.BoxCast(mainCamTransform.position, boxCastExtents, mainCamTransform.forward, out hit, mainCamTransform.rotation, maxRange))
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
        while(elapsedTime <=rate)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, target, elapsedTime / rate);
        }
        transform.forward = hit.normal;
    }
    private void OnDrawGizmos()
    {
        if (isHit)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(mainCamTransform.position, mainCamTransform.forward * hit.distance);
            Gizmos.DrawWireCube(mainCamTransform.position + mainCamTransform.forward * hit.distance, Vector3.one*thickness);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(mainCamTransform.position, mainCamTransform.forward * maxRange);
        }
    }
}

