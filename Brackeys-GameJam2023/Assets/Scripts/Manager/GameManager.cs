using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public DependencyInjector dependencyInjector;

    private Character player;
    public Action playerSet;

    [SerializeField] private Gate gate;
    [SerializeField] private List<Character> characterTypes;
    [SerializeField] private List<IObjective> currentObjectives = new List<IObjective>();
    [SerializeField] private List<Transform> spawnPositions = new List<Transform>();
    [SerializeField] private Vector2 spawnArea = new Vector2(1, 1);

    private List<Character> enemies = new List<Character>();

    private int waveNumber = 0;
    private int enemyThreshold;

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
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
    private void Start()
    {
        Initialise();
    }
    private void Initialise()
    {
        SpawnCharacter(characterTypes[Random.Range(0, characterTypes.Count)]);
        HackCharacter(enemies[0]);
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
            Character character = Instantiate(characterType, spawnPositions[Random.Range(0, spawnPositions.Count)].position, characterType.transform.rotation, dependencyInjector.enemyParent);
            character.Switch(false);
            enemies.Add(character);
        }
    }
    private Vector3 GetRandomPosition()
    {
        Vector3 spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Count)].position;
        spawnPosition += (Vector3.forward * Random.Range(-spawnArea.y, spawnArea.y) + Vector3.right * Random.Range(-spawnArea.x, spawnArea.x));
        return Vector3.zero;
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
