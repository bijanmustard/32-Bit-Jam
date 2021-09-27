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

    //Singleton refs
    private static Textbox_Canvas instance;
    //All current textboxes
    Textbox[] myTBXs => GetComponentsInChildren<Textbox>();

    //DEBUG
    static bool txb_spawn = false;
    static bool dbg_tog = false;

    private void Awake()
    {
        //1. Set singleton
        if (instance != null && instance != this) Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //Set output cam
        //TextboxManager.InitializeCanvas();
    }

    private void Start()
    {
        //Set canvas camera
        GetComponent<Canvas>().worldCamera = RenderController.textboxCam;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if(txb_spawn == false)
            {
                Textbox_Main t = (Textbox_Main)TextboxManager.SpawnTextbox("Testbox_Main");
                if (t == null) { Debug.Log("Returning null?"); return; }
                TextboxManager.SetMainTextbox(t);
                txb_spawn = true;
            }
        }
    }

}
