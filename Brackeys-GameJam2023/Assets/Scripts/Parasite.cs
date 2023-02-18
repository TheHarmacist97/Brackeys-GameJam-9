using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class Parasite : MonoBehaviour
{
    public ParasiteData parasiteData;
    private readonly List<Type> ParasiteComponents = new() { typeof(SearchForJackSpots), typeof(TeleportAbility) };
    private SearchForJackSpots searchForJackSpots;
    private TeleportAbility teleportAbility;
    private PlayerMovement playerMovement;
    private PlayerCameraSystem playerCameraSystem;
    private InputHandler inputHandler;
    private PlayerMovementVFX playerMovementVFX;
    private SpiderProceduralAnimation spiderProceduralAnimation;

    private void Awake()
    {
        GiveParasiticComponents();
    }
    private void Start()
    {
        GameManager.Instance.PlayerDeathEvent += OnBotDeath;
        QuickTimeEvent.Instance.hijackComplete += (result) => OnHijack(result);
        StartCoroutine(GetComponents());
    }

    private IEnumerator GetComponents()
    {
        yield return null;
        searchForJackSpots = GetComponent<SearchForJackSpots>();
        teleportAbility = GetComponent<TeleportAbility>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCameraSystem = GetComponent<PlayerCameraSystem>();
        inputHandler = GetComponent<InputHandler>();
        playerMovementVFX = GetComponent<PlayerMovementVFX>();
        spiderProceduralAnimation = transform.GetChild(0).GetComponent<SpiderProceduralAnimation>();
    }

    private void GiveParasiticComponents()
    {
        foreach (var component in ParasiteComponents)
        {
            if (!TryGetComponent(component, out _))
            {
                gameObject.AddComponent(component);
            }
        }
    }

    private void OnHijack(bool result)
    {
        if (!result) return;

        searchForJackSpots.enabled = false;
        teleportAbility.enabled = false;
        playerMovement.enabled = false;
        playerCameraSystem.enabled = false;
        inputHandler.enabled = false;
        playerMovementVFX.enabled = false;
        spiderProceduralAnimation.enabled = false;
    }

    private void OnBotDeath()
    {
        searchForJackSpots.enabled = true;
        teleportAbility.enabled = true;
        playerMovement.enabled = true;
        playerCameraSystem.enabled = true;
        inputHandler.enabled = true;
        playerMovementVFX.enabled = true;
        spiderProceduralAnimation.enabled = true;
        AfterBotDeath();
    }

    private void AfterBotDeath()
    {
        teleportAbility.TeleportBack();
        playerMovement.Reset();
        searchForJackSpots.ResetParasite();
    }
}
