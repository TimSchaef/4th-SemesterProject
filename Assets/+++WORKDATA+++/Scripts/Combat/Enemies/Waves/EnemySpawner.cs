using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;
    
    [Header("Wave Setup")]
    [SerializeField] private WaveStage[] stages;

    [Header("Spawn Points (World)")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Limits")]
    [SerializeField] private int maxEnemies = 300;

    [Header("Timing")]
    [SerializeField] private float startSpawnDelay = 5f;

    public bool canSpawn = true;
    public int objectiveRemaining;

    private int totalKills;
    private int aliveEnemyCount;
    private float gameTime;
    private float spawnBudget;
    
    private bool objectiveActive;
    private InteractionObjective currentObjective;
    private SO_ObjectiveWave currentObjectiveData;

    private Transform[] activeSpawnPoints;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    
    private void Start()
    {
        activeSpawnPoints = spawnPoints;
        Weapon_UI.instance.killAmountText.text = totalKills.ToString(); 
        Debug.Log(totalKills);
        StartCoroutine(SpawnLoop());
    }

    private void Update()
    {
        gameTime += Time.deltaTime;
    }

    private IEnumerator SpawnLoop()
    {
        yield return new WaitForSeconds(startSpawnDelay);

        while (true)
        {
            SpawnEnemies();
            yield return new WaitForSeconds(1f);
        }
    }

    private void SpawnEnemies()
    {
        if (!canSpawn)
            return;

        if (aliveEnemyCount >= maxEnemies)
            return;

        WaveStage currentStage = GetCurrentStage();
        if (currentStage == null)
            return;

        spawnBudget += currentStage.budgetPerSecond;

        int spawnCap = 5;
        int spawned = 0;

        while (spawnBudget >= 1f && spawned < spawnCap)
        {
            SO_EnemyData enemy = PickEnemy(currentStage);
            if (enemy == null)
                return;

            SpawnEnemy(enemy);

            spawnBudget -= enemy.spawnCost;
            spawned++;
        }
    }

    private WaveStage GetCurrentStage()
    {
        WaveStage current = null;

        foreach (WaveStage stage in stages)
        {
            if (gameTime >= stage.startTime)
                current = stage;
        }

        return current;
    }

    private SO_EnemyData PickEnemy(WaveStage stage)
    {
        if (stage.enemies.Length == 0)
            return null;

        return stage.enemies[Random.Range(0, stage.enemies.Length)];
    }

    private void SpawnEnemy(SO_EnemyData enemyData)
    {
        if (activeSpawnPoints.Length == 0)
            return;

        Transform spawnPoint =
            activeSpawnPoints[
                Random.Range(0, activeSpawnPoints.Length)];

        GameObject enemy =
            EnemyPoolManager.Instance.GetEnemy(enemyData);

        if (enemy == null)
            return;

        enemy.transform.position = spawnPoint.position;
        enemy.transform.rotation = Quaternion.identity;

        enemy.SetActive(true);

        aliveEnemyCount++;

        EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();
        if (enemyBase != null)
            enemyBase.Initialize(this);
    }

    public void StartObjectiveWave(InteractionObjective objective)
    {
        print("started Wave");
        if (objectiveActive)
            return;

        Weapon_UI.instance.StartWaveUI(1);
        objectiveActive = true;
        currentObjective = objective;
        currentObjectiveData = objective.ObjectiveWave;

        canSpawn = false;

        activeSpawnPoints = objective.SpawnPoints;

        objective.CloseDoors();

        objectiveRemaining = currentObjectiveData.enemyCount;
        Weapon_UI.instance.objectiveEnemyText.text = objectiveRemaining.ToString();

        StartCoroutine(ActivateObjectiveEnemies());
    }

    private IEnumerator ActivateObjectiveEnemies()
    {
        for (int i = 0; i < currentObjectiveData.enemyCount; i++)
        {
            SO_EnemyData enemy =
                currentObjectiveData.enemies[
                    Random.Range(0, currentObjectiveData.enemies.Length)];

            SpawnEnemy(enemy);

            if (i % 5 == 0)
                yield return null;
        }
    }

    public void OnEnemyKilled()
    {
        aliveEnemyCount--;
        Weapon_UI.instance.killAmountText.text = totalKills.ToString();

        if (!objectiveActive)
            return;

        objectiveRemaining--;
        Weapon_UI.instance.objectiveEnemyText.text = objectiveRemaining.ToString();
        

        if (objectiveRemaining <= 0)
        {
            FinishObjective();
        }
    }

    private void FinishObjective()
    {
        currentObjective.OpenDoors();
        
        PlayerXP.Instance.pendingLevelUps += currentObjectiveData.rewards;
        PlayerXP.Instance.CheckLevelUp();

        activeSpawnPoints = spawnPoints;
        currentObjective.StartCooldown();

        objectiveActive = false;
        //TODO:: Set Next Wave as current objective
        // currentObjective = null;
        // currentObjectiveData = null;
        Weapon_UI.instance.StartWaveUI(0);

        canSpawn = true;
    }
}