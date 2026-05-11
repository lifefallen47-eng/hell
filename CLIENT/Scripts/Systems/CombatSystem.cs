using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Core combat system handling damage, abilities, and effects
/// </summary>
public class CombatSystem : MonoBehaviour
{
    [System.Serializable]
    public class DamageInfo
    {
        public float baseDamage;
        public float finalDamage;
        public bool isCritical;
        public string damageType; // physical, magical, etc
    }

    private PlayerData playerData;
    private List<StatusEffect> activeEffects = new List<StatusEffect>();

    void Start()
    {
        playerData = GetComponent<PlayerData>();
    }

    /// <summary>
    /// Calculate damage from attacker to defender
    /// </summary>
    public DamageInfo CalculateDamage(PlayerData attacker, PlayerData defender)
    {
        DamageInfo damageInfo = new DamageInfo();

        // Base damage from attacker's stats
        float baseDamage = attacker.GetTotalDamage();
        float defenseReduction = defender.GetTotalDefense() * 0.5f;

        // Calculate critical chance
        float critChance = (attacker.luck * 0.005f) + 0.05f; // 5% + luck bonus
        bool isCritical = Random.value < critChance;

        if (isCritical)
        {
            damageInfo.baseDamage = baseDamage * 1.5f; // 50% bonus
            damageInfo.isCritical = true;
        }
        else
        {
            damageInfo.baseDamage = baseDamage;
            damageInfo.isCritical = false;
        }

        // Apply defense
        float rawDamage = damageInfo.baseDamage - defenseReduction;

        // Add variance (±10%)
        float variance = 0.9f + (Random.value * 0.2f);
        damageInfo.finalDamage = rawDamage * variance;

        // Ensure minimum damage
        if (damageInfo.finalDamage < 0)
            damageInfo.finalDamage = 0;

        return damageInfo;
    }

    /// <summary>
    /// Apply damage to target
    /// </summary>
    public void DealDamage(PlayerData attacker, PlayerData defender, float damage)
    {
        // Check dodge
        float dodgeChance = (defender.agility * 0.01f) + 0.05f;
        if (Random.value < dodgeChance)
        {
            Debug.Log($"[COMBAT] {defender.playerName} dodged the attack!");
            return;
        }

        // Apply damage
        defender.currentHP -= damage;
        Debug.Log($"[COMBAT] {attacker.playerName} dealt {damage:F1} damage to {defender.playerName}");

        // Check if dead
        if (defender.currentHP <= 0)
        {
            defender.currentHP = 0;
            defender.isAlive = false;
            OnPlayerDeath(defender, attacker);
        }
    }

    /// <summary>
    /// Normal attack
    /// </summary>
    public void Attack(PlayerData attacker, PlayerData defender)
    {
        DamageInfo damage = CalculateDamage(attacker, defender);
        DealDamage(attacker, defender, damage.finalDamage);

        if (damage.isCritical)
            Debug.Log("[COMBAT] CRITICAL HIT!");
    }

    /// <summary>
    /// Power attack (costs mana, higher damage)
    /// </summary>
    public void PowerAttack(PlayerData attacker, PlayerData defender)
    {
        if (attacker.currentMana < 20)
        {
            Debug.Log("[COMBAT] Not enough mana for power attack!");
            return;
        }

        attacker.currentMana -= 20;

        DamageInfo damage = CalculateDamage(attacker, defender);
        damage.finalDamage *= 2f; // 2x damage multiplier
        DealDamage(attacker, defender, damage.finalDamage);
    }

    /// <summary>
    /// Apply status effect
    /// </summary>
    public void ApplyStatusEffect(PlayerData target, string effectType, float duration, float magnitude = 0)
    {
        StatusEffect effect = new StatusEffect
        {
            type = effectType,
            duration = duration,
            magnitude = magnitude
        };

        activeEffects.Add(effect);
        Debug.Log($"[COMBAT] {target.playerName} is now {effectType}!");
    }

    /// <summary>
    /// Handle player death
    /// </summary>
    private void OnPlayerDeath(PlayerData deadPlayer, PlayerData killer)
    {
        Debug.Log($"[COMBAT] {deadPlayer.playerName} has been defeated!");

        // Loss penalties
        long lostExp = (long)(deadPlayer.exp * 0.05f);
        deadPlayer.exp -= lostExp;

        long droppedGold = deadPlayer.gold / 10; // Drop 10% of gold
        deadPlayer.gold -= droppedGold;

        if (deadPlayer.exp < 0)
        {
            deadPlayer.level--;
            deadPlayer.exp = 0;
        }

        // Killer rewards
        if (killer != null)
        {
            killer.AddPKKill();
            killer.gold += droppedGold;
            Debug.Log($"[COMBAT] {killer.playerName} gained PK kill and {droppedGold} gold");
        }

        // Respawn
        Respawn(deadPlayer);
    }

    /// <summary>
    /// Respawn player
    /// </summary>
    private void Respawn(PlayerData player)
    {
        player.isAlive = true;
        player.currentHP = player.maxHP;
        player.currentMana = player.maxMana;
        player.position = Vector3.zero; // Safe spawn point
        Debug.Log($"[COMBAT] {player.playerName} respawned");
    }
}

/// <summary>
/// Status effect data
/// </summary>
public class StatusEffect
{
    public string type; // stun, slow, burn, poison, etc
    public float duration;
    public float magnitude; // Severity
    public float elapsedTime = 0;
}
