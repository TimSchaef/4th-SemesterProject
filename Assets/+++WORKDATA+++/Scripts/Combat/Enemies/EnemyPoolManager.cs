using System.Collections.Generic;
using UnityEngine;


public class EnemyPoolManager : MonoBehaviour
{
    public static EnemyPoolManager Instance;


    [SerializeField] private SO_EnemyData[] enemies;


    private Dictionary<SO_EnemyData, Queue<GameObject>> pools = new();



    private void Awake()
    {
        if (Instance == null) 
            Instance = this;

        CreatePools();
    }



    private void CreatePools()
    {
        foreach(SO_EnemyData enemyData in enemies)
        {
            Queue<GameObject> queue = new();


            for(int i = 0; i < enemyData.poolSize; i++)
            {
                GameObject enemy =
                    Instantiate(enemyData.enemyPrefab);


                enemy.SetActive(false);


                queue.Enqueue(enemy);
            }


            pools.Add(enemyData, queue);
        }
    }



    public GameObject GetEnemy(SO_EnemyData data)
    {
        if(!pools.ContainsKey(data))
        {
            Debug.LogError(
                "No pool for " + data.name
            );

            return null;
        }


        Queue<GameObject> pool =
            pools[data];


        GameObject enemy;


        if(pool.Count > 0)
        {
            enemy = pool.Dequeue();
        }
        else
        {
            // fallback if you underestimated pool size
            enemy = Instantiate(data.enemyPrefab);
        }
        return enemy;
    }



    public void ReturnEnemy(SO_EnemyData data, GameObject enemy)
    {
        enemy.SetActive(false);

        pools[data].Enqueue(enemy);
    }
}