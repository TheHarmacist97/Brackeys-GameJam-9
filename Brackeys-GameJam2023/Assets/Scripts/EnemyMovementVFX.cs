using System;
using UnityEngine;

public class EnemyMovementVFX : MonoBehaviour
{
    private CharacterData data;
    private float muzzleRotateSpeed;
    private Transform target;
    private Transform turretUnit;
    private Transform mobilityUnit;


    private Vector3 currentRot;
    private Vector3 difference;
    private Vector3 lastPos;
    private Vector3 origRot;

    private void Awake()
    {
        data = GetComponent<Character>().data;
        GameManager.Instance.playerSet += UpdateTarget;
        Init();
    }

    private void UpdateTarget()
    {
        target = GameManager.Instance.Player.transform;
    }

    public void Init()
    {
        turretUnit = data.turretBase;
        muzzleRotateSpeed = data.characterSpecs.muzzleRotateSpeed;
        mobilityUnit = data.mobilityUnit;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(target != null)
            TurretOrientation();
        MobilityUnitOrientation();
    }

    private void TurretOrientation()
    {
        currentRot = turretUnit.forward;
        currentRot = Vector3.RotateTowards(currentRot, target.position - turretUnit.position, muzzleRotateSpeed, 0.0f);
        currentRot.y = Mathf.Clamp(currentRot.y, -0.5f, 0.5f);
        turretUnit.forward = currentRot;
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
