using UnityEngine;

public class UpgradePickup : MonoBehaviour
{
    [SerializeField] WeaponUpgradeSO upgrade;

    private void OnTriggerEnter(Collider other)
    {
        Weapon weapon = other.GetComponentInParent<Weapon>();

        if (weapon != null)
        {
            weapon.AddUpgrade(upgrade);
            Debug.Log("Pickup collected: " + upgrade.name);
            Destroy(gameObject);
        }
    }
}