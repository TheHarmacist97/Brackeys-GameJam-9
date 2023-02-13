using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private CharacterSpecs characterSpecs;
    [SerializeField] private WeaponSpecs weaponSpecs;
    public CharacterSpecs CharacterSpecs
    {
        get
        {
            return characterSpecs;
        }
        private set { characterSpecs = value; }
    }
    public WeaponSpecs WeaponSpecs
    {
        get
        {
            return weaponSpecs;
        }
        private set { weaponSpecs = value; }
    }
    private List<Type> playerComponents = new List<Type> { typeof(PlayerMovement) };
    private List<Type> enemyComponents = new List<Type>();
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
