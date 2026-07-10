using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    [SerializeField] private Image[] upgradeIcons;
    [SerializeField] private TextMeshProUGUI[] upgradeDescription;
    [SerializeField] public TextMeshProUGUI levelText;

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

        playerWeapon.canShoot = false;
        screenOpen = true;

        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        upgradePanel.SetActive(true);
        upgradePanel.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetUpdate(true); 

        GenerateChoices();
    }

    private void GenerateChoices()
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
        playerWeapon.damage  *= 1.1f; 

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
        StartCoroutine(ChangeButtonInteractable(true));

        if (hasChoice)
        {
            upgradeName[i].text = currentChoices[i].upgradeName;
            upgradeIcons[i].sprite = currentChoices[i].upgradeIcon;
            upgradeDescription[i].text = currentChoices[i].upgradeDescription;
        }
    }
}

    private IEnumerator ChangeButtonInteractable(bool state)
    {
        yield return new WaitForSeconds(0.25f);
        foreach (var button in upgradeButtons)
        {
            button.interactable = state;
        }
    }

    public void PickUpgrade(int index)
    {
        if (currentChoices[index] == null)
            return;

        playerWeapon.AddUpgrade(currentChoices[index]);
        Debug.Log($"Picked {currentChoices[index].upgradeName}");

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
        foreach (var button in upgradeButtons)
        {
            button.interactable = false;
        }
        StartCoroutine(CloseUpgradeScreenRoutine());
        
        screenOpen = false;

        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private IEnumerator CloseUpgradeScreenRoutine()
    {
        upgradePanel.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f).SetUpdate(true); 
        upgradePanel.SetActive(false);
        playerWeapon.canShoot = true;
        yield return new WaitForSeconds(0.5f);
    }
}