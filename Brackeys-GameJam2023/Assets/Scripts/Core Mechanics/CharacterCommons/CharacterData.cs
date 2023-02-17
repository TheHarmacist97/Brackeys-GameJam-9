using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    //Class that holds externally stored data
    public List<Transform> muzzles;
    public List<WeaponSO> weaponsData;
    public CharacterSO characterSpecs;
    public CameraProfile cameraProfile;
    public Transform mobilityUnit;
    public Transform jackInSpot;
    public Transform target;
    public Transform lookAtTarget;
}
