using System;
using UnityEngine;

[Serializable]
public class WaveStage
{ 
        public float startTime;


        [Header("Spawn Pressure")]
        public float budgetPerSecond = 5f;


        [Header("Available Enemies")]
        public SO_EnemyData[] enemies;
}
