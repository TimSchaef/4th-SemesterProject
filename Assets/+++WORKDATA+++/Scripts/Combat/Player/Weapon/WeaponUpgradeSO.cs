using UnityEngine;

public abstract class WeaponUpgradeSO : ScriptableObject
{
    public string upgradeName;
    public string upgradeDescription;
    public string upgradeID;
    public Sprite upgradeIcon;

    public int tier = 1;
    public WeaponUpgradeSO nextTier;
    
    public abstract void Modify(ref WeaponShot shot);
    
    public virtual void Apply(Weapon weapon, Health playerHealth, PlayerXP xp) { }
}