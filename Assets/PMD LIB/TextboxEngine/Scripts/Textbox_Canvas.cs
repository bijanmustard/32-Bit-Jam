using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 9/23/21
 * Script for Textbox_Canvas, used by TextboxManager in TextboxEngine.
 */

public class Textbox_Canvas : MonoBehaviour
{
    Textbox[] myTBXs;

    private void Start()
    {
        myTBXs = GetComponentsInChildren<Textbox>();
        foreach(Textbox t in myTBXs)
        {
            
        }
    }

}
