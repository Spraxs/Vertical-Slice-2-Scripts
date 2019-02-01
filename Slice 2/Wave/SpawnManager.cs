using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    [SerializeField]
    private float minimalEnemySpawnDelay;
    [SerializeField]
    private float maximumEnemySpawnDelay;

    private EnemySpawn[] enemySpawns;

    public List<GameObject> enemies;

    public int spawnedEnemies;

    private float enemySpawnDelayInSeconds;
    
    public List<GameObject> remainingEnemies;

    private bool canSpawnEnemy;

    private WaveManager waveManager;

    // Use this for initialization
    void Start () {

        enemySpawns = FindObjectsOfType<EnemySpawn>();
        canSpawnEnemy = true;

        waveManager = GetComponent<WaveManager>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!canSpawnEnemy) return;
        enemySpawnDelayInSeconds = UnityEngine.Random.Range(minimalEnemySpawnDelay, maximumEnemySpawnDelay);

        SpawnEnemy();

        StartCoroutine(SpawnCooldown());
	}

    private void SpawnEnemy()
    {
        if (enemySpawns.Length == 0 || enemies.Count == 0 || remainingEnemies.Count == 0) return;

        int index = UnityEngine.Random.Range(0, remainingEnemies.Count -1);

        GameObject enemyObject = remainingEnemies[index];

        int i = UnityEngine.Random.Range(0, enemySpawns.Length);

        EnemySpawn enemySpawn = enemySpawns[i];

        enemySpawn.Spawn(enemyObject);

        remainingEnemies.Remove(enemyObject);

    }

    public void EnemyDeath()
    {
        spawnedEnemies -= 1;

        if (spawnedEnemies < 0)
        {
            spawnedEnemies = 0;
        }

        if (spawnedEnemies == 0)
        {
            int wave = waveManager.wave;
            waveManager.SetWave(wave + 1);
        }
    }

    private IEnumerator SpawnCooldown()
    {
        canSpawnEnemy = false;
        yield return new WaitForSeconds(enemySpawnDelayInSeconds);
        canSpawnEnemy = true;
    }
}