using UnityEngine;

/// <summary>
/// Player leveling and experience system
/// </summary>
public class LevelingSystem : MonoBehaviour
{
    private PlayerData playerData;

    void Start()
    {
        playerData = GetComponent<PlayerData>();
    }

    /// <summary>
    /// Award experience to player
    /// </summary>
    public void GainEXP(long amount)
    {
        playerData.exp += amount;
        playerData.totalExp += amount;

        Debug.Log($"[LEVELING] {playerData.playerName} gained {amount} EXP");

        // Check for level ups
        while (playerData.exp >= playerData.GetRequiredExp())
        {
            playerData.exp -= playerData.GetRequiredExp();
            LevelUp();
        }
    }

    /// <summary>
    /// Award scaled experience based on difficulty
    /// </summary>
    public void GainScaledEXP(long baseExp, int enemyLevel)
    {
        float difficultyModifier = 1f;

        // Calculate difficulty modifier
        if (playerData.level > enemyLevel)
        {
            // Reduce XP for overleveled fights
            difficultyModifier = (float)enemyLevel / playerData.level;
        }
        else if (playerData.level < enemyLevel)
        {
            // Increase XP for underleveled fights
            difficultyModifier = (float)enemyLevel / playerData.level;
        }

        long scaledExp = (long)(baseExp * difficultyModifier);
        GainEXP(scaledExp);
    }

    /// <summary>
    /// Level up the player
    /// </summary>
    private void LevelUp()
    {
        playerData.level++;
        playerData.currentHP = playerData.GetTotalHP();
        playerData.currentMana = playerData.maxMana;

        Debug.Log($"[LEVELING] {playerData.playerName} leveled up to {playerData.level}!");

        // Award attribute points every 10 levels
        if (playerData.level % 10 == 0)
        {
            playerData.freeAttributePoints += 10;
            Debug.Log($"[LEVELING] +10 Attribute Points awarded!");
        }

        // Award skill points or other rewards based on level
        OnLevelUpReward(playerData.level);
    }

    /// <summary>
    /// Give special rewards at certain levels
    /// </summary>
    private void OnLevelUpReward(int level)
    {
        switch (level)
        {
            case 10:
                Debug.Log("[LEVELING] Unlocked: Advanced Abilities");
                break;
            case 25:
                Debug.Log("[LEVELING] Unlocked: Raid Content");
                break;
            case 50:
                Debug.Log("[LEVELING] Unlocked: PvP Arena");
                break;
            case 100:
                Debug.Log("[LEVELING] Unlocked: Divine Realm");
                break;
            case 150:
                Debug.Log("[LEVELING] Unlocked: Mythic Dungeons");
                break;
        }
    }

    /// <summary>
    /// Allocate free attribute points
    /// </summary>
    public void AddAttribute(string attributeType, int points)
    {
        if (playerData.freeAttributePoints < points)
        {
            Debug.LogWarning("[LEVELING] Not enough free attribute points!");
            return;
        }

        switch (attributeType.ToLower())
        {
            case "strength":
                playerData.strength += points;
                break;
            case "agility":
                playerData.agility += points;
                break;
            case "vitality":
                playerData.vitality += points;
                playerData.maxHP = playerData.GetTotalHP();
                break;
            case "intelligence":
                playerData.intelligence += points;
                playerData.maxMana += points * 5;
                break;
            case "defense":
                playerData.defense += points;
                break;
            case "luck":
                playerData.luck += points;
                break;
            default:
                Debug.LogWarning($"[LEVELING] Unknown attribute: {attributeType}");
                return;
        }

        playerData.freeAttributePoints -= points;
        Debug.Log($"[LEVELING] {playerData.playerName} +{points} {attributeType}");
    }
}
