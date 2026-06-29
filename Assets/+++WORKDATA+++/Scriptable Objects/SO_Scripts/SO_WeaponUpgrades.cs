using UnityEngine;

[CreateAssetMenu(fileName = "SO_WeaponUpgrades", menuName = "Scriptable Objects/SO_WeaponUpgrades")]
public class SO_WeaponUpgrades : ScriptableObject
{
    public string upgradeName;
    [TextArea] public string upgradeDescription;
    
    public float damage;
    public float reloadReduction;
    public int ammoIncrease;
    public float recoilIncrease;
    public float fireRateIncrease;
    
}
