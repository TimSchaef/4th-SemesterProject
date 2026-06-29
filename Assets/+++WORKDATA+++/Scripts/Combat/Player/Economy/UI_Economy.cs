using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Economy : MonoBehaviour
{
    public static Action OnUpdateEconomy;
    
    [SerializeField] private Image economyIcon;
    [SerializeField] private TextMeshProUGUI economyText;
    
    [SerializeField] private PlayerEconomy economy;

    private void Start()
    {
        economyText.text = economy.currentPlayerEconomy.ToString();
    }

    private void UpdateEconomy()
    {
        economyText.text = economy.currentPlayerEconomy.ToString();
    }

    private void OnEnable()
    {
        OnUpdateEconomy += UpdateEconomy;
    }

    private void OnDisable()
    {
        OnUpdateEconomy -= UpdateEconomy;
    }
}
