using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnController : MonoBehaviour
{
    public GameObject[] spawnPoints;
    //0 to 1. 0 being no chance to spawn. 1 being they will always spawn
    public float startSpawnRate;
    public float timeBtwnWaves;
    public float timeBetweenSpawns;
    private float timer;
    public int currentWave = 1;
    private int maxEnemiesAllowed = 1;

    public GameObject[] enemies;
    private List<GameObject> spawnableEnemies = new List<GameObject>();
    public GameObject waveClearText;

    public bool gameover;
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
            StartCoroutine(WaveClearText());
            elapsedTime -= timeBtwnWaves;
            UpdateWave();
        }
    }
    IEnumerator WaveClearText()
    {

        waveClearText.SetActive(true);
        waveClearText.GetComponent<RectTransform>().anchoredPosition = new Vector2(-1500, 0);
        var dest = GameObject.Find("Center");
        waveClearText.GetComponent<MoveTowards>().dest = dest.transform;
        yield return new WaitForSeconds(.1f);
        GameObject.FindObjectOfType<EffectsController>().ScreenFlash(Vector2.zero);
        yield return new WaitForSeconds(.8f);
        waveClearText.GetComponent<MoveTowards>().dest = GameObject.Find("Wave Clear dest 2").transform;

    }
    //Will increment wave and set all relevent variables to their desired values
    private void UpdateWave()
    {
        currentWave++;
        Text waveText = GameObject.Find("Wave Text").GetComponent<Text>();
        waveText.text = "Wave " + currentWave;
        waveText.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -1000);

        var normalIncrementRate = .05f;
        switch (currentWave)
        {
            case 1:
                break;
            case 2:
                startSpawnRate += normalIncrementRate;
                maxEnemiesAllowed++;
                timeBetweenSpawns /= 1.2f;
                break;
            case 3:
                startSpawnRate += normalIncrementRate;
                timeBtwnWaves++;
                spawnableEnemies.Add(enemies[1]);
                break;
            case 4:
                maxEnemiesAllowed++;
                break;
            case 5:
                startSpawnRate += normalIncrementRate;
                timeBetweenSpawns /= 1.2f;
                timeBtwnWaves++;
                break;
            case 8:
                startSpawnRate += normalIncrementRate;
                timeBetweenSpawns /= 1.2f;
                maxEnemiesAllowed++;
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
