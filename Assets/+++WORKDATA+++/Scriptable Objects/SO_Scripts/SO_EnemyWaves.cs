using UnityEngine;

[CreateAssetMenu(fileName = "SO_EnemyWaves", menuName = "Scriptable Objects/SO_EnemyWaves")]
public class SO_EnemyWaves : ScriptableObject
{
    [SerializeField] private EnemyBase[] _enemies;
    
}
