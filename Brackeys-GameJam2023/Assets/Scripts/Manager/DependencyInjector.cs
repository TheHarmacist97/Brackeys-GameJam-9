using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This purpose of this class is to give other scripts references to objects in Scene
[System.Serializable]
public class DependencyInjector
{
    public GameObject firstPersonCamera;
    public GameObject thirdPersonCamera;
    public Transform enemyParent;
    public Transform objectiveParent;
}
