using UnityEngine;

[CreateAssetMenu(fileName = "InteractionConfig", menuName = "Config/Interaction")]
public class InteractionConfig : ScriptableObject
{
    [field: SerializeField] public float ValveRotateSpeed { get; private set; } = 90f;
    [field: SerializeField] public float ValveReturnSpeed { get; private set; } = 120f;
    [field: SerializeField] public float DoorMoveSpeed { get; private set; } = 2f;
    [field: SerializeField] public float InspectRotateSpeed { get; private set; } = 150f;
}