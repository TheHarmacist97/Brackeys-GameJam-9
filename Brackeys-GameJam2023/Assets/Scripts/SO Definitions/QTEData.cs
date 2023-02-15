using UnityEngine;

[System.Serializable]
public class QTEData
{
    public int winsRequired;
    public int waves;
    public float timeAllowedPerWave;
    public float bufferTime;
    [Range(0.1f,1f)] public float successDecayRate;
    [Range(0.05f, 0.2f)] public float correctHitIncrement;
}
