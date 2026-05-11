const express = require('express');
const router = express.Router();

// Temporary player storage
const players = new Map();

/**
 * Create new character
 */
router.post('/characters', (req, res) => {
    try {
        const { username, characterName } = req.body;

        if (!characterName) {
            return res.status(400).json({ error: 'Character name required' });
        }

        const playerData = {
            playerID: require('crypto').randomUUID(),
            username,
            playerName: characterName,
            level: 1,
            exp: 0,
            strength: 1,
            agility: 1,
            vitality: 1,
            intelligence: 1,
            defense: 1,
            luck: 1,
            maxHP: 100,
            currentHP: 100,
            maxMana: 100,
            currentMana: 100,
            gold: 0,
            currentZone: 'Ashenfall',
            createdAt: new Date()
        };

        players.set(playerData.playerID, playerData);

        console.log(`[PLAYERS] Character created: ${characterName}`);

        res.status(201).json(playerData);
    } catch (error) {
        console.error('[PLAYERS ERROR]', error);
        res.status(500).json({ error: 'Character creation failed' });
    }
});

/**
 * Get player data
 */
router.get('/:playerID', (req, res) => {
    try {
        const { playerID } = req.params;
        const player = players.get(playerID);

        if (!player) {
            return res.status(404).json({ error: 'Player not found' });
        }

        res.json(player);
    } catch (error) {
        res.status(500).json({ error: 'Failed to fetch player data' });
    }
});

/**
 * Update player data
 */
router.put('/:playerID', (req, res) => {
    try {
        const { playerID } = req.params;
        const player = players.get(playerID);

        if (!player) {
            return res.status(404).json({ error: 'Player not found' });
        }

        // Update allowed fields
        const allowedFields = ['level', 'exp', 'strength', 'agility', 'vitality', 
                              'intelligence', 'defense', 'luck', 'currentHP', 
                              'currentMana', 'gold', 'currentZone', 'pkKills'];

        for (const field of allowedFields) {
            if (req.body[field] !== undefined) {
                player[field] = req.body[field];
            }
        }

        players.set(playerID, player);
        console.log(`[PLAYERS] Player updated: ${playerID}`);

        res.json(player);
    } catch (error) {
        res.status(500).json({ error: 'Failed to update player' });
    }
});

/**
 * Get all players (for testing)
 */
router.get('/', (req, res) => {
    try {
        const allPlayers = Array.from(players.values());
        res.json({ count: allPlayers.length, players: allPlayers });
    } catch (error) {
        res.status(500).json({ error: 'Failed to fetch players' });
    }
});

module.exports = router;
