using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using System.Collections;

public class HUDController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _interactionHintText;
    [SerializeField] private Image _heldItemIcon;
    [SerializeField] private GameObject _questCheckbox;
    [SerializeField] private GameObject _questCheckmark;
    [SerializeField] private GameObject _questPanel;
    [SerializeField] private TextMeshProUGUI _questText;
    [SerializeField] private GameObject _inspectPanel;
    [SerializeField] private TextMeshProUGUI _inspectDescriptionText;

    private InteractionService _interactionService;
    private PlayerState _playerState;
    private ItemHolder _itemHolder;

    [Inject]
    public void Construct(
        InteractionService interactionService,
        PlayerState playerState,
        ItemHolder itemHolder)
    {
        _interactionService = interactionService;
        _playerState = playerState;
        _itemHolder = itemHolder;

        _interactionService.OnHoverTextChanged += UpdateHintText;
        _playerState.OnModeChanged += OnPlayerModeChanged;
    }

    private void Start()
    {
        UpdateHintText(string.Empty);
        _heldItemIcon.gameObject.SetActive(false);
        _questCheckbox.SetActive(false);
        _inspectPanel.SetActive(false);
        _questCheckmark.SetActive(false);
        _questPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        if (_interactionService != null)
            _interactionService.OnHoverTextChanged -= UpdateHintText;
        if (_playerState != null)
            _playerState.OnModeChanged -= OnPlayerModeChanged;
    }

    public void UpdateHintText(string text) => _interactionHintText.text = text;

    private void OnPlayerModeChanged(PlayerMode mode)
    {
        if (mode == PlayerMode.Inspecting)
        {
            _inspectPanel.SetActive(true);
            if (_itemHolder.HeldItem != null)
                _inspectDescriptionText.text = _itemHolder.HeldItem.Description;
        }
        else
        {
            _inspectPanel.SetActive(false);
        }
    }

    public void ShowHeldItemIcon(Sprite icon = null)
    {
        _heldItemIcon.gameObject.SetActive(true);
        if (icon != null)
            _heldItemIcon.sprite = icon;
    }
    public void HideHeldItemIcon() => _heldItemIcon.gameObject.SetActive(false);

    public void ShowQuest(string description)
    {
        _questPanel.SetActive(true);
        _questCheckbox.SetActive(true);
        _questCheckmark.SetActive(false);
        _questText.text = description;
    }

    public void CompleteQuest()
    {
        _questCheckmark.SetActive(true);
        StartCoroutine(HideQuestUIAfterDelay(10f));
    }

    private IEnumerator HideQuestUIAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _questCheckbox.SetActive(false);
        _questCheckmark.SetActive(false);
        _questPanel.SetActive(false);
    }
}