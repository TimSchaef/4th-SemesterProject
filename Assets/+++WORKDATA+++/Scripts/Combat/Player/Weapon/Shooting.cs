using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Camera playerCamera;
    [SerializeField] private LineRenderer tracer;
    [SerializeField] private float tracerDuration;

    [Header("Shooting Stats")]
    [SerializeField] private float damage;
    [SerializeField] private float fireRate;
    [SerializeField] private float range;
    [SerializeField] private float recoilForce;

    [Header("Ammo")]
    [SerializeField] private int ammoCount;
    [SerializeField] private int maxAmmo;
    [SerializeField] private float reloadCooldown;

    private float fireCooldown;
    private bool isReloading;

    [Header("Testings")]
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private Image reloadImage;
    [SerializeField] private UpgradeUI upgradeUI;

    [Header("Upgrades")]
    [SerializeField] private SO_WeaponUpgrades[] _weaponUpgrades;
    
    private PlayerInputs _playerInputs;

    void Awake()
    {
        _playerInputs = GetComponent<PlayerInputs>();
    }

    void Start()
    {
            ammoCount = maxAmmo;
            SetAmmoText();
    }

    void Update()
    {
        // LEFT CLICK: shooting
        if (_playerInputs.ShootInput && CanShoot())
        {
            Shoot();
        }

        // RIGHT CLICK: backward boost
        if (_playerInputs.ShootTwoInput && CanShoot()) 
        {
            RecoilShot();
        }
    }

    private bool CanShoot()
    {
        return !isReloading && Time.time >= fireCooldown && ammoCount > 0;
    }

    private void Shoot()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit hit;
        Vector3 endPoint;

        if (Physics.Raycast(ray, out hit, range))
        {
            endPoint = hit.point;

            if (hit.collider.CompareTag("Enemy"))
            {
                Destroy(hit.collider.gameObject);
                PlayerEconomy.OnGainEconomy?.Invoke(1);
                UI_Economy.OnUpdateEconomy?.Invoke();
            }
        }
        else
        {
            endPoint = ray.origin + ray.direction * range;
        }

        ReduceAmmo();
        StartCoroutine(ShowTracer(ray.origin, endPoint));
    }

    private void RecoilShot()
    {
        Vector3 recoilDirection = -playerCamera.transform.forward; 
        recoilDirection.y = recoilDirection.y / 3;
        PlayerMovement playerMovement = GetComponentInParent<PlayerMovement>(); 
        //playerMovement.AddVelocity(recoilDirection * recoilForce); 
        ReduceAmmo();
    }

    IEnumerator ShowTracer(Vector3 start, Vector3 end)
    {
        tracer.enabled = true;
        tracer.SetPosition(0, start);
        tracer.SetPosition(1, end);

        yield return new WaitForSeconds(tracerDuration);

        tracer.enabled = false;
    }

    public void ApplyUpgrades(SO_WeaponUpgrades upgrades)
    {
        damage += upgrades.damage;
        maxAmmo += upgrades.ammoIncrease;
        reloadCooldown -= upgrades.reloadReduction;
        fireRate += upgrades.fireRateIncrease;
        recoilForce += upgrades.recoilIncrease;
        

        SetAmmoText();
    }

    private void ReduceAmmo()
    {
        fireCooldown = Time.time + 1f / fireRate;

        ammoCount--;
        SetAmmoText();

        if (ammoCount <= 0 && !isReloading)
            StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        isReloading = true;

        if (reloadImage != null)
            reloadImage.fillAmount = 0f;

        float elapsed = 0f;

        while (elapsed < reloadCooldown)
        {
            elapsed += Time.deltaTime;

            if (reloadImage != null)
                reloadImage.fillAmount = elapsed / reloadCooldown;

            yield return null;
        }

        ammoCount = maxAmmo;
        isReloading = false;

        if (reloadImage != null)
            reloadImage.fillAmount = 0f;

        SetAmmoText();
    }

    private void SetAmmoText()
    {
        if (ammoText != null)
        {
            ammoText.text = ammoCount + "/" + maxAmmo;
        }
    }
}