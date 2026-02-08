using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public enum ItemType
{
    None,
    Weapon,
    Armor,
    Consumable
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemType itemType;
    public GameObject worldPrefab;
}
