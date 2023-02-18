using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
[RequireComponent(typeof(SpawnManager))]
public class GameManager : StaticInstances<GameManager>
{
    public DependencyInjector dependencyInjector;

    public Parasite parasite;
    public Character player;
     
    [SerializeField] private Gate gate;
    
    [SerializeField] private List<IObjective> currentObjectives = new List<IObjective>();



    private SpawnManager spawnManager;
    public Action playerSet;
    public Action PlayerDeathEvent;

    public Character Player
    {
        get { return player; }
        set
        {
            player = value;
            playerSet?.Invoke();
        }
    }   
    private void Start()
    {
        Initialise();
        player.Switch(true);
        playerSet?.Invoke();
    }
    private void Update()
    {
        if (player != null)
        {
            if(Input.GetKeyDown(KeyCode.Alpha0))
            {
                KillPlayer();
            }
        }
    }

    private void KillPlayer()
    {
        player.Die();
    }

    private void Initialise()
    {
        spawnManager = GetComponent<SpawnManager>();
        spawnManager.SpawnNewWave();
        StartObjectives();
    }

    #region Public Functions
    public void HackCharacter(Character character)
    {
        character.Switch(true);
        spawnManager.RemoveEnemy(character);
        Player = character;
    }
    public void EnemyDestroyed(Character character)
    {
        spawnManager.RemoveEnemy(character);
    }
    public void UpdateObjective(IObjective objective)
    {
        if (!currentObjectives.Contains(objective))
            return;
        currentObjectives.Remove(objective);
        //Update Gate
        gate.ObjecitvesUpdated();
    }
    public void LoadNextScene()
    {
        Debug.Log("NEXT SCENE");
    }

    public void PlayerCharacterDeath()
    {
        Debug.Log("death");
        parasite.transform.SetParent(null, true);
        PlayerDeathEvent?.Invoke();
        HackCharacter(parasite.GetComponent<Character>());
    }
    #endregion

    #region Utility Functions
    #region Spawner
    
    #endregion
    
    private void StartObjectives()
    {
        foreach(IObjective objectives in currentObjectives)
        {
            Debug.Log("Called startObjectives");
            objectives.StartObjective();
        }
    }

    
    #endregion

}
