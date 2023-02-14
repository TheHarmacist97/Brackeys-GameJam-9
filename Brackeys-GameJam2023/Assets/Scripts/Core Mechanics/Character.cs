using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    [SerializeField] private CharacterSpecs characterSpecs;
    [SerializeField] private WeaponSO weaponSpecs;
    public GameObject fpp;
    public GameObject tpp;
    private void Awake()
    {
        Switch(true);
    }
    public CharacterSpecs CharacterSpecs
    {
        get
        {
            return characterSpecs;
        }
        private set { characterSpecs = value; }
    }
    public WeaponSO WeaponSpecs
    {
        get
        {
            return weaponSpecs;
        }
        private set { weaponSpecs = value; }
    }
    
    private List<Type> playerComponents = new List<Type> { typeof(PlayerMovement), typeof(PlayerCameraSystem) };
    private List<Type> enemyComponents = new List<Type>(); //{ typeof(Enemy), typeof(NavMeshAgent), typeof(WeaponsManager)}; will need these later
    public void Switch(bool isPlayer)
    {
        foreach (Type type in isPlayer ? enemyComponents : playerComponents)
        {
            if (TryGetComponent(type, out var component))
            {
                Destroy(component);
            }
        }
        foreach (Type type in isPlayer ? playerComponents : enemyComponents)
        {
            if (!TryGetComponent(type, out _))
            {
                gameObject.AddComponent(type);
            }
        }
    }

}
