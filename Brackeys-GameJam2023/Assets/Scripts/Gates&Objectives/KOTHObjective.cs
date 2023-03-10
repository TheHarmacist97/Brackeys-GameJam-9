using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KOTHObjective : IObjective
{
    [SerializeField]
    private Character player;
    [SerializeField]
    private float fillRate = 10f;
    [SerializeField]
    private float decayRate = 10f;
    [SerializeField]
    private float fillBar = 0f;
    [SerializeField]
    
    [Range(1f, 20f)]
    private float range = 10.0f;
    
    private bool objectiveCompleted;
    private KOTHAnimations animations;
    private KingOfTheHillMarker kothMarker;
    public override void ObjectiveCompleted()
    {
        Debug.Log("OBJECTIVE COMPLETED");
        objectiveCompleted = true;
        kothMarker.SetMarker(false);
        GameManager.Instance.UpdateObjective(this);    
    }

    public override void StartObjective()
    {
        Debug.Log("Objective Started");
        GetPlayer();
        objectiveCompleted = false;
        animations = GetComponent<KOTHAnimations>();
        kothMarker = transform.GetChild(1).GetChild(0).GetComponent<KingOfTheHillMarker>();
        GameManager.Instance.playerSet += GetPlayer;
    }
    private void Update()
    {
        if (player == null || objectiveCompleted) return;
        if(Vector3.Distance(transform.position, player.transform.position)<=range)
        {
            fillBar += fillRate * Time.deltaTime;
            animations.SetLerp(fillBar / 100f);
        }
        else
        {
            fillBar -= decayRate * Time.deltaTime;
            fillBar = Mathf.Max(fillBar, 0f);
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
