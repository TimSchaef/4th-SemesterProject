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

    [SerializeField] public TextMeshProUGUI objectiveEnemyText;
    [SerializeField] private Image objectiveImage;
    [SerializeField] public GameObject bossBarParent;
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
        objectiveEnemyText.DOFade(value, 0.5f);
    }
}
