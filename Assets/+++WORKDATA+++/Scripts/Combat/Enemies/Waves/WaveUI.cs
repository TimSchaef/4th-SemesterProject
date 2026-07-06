using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    public static WaveUI Instance;
    
    [SerializeField] private TextMeshProUGUI TMP_enemyCounter;
    [SerializeField] private Image Image_aliveEnemies;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    
    void Start()
    {
        UpdateEnemyAmount();
    }

    public void ChangeUIState(bool state)
    {
        TMP_enemyCounter.gameObject.SetActive(state);
        Image_aliveEnemies.gameObject.SetActive(state);
    }
    public void UpdateEnemyAmount()
    {
        TMP_enemyCounter.text = EnemySpawner.Instance.objectiveRemaining.ToString();
    }
}
