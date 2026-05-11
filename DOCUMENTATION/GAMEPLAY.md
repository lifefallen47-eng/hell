# HELLFALL: ETERNAL ASCENSION - Gameplay Guide

## Core Mechanics

### 1. Character Progression

#### Leveling System
- **Max Level**: 200+ (infinite with world scaling)
- **Experience Required**: `100 * (level ^ 1.5)`
- **Level Milestones**:
  - Every 10 levels: +10 free attribute points
  - Level 25: Unlock advanced abilities
  - Level 50: Unlock raid content
  - Level 100: Unlock PvP arenas
  - Level 150: Unlock divine realm

#### Attributes
| Attribute | Effect |
|-----------|--------|
| **Strength** | Increases physical damage by 2 per point |
| **Agility** | Increases attack speed & dodge by 1% per point |
| **Vitality** | Increases max HP by 10 per point |
| **Intelligence** | Increases max Mana by 5 per point |
| **Defense** | Reduces physical damage taken by 0.5% per point |
| **Luck** | Increases critical chance & rare drops by 0.5% per point |

#### Resource System
```
Health Points (HP)
├── Restored by: Healing potions, regeneration abilities
├── Reduced by: Enemy attacks, environmental damage
└── Death occurs at: 0 HP

Mana Points (MP)
├── Restored by: Mana potions, meditation, time
├── Required for: Spells, special abilities
└── Regeneration: 10 MP/second

Stamina (optional)
├── Restored by: Resting, food
├── Required for: Running, dodging, heavy attacks
└── Regeneration: 20 Stamina/second
```

---

### 2. Combat System

#### Attack Types
```
Normal Attack
├── Cooldown: 0.5 seconds
├── Damage: (Strength * 2) - (Enemy Defense * 0.5)
├── Range: Melee (2 meters)
└── No resource cost

Power Attack
├── Cooldown: 2 seconds
├── Damage: (Strength * 4) - (Enemy Defense * 0.25)
├── Range: Melee (2 meters)
├── Cost: 20 Mana
└── Stuns for 1 second

Ability (varies)
├── Cooldown: 5-30 seconds
├── Damage: Variable
├── Range: 5-20 meters
├── Cost: 30-100 Mana
└── Effects: Knockback, stun, slow, etc.
```

#### Combat Flow
```
1. Player targets enemy
   ↓
2. Enter combat state
   └─ Can't use mounts
   └─ Can't trade
   └─ Can't rest
   ↓
3. Perform attacks
   └─ Normal attack (cheap)
   └─ Ability (expensive)
   └─ Item use
   ↓
4. Receive damage
   └─ Update HP
   └─ Apply effects (burn, poison, slow)
   ↓
5. Enemy dies OR player flees
   └─ Exit combat state
   └─ Restore resources over time
```

#### Status Effects
| Effect | Duration | Source | Impact |
|--------|----------|--------|--------|
| **Stun** | 1-3s | Abilities | Cannot move or attack |
| **Slow** | 5s | Abilities | Move speed -50% |
| **Burn** | 10s | Fire spells | Take 10 damage/sec |
| **Poison** | 15s | Poison abilities | Take 5 damage/sec |
| **Bleed** | 20s | Weapons | Take 2 damage/sec |
| **Weakness** | 10s | Curses | Damage dealt -30% |
| **Vulnerability** | 10s | Debuffs | Damage taken +20% |

---

### 3. Combat Mechanics

#### Damage Calculation
```
base_damage = (attacker_strength * weapon_multiplier)
target_defense = (defender_defense * 0.5)
critical_chance = (attacker_luck * 0.005) + 0.05
critical_multiplier = 1.5

if (random() < critical_chance) {
    damage = base_damage * critical_multiplier - target_defense
    // "CRITICAL HIT!"
} else {
    damage = base_damage - target_defense
}

// Apply variance (±10%)
final_damage = damage * (0.9 + random(0, 0.2))
```

