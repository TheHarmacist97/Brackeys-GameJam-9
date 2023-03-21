using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    [SerializeField] GameObject particles;

    private void Awake()
    {
        GameManager.Instance.CharacterDeathEvent += SpawnParticle;
    }

    private void SpawnParticle(Vector3 spawnPos)
    {
        Instantiate(particles, spawnPos, Quaternion.identity);  
    }
}
