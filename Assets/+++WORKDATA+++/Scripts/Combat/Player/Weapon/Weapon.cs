using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class Weapon : MonoBehaviour
{
    [FormerlySerializedAs("playerCamera")]
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

    [Header("Ammo")]
    public int ammo;
    [SerializeField] int maxAmmo;
    public float reloadTime;

    [Header("Muzzle")] 
    [SerializeField] private Light muzzleLight;
    [SerializeField] private float flashTime = 0.05f;
    [SerializeField] private ParticleSystem muzzleParticle;

    float nextFireTime;
    bool reloading;

    private PlayerXP _playerXP;
    private Health _playerHealth;
    PlayerInputs inputs;

    private Dictionary<string, WeaponUpgradeSO> ownedUpgrades;
    private List<Vector3> tracerPoints = new List<Vector3>();

    void Awake()
    {
        _playerXP = FindObjectOfType<PlayerXP>();
        inputs = GetComponent<PlayerInputs>();
        _playerHealth = GetComponent<Health>();
    }

    void Start()
    {
        ammo = maxAmmo;
        Weapon_UI.instance.UpdateAmmo();
        ownedUpgrades = new Dictionary<string, WeaponUpgradeSO>();
    }

    void Update()
    {
        if (inputs.ShootInput && CanShoot())
            Shoot();
    }

    bool CanShoot()
    {
        return !reloading &&
               Time.time >= nextFireTime &&
               ammo > 0;
    }

    void Shoot()
    {
        nextFireTime = Time.time + 1f / fireRate;
        ammo--;
        Weapon_UI.instance.UpdateAmmo();
        PlayerJuice.Instance.CameraKick();
        muzzleParticle.Play();
        gunAnimator.SetTrigger("isShooting");

        WeaponShot baseShot = new WeaponShot
        {
            origin = cameraTransform.position, 
            direction = cameraTransform.forward, 
            damage = damage,
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

        if (ammo <= 0 && !reloading)
            StartCoroutine(Reload());
    }

    void FireMultipleShots(WeaponShot shot)
    {
        ExecuteShot(shot);
        
        for (int i = 0; i < shot.extraProjectiles; i++)
        {
            Vector3 spreadDir = ApplySpread(
                shot.direction,
                shot.spreadAngles,
                i,
                shot.extraProjectiles
            );

            WeaponShot individual = shot;
            individual.direction = spreadDir;

            ExecuteShot(individual);
        }
    }

    Vector3 ApplySpread(Vector3 forward, float angle, int index, int total)
    {
        if (total <= 0)
            return forward;
        
        float step = angle / total;
        float current = -angle / 2f + step * index;

        Quaternion rot = Quaternion.AngleAxis(current, Vector3.up);
        return rot * forward;
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
        if (!hit.collider.CompareTag("Enemy"))
            return;

        if (hit.collider.TryGetComponent<Health>(out var health))
        {
            health.TakeDamage(shot.damage);
        }

        if (shot.bounces > 0)
        {
            StartCoroutine(RicochetChain(hit.collider.gameObject, hit.point, shot));
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

    IEnumerator Reload()
    {
        reloading = true;
        gunAnimator.SetTrigger("isReloading");

        float t = 0f;
        while (t < reloadTime)
        {
            t += Time.deltaTime;
            yield return null;
        }

        ammo = maxAmmo;
        reloading = false;
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

    public void IncreaseFireRate(float amount) => fireRate += amount;

    public void IncreaseMaxAmmo(int amount)
    {
        maxAmmo += amount;
        ammo = maxAmmo;
    }
    
}