using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class Parasite : MonoBehaviour
{
    public ParasiteData parasiteData;
    //public Character character;
    List<Type> ParasiteComponents = new List<Type>() { typeof(SearchForJackSpots)};
    private void Awake()
    {
        //character = GetComponent<Character>();
        foreach (var component in ParasiteComponents)
        {
            if (!TryGetComponent(component, out _))
            {
                gameObject.AddComponent(component);
            }
        }
    }

}
