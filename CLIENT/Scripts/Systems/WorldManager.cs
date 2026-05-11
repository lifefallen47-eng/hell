using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// World management system
/// Handles zones, dynamic scaling, and world events
/// </summary>
public class WorldManager : MonoBehaviour
{
    [System.Serializable]
    public class Zone
    {
        public string zoneName;
        public int minLevel;
        public int maxLevel;
        public float dangerLevel = 1f; // Scaling multiplier
        public Vector3 center;
        public float radius;
        public List<string> npcs = new List<string>();
        public List<string> bosses = new List<string>();
    }

    public int worldLevel = 1;
    public int averagePlayerLevel = 1;

    public List<Zone> zones = new List<Zone>();
    
    private float hellWaveTimer = 0f;
    private const float HELL_WAVE_INTERVAL = 7200f; // 2 hours in seconds

    void Start()
    {
        InitializeZones();
    }

    void Update()
    {
        // Update world level based on average player level
        UpdateWorldLevel();

        // Track hell waves
        UpdateHellWaves();
    }

    /// <summary>
    /// Initialize all zones in the world
    /// </summary>
    private void InitializeZones()
    {
        zones.Add(new Zone
        {
            zoneName = "Ashenfall",
            minLevel = 1,
            maxLevel = 20,
            dangerLevel = 1f,
            center = Vector3.zero,
            radius = 500f
        });

        zones.Add(new Zone
        {
            zoneName = "Darkwood Forest",
            minLevel = 15,
            maxLevel = 35,
            dangerLevel = 1.5f,
            center = new Vector3(1000, 0, 0),
            radius = 750f
        });

        zones.Add(new Zone
        {
            zoneName = "Infernal Wastes",
            minLevel = 40,
            maxLevel = 70,
            dangerLevel = 2f,
            center = new Vector3(0, 0, 1500),
            radius = 1000f
        });

        zones.Add(new Zone
        {
            zoneName = "Eternal Abyss",
            minLevel = 75,
            maxLevel = 150,
            dangerLevel = 3f,
            center = new Vector3(-1500, 0, 0),
            radius = 1500f
        });

        Debug.Log($"[WORLD] Initialized {zones.Count} zones");
    }

    /// <summary>
    /// Update world level based on average player level
    /// </summary>
    private void UpdateWorldLevel()
    {
        // Calculate average from all connected players
        // This would be done on server in production
        
        int newWorldLevel = averagePlayerLevel;
        
        if (newWorldLevel != worldLevel)
        {
            worldLevel = newWorldLevel;
            ScaleWorld();
        }
    }

    /// <summary>
    /// Scale all enemies and bosses based on world level
    /// </summary>
    private void ScaleWorld()
    {
        Debug.Log($"[WORLD] World level increased to {worldLevel}");

        foreach (Zone zone in zones)
        {
            zone.dangerLevel = 1f + ((worldLevel - 1) * 0.1f);
            Debug.Log($"[ZONE] {zone.zoneName} danger level: {zone.dangerLevel}x");
        }
    }

    /// <summary>
    /// Get zone by name
    /// </summary>
    public Zone GetZone(string zoneName)
    {
        return zones.Find(z => z.zoneName == zoneName);
    }

    /// <summary>
    /// Get appropriate zone for player level
    /// </summary>
    public Zone GetZoneForLevel(int playerLevel)
    {
        foreach (Zone zone in zones)
        {
            if (playerLevel >= zone.minLevel && playerLevel <= zone.maxLevel)
            {
                return zone;
            }
        }

        // Return highest zone if player is too high level
        return zones[zones.Count - 1];
    }

    /// <summary>
    /// Update hell wave timer
    /// </summary>
    private void UpdateHellWaves()
    {
        hellWaveTimer -= Time.deltaTime;

        if (hellWaveTimer <= 0)
        {
            StartHellWave();
            hellWaveTimer = HELL_WAVE_INTERVAL;
        }
    }

    /// <summary>
    /// Start a hell wave event
    /// </summary>
    private void StartHellWave()
    {
        Debug.Log("[HELL WAVE] A hell wave is beginning!");
        // Spawn demons, start event timer, etc
    }

    /// <summary>
    /// Get all zones sorted by level requirement
    /// </summary>
    public List<Zone> GetZonesSortedByLevel()
    {
        List<Zone> sorted = new List<Zone>(zones);
        sorted.Sort((a, b) => a.minLevel.CompareTo(b.minLevel));
        return sorted;
    }

    /// <summary>
    /// Check if player is in a PvP zone
    /// </summary>
    public bool IsPvPZone(string zoneName)
    {
        // Specific zones allow PvP
        return zoneName == "Infernal Wastes" || zoneName == "Eternal Abyss";
    }

    /// <summary>
    /// Get world status
    /// </summary>
    public string GetWorldStatus()
    {
        return $@"
=== WORLD STATUS ===
World Level: {worldLevel}
Zones: {zones.Count}
Hell Wave In: {hellWaveTimer / 60:F1} minutes

=== ACTIVE ZONES ===
{string.Join("\n", zones.ConvertAll(z => $"{z.zoneName} (Level {z.minLevel}-{z.maxLevel}, Danger: {z.dangerLevel}x)"))}
";
    }
}

/// <summary>
/// NPC Manager for handling all NPCs in world
/// </summary>
public class NPCManager : MonoBehaviour
{
    [System.Serializable]
    public class NPC
    {
        public string npcID;
        public string npcName;
        public string role;
        public Vector3 position;
        public string[] dialogues;
        public Dictionary<string, string> memories = new Dictionary<string, string>();
    }

    private Dictionary<string, NPC> npcs = new Dictionary<string, NPC>();

    /// <summary>
    /// Register an NPC
    /// </summary>
    public void RegisterNPC(NPC npc)
    {
        npcs[npc.npcID] = npc;
        Debug.Log($"[NPC] Registered {npc.npcName}");
    }

    /// <summary>
    /// Get NPC by ID
    /// </summary>
    public NPC GetNPC(string npcID)
    {
        if (npcs.ContainsKey(npcID))
            return npcs[npcID];
        return null;
    }

    /// <summary>
    /// Store memory of player interaction
    /// </summary>
    public void RememberInteraction(string npcID, string playerName, string memory)
    {
        if (npcs.ContainsKey(npcID))
        {
            npcs[npcID].memories[playerName] = memory;
            Debug.Log($"[NPC MEMORY] {npcs[npcID].npcName} remembers: {memory}");
        }
    }

    /// <summary>
    /// Get memory of player
    /// </summary>
    public string RecallMemory(string npcID, string playerName)
    {
        if (npcs.ContainsKey(npcID) && npcs[npcID].memories.ContainsKey(playerName))
        {
            return npcs[npcID].memories[playerName];
        }
        return "I don't know you.";
    }
}
