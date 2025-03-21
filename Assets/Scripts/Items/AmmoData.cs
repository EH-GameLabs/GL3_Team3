using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AmmoData", menuName = "Collectibles/AmmoData")]
public class AmmoData : ItemData
{
    public int ammo;
    public GunType gunType;
}
