using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSpec", menuName = "Character Specification", order = 1)]
public class CharacterSpecs : ScriptableObject
{
    public float maxMoveSpeed;
    public float mass;
    public float jumpForce;
    public float accel;
    public float rotateSpeed;
    public float effectiveRange;
    public float falloffRange;

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
