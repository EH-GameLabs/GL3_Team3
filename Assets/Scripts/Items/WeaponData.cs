using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Collectibles/Weapon")]
public class WeaponData : ItemData
{
    public int damage;
    public float fireRate;
    public GunType weaponType;
    public FirePointType firePointType;
    public int startingAmmo; // 0 usa energia
    public int energyUsed;
    public GameObject projectilePref;

    [Header("Firepoints")]
    public List<Transform> firePoints;

}