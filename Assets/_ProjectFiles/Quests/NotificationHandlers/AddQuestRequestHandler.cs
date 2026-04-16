using UniMediator.Runtime;
using VContainer;

public class AddQuestRequestHandler : IRequestHandler<AddQuestRequest>
{
    private readonly QuestManager _questManager;

    [Inject]
    public AddQuestRequestHandler(QuestManager questManager)
    {
        _questManager = questManager;
    }

    public void Handle(AddQuestRequest request)
    {
        _questManager.AddQuest(request.Quest);
    }
}