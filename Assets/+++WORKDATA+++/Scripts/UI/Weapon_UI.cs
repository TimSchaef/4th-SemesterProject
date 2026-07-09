using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Weapon_UI : MonoBehaviour
{
    public static Weapon_UI  instance;
    [SerializeField] private Weapon weaponRef;
    [SerializeField] private TextMeshProUGUI ammoText;

    [Header("Kill Stats")] 
    [SerializeField] public TextMeshProUGUI killAmountText;
    [SerializeField] public TextMeshProUGUI waveStartedText;
    [SerializeField] public TextMeshProUGUI waveRewardText;
    [SerializeField] private Image objectiveImage;
    [SerializeField] public Image bossBarFrame;
    [SerializeField] public Image objectiveBossBar;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void UpdateAmmo()
    {
        ammoText.text = weaponRef.ammo.ToString();
    }

    public void StartWaveUI(float value)
    {
        print($"StartWaveUI: {value}");
        objectiveImage.DOFade(value, 0.5f);
        bossBarFrame.DOFade(value, 0.5f);
        objectiveBossBar.DOFade(value, 0.5f);
        waveStartedText.DOFade(value, 0.5f);
        waveRewardText.DOFade(value, 0.5f);
    }
}
