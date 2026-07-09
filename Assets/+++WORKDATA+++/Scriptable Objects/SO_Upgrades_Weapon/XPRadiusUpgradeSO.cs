using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Range Upgrade")]
public class XPRangeUpgradeSO : WeaponUpgradeSO
{
    public float rangeBonus = 5f;

    public override void Modify(ref WeaponShot shot)
    {
        
    }

    public override void Apply(Weapon weapon, Health playerHealth, PlayerXP xp)
    {
        xp.xpCollector.IncreaseRadius(rangeBonus);
    }
}