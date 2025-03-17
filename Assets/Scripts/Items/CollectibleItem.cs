using UnityEngine;

public class CollectibleItem : MonoBehaviour, ICollectible
{
    public ItemData itemData;

    public void Collect(Player player)
    {
        switch (itemData.type)
        {
            case ItemsType.Weapon:
                player.EquipWeapon((WeaponData)itemData);
                break;
            case ItemsType.Ammo:
                player.CollectAmmo((AmmoData)itemData);
                break;
            case ItemsType.Key:
                player.CollectItem(itemData);
                break;
            case ItemsType.Hostage:
                player.CollectHostage(itemData);
                break;
        }

        Destroy(gameObject); // Rimuove l'oggetto dalla scena
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect(other.GetComponent<Player>());
        }
    }
}