#### Armor System
- **Light Armor**: -25% defense, +10% dodge
- **Medium Armor**: +50% defense, +5% dodge
- **Heavy Armor**: +100% defense, -5% dodge
- **Shield**: +30% defense

#### Dodge & Block
```
dodge_chance = (agility * 0.01) + base_dodge
block_chance = (shield_type) + 0.1

if random() < dodge_chance:
    damage = 0  // "DODGED!"
elif random() < block_chance:
    damage = damage * 0.5  // "BLOCKED!"
```

---

### 4. Experience & Leveling

#### Experience Gain
```
Enemy Defeat:
- Base EXP = Enemy Level * 100
- Difficulty Modifier = (Player Level / Enemy Level)
  - If Player Level > Enemy Level: ÷ (Player Level / Enemy Level)
  - If Player Level < Enemy Level: × (Enemy Level / Player Level)
- Quest Completion: 500-5000 EXP
- Boss Defeat: 10000+ EXP
- Dungeon Completion: 5000 + Boss EXP

Example:
- Level 10 player defeats Level 10 enemy: 1000 EXP
- Level 20 player defeats Level 10 enemy: 500 EXP (less rewarding)
- Level 5 player defeats Level 10 enemy: 2000 EXP (bonus for difficulty)
```

#### Death Penalty
```
On Death:
├─ Lose 5% of current EXP
├─ Drop 10% of carried gold
├─ Respawn at last checkpoint
├─ Temporary debuff (no death XP for 5 minutes)
└─ Equipment durability -10%
```

---

### 5. Items & Inventory

#### Item Rarities
```
Common (White)
├─ Drop chance: 50%
├─ Base stats +50%
└─ Vendor price: 1x base

Uncommon (Green)
├─ Drop chance: 30%
├─ Base stats +100%
└─ Vendor price: 2x base

Rare (Blue)
├─ Drop chance: 15%
├─ Base stats +200%
├─ 1 special affix
└─ Vendor price: 5x base

Epic (Purple)
├─ Drop chance: 4%
├─ Base stats +400%
├─ 2 special affixes
└─ Vendor price: 10x base

Legendary (Orange)
├─ Drop chance: 0.9%
├─ Base stats +800%
├─ 3 special affixes
└─ Vendor price: 50x base

Mythic (Red)
├─ Drop chance: 0.09%
├─ Base stats +1600%
├─ 4 special affixes
├─ Unique passive ability
└─ Vendor price: 200x base

Divine (Gold)
├─ Drop chance: 0.01%
├─ Base stats +3200%
├─ 5 special affixes
├─ Unique passive ability
├─ Binding on pickup
└─ Unsellable
```

#### Inventory System
- **Max Slots**: 100 (expandable with gold)
- **Weight Limit**: 200 kg (varies with vitality)
- **Types**: Equipment, consumables, quest items, materials

#### Equipment Slots
```
Head (Helmet)
├─ HP: +30
├─ Defense: +5
└─ Special: +Intelligence

Chest (Armor)
├─ HP: +50
├─ Defense: +10
└─ Special: +Vitality

Legs (Greaves)
├─ HP: +40
├─ Defense: +8
└─ Special: Movement speed +5%

Feet (Boots)
├─ HP: +20
├─ Defense: +3
└─ Special: Movement speed +10%

Main Hand (Weapon)
├─ Damage: +20
├─ Special: +Strength

Off Hand (Shield/Weapon)
├─ Defense: +8 (shield)
└─ Damage: +10 (weapon)

Rings (2 slots)
├─ Special affixes only
└─ No stat restrictions

Amulet
├─ Special affixes only
└─ Unique effects
```

---

### 6. Quests & Missions

#### Quest Types
```
Kill Quests
├─ Objective: Defeat X enemies
├─ Reward: EXP + Gold
└─ Time limit: None

Collection Quests
├─ Objective: Gather X items
├─ Reward: EXP + Gold + Items
└─ Respawn time: Varies

Exploration Quests
├─ Objective: Discover locations
├─ Reward: EXP + Gold + Items
└─ Progress tracking: Map

Story Quests
├─ Objective: Complete narrative
├─ Reward: EXP + Gold + Rare items
├─ Locked behind level
└─ Cannot be abandoned

Repeatable Quests
├─ Objective: Daily/Weekly challenges
├─ Reward: Gold + Materials
└─ Reset frequency: Daily/Weekly

Raid Quests
├─ Objective: Defeat boss
├─ Reward: 10000+ EXP + Legendary items
├─ Party required: 3-10 players
└─ Difficulty scales with party
```

