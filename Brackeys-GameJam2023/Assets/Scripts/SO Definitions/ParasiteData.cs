using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ParasiteSO", menuName = "ParasiteData", order = 5)]
public class ParasiteData : ScriptableObject
{
    [Range(0.1f, 2f)] public float boxCastThickness;
    public float maxRange;
    public float maxTime;
    public float rate;
    public int maxPulses;
    public LayerMask enemyLayer;
}
