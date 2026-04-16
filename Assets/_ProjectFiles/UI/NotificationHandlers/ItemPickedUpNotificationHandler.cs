using UniMediator.Runtime;
using VContainer;

public class ItemPickedUpNotificationHandler : INotificationHandler<ItemPickedUpNotification>
{
    private readonly HUDController _hud;

    [Inject]
    public ItemPickedUpNotificationHandler(HUDController hud)
    {
        _hud = hud;
    }

    public void Handle(ItemPickedUpNotification notification)
    {
        _hud.ShowHeldItemIcon(notification.Item.Icon);
    }
}