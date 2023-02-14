using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour, IDamageble
{
    [SerializeField] private CharacterSpecs characterSpecs;
    [SerializeField] private WeaponSO weaponSpecs;

    private GameObject fpp;
    private GameObject tpp;

    #region public References
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
    public int currentHealth { get => this.currentHealth; set => this.currentHealth = value; }
    public int totalHealth { get => this.totalHealth; set => this.totalHealth = value; }
    #endregion

    private List<Type> playerComponents = new List<Type> { typeof(PlayerMovement), typeof(PlayerCameraSystem), typeof(WeaponsManager) };
    private List<Type> enemyComponents = new List<Type>();//{ typeof(Enemy), typeof(NavMeshAgent), typeof(WeaponsManager)}; will need these later
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
}
