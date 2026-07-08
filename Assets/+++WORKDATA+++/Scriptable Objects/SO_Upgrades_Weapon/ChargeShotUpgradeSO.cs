using UnityEngine;


[CreateAssetMenu(menuName = "Upgrades/Charge Shot")]
public class ChargeShotUpgradeSO : WeaponUpgradeSO
{
    public float maxChargeTime = 2f;
    public float damageMultiplier = 3f;

    public override void Modify(ref WeaponShot shot)
    {
        shot.damage *= damageMultiplier;
    }
    public override void Apply(Weapon weapon, Health playerHealth, PlayerXP xp)
    {
        weapon.EnableChargeshot(maxChargeTime, damageMultiplier);
    }
}
