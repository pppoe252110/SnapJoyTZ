using UniMediator.Runtime;

public struct AddQuestRequest : IRequest
{
    public QuestData Quest;
}

public struct QuestAddedNotification : INotification
{
    public QuestData Quest;
}

public struct QuestCompletedNotification : INotification
{
    public QuestData Quest;
}