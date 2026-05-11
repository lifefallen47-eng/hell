-- PostgreSQL Database initialization for Hellfall

-- Create players table
CREATE TABLE IF NOT EXISTS players (
    player_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    username VARCHAR(50) UNIQUE NOT NULL,
    player_name VARCHAR(50) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    
    -- Character progression
    level INT DEFAULT 1,
    exp BIGINT DEFAULT 0,
    total_exp BIGINT DEFAULT 0,
    
    -- Attributes
    strength INT DEFAULT 1,
    agility INT DEFAULT 1,
    vitality INT DEFAULT 1,
    intelligence INT DEFAULT 1,
    defense INT DEFAULT 1,
    luck INT DEFAULT 1,
    free_attributes INT DEFAULT 10,
    
    -- Resources
    max_hp FLOAT DEFAULT 100,
    current_hp FLOAT DEFAULT 100,
    max_mana FLOAT DEFAULT 100,
    current_mana FLOAT DEFAULT 100,
    
    -- Economy
    gold BIGINT DEFAULT 0,
    bank BIGINT DEFAULT 0,
    
    -- PvP
    pk_kills INT DEFAULT 0,
    morality FLOAT DEFAULT 0,
    is_world_threat BOOLEAN DEFAULT FALSE,
    bounty_amount INT DEFAULT 0,
    
    -- Guild
    guild_id UUID,
    guild_role VARCHAR(50),
    
    -- Location
    position_x FLOAT DEFAULT 0,
    position_y FLOAT DEFAULT 0,
    position_z FLOAT DEFAULT 0,
    current_zone VARCHAR(100) DEFAULT 'Ashenfall',
    
    -- Timestamps
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    last_login TIMESTAMP,
    last_logout TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Create guilds table
CREATE TABLE IF NOT EXISTS guilds (
    guild_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    guild_name VARCHAR(100) UNIQUE NOT NULL,
    leader_id UUID NOT NULL REFERENCES players(player_id),
    description TEXT,
    level INT DEFAULT 1,
    treasury BIGINT DEFAULT 0,
    max_members INT DEFAULT 100,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Create items table
CREATE TABLE IF NOT EXISTS items (
    item_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    player_id UUID NOT NULL REFERENCES players(player_id),
    item_name VARCHAR(100) NOT NULL,
    rarity VARCHAR(20) NOT NULL,
    item_slot VARCHAR(50),
    level INT DEFAULT 1,
    damage_bonus FLOAT DEFAULT 0,
    defense_bonus FLOAT DEFAULT 0,
    hp_bonus FLOAT DEFAULT 0,
    mana_bonus FLOAT DEFAULT 0,
    is_bound BOOLEAN DEFAULT FALSE,
    item_value BIGINT DEFAULT 0,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Create quests table
CREATE TABLE IF NOT EXISTS quests (
    quest_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    player_id UUID NOT NULL REFERENCES players(player_id),
    quest_name VARCHAR(100) NOT NULL,
    description TEXT,
    quest_type VARCHAR(50),
    required_level INT DEFAULT 1,
    is_repeatable BOOLEAN DEFAULT FALSE,
    is_completed BOOLEAN DEFAULT FALSE,
    progress INT DEFAULT 0,
    reward_exp BIGINT DEFAULT 0,
    reward_gold BIGINT DEFAULT 0,
    accepted_at TIMESTAMP,
    completed_at TIMESTAMP
);

-- Create world bosses table
CREATE TABLE IF NOT EXISTS world_bosses (
    boss_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    boss_name VARCHAR(100) NOT NULL,
    level INT NOT NULL,
    current_health FLOAT NOT NULL,
    max_health FLOAT NOT NULL,
    position_x FLOAT,
    position_y FLOAT,
    position_z FLOAT,
    zone VARCHAR(100),
    last_defeated_by UUID REFERENCES players(player_id),
    last_defeated_at TIMESTAMP,
    respawn_time INT,
    is_alive BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Create indexes for performance
CREATE INDEX idx_players_username ON players(username);
CREATE INDEX idx_players_level ON players(level);
CREATE INDEX idx_players_zone ON players(current_zone);
CREATE INDEX idx_guilds_leader ON guilds(leader_id);
CREATE INDEX idx_items_player ON items(player_id);
CREATE INDEX idx_quests_player ON quests(player_id);
CREATE INDEX idx_world_bosses_zone ON world_bosses(zone);

-- Add foreign key constraint for guild members
ALTER TABLE players 
ADD CONSTRAINT fk_guild_id 
FOREIGN KEY (guild_id) REFERENCES guilds(guild_id) ON DELETE SET NULL;

CREATE TABLE IF NOT EXISTS guild_members (
    guild_id UUID NOT NULL REFERENCES guilds(guild_id) ON DELETE CASCADE,
    player_id UUID NOT NULL REFERENCES players(player_id) ON DELETE CASCADE,
    role VARCHAR(50) NOT NULL,
    joined_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (guild_id, player_id)
);

CREATE TABLE IF NOT EXISTS territories (
    territory_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    territory_name VARCHAR(100) NOT NULL,
    guild_id UUID REFERENCES guilds(guild_id) ON DELETE SET NULL,
    zone VARCHAR(100) NOT NULL,
    position_x FLOAT,
    position_y FLOAT,
    position_z FLOAT,
    radius FLOAT,
    captured_at TIMESTAMP,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS trading_logs (
    log_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    from_player UUID NOT NULL REFERENCES players(player_id),
    to_player UUID NOT NULL REFERENCES players(player_id),
    item_id UUID REFERENCES items(item_id),
    gold_amount BIGINT DEFAULT 0,
    transaction_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
