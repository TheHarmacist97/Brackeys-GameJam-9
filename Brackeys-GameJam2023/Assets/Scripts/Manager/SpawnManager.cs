using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private int waveNumber = 0;
    private List<Character> enemies = new List<Character>();
    private Transform enemyParent;
    private int enemyThreshold;
    [SerializeField] private List<Character> characterTypes;
    [SerializeField] private List<Transform> spawnPositions = new List<Transform>();
    [SerializeField] private Vector2 spawnArea = new Vector2(10, 10);

    public void SpawnNewWave()
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
            Character character = Instantiate(characterType, GetRandomPosition(), characterType.transform.rotation, enemyParent);
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
    public void RemoveEnemy(Character character)
    {
        enemies.Remove(character);
        CheckNewWave();
    }
    private void CheckNewWave()
    {
        if (enemies.Count <= enemyThreshold)
        {
            SpawnNewWave();
        }
    }
}
