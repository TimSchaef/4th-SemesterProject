using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Max HP")]
public class MaxHPUpgradeSO : WeaponUpgradeSO
{
    public float hpBonus = 5f;

    public override void Modify(ref WeaponShot shot)
    {
       
    }

    public override void Apply(Weapon weapon, Health playerHealth, PlayerXP xp)
    {
        playerHealth.IncreaseMaxHP(hpBonus);
    }
}