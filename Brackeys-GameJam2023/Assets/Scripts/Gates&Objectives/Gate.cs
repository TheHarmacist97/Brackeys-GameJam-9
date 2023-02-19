using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private int totalObjectives = 0;
    private int currentObjectives = 0;
    private CapsuleCollider boxColllider;
    public void Initialise(int totalObjectives)
    {
        boxColllider= GetComponent<CapsuleCollider>();
        currentObjectives = 0;
        this.totalObjectives = totalObjectives;
        boxColllider.isTrigger = false;
    }
    public void ObjecitvesUpdated()
    {
        currentObjectives++;
        if(currentObjectives>=totalObjectives)
        {
            boxColllider.isTrigger = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Character>(out Character character))
        {
            Debug.Log("CURRENT OBJECTIVES VS TOTAL OBJECTIVES: " + currentObjectives + " :: " + totalObjectives);
            if(currentObjectives >= totalObjectives)
                if(character._characterType==Character.CharacterType.HOST|| character._characterType==Character.CharacterType.PARASITE)
                GameManager.Instance.LoadNextScene();
        }
    }
}
