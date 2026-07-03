using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Fire Rate")]
public class FireRateUpgradeSO : WeaponUpgradeSO
{
    public float fireRateBonus = 0.5f;

    public override void Modify(ref WeaponShot shot)
    {
       
    }

    public override void Apply(Weapon weapon, Health playerHealth, PlayerXP xp)
    {
        weapon.IncreaseFireRate(fireRateBonus);
    }
}