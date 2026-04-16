using UnityEngine;
using VContainer;

public class FirstPersonCamera : MonoBehaviour
{
    public Camera Camera => _playerCamera;

    [SerializeField] private Camera _playerCamera;
    [SerializeField] private Transform _playerBody;
    [SerializeField] private float _pitchMin = -90f;
    [SerializeField] private float _pitchMax = 90f;

    private PlayerConfig _config;
    private PlayerState _playerState;
    private PlayerInputActions _playerInput;

    private float _yaw;
    private float _pitch;

    [Inject]
    public void Construct(PlayerConfig config, PlayerState playerState, PlayerInputActions playerInput)
    {
        _config = config;
        _playerState = playerState;
        _playerInput = playerInput;
    }
        
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (_playerState.IsInputBlocked)
            return;

        Vector2 look = _playerInput.Player.Look.ReadValue<Vector2>();

        float mouseX = look.x * _config.MouseSensitivity;
        float mouseY = look.y * _config.MouseSensitivity;

        _yaw += mouseX;
        _pitch -= mouseY;
        _pitch = Mathf.Clamp(_pitch, _pitchMin, _pitchMax);

        transform.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
        _playerBody.rotation = Quaternion.Euler(0f, _yaw, 0f);
    }
}