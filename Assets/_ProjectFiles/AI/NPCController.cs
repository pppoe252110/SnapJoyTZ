using System;
using UniMediator.Runtime;
using UnityEngine;
using VContainer;

public class NPCController : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueData _dialogueData;
    [SerializeField] private DialogueData _questCompleteDialogue;
    [SerializeField] private DialogueData _questCompletedDialogue;
    [SerializeField] private QuestData _npcQuest;

    private IMediator _mediator;
    private QuestManager _questManager;
    private ItemHolder _itemHolder;

    [Inject]
    public void Construct(IMediator mediator, QuestManager questManager, ItemHolder itemHolder)
    {
        _mediator = mediator;
        _questManager = questManager;
        _itemHolder = itemHolder;
    }

    public void Interact(GameObject instigator)
    {
        var activeQuest = _questManager.GetActiveQuestWithItemRequirement();

        if (activeQuest != null &&
            !string.IsNullOrEmpty(activeQuest.RequiredItemId) &&
            _itemHolder.HeldItem != null &&
            string.Equals(_itemHolder.HeldItem.Id, activeQuest.RequiredItemId, StringComparison.OrdinalIgnoreCase))
        {
            _questManager.CompleteQuest(activeQuest);

            Destroy(_itemHolder.HeldItem.Transform.gameObject);
            _itemHolder.ClearHeldItem();

            if (_questCompleteDialogue != null)
                _mediator.Send(new ShowDialogueRequest { Dialogue = _questCompleteDialogue });
            else
                _mediator.Send(new ShowDialogueRequest { Dialogue = _dialogueData });
            return;
        }

        if (_questManager.IsQuestCompleted(_npcQuest.Id))
            _mediator.Send(new ShowDialogueRequest { Dialogue = _questCompletedDialogue });
        else
            _mediator.Send(new ShowDialogueRequest { Dialogue = _dialogueData });
    }

    public string GetHoverText(GameObject instigator) => "E - поговорить";
    public void OnHoverEnter() { }
    public void OnHoverExit() { }
}