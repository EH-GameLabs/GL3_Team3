using UnityEngine;

public class CollectibleItem : MonoBehaviour, ICollectible
{
    public ItemData itemData;

    public void Collect(Player player)
    {
        if (itemData is WeaponData weapon)
        {
            player.EquipWeapon(weapon);
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