using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Collectibles/Weapon")]
public class WeaponData : ItemData
{
    public int damage;
    public float fireRate;
    public GameObject Gun; //modello 3d con relativo firePoint
    public GunType type;
}