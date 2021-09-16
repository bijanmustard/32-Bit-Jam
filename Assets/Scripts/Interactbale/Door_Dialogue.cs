using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 9/15/21
 * Dialogue for door events.
 */

public class Door_Dialogue : Dialogue
{
    Door_Interactable interactable;

    protected override string filename => "Dialogues/interactable_messages";

    protected override void Awake()
    {
        base.Awake();
        //1. Get ref to interactable script;
        interactable = GetComponent<Door_Interactable>();
        //2. replace variables in rawLines
        for(int i = 0; i < rawLines.Length; i++)
        {
            if(rawLines[i].Contains("<$doorkey>"))
            rawLines[i] = rawLines[i].Replace("<$doorkey>", interactable.requiredItem);
        }
    }

    public override void OnDialogueStart()
    {
        FindObjectOfType<Player>().ToggleInput(false);
    }

    public override void OnDialogueEnd()
    {
        FindObjectOfType<Player>().ToggleInput(true);
        if (!interactable.isLocked) Destroy(interactable.gameObject);
       
    }

    protected override void DEvent0(TextReader reader)
    {
        interactable.isLocked = false;
        
    }
}
