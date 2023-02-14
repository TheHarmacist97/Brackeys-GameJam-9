using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public DependencyInjector dependencyInjector;

    private Character player;
    private Action playerSet;
    public Character Player
    {
        get { return player; }
        set 
        {
            player = value; 
            playerSet?.Invoke();
        }
    }

    private void Awake()
    {
        if(Instance !=null && Instance!=this)
            Destroy(this);
        else
            Instance = this;
    }
    
}
