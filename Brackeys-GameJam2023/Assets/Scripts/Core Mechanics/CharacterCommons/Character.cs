using System;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(WeaponsManager))]
[RequireComponent(typeof(CharacterMovementVFX))]
public class Character : MonoBehaviour, IDamageable
{
    private CharacterMovementVFX cmVFX;
    private WeaponsManager weaponsManager;

    private GameObject fpp;
    private GameObject tpp;

    #region public References
    public CharacterData data;
    public GameObject FirstPersonCamera
    {
        get
        {
            if (fpp == null)
                fpp = GameManager.Instance.dependencyInjector.firstPersonCamera;
            return fpp;
        }
    }
    public GameObject ThirdPersonCamera
    {
        get
        {
            if (tpp == null)
                tpp = GameManager.Instance.dependencyInjector.thirdPersonCamera;
            return tpp;
        }
    }

    public CharacterSO CharacterSpecs
    {
        get
        {
            return data.characterSpecs;
        }
    }
    public List<WeaponSO> WeaponData
    {
        get
        {
            return data.weaponsData;
        }

    }
    public int currentHealth { get => this.currentHealth; set => this.currentHealth = value; }
    public int totalHealth { get => this.totalHealth; set => this.totalHealth = value; }
    #endregion

    private List<Type> playerComponents = new List<Type> { typeof(PlayerMovement), typeof(PlayerCameraSystem), typeof(PlayerWeaponInput) };
    private List<Type> enemyComponents = new List<Type>();//{ typeof(Enemy), typeof(NavMeshAgent)}; will need these later

    private void Awake()
    {
        Debug.Log("awake called");
        CommonComponentGet();
        CommonComponentInit();
    }
    private void CommonComponentGet()
    {
        cmVFX = GetComponent<CharacterMovementVFX>();
        weaponsManager = GetComponent<WeaponsManager>();
    }

    private void CommonComponentInit()
    {
        cmVFX.Init(data.characterSpecs.muzzleRotateSpeed, data.turretBase, data.mobilityUnit);
        weaponsManager.Init(data.weaponsData.ToArray(), data.muzzles.ToArray());
    }
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


    #region IDamageable properties
    public void TakeDamage(int value)
    {
        currentHealth -= value;
        if (currentHealth < 0)
        {
            Die();
        }
    }

    public void TakeHeal(int value)
    {
        currentHealth += value;
        currentHealth = Mathf.Min(currentHealth, totalHealth);
    }

    public void Die()
    {
        Destroy(gameObject);
    }
    #endregion
}
