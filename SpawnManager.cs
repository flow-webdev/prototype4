using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject[] powerupPrefab;
    public GameObject bossPrefab;
    private float spawnRange = 9f;
    public int enemyCount;
    public int waveNumber = 1;
    public int bossWave = 5;
    
    void Update() {
        
        // If there are no enemies, creates more
        enemyCount = FindObjectsOfType<Enemy>().Length;

        if (enemyCount == 0 && waveNumber % bossWave != 0) {
            SpawnEnemyWave(waveNumber);
            SpawnPowerup();
            waveNumber++;
        } else if (enemyCount == 0 && waveNumber % bossWave == 0) {
            SpawnBoss();
            SpawnPowerup();
            waveNumber++;
        }
    }

    void SpawnEnemyWave(int enemiesToSpawn) {

        // Instantiate new enemies
        for (int i = 0; i < enemiesToSpawn; i++) {
            Instantiate(enemyPrefab, Vector3SpawnPosition(), enemyPrefab.transform.rotation);
        }
    }
    void SpawnPowerup() {
        int index = Random.Range(0, powerupPrefab.Length);
        Instantiate(powerupPrefab[index], Vector3SpawnPosition(), powerupPrefab[index].transform.rotation);
    }

    void SpawnBoss() {
        Instantiate(bossPrefab, Vector3SpawnPosition(), bossPrefab.transform.rotation);
    }

    Vector3 Vector3SpawnPosition() {

        // Return a Vector3
        float spanwPosX = Random.Range(-spawnRange, spawnRange);
        float spanwPosZ = Random.Range(-spawnRange, spawnRange);

        Vector3 randomPos = new Vector3(spanwPosX, 0.1f, spanwPosZ);

        return randomPos;
    }
}
