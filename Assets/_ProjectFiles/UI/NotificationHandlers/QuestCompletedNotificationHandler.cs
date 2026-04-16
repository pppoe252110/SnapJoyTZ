using UniMediator.Runtime;
using VContainer;

public class QuestCompletedNotificationHandler : INotificationHandler<QuestCompletedNotification>
{
    private readonly HUDController _hud;

    [Inject]
    public QuestCompletedNotificationHandler(HUDController hud)
    {
        _hud = hud;
    }

    public void Handle(QuestCompletedNotification notification)
    {
        _hud.CompleteQuest();
    }
}