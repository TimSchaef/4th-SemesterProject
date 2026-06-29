using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerEconomy : MonoBehaviour
{
    public static Action<float> OnGainEconomy;
    public static Action<float> OnLoseEconomy;
    [SerializeField] public float currentPlayerEconomy = 0;

    private void GainEconomy(float amount)
    {
        currentPlayerEconomy += amount;
        Debug.Log(currentPlayerEconomy);
    }

    private void LoseEconomy(float amount)
    {
        if (currentPlayerEconomy < amount) return;
        
        currentPlayerEconomy -= amount;
    }

    private void OnEnable()
    {
        OnGainEconomy += GainEconomy;
        OnLoseEconomy += LoseEconomy;
    }

    private void OnDisable()
    {
        OnGainEconomy -= GainEconomy;
        OnLoseEconomy -= LoseEconomy;
    }
}
