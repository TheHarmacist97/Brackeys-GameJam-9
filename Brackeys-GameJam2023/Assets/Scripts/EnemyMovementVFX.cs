using System;
using UnityEngine;

public class EnemyMovementVFX : MonoBehaviour
{
    private CharacterData data;
    private float muzzleRotateSpeed;
    private Transform target;
    private Transform turretUnit;
    private Transform mobilityUnit;

    private bool playerDead;
    private Vector3 currentRot;
    private Vector3 difference;
    private Vector3 lastPos;
    private Vector3 origRot;

    private void OnEnable()
    {
        QuickTimeEvent.instance.hijackComplete += SetPlayerStatus;
    }

    private void OnDisable()
    {
        QuickTimeEvent.instance.hijackComplete -= SetPlayerStatus;
    }

    private void Awake()
    {
        data = GetComponent<Character>().data;
        GameManager.Instance.playerSet += UpdateTarget;
        UpdateTarget();
        Init();
    }

    private void UpdateTarget()
    {
        target = GameManager.Instance.Player.transform;
    }

    public void Init()
    {
        muzzleRotateSpeed = data.characterSpecs.muzzleRotateSpeed;
        mobilityUnit = data.mobilityUnit;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(!playerDead)
            TurretUnitOrientation();
        MobilityUnitOrientation();
    }
     
    private void SetPlayerStatus(bool result)
    {
        playerDead = result;
    }

    private void TurretUnitOrientation()
    {
        if(target!=null)
        {
            data.lookAtTarget.position = target.position;
        }
    }

    private void MobilityUnitOrientation()
    {
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
}
