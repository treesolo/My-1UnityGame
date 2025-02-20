using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject powerupPrefab;
    public GameObject autoCanonPrefab;
    public GameObject enemyBoss;
    private float spawnRange = 9.0f;
    public int enemyCount;
    private string powerupTag = "Powerup";
    private string autocanonTag = "Autocanon";
    public int powerupCount;
    public int autoCanonCount;
    public int waveNumber = 1;
    private PlayerController playerCont;


    // Start is called before the first frame update
    void Start()
    {
        playerCont = GameObject.Find("Player").GetComponent<PlayerController>();
        SpawnEnemyWave(waveNumber);
        Instantiate(autoCanonPrefab, GenerateSpawnPosition(), autoCanonPrefab.transform.rotation);

        Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);    
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length + FindObjectsByType<EnemyHard>(FindObjectsSortMode.None).Length + FindObjectsByType<EnemyBoss>(FindObjectsSortMode.None).Length;
        GameObject[] neededTag = GameObject.FindGameObjectsWithTag(powerupTag);
        powerupCount = neededTag.Length;
        GameObject[] needededTag = GameObject.FindGameObjectsWithTag(autocanonTag);
        autoCanonCount = needededTag.Length;
        if (enemyCount == 0 && !playerCont.gameOver)
        {
            waveNumber++;
            SpawnEnemyWave(waveNumber);
            if (powerupCount < 5)
            {
                Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);
            }
            if (autoCanonCount < 2)
            {
                Instantiate(autoCanonPrefab, GenerateSpawnPosition(), autoCanonPrefab.transform.rotation);
            }
        }
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            GameObject enemySpawning;
            if (waveNumber >= 10)
            {
                enemySpawning = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            } else if (waveNumber >= 5)
            {
                enemySpawning = enemyPrefabs[Random.Range(0, 2)];
            } else { enemySpawning = enemyPrefabs[1]; }
            if (waveNumber % 5 == 0)
            {
                enemiesToSpawn = waveNumber / 5;
                enemySpawning = enemyBoss;
            }
            Instantiate(enemySpawning, GenerateSpawnPosition(), Quaternion.identity);
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);

        Vector3 randomPos = new Vector3(spawnPosX, 1, spawnPosZ);

        return randomPos;
    }
}
