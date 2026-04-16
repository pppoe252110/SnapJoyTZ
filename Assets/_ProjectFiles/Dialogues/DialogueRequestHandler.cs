using UniMediator.Runtime;
using VContainer;

public class DialogueRequestHandler : IRequestHandler<ShowDialogueRequest>
{
    private readonly DialogueUIController _dialogueUI;
    private readonly PlayerState _playerState;

    [Inject]
    public DialogueRequestHandler(DialogueUIController dialogueUI, PlayerState playerState)
    {
        _dialogueUI = dialogueUI;
        _playerState = playerState;
    }

    public void Handle(ShowDialogueRequest request)
    {
        _playerState.SetMode(PlayerMode.Dialogue);
        _dialogueUI.ShowDialogue(request.Dialogue, OnDialogueFinished);
    }

    private void OnDialogueFinished()
    {
        _playerState.SetMode(PlayerMode.Free);
    }
}