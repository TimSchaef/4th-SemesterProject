using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Heal")]
public class RegenUpgradeSO : WeaponUpgradeSO
{
    public float regenIncrease = 0.5f;

    public override void Modify(ref WeaponShot shot)
    {
       
    }

    public override void Apply(Weapon weapon, Health playerHealth, PlayerXP xp)
    {
        playerHealth.IncreaseRegen(regenIncrease);
    }
}