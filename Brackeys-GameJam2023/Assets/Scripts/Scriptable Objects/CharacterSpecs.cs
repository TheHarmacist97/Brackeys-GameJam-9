using UnityEngine;

[CreateAssetMenu(fileName = "CharachterSpec", menuName = "Charachter Specification", order = 1)]
public class CharacterSpecs : ScriptableObject
{
    public float maxMoveSpeed;
    public float accel;
    public float rotateSpeed;
    public float effectiveRange;
    public WeaponSpecs weapon;
}
