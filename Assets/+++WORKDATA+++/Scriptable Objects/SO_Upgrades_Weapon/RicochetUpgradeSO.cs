using UnityEngine;


[CreateAssetMenu(menuName = "Upgrades/Ricochet")]
public class RicochetUpgradeSO : WeaponUpgradeSO   // Start is called once before the first execution of Update after the MonoBehaviour is created
{
    public int ricochetCount = 1;

    public override void Modify(ref WeaponShot shot)
    {
        shot.bounces = Mathf.Max(shot.bounces, ricochetCount);
    }
}
