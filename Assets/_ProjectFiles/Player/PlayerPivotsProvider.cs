using UnityEngine;

public class PlayerPivotsProvider : MonoBehaviour, IItemHolderPivots
{
    [SerializeField] private Transform _handPivot;
    [SerializeField] private Transform _inspectPivot;

    public Transform HandPivot => _handPivot;
    public Transform InspectPivot => _inspectPivot;
}