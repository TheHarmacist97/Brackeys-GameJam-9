using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KOTHObjective : IObjective
{
    private Character player;
    private Transform objectiveTransform;

    private float fillRate = 1f;
    private float decayRate = 0.1f;
    private float fillBar = 0f;
    private float range = 5.0f;

    public override void ObjectiveCompleted()
    {
        GameManager.Instance.UpdateObjective(this);    
    }

    public override void StartObjective()
    {
        GetPlayer();
        GameManager.Instance.playerSet += GetPlayer;
    }
    private void Update()
    {
        if (player == null) return;
        if(Vector3.Distance(objectiveTransform.position, player.transform.position)<=range)
        {
            fillBar += fillRate * Time.deltaTime;
        }
        else
        {
            fillBar -= decayRate * Time.deltaTime;
        }

        if(fillBar>=100)
        {
            ObjectiveCompleted();
            this.enabled = false;
        }
    }
    private void GetPlayer()
    {
        this.player = GameManager.Instance.Player;
    }
}
