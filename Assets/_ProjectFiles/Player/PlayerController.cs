using UnityEngine;
using VContainer;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Transform _cameraTransform;

    private PlayerInputActions _playerInput;
    private PlayerConfig _config;
    private PlayerState _playerState;

    [Inject]
    public void Construct(PlayerInputActions playerInput, PlayerConfig playerConfig, PlayerState playerState)
    {
        _playerInput = playerInput;
        _config = playerConfig;
        _playerState = playerState;
    }

    private void Update()
    {
        if (_playerState.IsInputBlocked)
        {
            return;
        }
        Vector2 playerMove = _playerInput.Player.Move.ReadValue<Vector2>();

        Vector3 move = new Vector3(playerMove.x, 0, playerMove.y);
        move.Normalize();

        move = Quaternion.Euler(0, _characterController.transform.eulerAngles.y, 0) * move;

        _characterController.SimpleMove(move * _config.WalkSpeed);
    }
}