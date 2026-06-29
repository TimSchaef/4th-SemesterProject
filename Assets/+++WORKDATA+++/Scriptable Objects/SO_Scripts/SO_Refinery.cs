using UnityEngine;

[CreateAssetMenu(fileName = "SO_Refinery", menuName = "Scriptable Objects/SO_Refinery")]
public class SO_Refinery : ScriptableObject
{
    public int neededEconomy = 50;
    
    [Tooltip("Should be low numbers, get added to a multiplier, so 0.3 means multiplier is 1.3")]
    [Range(0.1f,5f)]
    public float repairedMultiplier;
}
