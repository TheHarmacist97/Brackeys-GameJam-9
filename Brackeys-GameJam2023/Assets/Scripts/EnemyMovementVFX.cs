using UnityEngine;

public class EnemyMovementVFX : MonoBehaviour
{
    private CharacterData data;
    private Transform mobilityUnit;

    private bool playerDead;
    private Vector3 difference;
    private Vector3 lastPos;
    private Vector3 origRot;
    private Enemy enemyComponent;

    private void OnEnable()
    {
        QuickTimeEvent.Instance.hijackComplete += SetPlayerStatus;
    }

    private void OnDisable()
    {
        QuickTimeEvent.Instance.hijackComplete -= SetPlayerStatus;
    }

    private void Awake()
    {
        data = GetComponent<Character>().data;
        enemyComponent = GetComponent<Enemy>();
        Init();
    }

    public void Init()
    {
        mobilityUnit = data.mobilityUnit;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!playerDead)
            TurretUnitOrientation();
        MobilityUnitOrientation();
    }

    private void SetPlayerStatus(bool result)
    {
        playerDead = result;
    }

    private void TurretUnitOrientation()
    {
        data.target.position = enemyComponent.smoothTarget;
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
