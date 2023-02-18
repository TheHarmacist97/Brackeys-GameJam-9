using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

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
    public Transform center;
    public Transform lookAtTarget;
    public AimConstraint headConstraint;
    public AimConstraint leftTurretConstraint;
    public AimConstraint rightTurretConstraint;
}
