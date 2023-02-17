using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        STUN,
        CHASE,
        ATTACK
    }
    private Character character;
    private CharacterData data;
    [SerializeField] private EnemyState enemyState;
    [SerializeField] private Transform target;

    private NavMeshAgent agent;
    private WeaponsManager weaponsManager;

    private Trigger outsideFalloffRangeTrigger;
    private float distanceToPlayer;
    private readonly float stunDuration = 10f;
    private float elapsedTime = 0f;

    private void Awake()
    {
        character = GetComponent<Character>();
        data = character.data;
        agent = GetComponent<NavMeshAgent>();
        weaponsManager = GetComponent<WeaponsManager>();
        Init();
        outsideFalloffRangeTrigger = new Trigger(() => { StartAttack(); });
        GameManager.Instance.playerSet += UpdateTarget;
    }

    private void Init()
    {
        agent.updateRotation = false;
        agent.acceleration = data.characterSpecs.accel;
        agent.angularSpeed = data.characterSpecs.rotateSpeed;
        agent.speed = data.characterSpecs.maxMoveSpeed;
    }

    private void Update()
    {
        if (enemyState != EnemyState.STUN)
            TrackDistance();
        else
        {
            elapsedTime -= Time.deltaTime;
            if (elapsedTime <= 0)
            {
                TrackDistance();
            }
        }
    }

    private void TrackDistance()
    {
        if (target == null) return;
        distanceToPlayer = (transform.position - target.position).sqrMagnitude;
        if (distanceToPlayer > data.characterSpecs.FalloffRangeSquare)
        {
            outsideFalloffRangeTrigger.Reset();
            if (distanceToPlayer > data.characterSpecs.EffectiveRangeSquare)
            {
                GiveChase();
            }
        }
        else if (distanceToPlayer < data.characterSpecs.FalloffRangeSquare)
        {
            if (distanceToPlayer < data.characterSpecs.EffectiveRangeSquare)
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

    private void UpdateTarget()
    {
        target = GameManager.Instance.Player.transform;
    }

    public void StunEnemy()
    {
        enemyState = EnemyState.STUN;
        elapsedTime = stunDuration;
    }

}
