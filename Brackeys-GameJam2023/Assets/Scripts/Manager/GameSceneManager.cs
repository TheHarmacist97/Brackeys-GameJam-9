using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    
    public static GameSceneManager instance;

    private void Awake()
    {
        if(instance!=null && instance != this)
        {
            Destroy(this);
        }
        else
            instance= this;
    }
}
