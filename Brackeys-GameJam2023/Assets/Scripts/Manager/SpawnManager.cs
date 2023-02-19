using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private int waveNumber = 0;
    private List<Character> enemies = new List<Character>();
    private Transform enemyParent;
    [SerializeField] private int enemyThreshold = 3;
    [SerializeField] private List<Character> characterTypes;
    [SerializeField] private List<SpawnPosition> spawnPositions = new List<SpawnPosition>();
    [SerializeField] private float timeBetweenSpawn;
    private bool canSpawnWave = true;

    private void SpawnNewWave()
    {
        if (canSpawnWave)
        {
            StartCoroutine(SpawnLogic(waveNumber++));
        }
    }
    public void RemoveEnemy(Character character)
    {
        enemies.Remove(character);
    }
    public void Initialise(Transform enemyParent)
    {
        characterTypes = new List<Character>(Resources.LoadAll<Character>("Prefabs/Enemies/"));
        this.enemyParent = enemyParent;
        StartCoroutine(CheckNewWave());
    }
    private IEnumerator CheckNewWave()
    {
        while (true)
        {
            if (enemies == null)
                break;
            if (enemies.Count <= enemyThreshold)
            {
                SpawnNewWave();
            }
            yield return new WaitForSeconds(1f);
        }
    }
    private IEnumerator SpawnLogic(int waveNumber)
    {
        //1) Count how many enemies to spawn
        //2) Iterate through each SpawnPoint and check to see if the spawn point is open to 
        //3) If the Spawn point it open for Instantiation. Then instantiate the enemy there
        //4) keep doing till all the enemies have spawned
        canSpawnWave = false;
        int totalCount = 0;
        int[] enemyCount = new int[characterTypes.Count];
        waveNumber = Mathf.Min(waveNumber, GameConfig.waveInfo.GetLength(1));
        for (int i = 0; i < characterTypes.Count; i++)
        {
            enemyCount[i] = GameConfig.waveInfo[i, waveNumber];
            totalCount += GameConfig.waveInfo[i, waveNumber];
        }
        int currentCount = 1;
        while (currentCount < totalCount)
        {
            foreach (SpawnPosition position in spawnPositions)
            {
                if (!position.isEmpty) continue;
                int i = GetIndex(enemyCount);
                if (i == -1) break;
                enemyCount[i]--;
                SpawnCharacterAtPosition(position, characterTypes[i]);
                currentCount++;
            }
            yield return new WaitForSeconds(timeBetweenSpawn);
        }
        canSpawnWave = true;
    }
    private int GetIndex(int[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] > 0)
                return i;
        }
        return -1;
    }
    private void SpawnCharacterAtPosition(SpawnPosition position, Character characterType)
    {
        if (!position.isEmpty) return;
        position.isEmpty = false;
        Character character = Instantiate(characterType, position.transform.position, position.transform.rotation, enemyParent);

        character.Switch(false);
        enemies.Add(character);
    }
}
