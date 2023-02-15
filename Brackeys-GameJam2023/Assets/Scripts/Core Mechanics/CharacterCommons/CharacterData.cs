using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    //Class that holds externally stored data
    public Transform mobilityUnit;
    public Transform turretBase;
    public List<Transform> muzzles;
    public List<WeaponSO> weaponsData;
    public CharacterSO characterSpecs;
    public Collider jackInSpot;
}
