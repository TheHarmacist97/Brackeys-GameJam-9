using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

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

    private NavMeshAgent agent;
    private WeaponsManager weaponsManager;
    private Transform playerTransform;
    private Trigger outsideFalloffRangeTrigger;
    private float distanceToPlayer;
    private readonly float stunDuration = 10f;
    private float elapsedTime = 0f;
    private bool playerDead;

    private AimConstraint headAimConstraint;
    private AimConstraint lTurretConstraint;
    private AimConstraint rTurretConstraint;

    public Vector3 smoothTarget;
    private Vector3 lastTarget;

    private void OnEnable()
    {
        GameManager.Instance.playerSet += UpdateTarget;
        QuickTimeEvent.instance.hijackComplete += ResetEnemy;
    }

    private void OnDestroy()
    {
        if (!this.gameObject.scene.isLoaded) return;
        QuickTimeEvent.instance.hijackComplete -= ResetEnemy;
        GameManager.Instance.playerSet -= UpdateTarget;
    }


    private void Start()
    {
        character = GetComponent<Character>();
        data = character.data;
        playerTransform = GameManager.Instance.Player.GetComponent<Character>().data.center;
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
        headAimConstraint = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<AimConstraint>();
        lTurretConstraint = headAimConstraint.transform.GetChild(0).GetChild(0).GetComponent<AimConstraint>();
        rTurretConstraint = headAimConstraint.transform.GetChild(0).GetChild(1).GetComponent<AimConstraint>();
    }

    private void Update()
    {
        if (enemyState != EnemyState.STUN)
        {
            data.target.position = playerTransform.position;
            TrackDistance();
        }
        else if(!playerDead)
        {
            elapsedTime -= Time.deltaTime;
            if (elapsedTime <= 0)
            {
                SetStateOfConstraints(true);
                TrackDistance();
            }
        }

        if(playerTransform!=null)
        {
            smoothTarget = Vector3.Lerp(lastTarget, playerTransform.position, data.characterSpecs.muzzleRotateSpeed);
            lastTarget = smoothTarget;
            data.target.position = smoothTarget;
        }
    }

    private void TrackDistance()
    {
        if (playerTransform == null) return;
        distanceToPlayer = (transform.position - playerTransform.position).sqrMagnitude;
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
        agent.destination = playerTransform.position;
        weaponsManager.StopFiring();
    }

    private void StartAttack()
    {
        agent.ResetPath();
        weaponsManager.FireContinually(true, data.target);
    }

    private void UpdateTarget()
    {
        playerTransform = GameManager.Instance.Player.GetComponent<Character>().data.center;
        lastTarget = playerTransform.position;
    }

    public void StunEnemy()
    {
        Debug.Log("stunned");
        weaponsManager.StopFiring();
        SetStateOfConstraints(false);
        enemyState = EnemyState.STUN;
        elapsedTime = stunDuration;
    }
    
    public void ResetEnemy(bool result)
    {
        if(result)
        {
            enemyState = EnemyState.CHASE;
            elapsedTime = 0;
            SetStateOfConstraints(true);
        }
        else
        {
            enemyState = EnemyState.STUN;
            playerDead = true;
        }
    }
    

    private void SetStateOfConstraints(bool state)
    {
        headAimConstraint.enabled = state;
        lTurretConstraint.enabled = state;
        rTurretConstraint.enabled = state;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(smoothTarget, 1f);
    }
}
