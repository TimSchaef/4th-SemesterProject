using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerXP : MonoBehaviour
{
    public static PlayerXP Instance;
    [Header("XP")]
    [SerializeField] public float currentXP;
    [SerializeField] public int level = 1;
    [SerializeField] public float xpToNextLevel = 5;
    [SerializeField] public int pendingLevelUps;
    [SerializeField] public float xpMultiplier = 1f;
    
    [Header("Game Settings")]
    [SerializeField] private float xpIncrease = 1.2f;
    [SerializeField] private float levelUpDelay = 1f;

    [Space] 
    [SerializeField] private Image currentXPImage;
    [SerializeField] private UpgradeManager upgradeManager;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        UpgradeManager.Instance.levelText.text = level.ToString();
    }


    public void AddXP(int amount)
    {
        currentXP += (amount * xpMultiplier);
        currentXPImage.DOFillAmount(currentXP / xpToNextLevel, 0.2f);

        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            level++;
            UpgradeManager.Instance.levelText.text = level.ToString();
            
            xpToNextLevel *= xpIncrease;

            pendingLevelUps++;
        }

        StartCoroutine(LevelUpDelay());
    }

    public void CheckLevelUp()
    {
        
        if (pendingLevelUps > 0)
        {
            Time.timeScale = 0;
            upgradeManager.OpenUpgradeScreen();
        }
    }

    private IEnumerator LevelUpDelay()
    {
        yield return new WaitForSeconds(levelUpDelay);
        CheckLevelUp();
    }
}
