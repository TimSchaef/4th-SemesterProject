using UnityEngine;

public struct WeaponShot
{
    public Vector3 origin;
    public Vector3 direction;

    public float damage;
    public float range;

    public int extraProjectiles;
    public float spreadAngles;
    public int bounces;
}
