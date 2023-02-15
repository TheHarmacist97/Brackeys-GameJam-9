using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private int totalObjectives = 0;
    private int currentObjectives = 0;
    public void Initialise(int totalObjectives)
    {
        currentObjectives = 0;
        this.totalObjectives = totalObjectives;

    }
    public void ObjecitvesUpdated()
    {
        currentObjectives++;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameConfig.Constants.PLAYER_TAG) && currentObjectives>=totalObjectives)
        {
            //Player to die
            GameManager.Instance.LoadNextScene();
        }
    }
}
