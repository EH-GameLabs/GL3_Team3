using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Collectibles/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
}