#### Quest Progression
```
Quest Unlocked
  ↓
Quest Accepted
  ↓
Objective Progress (0%, 25%, 50%, 75%, 100%)
  ↓
Objective Complete
  ↓
Quest Turn-in (NPC)
  ↓
Reward Received
  ↓
Quest Marked as Complete
```

---

### 7. PvP System

#### PvP Modes
```
Duel (1v1)
├─ No rewards
├─ No penalties
├─ 5 minute timeout
└─ Consensual

Arena (3v3, 5v5)
├─ Ranked matchmaking
├─ Rating-based rewards
├─ Weekly tournaments
└─ Skill-based difficulty

World PvP
├─ Risky farming areas
├─ High rewards
├─ PK kill tracking
├─ Morality impact
└─ Bounty system

Guild Wars
├─ Guild vs Guild
├─ Territory control
├─ Scheduled events
└─ Rare rewards
```

#### PvP Ranking
```
Bronze (0-1000 rating)
Silver (1000-2000 rating)
Gold (2000-3000 rating)
Platinum (3000-4000 rating)
Diamond (4000-5000 rating)
Master (5000+ rating)

Rating System:
- Win: +20-50 rating
- Loss: -20-50 rating
- Bonus: +10 for outnumbered victory
- Penalty: -10 for abandoning match
```

#### PK System
```
PK Kill Tracking
├─ 1-5 kills: Normal (no penalty)
├─ 6-25 kills: Wanted (slight stat penalty -5%)
├─ 26-50 kills: Hunted (stat penalty -10%)
├─ 51-100 kills: Marked for Death (stat penalty -20%, bounty)
└─ 100+ kills: World Threat (all stats -30%, permanent bounty)

Morality System (-100 to +100):
├─ +100: Saint (NPCs give discounts)
├─ +50: Benevolent (no PvP cooldown)
├─ 0: Neutral (normal)
├─ -50: Malicious (NPCs hostile)
└─ -100: Evil (blacklisted from cities)

Kill a Player:
├─ Lose 10 morality points
├─ Gain 1 PK kill
├─ Victim drops 20% of gold
└─ Victim loses 2% EXP

Get Killed by Player:
├─ Killer gains morality loss
├─ You lose gold & EXP
├─ You respawn at checkpoint
└─ Can place bounty on killer
```

---

### 8. World Scaling

#### Dynamic Difficulty
```
World Level = Average Player Level

Example:
- Average level: 10 → World Level 10
- Enemies level: 8-12 (scaled)
- Boss stats scale with world level
- Rare drop rates increase with world level

World Level Progression:
Level 1 → Level 10 → Level 25 → Level 50 → Level 100 → Level 200+
```

#### Mob Scaling (Team-based)
```
Base Enemy Stats:
├─ HP: 100
├─ Damage: 10

Team Size Multiplier:
├─ Solo (1): × 1.0
├─ Duo (2): × 1.05
├─ Small Team (3-5): × (1 + (teamSize - 1) * 0.05)
├─ Large Team (6-10): × 1.45
└─ Raid (11+): × 1.5

Example:
- Solo player fights: 100 HP enemy
- 3-player team fights: 110 HP enemy (10% stronger)
- 10-player team fights: 145 HP enemy (45% stronger)
```

---

### 9. Hell Wave Events

#### Event Schedule
```
Frequency: Every 2 hours
Duration: 15-30 minutes
Difficulty: Scales with world level
Warning: 5-minute announcement before start
```

