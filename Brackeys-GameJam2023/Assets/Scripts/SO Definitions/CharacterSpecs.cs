using UnityEditor;
using UnityEngine;

public enum WalkType
{
    HOVER,
    TREAD,
    SPIDER
}

[CreateAssetMenu(fileName = "CharacterSpec", menuName = "Character Specification", order = 1)]
public class CharacterSpecs : ScriptableObject
{
    [Header("NavMesh Variables")]
    public float maxMoveSpeed;
    public float mass;
    public float jumpForce;
    public float accel;
    public float rotateSpeed;
    public float effectiveRange;
    public float falloffRange;

    [Header("Visuals")]
    public WalkType walkType;

    [Header("Turret Variables")]
    public float muzzleRotateSpeed;


    private float effectiveRangeSquare;
    public float EffectiveRangeSquare { 
        get
        {
            if (effectiveRange == 0)
                return 0;

            if(effectiveRangeSquare==0)
                effectiveRangeSquare = effectiveRange * effectiveRange;
            return effectiveRangeSquare;
        } 
        set { effectiveRangeSquare = value; }
    }
    private float falloffRangeSquare;
    public float FalloffRangeSquare
    {
        get
        {
            if(falloffRange==0)
                return 0;
            
            if(falloffRangeSquare == 0)
                falloffRangeSquare = falloffRange * falloffRange;
            return falloffRangeSquare;
        }
        set
        {
            falloffRange = value;
        }
    }
}
