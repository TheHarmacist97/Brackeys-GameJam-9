using System;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpawnManager))]
[RequireComponent(typeof(QuickTimeEvent))]
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
    public Action<Vector3> CharacterDeathEvent;

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
    public void KillPlayer()
    {
        CharacterDeathEvent(player.transform.position);
        player.Die();
    }

    private void Initialise()
    {
        spawnManager = GetComponent<SpawnManager>();
        spawnManager.Initialise(dependencyInjector.enemyParent);
        StartObjectives();
    }

    #region Public Functions
    public void HackCharacter(Character character)
    {
        if (!character.Equals(null))
            Debug.Log(character.gameObject.name);
        character.Switch(true);
        spawnManager.RemoveEnemy(character);
        Player = character;
    }
    public void EnemyDestroyed(Character character)
    {
        CharacterDeathEvent(character.transform.position);
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
        Debug.Log("Loading Next GamePlay Scene");
        GameSceneManager.Instance.SetScene(GameSceneManager.GameScene.GAME_PLAY);
    }
    public void GameOver()
    {
        Debug.Log("Loading Game Over");
        GameSceneManager.Instance.SetScene(GameSceneManager.GameScene.GAME_END);
    }
    public void PlayerCharacterDeath()
    {
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
        gate.Initialise(currentObjectives.Count);
    }

    
    #endregion

}
