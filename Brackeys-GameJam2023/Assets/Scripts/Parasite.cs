using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class Parasite : MonoBehaviour
{
    public ParasiteData parasiteData;
    private readonly List<Type> ParasiteComponents = new() { typeof(SearchForJackSpots), typeof(TeleportAbility)};
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
        searchForJackSpots = GetComponent<SearchForJackSpots>();
        teleportAbility = GetComponent<TeleportAbility>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCameraSystem= GetComponent<PlayerCameraSystem>();
        inputHandler= GetComponent<InputHandler>(); 
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

    private void SetComponentState(bool state)
    {

    }
}
