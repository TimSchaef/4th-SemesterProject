using UnityEngine;

[CreateAssetMenu(fileName = "SO_ObjectiveWave", menuName = "Scriptable Objects/SO_ObjectiveWave")]
public class SO_ObjectiveWave : ScriptableObject
{
    public SO_EnemyData[] enemies;
    public int enemyCount;
    public int rewardUpgradesCount;
    public GameObject[] doorsToClose;
}
