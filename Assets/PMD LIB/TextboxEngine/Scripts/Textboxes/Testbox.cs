using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testbox : Textbox_Main
{

    protected override void Awake()
    {
        base.Awake();
        //Set this to main textbox
        TextboxManager.SetMainTextbox(this);
    }

 

    public override void OnSubtitleSet()
    {
        
    }

    public override void OnTitleSet()
    {
        
    }

    public override void OnDisplayOptions()
    {
        
    }
}
