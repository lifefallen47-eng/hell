using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Quest types
/// </summary>
public enum QuestType
{
    Kill,
    Collect,
    Explore,
    Story,
    Raid
}

/// <summary>
/// Quest data structure
/// </summary>
[System.Serializable]
public class Quest
{
    public string questID;
    public string questName;
    public string description;
    public QuestType questType;

    public int requiredLevel = 1;
    public bool isRepeatable = false;
    public bool isCompleted = false;

    public int targetKills = 0;
    public int currentKills = 0;

    public List<string> locations = new List<string>();

    public long rewardExp = 0;
    public long rewardGold = 0;
    public List<Item> rewardItems = new List<Item>();

    public Quest(string name, QuestType type)
    {
        questID = System.Guid.NewGuid().ToString();
        questName = name;
        questType = type;
    }

    /// <summary>
    /// Get quest progress (0-1)
    /// </summary>
    public float GetProgress()
    {
        if (questType == QuestType.Kill)
            return (float)currentKills / targetKills;
        return 0;
    }

    /// <summary>
    /// Check if quest is completed
    /// </summary>
    public bool IsComplete()
    {
        return questType switch
        {
            QuestType.Kill => currentKills >= targetKills,
            QuestType.Collect => currentKills >= targetKills, // Reusing for collection count
            _ => isCompleted
        };
    }
}

/// <summary>
/// Quest system manager
/// </summary>
public class QuestSystem : MonoBehaviour
{
    private PlayerData playerData;
    private List<Quest> activeQuests = new List<Quest>();
    private List<Quest> completedQuests = new List<Quest>();

    void Start()
    {
        playerData = GetComponent<PlayerData>();
    }

    /// <summary>
    /// Accept a quest
    /// </summary>
    public bool AcceptQuest(Quest quest)
    {
        // Check level requirement
        if (playerData.level < quest.requiredLevel)
        {
            Debug.LogWarning($"[QUEST] Level {quest.requiredLevel} required");
            return false;
        }

        // Check if already accepted
        if (activeQuests.Any(q => q.questID == quest.questID))
        {
            Debug.LogWarning($"[QUEST] Already accepted this quest");
            return false;
        }

        activeQuests.Add(quest);
        Debug.Log($"[QUEST] Accepted: {quest.questName}");
        return true;
    }

    /// <summary>
    /// Progress a kill quest
    /// </summary>
    public void ProgressKillQuest(string questName, int amount = 1)
    {
        Quest quest = activeQuests.FirstOrDefault(q => q.questName == questName && q.questType == QuestType.Kill);
        if (quest == null)
            return;

        quest.currentKills += amount;
        Debug.Log($"[QUEST] {quest.questName}: {quest.currentKills} / {quest.targetKills}");

        if (quest.IsComplete())
            CompleteQuest(quest);
    }

    /// <summary>
    /// Complete a quest
    /// </summary>
    public void CompleteQuest(Quest quest)
    {
        if (!activeQuests.Contains(quest))
            return;

        activeQuests.Remove(quest);
        completedQuests.Add(quest);

        // Award rewards
        playerData.gold += quest.rewardGold;
        GetComponent<LevelingSystem>().GainEXP(quest.rewardExp);

        foreach (Item item in quest.rewardItems)
        {
            GetComponent<InventorySystem>().AddItem(item);
        }

        Debug.Log($"[QUEST] Completed: {quest.questName}");
        Debug.Log($"[QUEST] Rewards: {quest.rewardExp} EXP, {quest.rewardGold} gold");
    }

    /// <summary>
    /// Abandon a quest
    /// </summary>
    public bool AbandonQuest(string questID)
    {
        Quest quest = activeQuests.FirstOrDefault(q => q.questID == questID);
        if (quest == null)
            return false;

        activeQuests.Remove(quest);
        Debug.Log($"[QUEST] Abandoned: {quest.questName}");
        return true;
    }

    /// <summary>
    /// Get active quest list
    /// </summary>
    public List<Quest> GetActiveQuests()
    {
        return activeQuests;
    }

    /// <summary>
    /// Get quest status
    /// </summary>
    public string GetQuestStatus()
    {
        string status = "=== ACTIVE QUESTS ===\n";
        foreach (Quest quest in activeQuests)
        {
            status += $"{quest.questName}: {(quest.GetProgress() * 100):F0}%\n";
        }
        return status;
    }
}
