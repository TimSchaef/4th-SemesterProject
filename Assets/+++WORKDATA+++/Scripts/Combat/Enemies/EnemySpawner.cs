using System.Collections;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    [Header("Wave Setup")]
    [SerializeField] private WaveStage[] stages;


    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;


    [Header("Limits")]
    [SerializeField] private int maxEnemies = 300;


    [Header("Timing")]
    [SerializeField] private float startSpawnDelay = 5f;

    public bool canSpawn = true;
    private int aliveEnemyCount;
    private float gameTime;
    private float spawnBudget;



    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }
    
    private void Update()
    {
        gameTime += Time.deltaTime;
    }
    
    private IEnumerator SpawnLoop()
    {
        yield return new WaitForSeconds(startSpawnDelay);

        while(true)
        {
            SpawnEnemies();
            yield return new WaitForSeconds(1f);
        }
    }
    private void SpawnEnemies()
    {
        if(aliveEnemyCount >= maxEnemies)
            return;
        
        WaveStage currentStage = GetCurrentStage();
        
        if(currentStage == null)
            return;

        // Add spawn power over time
        spawnBudget += currentStage.budgetPerSecond;

        while(spawnBudget >= 1)
        {
            SO_EnemyData enemy =
                PickEnemy(currentStage);


            if(enemy == null)
                return;
            
            SpawnEnemy(enemy);
            
            spawnBudget -= enemy.spawnCost;
        }
        Debug.Log("Alive: " + aliveEnemyCount + " Budget: " + spawnBudget);
    }

    private WaveStage GetCurrentStage()
    {
        WaveStage current = null;


        foreach(WaveStage stage in stages)
        {
            if(gameTime >= stage.startTime)
            {
                current = stage;
            }
        }
        return current;
    }
    
    private SO_EnemyData PickEnemy(WaveStage stage)
    {
        if(stage.enemies.Length == 0)
            return null;
        
        return stage.enemies[Random.Range(0, stage.enemies.Length)];
    }
    private void SpawnEnemy(SO_EnemyData enemyData)
    {
        if (spawnPoints.Length == 0)
            return;
        
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];


        GameObject enemy = EnemyPoolManager.Instance.GetEnemy(enemyData);
        if (enemy == null)
            return;


        // Put enemy at spawn position while inactive
        enemy.transform.position =
            spawnPoint.position;

        enemy.transform.rotation =
            Quaternion.identity;


        // Now wake it up
        enemy.SetActive(true);
        aliveEnemyCount++;

        EnemyBase enemyBase =
            enemy.GetComponent<EnemyBase>();


        if (enemyBase != null)
        {
            enemyBase.Initialize(this);
        }
    }
    
    public void OnEnemyKilled()
    {
        aliveEnemyCount--;
    }
}