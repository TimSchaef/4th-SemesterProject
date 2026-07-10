using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;
using Image = UnityEngine.UI.Image;

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
    [SerializeField] private Color damageColor;
    [SerializeField] private float flashDuration = 0.5f;
    [SerializeField] private Transform popupPosition;
    
    private Color[] originalColors;
    private float[] originalAlphas;
    private Material[] hitMaterials;

    private float regenTimer;
    private EnemyBase enemy;

    public bool IsDead => currentHealth <= 0;

    private void Awake()
    {
        enemy = GetComponent<EnemyBase>();
        currentHealth = maxHealth;

        hitMaterials = renderer.materials;
        originalColors = new Color[hitMaterials.Length];
        originalAlphas = new float[hitMaterials.Length];

        for (int i = 0; i < hitMaterials.Length; i++)
        {
            originalColors[i] = hitMaterials[i].color;
            originalAlphas[i] = hitMaterials[i].color.a;
        }
    }

    private void Update()
    {
        HandleRegen();
    }
    
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        regenTimer = 0;

        for (int i = 0; i < hitMaterials.Length; i++)
        {
            hitMaterials[i].DOKill();

            Color color = originalColors[i];
            color.a = originalAlphas[i];

            hitMaterials[i].color = color;
        }
    }

    public void TakeDamage(float damage, bool isCritical)
    {
        if (IsDead)
            return;

        currentHealth -= damage;

        if (gameObject.CompareTag("Player"))
        {
            LoseHealth();
            PlayerJuice.Instance.GetDamage();
            
            if (currentHealth <= 0)
            {
                UIManager.Instance.ShowEndPanel();
                Cursor.visible =  true;
                Cursor.lockState =  CursorLockMode.None;
            }
        }

        if (gameObject.CompareTag("Enemy") || gameObject.CompareTag("Enemy Crit"))
        {
            FlashDamage();
            DamageManager.Instance.Show(damage, popupPosition.position, isCritical);
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    private void FlashDamage()
    {
        for (int i = 0; i < hitMaterials.Length; i++)
        {
            hitMaterials[i].DOKill();

            Color flash = damageColor;
            flash.a = originalAlphas[i];

            hitMaterials[i].color = flash;

            hitMaterials[i]
                .DOColor(originalColors[i], flashDuration);
        }
    }

    private void LoseHealth()
    { 
        if (playerHP != null)
        {
            playerHP.DOFillAmount(currentHealth / maxHealth, flashDuration);

            DOTween.Kill("Health");

            playerHP.color = Color.white;

            playerHP.DOColor(Color.red, flashDuration).SetId("Health").SetLoops(2, LoopType.Yoyo);
        }
    }

    private void GainHealth()
    {
        if (playerHP != null)
            playerHP.DOFillAmount(currentHealth / maxHealth, 0.15f);
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

            currentHealth = Mathf.Min(currentHealth + regenRate, maxHealth);
            GainHealth();
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

        xp.GetComponent<XPPickup>()
            .Initialize(experiencePoints);
    }

    private void FadeDeath()
    {
        float fadeDuration = 0.5f;

        foreach (Material material in hitMaterials)
        {
            material.DOKill();
            material.DOFade(0, fadeDuration);
        }
    }
    
    private void Die()
    {
        DropXP();
        if (enemy != null)
            enemy.enabled = false;
        
        enemy.Die();
        FadeDeath();
    }
}

