using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Max Ammo")]
public class MaxAmmoUpgradeSO : WeaponUpgradeSO
{
    public int ammoBonus = 5;

    public override void Modify(ref WeaponShot shot)
    {
       
    }

    public override void Apply(Weapon weapon, Health playerHealth, PlayerXP xp)
    {
        weapon.IncreaseMaxAmmo(ammoBonus);
    }
}