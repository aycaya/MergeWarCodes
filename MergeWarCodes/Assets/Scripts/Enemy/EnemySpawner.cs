using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] GameObject[] bossPrefabs;
    [SerializeField] Transform[] enemySpawnLocations;
    EnemyListOnField enemyListOnField;
    [SerializeField] Vector2 EnemySpawnTime = new Vector2(1f, 2f);
    [SerializeField] Vector2 BossSpawnTime = new Vector2(1f, 2f);
    bool isStarted = false;

    private void Awake()
    {
        enemyListOnField = FindObjectOfType<EnemyListOnField>();
    }

    private void Start()
    {
    }
    private void Update()
    {
        if (!GameManager.gameStarted || GameManager.gameFinished || Tutorial.isTutorialOn)
        {
            return;
        }
        else if (GameManager.gameStarted && !isStarted)
        {
            isStarted = true;
            SpawnWave();

        }
    }
    public void SpawnEnemy(int enemyIndex, bool spawnBoss = false)
    {
        int spawnLocationIndex = Random.Range(0, enemySpawnLocations.Length);
        GameObject spawnedEnemy;
        if (spawnBoss)
        {
            spawnedEnemy = Instantiate(bossPrefabs[enemyIndex], enemySpawnLocations[spawnLocationIndex].transform.position, Quaternion.identity);
        }
        else
        {
            spawnedEnemy = Instantiate(enemyPrefabs[enemyIndex], enemySpawnLocations[spawnLocationIndex].transform.position, Quaternion.identity);
        }
        EnemyHealth spawnedEnemyHealth = spawnedEnemy.GetComponent<EnemyHealth>();
        if (spawnedEnemyHealth != null)
        {
            enemyListOnField.AddNewEnemy(spawnedEnemyHealth);
        }

    }
    public void SpawnBoss()
    {
        SpawnEnemy(0, true);
    }

    public void SpawnWave()
    {
        StartCoroutine(WaveSpawner());
        StartCoroutine(BossSpawner());
    }

    IEnumerator WaveSpawner()
    {
        while (true)
        {

            if (!Tutorial.isTutorialOn)
            {
                float timeToBeWait = Random.Range(EnemySpawnTime.x, EnemySpawnTime.y);
                yield return new WaitForSeconds(timeToBeWait);

            }
            int enemyIndex = Random.Range(0, enemyPrefabs.Length);
            SpawnEnemy(enemyIndex);
        }
    }

    IEnumerator BossSpawner()
    {
        while (true)
        {
            float timeToBeWaitAfterBoss = Random.Range(BossSpawnTime.x, BossSpawnTime.y);
            yield return new WaitForSeconds(timeToBeWaitAfterBoss);
            int bossIndex = Random.Range(0, bossPrefabs.Length);
            SpawnEnemy(bossIndex, true);
        }
    }
}
