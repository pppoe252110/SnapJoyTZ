using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DialogueData", menuName = "Dialogues/DialogueData")]
public class DialogueData : ScriptableObject
{
    public List<DialogueNode> Nodes = new List<DialogueNode>();

    public DialogueNode GetStartNode()
    {
        return Nodes.Count > 0 ? Nodes[0] : null;
    }

    public DialogueNode GetNodeByID(string id)
    {
        return Nodes.Find(n => n.ID == id);
    }
}