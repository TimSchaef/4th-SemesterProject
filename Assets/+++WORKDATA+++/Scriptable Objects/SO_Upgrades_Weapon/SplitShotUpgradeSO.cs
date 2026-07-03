using UnityEngine;


[CreateAssetMenu(menuName = "Upgrades/Split Shot")]
public class SplitShotUpgradeSO : WeaponUpgradeSO   // Start is called once before the first execution of Update after the MonoBehaviour is created
{
    public int extraProjectiles = 1;
    public float spreadAngles = 10f;

    public override void Modify(ref WeaponShot shot)
    {
        shot.extraProjectiles = Mathf.Max(shot.extraProjectiles, extraProjectiles);
        shot.spreadAngles = Mathf.Max(shot.spreadAngles, spreadAngles);
    }
}
