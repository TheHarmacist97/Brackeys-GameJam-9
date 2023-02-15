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
    [SerializeField] private CharacterSO specs;
    [SerializeField] private EnemyState enemyState;
    [SerializeField] private Transform target;

    private NavMeshAgent agent;
    private WeaponsManager weaponsManager;

    private Trigger outsideFalloffRangeTrigger;
    private float distanceToPlayer;

    [Header("Movement Visuals")]
    [SerializeField] Transform turretUnit;
    [SerializeField] Transform mobilityUnit;
    [SerializeField] float turnThreshhold;
    private Vector3 lastPos, difference, origRot, currentRot;
    private bool willTurn;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        weaponsManager = GetComponent<WeaponsManager>();
        Init();
        outsideFalloffRangeTrigger = new Trigger(() => { StartAttack(); });
        GameManager.Instance.playerSet += UpdateTarget;
    }
    private void Start()
    {
        lastPos = transform.position;
        switch (specs.walkType)
        {
            case WalkType.TREAD:
                willTurn = true;
                break;
            case WalkType.HOVER:
            case WalkType.SPIDER:
                break;
        }
        lastPos = transform.position;
    }

    private void Update()
    {
        TrackDistance();
    }

    private void Init()
    {
        agent.updateRotation = false;
        agent.acceleration = specs.accel;
        agent.angularSpeed = specs.rotateSpeed;
        agent.speed = specs.maxMoveSpeed;
    }

    private void TrackDistance()
    {
        if (target == null) return;
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
            if (distanceToPlayer < specs.EffectiveRangeSquare)
            {
                outsideFalloffRangeTrigger.Fire();
            }
        }
    }

    private void GiveChase()
    {
        enemyState = EnemyState.CHASE;
        agent.destination = target.position;
        weaponsManager.StopFiring();
    }

    private void StartAttack()
    {
        agent.ResetPath();
        weaponsManager.FireContinually(true);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(turretUnit.transform.position, currentRot * 5f);
    }
    private void UpdateTarget()
    {
        target = GameManager.Instance.Player.transform;
    }
}
