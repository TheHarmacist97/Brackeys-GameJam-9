using System.Collections;
using UnityEditor;
using UnityEngine;

public class SearchForJackSpots : MonoBehaviour
{
    public float maxRange;
    public LayerMask layer;
    public int maxPulses;
    public Collider[] jackInSpots;

    private int gotInRange;
    private bool alive = true;
    private float timeBetweenPulses;
    private WaitForSeconds pulseWait;
    private int currentPulses;

    private void Awake()
    {
        alive = true;
        jackInSpots = new Collider[20];
        timeBetweenPulses = 100f / maxPulses;
        pulseWait = new WaitForSeconds(timeBetweenPulses);
    }
    private void Start()
    {
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

    private void FixedUpdate()
    {
        if (gotInRange > 0)
        {
            for (int i = 0; i < gotInRange; i++)
            {
                float distance = Vector3.Distance(transform.position, jackInSpots[i].transform.position);
                Ray ray = new Ray(transform.position, jackInSpots[i].transform.position - transform.position);
                if (Physics.Raycast(ray, out RaycastHit hit, distance))
                {
                    if (hit.collider.CompareTag(GameConfig.Constants.JACK_TAG))
                    {
                        Debug.Log("Hit jackSpot "+hit.collider.transform.root.name);
                        Debug.DrawRay(transform.position, jackInSpots[i].transform.position - transform.position);
                    }
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, Vector3.up, maxRange);


    }
}
