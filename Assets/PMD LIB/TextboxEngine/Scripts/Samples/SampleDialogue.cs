using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleDialogue : Dialogue
{

    protected override string filename
    {
        get { return "Dialogues/sample"; }
    }
    protected override void DEvent0(TextReader reader)
    {
        //Move line pointer to line 7
        reader.SetLine(9);
        Debug.Log("event");
        
    }



}
