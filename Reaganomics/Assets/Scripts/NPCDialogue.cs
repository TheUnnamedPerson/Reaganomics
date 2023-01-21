using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour, System.IComparable<NPCDialogue>
{
    public int id;
    public string npcName;
    public Dialogue currentDialogue;
    public DialogueManager dialogueManager;
    public Player player;

    void Start ()
    {
        player = GameObject.FindObjectOfType<Player>();
    }

    public void StartDialogue ()
    {
        player.inPrompt = true;
        dialogueManager.PlayDialogue(currentDialogue);
    }

    public int CompareTo(NPCDialogue x)
    {
        // A null value means that this object is greater.
        if (x == null)
            return 1;

        else
            return this.id.CompareTo(x.id);
    }

}

