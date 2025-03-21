using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Collectibles/Weapon")]
public class WeaponData : ItemData
{
    public int damage;
    public float fireRate;
    public GunType weaponType;
    public int startingAmmo;
}