#### Wave Mechanics
```
Hell Wave 1:
├─ 50 demons spawn
├─ Reward: 1000 gold + materials
└─ Difficulty: Easy

Hell Wave 5:
├─ 150 demons spawn
├─ Boss appears (after 10 min)
├─ Reward: 5000 gold + rare items
└─ Difficulty: Medium

Hell Wave 10:
├─ 300 demons spawn
├─ 3 mini-bosses
├─ Boss appears (after 15 min)
├─ Reward: 20000 gold + legendary items
└─ Difficulty: Hard

Hell Wave 20+:
├─ 500+ demons spawn
├─ 5+ mini-bosses
├─ Final boss appears
├─ Reward: 100000 gold + mythic items
├─ Difficulty: Extreme
└─ Requires coordinated group
```

#### Survival Mechanics
```
Objective: Survive for duration or defeat boss

Rewards (based on:
├─ Time survived
├─ Enemies defeated
├─ Contributions
└─ Wave difficulty

Participation:
├─ Damage dealt: 50% of reward
├─ Healing done: 25% of reward
├─ Buffs applied: 15% of reward
└─ Kills: 10% bonus per kill
```

---

### 10. Boss Encounters

#### Boss Types
```
Named Bosses
├─ Unique mechanics
├─ Rare drops
├─ Story significance
└─ Scales with world level

World Bosses
├─ Spawn every 6 hours
├─ Can be killed by anyone
├─ High value rewards
└─ First hit gets loot priority

Raid Bosses
├─ Require 5-20 players
├─ Multiple phases
├─ Unique abilities
└─ Exclusive rare drops

Mythical Bosses
├─ Story-driven
├─ Once per server
├─ Permanent consequences
└─ Server-first rewards
```

#### Boss Mechanics Example (Infernal Titan)
```
Phase 1 (100-75% HP):
├─ Basic attacks
├─ Occasional meteor shower (area damage)
├─ 1 add (demon) spawns

Phase 2 (75-50% HP):
├─ Faster attacks
├─ Meteor shower (more frequent)
├─ 3 adds spawn
├─ Enrage meter (increased damage)

Phase 3 (50-0% HP):
├─ Multiple meteor showers
├─ 5 adds spawn
├─ Massive enrage (2x damage)
├─ Final attack (one-shot warning)
└─ On death: Rare drops + server announcement
```

---

## Progression Path Example

```
New Player Journey:

Day 1:
├─ Character creation
├─ Tutorial (level 1-5)
├─ Defeat starter enemies
├─ Equip first items
└─ Reach Level 5

Week 1:
├─ Level 5-15
├─ Complete first quests
├─ Earn first 1000 gold
├─ Equip uncommon items
└─ Join a guild

Week 2-4:
├─ Level 15-30
├─ Farm dungeons
├─ Earn rare items
├─ Participate in Hell Waves
├─ Gain guild reputation

Month 2:
├─ Level 30-50
├─ Join raid group
├─ Defeat raid bosses
├─ Earn legendary items
└─ Reach first major milestone

Month 3-6:
├─ Level 50-100
├─ Become raid leader
├─ PvP arena participation
├─ Territory control battles
└─ Approach endgame content

Month 7-12:
├─ Level 100-150
├─ Legendary items equipped
├─ Guild leadership
├─ Competitive PvP ranking
└─ Server-wide recognition

Year 2+:
├─ Level 150+
├─ Mythic items
├─ Legend status
├─ Server events participation
└─ Shape the world's history
```

---

## Tips & Strategy

### Beginner Tips
1. Follow the tutorial carefully
2. Equip better items as you find them
3. Join a guild early for support
4. Save gold for inventory expansion
5. Don't overextend in PvP early

### Mid-Game Strategy
1. Farm bosses for rare items
2. Join a raid guild
3. Focus on attribute allocation
4. Participate in Hell Waves
5. Build reputation with NPCs

### Endgame Strategy
1. Optimize gear with rare affixes
2. Lead raid groups
3. Participate in PvP ranking
4. Control territories with guild
5. Prepare for mythical bosses

---

**Last Updated**: 2026-05-11
**Version**: 1.0
