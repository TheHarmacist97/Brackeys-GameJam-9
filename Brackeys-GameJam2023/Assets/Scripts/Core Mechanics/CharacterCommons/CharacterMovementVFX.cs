using UnityEngine;

public class CharacterMovementVFX : MonoBehaviour
{
    public float muzzleRotateSpeed;
    public Vector3 target;
    private Transform turretUnit;
    private Transform mobilityUnit;


    private Vector3 currentRot;
    private Vector3 difference;
    private Vector3 lastPos;
    private Vector3 origRot;

    public void Init(float muzzleRotateSpeed, Transform turretUnit, Transform mobilityUnit)
    {
        this.turretUnit = turretUnit;
        this.mobilityUnit = mobilityUnit;
        this.muzzleRotateSpeed = muzzleRotateSpeed;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        MobilityUnitOrientation();
        TurretOrientation();
    }

    private void TurretOrientation()
    {
        currentRot = turretUnit.forward;
        currentRot = Vector3.RotateTowards(currentRot, target - turretUnit.position, muzzleRotateSpeed, 0.0f);
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
