using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

public class Character : MonoBehaviour, IDamageable
{
    public bool isPlayer;
    private GameObject fpp;
    private GameObject tpp;
    private CinemachineFreeLook tppCamera;
    private CinemachineVirtualCamera fppCamera;
    private int _currentHealth;
    private int _totalHealth;
    public Action OnDamage;
    public enum CharacterType
    {
        ENEMY,
        HOST,
        PARASITE
    }

    public CharacterType _characterType;
    private List<Type> playerComponents = new List<Type> { typeof(PlayerMovement), typeof(PlayerCameraSystem), typeof(InputHandler), typeof(PlayerMovementVFX) };
    private List<Type> enemyComponents = new List<Type>() { typeof(Enemy), typeof(NavMeshAgent), typeof(EnemyMovementVFX) }; //will need these now 

    #region public References
    public CharacterData data;
    public GameObject FirstPersonCamera
    {
        get
        {
            if (fpp == null)
            {
                fpp = GameManager.Instance.dependencyInjector.firstPersonCamera;
                fppCamera = fpp.GetComponent<CinemachineVirtualCamera>();
            }
            return fpp;
        }
    }
    public GameObject ThirdPersonCamera
    {
        get
        {
            if (tpp == null)
            {
                tpp = GameManager.Instance.dependencyInjector.thirdPersonCamera;
                tppCamera = tpp.GetComponent<CinemachineFreeLook>();
            }
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
    public int currentHealth { get => _currentHealth; set => _currentHealth = value; }
    public int totalHealth { get => _totalHealth; set => _totalHealth = value; }
    public CinemachineFreeLook TppCinemachineCamera
    {
        get
        {
            if (tppCamera == null)
                _ = ThirdPersonCamera;
            return tppCamera;
        }
    }
    public CinemachineVirtualCamera FppCamera
    {
        get
        {
            if (fppCamera == null)
            {
                _ = FirstPersonCamera;
            }
            return fppCamera;
        }
    }
    #endregion

    private void Awake()
    {
        _totalHealth = data.characterSpecs.maxHealth;
        _currentHealth = _totalHealth;
    }
    public void Switch(bool isPlayer)
    {
        this.isPlayer = isPlayer;
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
        if (isPlayer)
        {
            if (TryGetComponent<Parasite>(out _))
            {
                _characterType = CharacterType.PARASITE;
            }
            else
            {
                _characterType = CharacterType.HOST;
                Debug.Log("Charachter SPECS UPDATED");
                _totalHealth = data.characterSpecs.maxHealth * 10;
            }
        }
        else
        {
            _characterType = CharacterType.ENEMY;
        }
        _currentHealth = _totalHealth;
    }


    #region IDamageable properties
    public void TakeDamage(int value)
    {
        currentHealth -= value;
        OnDamage?.Invoke();
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
        switch (_characterType)
        {
            case CharacterType.HOST:
                GameManager.Instance.PlayerCharacterDeath();
                break;
            case CharacterType.PARASITE:
                GameManager.Instance.GameOver();
                break;
            default:
                GameManager.Instance.EnemyDestroyed(this);
                break;
        }
        Destroy(gameObject, isPlayer ? 2f : 0f);
    }
    #endregion
}
