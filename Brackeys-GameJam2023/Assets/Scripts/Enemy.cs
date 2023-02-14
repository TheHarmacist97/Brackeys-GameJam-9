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
        outsideFalloffRangeTrigger = new Trigger(() => { StartAttack();  });
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

    private void FixedUpdate()
    {
        TreadMovement();
        TurretOrientation();
    }

    private void TurretOrientation()
    {
        currentRot = turretUnit.forward;
        currentRot = Vector3.RotateTowards(currentRot, target.position - turretUnit.position, specs.muzzleRotateSpeed, 0.0f);
        currentRot.y = Mathf.Clamp(currentRot.y, -0.5f, 0.5f);
        turretUnit.forward = currentRot;
    }

    private void TreadMovement()
    {
        if (!willTurn) return;

        difference = transform.position - lastPos;
        difference.Normalize();
        difference.y = 0;
        if (difference.sqrMagnitude > 0)
        {
            origRot = mobilityUnit.transform.forward;
            mobilityUnit.transform.forward = Vector3.Lerp(origRot, difference, 0.2f);

        }
        lastPos = transform.position;
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
        weaponsManager.weapon.StopFiring();
    }

    private void StartAttack()
    {
        agent.ResetPath();
        weaponsManager.weapon.FireContinually();
    }


    private void OnDrawGizmos()
    {
        if (target == null || specs == null) return;
        Handles.color = Color.green;
        Handles.DrawWireDisc(target.position, Vector3.up, specs.falloffRange);
        Handles.color = Color.red;
        Handles.DrawWireDisc(target.position, Vector3.up, specs.effectiveRange);

        Gizmos.color = Color.red;
        Debug.DrawRay(turretUnit.transform.position, currentRot * 5f);
    }

}
