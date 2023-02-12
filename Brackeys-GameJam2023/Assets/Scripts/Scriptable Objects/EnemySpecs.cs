using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpec", menuName = "Enemy Specification", order = 1)]
public class EnemySpecs : ScriptableObject
{
    public float maxMoveSpeed;
    public float accel;
    public float rotateSpeed;
    public float effectiveRange;
    public WeaponSpecs weapon;
}
