using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 10f;
    //[SerializeField] private TextMeshProUGUI playerHP;
    [SerializeField] public int xpReward = 3;
    [SerializeField] private float regenRate = 0f;
    [SerializeField] private float regenInterval = 1f;
    
    [Header("Damage Feedback")]
    [SerializeField] private Renderer renderer;
    [SerializeField] private float flashDuration = 0.5f;
    [SerializeField] private Transform popupPosition;

    

    [SerializeField] private float currentHealth;
    
    private Color originalColor;
    private Material hitMaterial;
    private float regenTimer;
    private EnemyBase enemy;

    public bool IsDead => currentHealth <= 0;

    private void Awake()
    {
        enemy = GetComponent<EnemyBase>();
        renderer = GetComponentInChildren<Renderer>();
        currentHealth = maxHealth;
        hitMaterial = renderer.material;
        originalColor = hitMaterial.color;
    }
    
    
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        regenTimer = 0;

        hitMaterial.DOKill();
        hitMaterial.color = originalColor;
    }

    private void Start()
    {
        UpdateHP();
    }

    void Update()
    {
        HandleRegen();
    }
    public void TakeDamage(float damage)
    {
        if (IsDead)
            return;

        currentHealth -= damage;

        if (gameObject.CompareTag("Player"))
        {
            UpdateHP();
        }

        if (gameObject.CompareTag("Enemy"))
        {
            hitMaterial.DOKill();
            hitMaterial.color = Color.red;
            
            hitMaterial.DOColor(originalColor, flashDuration);
            DamageManager.Instance.Show(damage, popupPosition.position);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHP()
    {
        // if (playerHP != null)
        // {
        //     playerHP.text = "HP: " + currentHealth.ToString();
        // }
    }
    public void IncreaseMaxHP(float amount)
    {
        maxHealth += amount;
        currentHealth += amount;
    }
    private void HandleRegen()
    {
        if (IsDead)
            return;

        if (currentHealth >= maxHealth)
            return;

        regenTimer += Time.deltaTime;

        if (regenTimer >= regenInterval)
        {
            regenTimer = 0f;

            currentHealth = Mathf.Min(
                currentHealth + regenRate,
                maxHealth
            );

            UpdateHP();
        }
    }
    
    public void IncreaseRegen(float amount)
    {
        regenRate += amount;
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }
    
    
    void Die()
    {
        PlayerXP.Instance.AddXP(xpReward);
        if (enemy != null)
        {
            enemy.Die();
        }
        else
        {
            Destroy(gameObject); //TODO: Add Death condition
        }
    }
}