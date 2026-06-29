using UnityEngine;

public abstract class WeaponUpgradeSO : ScriptableObject
{
    public string upgradeName;
    public string upgradeDescription;
    public string upgradeID;

    public int tier = 1;
    public WeaponUpgradeSO nextTier;

    // ONLY for shoot-time effects (split shot, ricochet, etc)
    public abstract void Modify(ref WeaponShot shot);

    // ONLY for permanent effects (ammo, hp, fire rate, regen)
    public virtual void Apply(Weapon weapon, Health playerHealth, PlayerXP xp) { }
}