using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Inventory management system
/// </summary>
public class InventorySystem : MonoBehaviour
{
    public int maxSlots = 100;
    public float maxWeight = 200f;

    private PlayerData playerData;
    private float currentWeight = 0f;

    void Start()
    {
        playerData = GetComponent<PlayerData>();
    }

    /// <summary>
    /// Add item to inventory
    /// </summary>
    public bool AddItem(Item item)
    {
        // Check if inventory is full
        if (playerData.inventory.Count >= maxSlots)
        {
            Debug.LogWarning("[INVENTORY] Inventory is full!");
            return false;
        }

        // Check weight limit
        if (currentWeight + item.weight > maxWeight)
        {
            Debug.LogWarning("[INVENTORY] Item is too heavy!");
            return false;
        }

        playerData.inventory.Add(item);
        currentWeight += item.weight;

        Debug.Log($"[INVENTORY] Added {item.itemName} to inventory");
        return true;
    }

    /// <summary>
    /// Remove item from inventory
    /// </summary>
    public bool RemoveItem(string itemID)
    {
        Item item = playerData.inventory.FirstOrDefault(i => i.itemID == itemID);
        if (item == null)
            return false;

        playerData.inventory.Remove(item);
        currentWeight -= item.weight;

        Debug.Log($"[INVENTORY] Removed {item.itemName}");
        return true;
    }

    /// <summary>
    /// Equip item
    /// </summary>
    public bool EquipItem(string itemID)
    {
        Item item = playerData.inventory.FirstOrDefault(i => i.itemID == itemID);
        if (item == null)
            return false;

        // Check if slot is already occupied
        Item existingEquip = playerData.equipment.FirstOrDefault(e => e != null && e.slot == item.slot);
        if (existingEquip != null)
        {
            // Unequip existing item
            playerData.inventory.Add(existingEquip);
            playerData.equipment.Remove(existingEquip);
        }

        // Equip new item
        playerData.inventory.Remove(item);
        playerData.equipment.Add(item);

        Debug.Log($"[INVENTORY] Equipped {item.itemName}");
        return true;
    }

    /// <summary>
    /// Unequip item
    /// </summary>
    public bool UnequipItem(string itemID)
    {
        Item item = playerData.equipment.FirstOrDefault(e => e != null && e.itemID == itemID);
        if (item == null)
            return false;

        // Check inventory space
        if (playerData.inventory.Count >= maxSlots)
        {
            Debug.LogWarning("[INVENTORY] Inventory is full!");
            return false;
        }

        playerData.equipment.Remove(item);
        playerData.inventory.Add(item);

        Debug.Log($"[INVENTORY] Unequipped {item.itemName}");
        return true;
    }

    /// <summary>
    /// Use consumable item
    /// </summary>
    public bool UseItem(string itemID)
    {
        Item item = playerData.inventory.FirstOrDefault(i => i.itemID == itemID);
        if (item == null)
            return false;

        // Apply item effect
        if (item.itemName.Contains("Potion"))
        {
            if (item.itemName.Contains("Health"))
                playerData.currentHP = Mathf.Min(playerData.currentHP + 50, playerData.maxHP);
            else if (item.itemName.Contains("Mana"))
                playerData.currentMana = Mathf.Min(playerData.currentMana + 50, playerData.maxMana);
        }

        // Remove item
        item.quantity--;
        if (item.quantity <= 0)
            RemoveItem(itemID);

        Debug.Log($"[INVENTORY] Used {item.itemName}");
        return true;
    }

    /// <summary>
    /// Sell item to vendor
    /// </summary>
    public bool SellItem(string itemID)
    {
        Item item = playerData.inventory.FirstOrDefault(i => i.itemID == itemID);
        if (item == null)
            return false;

        long sellValue = (long)(item.value * 0.8f); // 80% of item value
        playerData.gold += sellValue;

        RemoveItem(itemID);

        Debug.Log($"[INVENTORY] Sold {item.itemName} for {sellValue} gold");
        return true;
    }

    /// <summary>
    /// Get total inventory weight
    /// </summary>
    public float GetTotalWeight()
    {
        return playerData.inventory.Sum(i => i.weight) + playerData.equipment.Sum(e => e != null ? e.weight : 0);
    }

    /// <summary>
    /// Get inventory status
    /// </summary>
    public string GetInventoryStatus()
    {
        return $@"
=== INVENTORY ===
Slots: {playerData.inventory.Count} / {maxSlots}
Weight: {GetTotalWeight():F1} / {maxWeight}
Gold: {playerData.gold}

=== EQUIPPED ===
{string.Join("\n", playerData.equipment.Where(e => e != null).Select(e => $"{e.slot}: {e.itemName}"))}
";
    }
}
