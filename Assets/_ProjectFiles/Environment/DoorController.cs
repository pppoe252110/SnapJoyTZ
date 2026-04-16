using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Vector3 _closedPosition;
    [SerializeField] private Vector3 _openOffset = new Vector3(0, 3, 0);
    [SerializeField] private float _smoothTime = 0.1f;

    private Vector3 _targetPosition;
    private Vector3 _velocity;

    private void Start()
    {
        _closedPosition = transform.position;
        _targetPosition = _closedPosition;
    }

    public void SetOpenProgress(float progress)
    {
        _targetPosition = _closedPosition + _openOffset * progress;
    }

    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, _targetPosition, ref _velocity, _smoothTime);
    }
}