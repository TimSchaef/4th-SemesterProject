using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Transform cameraTransform;  
    [SerializeField] private Transform shotPivot;
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private ParticleSystem impactPrefab;

    [Header("Stats")]
    public float damage = 1f;
    [SerializeField] private float fireRate = 5f;
    [SerializeField] private float range = 20f;
    [SerializeField] private float ricochetDelay = 1f;
    public bool canShoot = true;

    [Header("Ammo")]
    public int ammo;
    [SerializeField] int maxAmmo;
    public float reloadTime;

    [Header("Muzzle")] 
    [SerializeField] private Light muzzleLight;
    [SerializeField] private float flashTime = 0.05f;
    [SerializeField] private ParticleSystem muzzleParticle;
    
    [Header("Sounds")]
    [SerializeField] private AudioClip shotSound;
    [SerializeField] private AudioClip reloadSound;
    
    [Header("Charge Shot")]
    [SerializeField] private Image chargeShotImage;
    [SerializeField] private Image chargeShotFillImage;

    float nextFireTime;
    private TimeBuffer reloadBuffer;
    private TimeBuffer nextFireBuffer;
    
    private float baseFirerate;
    private PlayerXP _playerXP;
    private Health _playerHealth;
    PlayerInputs inputs;
    private PlayerMovement _playerMovement;

    #region Recoil Variables
    private bool _hasRecoil;
    private float _recoilStrength;
    private float _recoilUpwardForce;
    private float _nextRecoilTime;
    private float _recoilCooldown = 1.5f;
    
    #endregion
    
    #region Chargeshot Variables

    private bool _hasChargeshot;
    private bool isCharging;

    private float chargeTimer;
    private float maxChargeTime = 2f;
    private float chargeMultiplier = 3f;
    private float minCharge = 0.2f;
    
    #endregion
    

    private Dictionary<string, WeaponUpgradeSO> ownedUpgrades;

    void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerXP = GetComponent<PlayerXP>();
        inputs = GetComponent<PlayerInputs>();
        _playerHealth = GetComponent<Health>();
    }

    void Start()
    {
        reloadBuffer.Deactivate();
        nextFireBuffer.Deactivate();
        
        baseFirerate = fireRate;
        gunAnimator.SetFloat("FireSpeed", fireRate / baseFirerate);
        ammo = maxAmmo;
        Weapon_UI.instance.UpdateAmmo();
        ownedUpgrades = new Dictionary<string, WeaponUpgradeSO>();
        chargeShotImage.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isCharging)
        {
            chargeTimer += Time.deltaTime;
            chargeTimer = Mathf.Clamp(chargeTimer, 0f, maxChargeTime);

            if (chargeTimer >= minCharge)
            {
                if (!chargeShotImage.gameObject.activeSelf)
                {
                    chargeShotImage.gameObject.SetActive(true);
                    chargeShotFillImage.fillAmount = 0f;
                }

                float fillPercent = Mathf.InverseLerp(
                    minCharge,
                    maxChargeTime,
                    chargeTimer
                );

                chargeShotFillImage.DOFillAmount(fillPercent, 0.05f);
            }
        }
        
        if (ammo <= 0)
        {
            gunAnimator.SetTrigger("isReloading");
            //Play sound
            
            ammo = maxAmmo;
            reloadBuffer.Activate();
            Weapon_UI.instance.UpdateAmmo();
        }
        if(reloadBuffer.IsInTime(reloadTime))
            return;
        
        if (!_hasChargeshot)
        {
            if(ammo > 0 && inputs.ShootInput && !nextFireBuffer.IsInTime(nextFireTime) && canShoot)
                Shoot();
        }

        if(ammo > 0 && inputs.ShootTwoInput && !nextFireBuffer.IsInTime(nextFireTime) && _hasRecoil && canShoot)
        {
            FireRecoil();
            _nextRecoilTime = Time.time + _recoilCooldown;
        }
    }

    private void OnEnable()
    {
        inputs.ShootStarted += StartCharge;
        inputs.ShootReleased += ReleaseCharge;
    }

    private void OnDisable()
    {
        inputs.ShootStarted -= StartCharge;
        inputs.ShootReleased -= ReleaseCharge;
    }

    void Shoot()
    {
        gunAnimator.SetTrigger("isShooting");
        AudioManager.Instance.PlaySfx(shotSound);
        nextFireTime = 1 / fireRate;
        ammo--;
        Weapon_UI.instance.UpdateAmmo();
        PlayerJuice.Instance.CameraKick();
        muzzleParticle.Play();

        WeaponShot baseShot = new WeaponShot
        {
            origin = cameraTransform.position, 
            direction = cameraTransform.forward, 
            damage = Mathf.RoundToInt(damage),
            range = range,
            bounces = 0,
            extraProjectiles = 0,
            spreadAngles = 5f
        };

        foreach (var upgrade in ownedUpgrades.Values)
        {
            if (upgrade == null)
                continue;

            upgrade.Modify(ref baseShot);
        }

        FireMultipleShots(baseShot);
        nextFireBuffer.Activate();
    }

    void FireMultipleShots(WeaponShot shot)
    {
        int totalShots = shot.extraProjectiles + 1;

        for (int i = 0; i < totalShots; i++)
        {
            WeaponShot individual = shot;

            float offset = i - (totalShots - 1) * 0.5f;
            float angle = offset * shot.spreadAngles;

            individual.direction =
                Quaternion.AngleAxis(angle, Vector3.up) * shot.direction;

            ExecuteShot(individual);
        }
    }

    void ExecuteShot(WeaponShot shot)
    {
        Vector3 endPoint;
        Vector3 normal;

        if (Physics.Raycast(shot.origin, shot.direction, out RaycastHit hit, shot.range))
        {
            HandleHit(hit, shot);

            endPoint = hit.point;
            normal = hit.normal;
        }
        else
        {
            endPoint = shot.origin + shot.direction * shot.range;
            normal = -shot.direction;
        }
        
        SpawnImpact(endPoint, normal);
    }

    void HandleHit(RaycastHit hit, WeaponShot shot)
    {
        if (!hit.collider.CompareTag("Enemy") && !hit.collider.CompareTag("Enemy Crit"))
            return;

        float finalDamage = shot.damage;
        bool isCritical = false;

        if (hit.collider.CompareTag("Enemy Crit"))
        {
            finalDamage *= 2f;
            isCritical = true;
        }

        Health health = hit.collider.GetComponentInParent<Health>();

        if (health != null)
        {
            health.TakeDamage(finalDamage, isCritical);
        }

        if (shot.bounces > 0)
        {
            StartCoroutine(RicochetChain(health.gameObject, hit.point, shot));
        }
    }

    IEnumerator RicochetChain(GameObject enemy, Vector3 hitPos, WeaponShot shot)
    {
        HashSet<GameObject> hitEnemies = new HashSet<GameObject>();
        hitEnemies.Add(enemy);

        Vector3 currentPos = hitPos;

        for (int i = 0; i < shot.bounces; i++)
        {
            yield return new WaitForSeconds(ricochetDelay);

            Transform next = FindClosestEnemy(currentPos, hitEnemies);
            if (next == null) yield break;

            hitEnemies.Add(next.gameObject);

            if (next.TryGetComponent<Health>(out var health))
                health.TakeDamage(shot.damage);

            currentPos = next.position;
        }
    }

    Transform FindClosestEnemy(Vector3 pos, HashSet<GameObject> ignore)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        Transform closest = null;
        float best = range;

        foreach (var e in enemies)
        {
            if (ignore.Contains(e)) continue;

            float d = Vector3.Distance(pos, e.transform.position);
            if (d < best)
            {
                best = d;
                closest = e.transform;
            }
        }

        return closest;
    }
    
    void SpawnImpact(Vector3 position, Vector3 normal)
    {
        ParticleSystem fx = Instantiate(impactPrefab, position, Quaternion.LookRotation(normal));
        fx.Play();

        Destroy(fx.gameObject, fx.main.duration + fx.main.startLifetime.constantMax);
    }

    public void EnableRecoil(float strength, float upwardForce)
    {
        _hasRecoil = true;
        _recoilStrength = strength;
        _recoilUpwardForce = upwardForce;
    }

    public void EnableChargeshot(float maxTime, float multiplier)
    {
        _hasChargeshot = true;
        maxChargeTime = maxTime;
        chargeMultiplier = multiplier;
    }

    private void FireRecoil()
    {
        gunAnimator.SetTrigger("isShooting");
        AudioManager.Instance.PlaySfx(shotSound);
        nextFireTime = 1 / fireRate;
        ammo--;
        Weapon_UI.instance.UpdateAmmo();
        PlayerJuice.Instance.CameraKick();
        muzzleParticle.Play();
        
        Vector3 force = -cameraTransform.forward * _recoilStrength;
        force.y = Mathf.Max(force.y, _recoilUpwardForce);
        
        nextFireBuffer.Activate();
        _playerMovement.AddRecoil(force);
    }

    private void StartCharge()
    {
        if (!_hasChargeshot)
            return;

        if (nextFireBuffer.IsInTime(nextFireTime))
            return;

        isCharging = true;
        chargeTimer = 0f;

        chargeShotImage.gameObject.SetActive(false);
        chargeShotFillImage.fillAmount = 0f;
    }

    private void ReleaseCharge()
    {
        if (!_hasChargeshot || !isCharging)
            return;

        if (ammo <= 0)
            return;

        chargeShotImage.gameObject.SetActive(false);
        chargeShotFillImage.fillAmount = 0f;

        float chargePercent = chargeTimer / maxChargeTime;

        if (chargePercent < minCharge)
            Shoot();
        else
            ShootCharged(chargePercent);

        isCharging = false;
        chargeTimer = 0f;
    }
    
    private void ShootCharged(float charge)
    {
        gunAnimator.SetTrigger("isShooting");
        AudioManager.Instance.PlaySfx(shotSound);

        ammo--;
        Weapon_UI.instance.UpdateAmmo();

        PlayerJuice.Instance.CameraKick();
        muzzleParticle.Play();

        WeaponShot baseShot = new WeaponShot
        {
            origin = cameraTransform.position,
            direction = cameraTransform.forward,
            damage = damage * Mathf.Lerp(1f, chargeMultiplier, charge),
            range = range,
            bounces = 0,
            extraProjectiles = 0,
            spreadAngles = 5f
        };


        foreach (var upgrade in ownedUpgrades.Values)
        {
            if (upgrade == null)
                continue;

            upgrade.Modify(ref baseShot);
        }

        FireMultipleShots(baseShot);

        nextFireBuffer.Activate();
    }

    public void AddUpgrade(WeaponUpgradeSO upgrade)
    {
        if (ownedUpgrades.ContainsKey(upgrade.upgradeID))
            ownedUpgrades[upgrade.upgradeID] = upgrade;
        else
            ownedUpgrades.Add(upgrade.upgradeID, upgrade);

        upgrade.Apply(this, _playerHealth, _playerXP);
    }

    public bool HasUpgrade(string id) => ownedUpgrades.ContainsKey(id);

    public WeaponUpgradeSO GetUpgrade(string id) => ownedUpgrades[id];

    public void IncreaseFireRate(float amount)
    {
        fireRate += amount;
        gunAnimator.SetFloat("FireSpeed", fireRate / baseFirerate);
    } 

    public void IncreaseMaxAmmo(int amount)
    {
        maxAmmo += amount;
        ammo = maxAmmo;
    }
    
}

public struct TimeBuffer
{
    private float startTime;
    private float coolDownTime;

    public void Activate()
    {
        //Check if a cooldown is currently active
        if(coolDownTime > Time.time)
            return;

        startTime = Time.time;
    }

    public void Deactivate() => startTime = -99999;

    public void Cooldown(float coolDown)
    {
        Deactivate();
        coolDownTime = Time.time + coolDown;
    }

    public bool IsInTime(float timeFrame) => startTime + timeFrame > Time.time;
}