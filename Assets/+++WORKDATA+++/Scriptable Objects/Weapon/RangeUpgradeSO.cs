using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Range Upgrade")]
public class RangeUpgradeSO : WeaponUpgradeSO
{
    public float rangeBonus = 20f;

    public override void Modify(ref WeaponShot shot)
    {
        shot.range += rangeBonus;
    }
}