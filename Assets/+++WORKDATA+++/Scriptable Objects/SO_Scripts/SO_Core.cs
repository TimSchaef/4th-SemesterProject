using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "SO_Core", menuName = "Scriptable Objects/SO_Core")]
public class SO_Core : ScriptableObject
{
    public float maxHealth;
    private float currentHealth; 
    //public Image healthBar;
}
