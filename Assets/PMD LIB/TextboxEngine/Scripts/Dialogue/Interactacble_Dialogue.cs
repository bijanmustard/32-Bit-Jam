using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 9/15/21
 * Script for interactable dialogue messages/events. To be derived from.
 */

public class Interactacble_Dialogue : Dialogue
{
    protected override string filename => "Dialogues/interactable_messages";

    protected override void Awake()
    {
        base.Awake();
        //1. Replace variables in file with local vars
        for(int i = 0; i < rawLines.Length; i++)
        {
            if (rawLines[i].Contains("<$name>"))
            {
                rawLines[i] = rawLines[i].Replace("<$name>", name);
            }
        }
    }

    protected override void DEvent0(TextReader reader)
    {
        
        reader.SetLine(3);
    }

    public override void OnDialogueStart()
    {
       
        FindObjectOfType<Player>().ToggleInput(false);
    }

    public override void OnDialogueEnd()
    {
        FindObjectOfType<Player>().ToggleInput(true);
        Destroy(gameObject);
        
    }
}
