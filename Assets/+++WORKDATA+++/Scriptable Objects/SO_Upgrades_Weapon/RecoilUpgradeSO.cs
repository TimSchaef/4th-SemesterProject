using UnityEngine;


[CreateAssetMenu(menuName = "Upgrades/Recoil")]
public class RecoilUpgradeSO : WeaponUpgradeSO
{
    public float recoilStrength = 0.5f;
    public float upwardForce = 2f;

    public override void Modify(ref WeaponShot shot)
    {

    }
    public override void Apply(Weapon weapon, Health playerHealth, PlayerXP xp)
    {
        weapon.EnableRecoil(recoilStrength, upwardForce);
    }
}
