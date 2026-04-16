using UniMediator.Runtime;
using VContainer;

public class ItemDroppedNotificationHandler : INotificationHandler<ItemDroppedNotification>
{
    private readonly HUDController _hud;

    [Inject]
    public ItemDroppedNotificationHandler(HUDController hud)
    {
        _hud = hud;
    }

    public void Handle(ItemDroppedNotification notification)
    {
        _hud.HideHeldItemIcon();
    }
}