using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    public UpgradeManager manager;
    public GameObject panel;
    public Button[] buttons;
    public TextMeshProUGUI[] buttonTexts;

    public void ShowUpgrades()
    {
        //SO_WeaponUpgrades[] upgrades = manager.GetRandomUpgrades(3);

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(true);
            //buttonTexts[i].text = upgrades[i].upgradeName + "\n" + upgrades[i].upgradeDescription;

            int index = i; // closure for OnClick
            buttons[i].onClick.RemoveAllListeners(); // remove old listeners
            buttons[i].onClick.AddListener(() =>
            {
                manager.PickUpgrade(index);
                Hide();
            });
        }

        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}