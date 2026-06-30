using UnityEngine;

[CreateAssetMenu(fileName = "SO_ExperiencePoints", menuName = "Scriptable Objects/SO_ExperiencePoints")]
public class SO_ExperiencePoints : ScriptableObject
{
    [Header("Material Settings")]
    public Material xpMaterial;
    
    [Header("XP Settings")]
    public float xpAmount;
    
    [Header("Pickup Settings")]
    public float collectRadius; 
}
