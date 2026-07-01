using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Damage Upgrade")]
public class DamageUpgradeSO : WeaponUpgradeSO
{
    public float damageBonus = 1f;

    public override void Modify(ref WeaponShot shot)
    {
        shot.damage *= damageBonus;
    }
}