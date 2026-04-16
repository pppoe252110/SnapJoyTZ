using System;
using System.Collections.Generic;
using UniMediator.Runtime;

public class QuestManager : IDisposable
{
    private readonly IMediator _mediator;
    private readonly List<QuestData> _activeQuests = new();
    private readonly HashSet<string> _completedQuestIds = new();
    public QuestManager(IMediator mediator)
    {
        _mediator = mediator;
    }

    public void AddQuest(QuestData quest)
    {
        if (_activeQuests.Contains(quest)) return;
        _activeQuests.Add(quest);
        _mediator.Publish(new QuestAddedNotification { Quest = quest });

        if (quest.ItemToSpawn != null)
            quest.ItemToSpawn.SetActive(true);
    }

    public void CompleteQuest(QuestData quest)
    {
        if (_activeQuests.Contains(quest))
        {
            quest.IsCompleted = true;
            _activeQuests.Remove(quest);
            _completedQuestIds.Add(quest.Id);
            _mediator.Publish(new QuestCompletedNotification { Quest = quest });

            if (quest.ItemToSpawn != null)
                quest.ItemToSpawn.SetActive(false);

            UnityEngine.Object.Destroy(quest);
        }
    }

    public bool IsQuestCompleted(string questId) => _completedQuestIds.Contains(questId);

    public bool IsQuestActive(string questId) => _activeQuests.Exists(q => q.Id == questId);

    public QuestData GetActiveQuestWithItemRequirement()
    {
        foreach (var quest in _activeQuests)
        {
            if (!string.IsNullOrEmpty(quest.RequiredItemId))
                return quest;
        }
        return null;
    }

    public void Dispose()
    {
        _activeQuests.Clear();
    }
}