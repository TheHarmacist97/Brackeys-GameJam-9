using System;
using UnityEngine;

public class PlayerMovementVFX : MonoBehaviour
{
    private CharacterData data;
    private Camera cameraMain;
    private Transform turretUnit;

    private float rotateSpeed;
    private Vector3 target;
    private Vector3 currentRot;

    private void Awake()
    {
        cameraMain = Camera.main;
        data = GetComponent<Character>().data;
        Init();
    }

    public void Init()
    {
        turretUnit = data.turretBase;
        rotateSpeed = data.characterSpecs.rotateSpeed;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        target = cameraMain.ScreenToWorldPoint(Vector3.one*0.5f);
        TurretOrientation();
    }

    private void TurretOrientation()
    {
        currentRot = turretUnit.forward;
        currentRot = Vector3.RotateTowards(currentRot, turretUnit.position - target, rotateSpeed, 0.0f);
        currentRot.y = Mathf.Clamp(currentRot.y, -0.5f, 0.5f);
        turretUnit.forward = currentRot;
    }
}
