using UniMediator.Runtime;
using UnityEngine;
using VContainer;

public class QuestAddedNotificationHandler : INotificationHandler<QuestAddedNotification>
{
    private readonly HUDController _hud;

    [Inject]
    public QuestAddedNotificationHandler(HUDController hud)
    {
        _hud = hud;
    }

    public void Handle(QuestAddedNotification notification)
    {
        _hud.ShowQuest(notification.Quest.Description);
    }
}