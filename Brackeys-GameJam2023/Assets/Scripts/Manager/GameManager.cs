using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : StaticInstances<GameManager>
{
    public DependencyInjector dependencyInjector;

    public Parasite parasite;
    public Character player;
     
    [SerializeField] private Gate gate;
    [SerializeField] private List<Character> characterTypes;
    [SerializeField] private List<IObjective> currentObjectives = new List<IObjective>();
    [SerializeField] private List<Transform> spawnPositions = new List<Transform>();
    [SerializeField] private Vector2 spawnArea = new Vector2(10, 10);

    private List<Character> enemies = new List<Character>();

    private int waveNumber = 0;
    private int enemyThreshold;
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
        SpawnCharacter(characterTypes[Random.Range(0, characterTypes.Count)]);
        //HackCharacter(enemies[0]);
        SpawnNewWave(characterTypes);
        StartObjectives();
    }

    #region Public Functions
    public void HackCharacter(Character character)
    {
        if (!enemies.Contains(character))
            return;
        character.Switch(true);
        enemies.Remove(character);
        CheckNewWave();
        Player = character;
    }
    public void EnemyDestroyed(Character character)
    {
        enemies.Remove(character);
        CheckNewWave();
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
    private void SpawnNewWave(List<Character> characterTypes)
    {
        foreach (Character character in characterTypes)
        {
            SpawnCharacter(character, GameConfig.waveInfo[0, waveNumber]);
        }
    }
    private void SpawnCharacter(Character characterType, int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            Character character = Instantiate(characterType, GetRandomPosition(), characterType.transform.rotation, dependencyInjector.enemyParent);
            character.Switch(false);
            enemies.Add(character);
        }
    }
    private Vector3 GetRandomPosition()
    {
        Vector3 spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Count)].position;
        spawnPosition += (Vector3.forward * Random.Range(-spawnArea.y, spawnArea.y) + Vector3.right * Random.Range(-spawnArea.x, spawnArea.x));
        return spawnPosition;
    }
    #endregion
    
    private void StartObjectives()
    {
        foreach(IObjective objectives in currentObjectives)
        {
            Debug.Log("Called startObjectives");
            objectives.StartObjective();
        }
    }

    private void CheckNewWave()
    {
        if (enemies.Count <= enemyThreshold)
        {
            SpawnNewWave(characterTypes);
        }
    }
    #endregion

}
