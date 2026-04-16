using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using UniMediator.Runtime;
using VContainer;
using TMPro;

public class DialogueUIController : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI _speakerNameText;
    [SerializeField] private TextMeshProUGUI _npcText;
    [SerializeField] private Transform _choicesContainer;
    [SerializeField] private Button _choiceButtonPrefab;

    private IMediator _mediator;
    private QuestManager _questManager;
    private DialogueData _currentDialogue;
    private Action _onComplete;

    [Inject]
    public void Construct(IMediator mediator, QuestManager questManager)
    {
        _mediator = mediator;
        _questManager = questManager;
    }

    private void Start()
    {
        HideDialogue();
    }

    public void ShowDialogue(DialogueData dialogue, Action onComplete)
    {
        _currentDialogue = dialogue;
        _onComplete = onComplete;
        _panel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GoToNode(dialogue.GetStartNode());
    }

    private void GoToNode(DialogueNode node)
    {
        if (node == null)
        {
            CloseDialogue();
            return;
        }

        ExecuteAction(node.OnEnterAction);

        _speakerNameText.text = string.IsNullOrEmpty(node.SpeakerName) ? "NPC" : node.SpeakerName;
        _npcText.text = node.Text;

        foreach (Transform child in _choicesContainer)
            Destroy(child.gameObject);

        bool anyOptionAdded = false;

        if (node.Options != null && node.Options.Count > 0)
        {
            foreach (var option in node.Options)
            {
                if (!CheckCondition(option.Condition))
                    continue;

                var button = Instantiate(_choiceButtonPrefab, _choicesContainer);
                button.GetComponentInChildren<TextMeshProUGUI>().text = option.Text;
                button.onClick.AddListener(() => OnOptionSelected(option));
                anyOptionAdded = true;
            }
        }

        if (!anyOptionAdded)
        {
            var button = Instantiate(_choiceButtonPrefab, _choicesContainer);
            button.GetComponentInChildren<TextMeshProUGUI>().text = "[Продолжить]";
            button.onClick.AddListener(() => CloseDialogue());
        }
    }

    private void OnOptionSelected(DialogueOption option)
    {
        ExecuteAction(option.OnSelectAction);

        if (string.IsNullOrEmpty(option.NextNodeID))
        {
            CloseDialogue();
        }
        else
        {
            var nextNode = _currentDialogue.GetNodeByID(option.NextNodeID);
            GoToNode(nextNode);
        }
    }

    private void ExecuteAction(DialogueAction action)
    {
        if (action == null) return;
        switch (action.Type)
        {
            case DialogueAction.ActionType.AddQuest:
                QuestData questInstance = Instantiate(action.Quest);

                if (string.IsNullOrEmpty(questInstance.RequiredItemId))
                {
                    var availableItems = FindObjectsByType<PickableItem>(FindObjectsSortMode.None)
                        .Where(item => !string.Equals(item.Id, "key", StringComparison.OrdinalIgnoreCase)
                                       && !string.Equals(item.Id, "note", StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    if (availableItems.Count > 0)
                    {
                        var randomItem = availableItems[UnityEngine.Random.Range(0, availableItems.Count)];
                        questInstance.RequiredItemId = randomItem.Id;
                        questInstance.Description = $"Принести {randomItem.ItemName}";
                    }
                    else
                    {
                        return;
                    }
                }

                _mediator.Send(new AddQuestRequest { Quest = questInstance });
                break;

            case DialogueAction.ActionType.CompleteQuest:
                if (action.Quest != null)
                    _mediator.Publish(new QuestCompletedNotification { Quest = action.Quest });
                break;

            case DialogueAction.ActionType.CloseDialogue:
                CloseDialogue();
                break;
        }
    }
    private bool CheckCondition(Condition cond)
    {
        if (cond == null || cond.Type == Condition.ConditionType.None)
            return true;

        bool result = false;
        switch (cond.Type)
        {
            case Condition.ConditionType.HasQuest:
                result = _questManager.IsQuestActive(cond.TargetID);
                break;

            case Condition.ConditionType.QuestCompleted:
                result = _questManager.IsQuestCompleted(cond.TargetID);
                break;

            case Condition.ConditionType.HasItem:
                break;
        }
        return cond.Invert ? !result : result;
    }

    public void HideDialogue()
    {
        _panel.SetActive(false);
    }

    private void CloseDialogue()
    {
        HideDialogue();
        _onComplete?.Invoke();
        _onComplete = null;
    }
}