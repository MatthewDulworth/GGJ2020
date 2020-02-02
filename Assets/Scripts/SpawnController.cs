using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public GameObject[] spawnPoints;
    //0 to 1. 0 being no chance to spawn. 1 being they will always spawn
    public float startSpawnRate;
    public float timeBtwnWaves;
    public float timeBetweenSpawns;
    private float timer;
    private int currentWave;
    private int maxEnemiesAllowed = 999;

    public GameObject[] enemies;
    private List<GameObject> spawnableEnemies = new List<GameObject>();

    private float elapsedTime;
    // Start is called before the first frame update
    void Start()
    {
        timer = timeBetweenSpawns;
        spawnableEnemies.Add(enemies[0]);
        Time.timeScale = 0;
    }

    
    // Update is called once per frame
    void FixedUpdate()
    {
        timer -= Time.deltaTime;

        if (TrySpawnEnemies())
        {
            elapsedTime += Time.deltaTime;
        }
        //Wave cleared!!!
        if(elapsedTime / timeBtwnWaves >= 1)
        {
            print("Wave cleared!!!");
            elapsedTime -= timeBtwnWaves;
            UpdateWave();
        }
    }

    //Will increment wave and set all relevent variables to their desired values
    private void UpdateWave()
    {
        currentWave++;
        var normalIncrementRate = .1f;
        switch (currentWave)
        {
            case 1:
                break;
            case 2:
                startSpawnRate += normalIncrementRate;
                break;
            case 3:
                startSpawnRate += normalIncrementRate;
                break;
            default:
                break;
        }
    }

    private bool TrySpawnEnemies()
    {
        int curNumEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if(curNumEnemies >= maxEnemiesAllowed || timer >= 0)
        {
            return false;
        }
        timer = timeBetweenSpawns;
        bool enemySpawns = UnityEngine.Random.Range(0.0f, 1.0f) < startSpawnRate;
        if (!enemySpawns)
        {
            return true;
        }
        int spawnIndex = UnityEngine.Random.Range(0, spawnPoints.Length);
        GameObject enemySpawned = spawnableEnemies[UnityEngine.Random.Range(0, spawnableEnemies.Count)];
        Instantiate(enemySpawned, spawnPoints[spawnIndex].transform.position, spawnPoints[spawnIndex].transform.rotation);
        print("Spawned guy");
        elapsedTime += 1;
        return true;
    }
}
