using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;
    
    [Header("References")]
    [SerializeField] public Weapon playerWeapon;
    [SerializeField] public GameObject upgradePanel;
    [SerializeField] public PlayerXP playerXP;
    [SerializeField] private Button[] upgradeButtons;

    [Header("All Possible Base Upgrades")]
    [SerializeField] public List<WeaponUpgradeSO> allUpgrades;
    
    [Header("Upgrade Info")]
    [SerializeField] private TextMeshProUGUI[] upgradeName;
    [SerializeField] private Sprite[] upgradeIcons;

    private WeaponUpgradeSO[] currentChoices = new WeaponUpgradeSO[3];

    public bool screenOpen;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void OpenUpgradeScreen()
    {
        if (screenOpen)
            return;

        screenOpen = true;

        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        upgradePanel.SetActive(true);

        GenerateChoices();
    }

    void GenerateChoices()
{
    List<WeaponUpgradeSO> possibleChoices =
        new List<WeaponUpgradeSO>();

    foreach (var upgrade in allUpgrades)
    {
        if (upgrade.tier != 1)
            continue;

        if (!playerWeapon.HasUpgrade(upgrade.upgradeID))
        {
            possibleChoices.Add(upgrade);
        }
        else
        {
            WeaponUpgradeSO currentUpgrade =
                playerWeapon.GetUpgrade(upgrade.upgradeID);

            if (currentUpgrade.nextTier != null)
            {
                possibleChoices.Add(currentUpgrade.nextTier);
            }
        }
    }
    if (possibleChoices.Count == 0)
    {
        Debug.Log("No upgrades left. Granting damage.");

        playerWeapon.damage  += 1f; // or use a method

        playerXP.pendingLevelUps--;

        if (playerXP.pendingLevelUps > 0)
        {
            GenerateChoices();
        }
        else
        {
            CloseUpgradeScreen();
        }

        return;
    }
    if (possibleChoices.Count == 1)
    {
        playerWeapon.AddUpgrade(possibleChoices[0]);

        playerXP.pendingLevelUps--;

        if (playerXP.pendingLevelUps > 0)
        {
            GenerateChoices();
        }
        else
        {
            CloseUpgradeScreen();
        }

        return;
    }
    
    for (int i = 0; i < currentChoices.Length; i++)
    {
        currentChoices[i] = null;
    }

    for (int i = 0; i < currentChoices.Length; i++)
    {
        if (possibleChoices.Count <= 0)
            break;

        int randomIndex =
            Random.Range(0, possibleChoices.Count);

        currentChoices[i] =
            possibleChoices[randomIndex];

        possibleChoices.RemoveAt(randomIndex);
    }
    
    for (int i = 0; i < currentChoices.Length; i++)
    {
        bool hasChoice = currentChoices[i] != null;

        upgradeButtons[i].gameObject.SetActive(hasChoice);

        if (hasChoice)
        {
            upgradeName[i].text =
                currentChoices[i].upgradeName +
                "\nTier " +
                currentChoices[i].tier;
        }
    }
}

    public void PickUpgrade(int index)
    {
        if (currentChoices[index] == null)
            return;

        playerWeapon.AddUpgrade(currentChoices[index]);

        playerXP.pendingLevelUps--;

        if (playerXP.pendingLevelUps > 0)
        {
            GenerateChoices();
        }
        else
        {
            CloseUpgradeScreen();
        }
    }

    private void CloseUpgradeScreen()
    {
        
        screenOpen = false;

        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        upgradePanel.SetActive(false);
    }
}