using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "Quests/Quest Data")]
public class QuestData : ScriptableObject
{
    public string Id;
    public string Title;
    [TextArea] public string Description;
    public bool IsCompleted;

    public GameObject ItemToSpawn;

    public string RequiredItemId;
}