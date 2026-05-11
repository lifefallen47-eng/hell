const express = require('express');
const router = express.Router();

const worldData = {
    worldLevel: 1,
    activeHellWave: 0,
    zones: [
        {
            zoneName: 'Ashenfall',
            minLevel: 1,
            maxLevel: 20,
            dangerLevel: 1.0,
            playerCount: 0
        },
        {
            zoneName: 'Darkwood Forest',
            minLevel: 15,
            maxLevel: 35,
            dangerLevel: 1.5,
            playerCount: 0
        },
        {
            zoneName: 'Infernal Wastes',
            minLevel: 40,
            maxLevel: 70,
            dangerLevel: 2.0,
            playerCount: 0
        },
        {
            zoneName: 'Eternal Abyss',
            minLevel: 75,
            maxLevel: 150,
            dangerLevel: 3.0,
            playerCount: 0
        }
    ]
};

/**
 * Get world status
 */
router.get('/status', (req, res) => {
    res.json(worldData);
});

/**
 * Get zones
 */
router.get('/zones', (req, res) => {
    res.json(worldData.zones);
});

/**
 * Get zone by name
 */
router.get('/zones/:zoneName', (req, res) => {
    const zone = worldData.zones.find(z => z.zoneName === req.params.zoneName);
    if (!zone) {
        return res.status(404).json({ error: 'Zone not found' });
    }
    res.json(zone);
});

/**
 * Update world level
 */
router.post('/level-up', (req, res) => {
    worldData.worldLevel++;
    console.log(`[WORLD] World leveled up to ${worldData.worldLevel}`);
    res.json({ worldLevel: worldData.worldLevel });
});

/**
 * Start hell wave
 */
router.post('/hell-wave/start', (req, res) => {
    worldData.activeHellWave++;
    console.log(`[HELL WAVE] Wave ${worldData.activeHellWave} started`);
    res.json({ waveNumber: worldData.activeHellWave });
});

/**
 * End hell wave
 */
router.post('/hell-wave/end', (req, res) => {
    worldData.activeHellWave = 0;
    console.log(`[HELL WAVE] Wave ended`);
    res.json({ message: 'Hell wave ended' });
});

module.exports = router;
