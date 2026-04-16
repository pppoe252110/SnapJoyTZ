using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Config/Player")]
public class PlayerConfig : ScriptableObject
{
    [field: SerializeField] public float WalkSpeed { get; private set; } = 5f;
    [field: SerializeField] public float MouseSensitivity { get; private set; } = 2f;
    [field: SerializeField] public float InteractionRayDistance { get; private set; } = 3f;
    [field: SerializeField] public float InspectRotationSpeed { get; private set; } = 30f;
    [field: SerializeField] public LayerMask InteractionLayers { get; private set; } = -1;
}