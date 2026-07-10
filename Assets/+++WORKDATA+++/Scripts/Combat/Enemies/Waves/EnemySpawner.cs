using System.Collections;
using DG.Tweening;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;
    
    [Header("Wave Setup")]
    [SerializeField] private WaveStage[] stages;
    [SerializeField] private SO_ObjectiveWave[] objectiveWaves;

    [Header("Spawn Points (World)")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Limits")]
    [SerializeField] private int maxEnemies = 300;

    [Header("Timing")]
    [SerializeField] private float startSpawnDelay = 5f;

    [Header("Sound")] 
    [SerializeField] private AudioClip activateSound;

    public bool canSpawn = true;
    public int objectiveRemaining;

    private int totalKills;
    private int aliveEnemyCount;
    private float gameTime;
    private float spawnBudget;
    
    private bool objectiveActive;
    private int currentWave;
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
        
        AudioManager.Instance.PlaySfx(activateSound);
        Weapon_UI.instance.objectiveBossBar.gameObject.SetActive(true);
        Weapon_UI.instance.StartWaveUI(1);
        objectiveActive = true;
        currentObjective = objective;
        if (currentWave >= objectiveWaves.Length)
        {
            return;
        }

        currentObjectiveData = objectiveWaves[currentWave];
        currentWave++;

        canSpawn = false;

        activeSpawnPoints = objective.SpawnPoints;

        objective.CloseDoors();

        objectiveRemaining = currentObjectiveData.enemyCount;
        Weapon_UI.instance.objectiveBossBar.DOFillAmount((float)objectiveRemaining / currentObjectiveData.enemyCount , 0.8f);
        Weapon_UI.instance.waveRewardText.text = "Upgrades: " + currentObjectiveData.rewards.ToString();   

        StartCoroutine(ActivateObjectiveEnemies());
    }

    private IEnumerator ActivateObjectiveEnemies()
    {
        for (int i = 0; i < currentObjectiveData.enemyCount; i++)
        {
            SO_EnemyData enemy = currentObjectiveData.enemies[Random.Range(0, currentObjectiveData.enemies.Length)];
            SpawnEnemy(enemy);

            if (i % 5 == 0)
                yield return null;
        }
    }

    public void OnEnemyKilled()
    {
        totalKills++;
        aliveEnemyCount--;
        Weapon_UI.instance.killAmountText.text = totalKills.ToString();

        if (!objectiveActive)
            return;

        objectiveRemaining--;
        
        float fill = (float)objectiveRemaining / currentObjectiveData.enemyCount;

        Weapon_UI.instance.objectiveBossBar.DOKill();
        Weapon_UI.instance.objectiveBossBar.DOFillAmount(fill, 0.2f);
        

        if (objectiveRemaining <= 0)
        {
            FinishObjective();
        }
    }

    private void FinishObjective()
    {
        currentObjective.OpenDoors();
        
        PlayerXP.Instance.pendingLevelUps += currentObjectiveData.rewards;
        StartCoroutine(PlayerXP.Instance.LevelUpDelay());

        activeSpawnPoints = spawnPoints;
        currentObjective.StartCooldown();

        objectiveActive = false;
        Weapon_UI.instance.StartWaveUI(0);

        canSpawn = true;
    }
}