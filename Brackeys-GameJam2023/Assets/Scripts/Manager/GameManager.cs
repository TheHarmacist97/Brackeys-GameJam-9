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
    [SerializeField] private List<GameObject> objectiveTypes;

    private List<Character> enemies = new List<Character>();
    private List<IObjective> currentObjectives = new List<IObjective>();

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
    }
    public void LoadNextScene()
    {

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
            Character character = Instantiate(characterType, dependencyInjector.enemyParent);
            character.Switch(false);
            enemies.Add(character);
        }
    }
    #endregion
    
    private void StartObjectives()
    {
        
    }
    private void CreateObjective(GameObject objectiveType)
    {
        IObjective objective = Instantiate(objectiveType, dependencyInjector.objectiveParent).GetComponent<IObjective>();
        objective.StartObjective();
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
