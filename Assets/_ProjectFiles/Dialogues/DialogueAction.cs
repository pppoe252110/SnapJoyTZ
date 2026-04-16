using System;

[Serializable]
public class DialogueAction
{
    public enum ActionType { None, AddQuest, CompleteQuest, CloseDialogue }
    public ActionType Type;
    public string TargetID;
    public QuestData Quest;
    public string Parameter;
}

[Serializable]
public class Condition
{
    public enum ConditionType { None, HasQuest, QuestCompleted, HasItem }
    public ConditionType Type;
    public string TargetID;
    public bool Invert;
}