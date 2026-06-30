using System.Net.Mime;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Health Stats")]
    [SerializeField] public float maxHealth = 10f;
    [SerializeField] public float currentHealth;
    [SerializeField] private Image playerHP;
    [SerializeField] private float regenRate = 0f;
    [SerializeField] private float regenInterval = 1f;
    
    [Header("XP Refs")]
    [SerializeField] private SO_ExperiencePoints experiencePoints;
    [SerializeField] private GameObject xpPickupPrefab;
    [SerializeField] private Transform dropPosition;
    
    [Header("Damage Feedback")]
    [SerializeField] private Renderer renderer;
    [SerializeField] private float flashDuration = 0.5f;
    [SerializeField] private Transform popupPosition;
    
    private Color originalColor;
    private Material hitMaterial;
    private float regenTimer;
    private EnemyBase enemy;

    public bool IsDead => currentHealth <= 0;

    private void Awake()
    {
        enemy = GetComponent<EnemyBase>();
        currentHealth = maxHealth;
        hitMaterial = renderer.material;
        originalColor = hitMaterial.color;
    }
    
    private void Start()
    {
        
    }
    
    void Update()
    {
        HandleRegen();
    }
    
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        regenTimer = 0;

        hitMaterial.DOKill();
        hitMaterial.color = originalColor;
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
        if (playerHP != null)
        {
            playerHP.DOFillAmount(currentHealth / maxHealth, flashDuration);
            DOTween.Kill("Health");
            playerHP.DOColor(Color.red, flashDuration).SetId("Health").SetLoops(2, LoopType.Yoyo);
        }
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

            //UpdateHP();
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

    private void DropXP()
    {
        if (experiencePoints == null)
            return;
        
        GameObject xp = Instantiate(xpPickupPrefab, dropPosition.position, Quaternion.identity);
        xp.GetComponent<XPPickup>().Initialize(experiencePoints);
    }
    
    void Die()
    {
        if (enemy != null)
        {
            DropXP();
            enemy.Die();
        }
        else
        {
            Destroy(gameObject); //TODO: Ad Death condition
        }
    }
}