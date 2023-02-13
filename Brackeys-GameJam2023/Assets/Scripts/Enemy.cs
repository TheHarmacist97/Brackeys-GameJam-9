using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        CHASE,
        ATTACK
    }
    [SerializeField] private CharacterSpecs specs;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private EnemyState enemyState;
    [SerializeField] private Transform target;

    private Trigger outsideFalloffRangeTrigger;
    private float distanceToPlayer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        Init();
        outsideFalloffRangeTrigger = new Trigger(()=>StartAttack());
    }

    private void Init()
    {
        agent.acceleration = specs.accel;
        agent.angularSpeed = specs.rotateSpeed;
        agent.speed = specs.maxMoveSpeed;
    }

    private void Update()
    {
        TrackDistance();
    }

    private void TrackDistance()
    {
        distanceToPlayer = (transform.position - target.position).sqrMagnitude;
        if (distanceToPlayer > specs.FalloffRangeSquare)
        {
            outsideFalloffRangeTrigger.Reset();   
            if (distanceToPlayer > specs.EffectiveRangeSquare)
            {
                GiveChase();
            }
        }
        else if (distanceToPlayer < specs.FalloffRangeSquare)
        {
            if(distanceToPlayer < specs.EffectiveRangeSquare)
            {
                outsideFalloffRangeTrigger.Fire();
            }
        }
    }

    private void GiveChase()
    {
        enemyState = EnemyState.CHASE;
        agent.destination = target.position;
    }

    private void StartAttack()
    {
        agent.ResetPath();
    }


    private void OnDrawGizmos()
    {
        Handles.color = Color.green;
        Handles.DrawWireDisc(target.position, Vector3.up, specs.falloffRange);
        Handles.color = Color.red;
        Handles.DrawWireDisc(target.position, Vector3.up, specs.effectiveRange);
        
    }

}
