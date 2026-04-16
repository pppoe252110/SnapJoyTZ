using UniMediator.Runtime;

public struct ItemPickedUpNotification : INotification
{
    public IPickable Item;
}

public struct ItemDroppedNotification : INotification
{
    public IPickable Item;
}

public struct ShowDialogueRequest : IRequest
{
    public DialogueData Dialogue;
}