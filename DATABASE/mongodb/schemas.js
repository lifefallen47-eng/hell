/**
 * MongoDB Schemas for Hellfall
 * Note: These would be defined using Mongoose in production
 */

const playerSchema = {
    username: String,
    playerName: String,
    email: String,
    password: String, // Hashed
    
    // Character data
    level: Number,
    exp: Number,
    totalExp: Number,
    
    // Attributes
    strength: Number,
    agility: Number,
    vitality: Number,
    intelligence: Number,
    defense: Number,
    luck: Number,
    
    // Resources
    maxHP: Number,
    currentHP: Number,
    maxMana: Number,
    currentMana: Number,
    
    // Economy
    gold: Number,
    bank: Number,
    
    // PvP
    pkKills: Number,
    morality: Number,
    isWorldThreat: Boolean,
    bountyAmount: Number,
    
    // Guild
    guildID: String,
    guildRole: String,
    
    // Inventory
    inventory: [Object], // Array of items
    equipment: [Object], // Array of equipped items
    
    // Location
    position: Object, // {x, y, z}
    currentZone: String,
    
    // Timestamps
    createdAt: Date,
    lastLogin: Date,
    lastLogout: Date
};

const guildSchema = {
    guildID: String,
    guildName: String,
    leader: String,
    description: String,
    level: Number,
    treasury: Number,
    maxMembers: Number,
    members: [String], // Array of player IDs
    territories: [String], // Controlled zones
    createdAt: Date
};

const itemSchema = {
    itemID: String,
    itemName: String,
    rarity: String, // Common, Rare, Epic, Legendary, etc
    slot: String, // Head, Chest, MainHand, etc
    level: Number,
    
    // Stats
    damageBonus: Number,
    defenseBonus: Number,
    hpBonus: Number,
    manaBonus: Number,
    
    // Affixes
    affixes: [String],
    isBound: Boolean,
    
    // Trading
    value: Number,
    canTrade: Boolean
};

const worldBossSchema = {
    bossID: String,
    bossName: String,
    level: Number,
    health: Number,
    maxHealth: Number,
    location: Object, // {x, y, z}
    zone: String,
    lastDefeatedBy: String,
    lastDefeatedAt: Date,
    respawnTime: Number, // seconds
    isAlive: Boolean
};

const questSchema = {
    questID: String,
    questName: String,
    description: String,
    questType: String, // Kill, Collect, Explore, Story
    requiredLevel: Number,
    isRepeatable: Boolean,
    
    // Objectives
    targetKills: Number,
    locations: [String],
    
    // Rewards
    rewardExp: Number,
    rewardGold: Number,
    rewardItems: [Object]
};

const dungeon Schema = {
    dungeonID: String,
    dungeonName: String,
    level: Number,
    difficulty: String, // Normal, Hard, Mythic
    zone: String,
    
    // Boss encounters
    bosses: [Object],
    
    // Rewards
    lootTable: [Object],
    
    // State
    currentChallenges: Number,
    isActive: Boolean
};

module.exports = {
    playerSchema,
    guildSchema,
    itemSchema,
    worldBossSchema,
    questSchema,
    dungeonSchema
};
