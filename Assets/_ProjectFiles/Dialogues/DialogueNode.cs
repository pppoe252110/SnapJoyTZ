using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueNode
{
    public string ID;
    public string SpeakerName;
    [TextArea(3, 10)] public string Text;

    public List<DialogueOption> Options = new List<DialogueOption>();

    public DialogueAction OnEnterAction;
}

[Serializable]
public class DialogueOption
{
    public string Text;
    public string NextNodeID;

    public Condition Condition;

    public DialogueAction OnSelectAction;
}