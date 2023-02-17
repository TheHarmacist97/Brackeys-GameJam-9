using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class Parasite : MonoBehaviour
{
    public ParasiteData parasiteData;
    private readonly List<Type> ParasiteComponents = new() { typeof(SearchForJackSpots)};
    private void Awake()
    {
        GiveParasiticComponents();
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
}
