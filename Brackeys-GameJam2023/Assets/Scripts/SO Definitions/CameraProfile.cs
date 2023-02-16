using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CamProfile", menuName = "Camera Profile", order = 6)]
public class CameraProfile : ScriptableObject
{
    public float verticalFOVTPP;
    public float verticalFOVFPP;
    public RigData[] rigs;
}

[System.Serializable]   
public class RigData
{
    public float rigHeight;
    public float rigRadius;
    public float bodyYDamping;
    public float bodyZDamping;
    public float aimScreenX;
    public float aimScreenY;
    public float softZoneWidth;
    public float softZoneHeight;
}


