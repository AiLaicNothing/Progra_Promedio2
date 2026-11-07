using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    [Header("Enemies")]
    [SerializeField] private List<GameObject> enemyPrefabs = new();

    [Header("Spawn Points")]
    [SerializeField] private List<Transform> spawnPoints = new();

    [Header("Spawn Settings")]
    [SerializeField] private float minSpawnTime = 5f;
    [SerializeField] private float maxSpawnTime = 10f;
    [SerializeField] private int maxEnemiesToSpawn = 10;

    private int spawnedEnemies;

    private void Start()
    {
        if (enemyPrefabs.Count == 0 || spawnPoints.Count == 0) return;

        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        float minTime = Mathf.Max(0.1f, Mathf.Min(minSpawnTime, maxSpawnTime));
        float maxTime = Mathf.Max(minTime, maxSpawnTime);

        while (maxEnemiesToSpawn <= 0 || spawnedEnemies < maxEnemiesToSpawn)
        {
            float waitTime = Random.Range(minTime, maxTime);

            yield return new WaitForSeconds(waitTime);

            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        List<GameObject> validEnemies = enemyPrefabs.FindAll(enemy => enemy != null);

        List<Transform> validPoints = spawnPoints.FindAll(point => point != null);

        if (validEnemies.Count == 0 || validPoints.Count == 0)
        {
            Debug.LogWarning("Enemy spawner has no valid prefabs or points.");
            enabled = false;
            return;
        }

        GameObject selectedEnemy = validEnemies[Random.Range(0, validEnemies.Count)];

        Transform selectedPoint = validPoints[Random.Range(0, validPoints.Count)];

        Instantiate(selectedEnemy, selectedPoint.position, selectedPoint.rotation);

        spawnedEnemies++;
    }
}