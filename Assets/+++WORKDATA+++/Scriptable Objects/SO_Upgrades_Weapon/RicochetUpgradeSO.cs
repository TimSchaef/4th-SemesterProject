using UnityEngine;


[CreateAssetMenu(menuName = "Upgrades/Ricochet")]
public class RicochetUpgradeSO : WeaponUpgradeSO  
{
    public int ricochetCount = 1;

    public override void Modify(ref WeaponShot shot)
    {
        shot.bounces = Mathf.Max(shot.bounces, ricochetCount);
    }
}
