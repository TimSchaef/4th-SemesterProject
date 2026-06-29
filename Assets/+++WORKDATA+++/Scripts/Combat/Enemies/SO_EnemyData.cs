using UnityEngine;

[CreateAssetMenu(fileName = "SO_EnemyData", menuName = "Scriptable Objects/SO_EnemyData")]
public class SO_EnemyData : ScriptableObject
{
    [Header("Enemy Information")] 
    public GameObject enemyPrefab;

    [Header("Wave Settings")]
    public int spawnCost = 1;

    [Header("Pool Settings")]
    public int poolSize = 100;
}
