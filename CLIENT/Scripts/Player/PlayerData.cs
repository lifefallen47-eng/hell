using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Core player data structure
/// Holds all character information
/// </summary>
[System.Serializable]
public class PlayerData
{
    public string playerID;
    public string playerName;

    // Progression
    public int level = 1;
    public long exp = 0;
    public long totalExp = 0;

    // Core Attributes
    public int strength = 1;
    public int agility = 1;
    public int vitality = 1;
    public int intelligence = 1;
    public int defense = 1;
    public int luck = 1;

    public int freeAttributePoints = 10;

    // Combat
    public float maxHP = 100;
    public float currentHP = 100;
    public float maxMana = 100;
    public float currentMana = 100;
    public float maxStamina = 100;
    public float currentStamina = 100;

    // PvP
    public int pkKills = 0;
    public float morality = 0f; // -100 to +100
    public bool isWorldThreat = false;
    public int bountyAmount = 0;

    // Economy
    public long gold = 0;
    public long bank = 0;

    // Guild
    public string guildID = "";
    public string guildRole = "";

    // Inventory
    public List<Item> inventory = new List<Item>();
    public List<Item> equipment = new List<Item>();

    // Status
    public bool isAlive = true;
    public bool inCombat = false;
    public Vector3 position = Vector3.zero;
    public string currentZone = "Ashenfall";

    // Timestamps
    public long createdAt = 0;
    public long lastLogin = 0;
    public long lastLogout = 0;

    public PlayerData(string name)
    {
        playerName = name;
        playerID = System.Guid.NewGuid().ToString();
        createdAt = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        lastLogin = createdAt;
    }

    /// <summary>
    /// Calculate total HP including equipment bonuses
    /// </summary>
    public float GetTotalHP()
    {
        float baseHP = 100 + (vitality * 10);
        float equipmentBonus = 0;

        foreach (Item item in equipment)
        {
            if (item != null && item.hpBonus > 0)
                equipmentBonus += item.hpBonus;
        }

        return baseHP + equipmentBonus;
    }

    /// <summary>
    /// Calculate damage including equipment
    /// </summary>
    public float GetTotalDamage()
    {
        float baseDamage = strength * 2f;
        float weaponDamage = 0;

        // Get equipped weapon damage
        Item mainHand = equipment.Find(e => e != null && e.slot == ItemSlot.MainHand);
        if (mainHand != null)
            weaponDamage = mainHand.damageBonus;

        return baseDamage + weaponDamage;
    }

    /// <summary>
    /// Calculate defense including equipment
    /// </summary>
    public float GetTotalDefense()
    {
        float baseDefense = defense;
        float equipmentDefense = 0;

        foreach (Item item in equipment)
        {
            if (item != null && item.defenseBonus > 0)
                equipmentDefense += item.defenseBonus;
        }

        return baseDefense + equipmentDefense;
    }

    /// <summary>
    /// Add PK kill and update difficulty multiplier
    /// </summary>
    public void AddPKKill()
    {
        pkKills++;
        morality -= 10;

        if (morality < -100) morality = -100;

        if (pkKills >= 100)
            isWorldThreat = true;
    }

    /// <summary>
    /// Get difficulty multiplier based on PK kills
    /// </summary>
    public float GetDifficultyMultiplier()
    {
        return 1f + ((pkKills / 5f) * 0.10f);
    }

    /// <summary>
    /// Get player status string for debugging
    /// </summary>
    public string GetStatus()
    {
        return $@"
=== {playerName} ===
Level: {level}
EXP: {exp} / {GetRequiredExp()}
HP: {currentHP} / {maxHP}
Mana: {currentMana} / {maxMana}

Attributes:
  Strength:     {strength}
  Agility:      {agility}
  Vitality:     {vitality}
  Intelligence: {intelligence}
  Defense:      {defense}
  Luck:         {luck}

PvP:
  PK Kills:     {pkKills}
  Morality:     {morality}
  Bounty:       {bountyAmount}
  World Threat: {isWorldThreat}

Gold: {gold}
Zone: {currentZone}
";
    }

    /// <summary>
    /// Get required experience for next level
    /// </summary>
    public long GetRequiredExp()
    {
        return (long)(100 * System.Math.Pow(level, 1.5f));
    }
}

/// <summary>
/// Item rarity enum
/// </summary>
public enum ItemRarity
{
    Common = 0,
    Uncommon = 1,
    Rare = 2,
    Epic = 3,
    Legendary = 4,
    Mythic = 5,
    Divine = 6
}

/// <summary>
/// Item slot enum
/// </summary>
public enum ItemSlot
{
    Head,
    Chest,
    Legs,
    Feet,
    MainHand,
    OffHand,
    Ring1,
    Ring2,
    Amulet,
    Inventory
}

/// <summary>
/// Item data structure
/// </summary>
[System.Serializable]
public class Item
{
    public string itemID;
    public string itemName;
    public string description;
    public ItemRarity rarity;
    public ItemSlot slot;

    public float damageBonus = 0;
    public float defenseBonus = 0;
    public float hpBonus = 0;
    public float manaBonus = 0;

    public int attributeBonus = 0; // Bonus to one attribute
    public string attributeType = ""; // strength, agility, etc

    public int level = 1;
    public long value = 0; // Gold value
    public float weight = 0;
    public int quantity = 1;

    public List<string> affixes = new List<string>(); // Special attributes
    public bool isBound = false; // Can't be traded

    public Item(string name, ItemRarity rarity, ItemSlot slot)
    {
        itemID = System.Guid.NewGuid().ToString();
        itemName = name;
        this.rarity = rarity;
        this.slot = slot;
    }

    /// <summary>
    /// Get color based on rarity
    /// </summary>
    public Color GetRarityColor()
    {
        return rarity switch
        {
            ItemRarity.Common => Color.white,
            ItemRarity.Uncommon => Color.green,
            ItemRarity.Rare => Color.blue,
            ItemRarity.Epic => new Color(0.6f, 0, 1f), // Purple
            ItemRarity.Legendary => Color.yellow,
            ItemRarity.Mythic => new Color(1f, 0.5f, 0), // Orange
            ItemRarity.Divine => Color.yellow,
            _ => Color.white
        };
    }
}
