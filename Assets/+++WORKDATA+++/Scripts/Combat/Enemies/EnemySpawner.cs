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
    private bool objectiveActive;
    private SO_ObjectiveWave currentObjective;
    private int objectiveRemaining;



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
        if (!canSpawn) return;
        
        if(aliveEnemyCount >= maxEnemies)
            return;
        
        WaveStage currentStage = GetCurrentStage();
        
        if(currentStage == null)
            return;
        
        spawnBudget += currentStage.budgetPerSecond;

        while(spawnBudget >= 1)
        {
            SO_EnemyData enemy = PickEnemy(currentStage);
            
            if(enemy == null)
                return;
            
            SpawnEnemy(enemy);
            
            spawnBudget -= enemy.spawnCost;
        }

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
        
        enemy.transform.position =
            spawnPoint.position;

        enemy.transform.rotation =
            Quaternion.identity;
        
        enemy.SetActive(true);
        aliveEnemyCount++;

        EnemyBase enemyBase =
            enemy.GetComponent<EnemyBase>();


        if (enemyBase != null)
        {
            enemyBase.Initialize(this);
        }
    }

    public void StartObjectiveWave(SO_ObjectiveWave wave)
    {
        if (objectiveActive)
            return;

        objectiveActive = true;
        currentObjective = wave;
        canSpawn = false;

        objectiveRemaining = wave.enemyCount;
        CloseDoors(wave);
        StartCoroutine(ActivateObjectiveEnemies());
    }

    private IENumerator ActivateObjectiveEnemies()
    {
        for (int i = 0; i < currentObjective.enemyCount; i++)
        {
            SO_EnemyData enemy = currentObjective.enemies[Random.Range(0, currentObjective.enemies.Length)];

            SpawnEnemy(enemy);

            yield return null;
        }
    }
    
    public void OnEnemyKilled()
    {
        aliveEnemyCount--;

        if (objectiveActive)
        {
            objectiveRemaining--;
            if (objectiveRemaining <= 0)
            {
                FinishObjective();
            }
        }
    }

    private void OpenDoors(SO_ObjectiveWave wave)
    {
        foreach (doorToClose door in wave)
        {
            gameObject.SetActive(true);
        }
    }
    
    private void CloseDoors(SO_ObjectiveWave wave)
    {
        foreach (doorToClose door in wave)
        {
            gameObject.SetActive(false);
        }
    }

    private void FinishObjective()
    {
        OpenDoors(currentObjective);

        objectiveActive = false;
        currentObjective = null;

        canSpawn = true;
    }
}