using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/XP Gain")]
public class XPUpgradeSO : WeaponUpgradeSO
{
    public float multiplierIncrease = 0.2f;

   public override void Modify(ref WeaponShot shot)
    {
       
    }

    public override void Apply(Weapon weapon, Health playerHealth, PlayerXP xp)
    {
        xp.xpMultiplier += multiplierIncrease;;
    }
}