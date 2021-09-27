using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Code © Bijan Pourmand
 * Authored 6/25/21, Revised 9/7/21
 * Script for textbox manager. Manages the starting of dialogue events, show/hide of textboxes.
 */

/*
 * METATEXT GLOSSARY
 * <&t(string)> -- Update name text
 * <&w> -- Trigger break
 * <&s(int)> -- set read speed
 * <&x> -- Exit text event
 * {<1..>;<2..>;..<n..>} -- Options (limit of four)
 * <&d(int)> -- change expression (for initimate dialogue)
 * <&e(int)> -- call (int) event
 */

public static class TextboxManager
{
    static bool active = false;
    public static bool IsActive => active;

    //Static ref to Textbox canvas
    static Canvas canvas;
    static TextReader reader;

    const string fp_styles = "TextboxEngine/Styles";
    const string fp_canvas = "TextboxEngine/Textbox_Canvas";

    //Variables for current loaded dialogue
    public static Dialogue curDialogue;
    static TextAsset asset;
    static string[] lines;

    //Ref to current main textbox 
    static Textbox_Main mainTextbox;

    //Dictionary for all currently loaded textboxes
    static Dictionary<string, GameObject> styles = new Dictionary<string, GameObject>();
    //Dictionary that keeps tab of spawned textboxes
    static Dictionary<string, Textbox> textboxes = new Dictionary<string, Textbox>();

    //bools for if mainTextbox is running
    public static bool IsRunning { get { return reader.isRunning; } }

    //Text sounds
    public static AudioClip charSound;

   

    //Called upon first call of TextboxManager
    static TextboxManager()
    {
        //1. Init dictionary
        GameObject[] prefabs = Resources.LoadAll<GameObject>(fp_styles);
        foreach (GameObject obj in prefabs)
        {
            styles.Add(obj.name, obj);
        }

        
    }

    //InitializeCanvas is called to initialize the canvas.
    public static void InitializeCanvas()
    {
        Debug.Log($"TextboxManager initializing canvas");
        //1. If canvas isn't present, set reference
        if(canvas == null)
        {
            //1a. If texbox canvas is present, set to canvas
            if (GameObject.FindObjectOfType<Textbox_Canvas>() != null)
                canvas = GameObject.FindObjectOfType<Textbox_Canvas>().GetComponent<Canvas>();
            //1b. If not, spawn one
            else canvas = GameObject.Instantiate(Resources.Load<GameObject>(fp_canvas)).GetComponent<Canvas>();
        }
        //2. Set camera output to main
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.pixelPerfect = true;
        canvas.worldCamera = RenderController.UICam;
        //3. Reset reader ref
        reader = canvas.GetComponent<TextReader>();
    }

    //SetActive is called to toggle whether textboxes are enabled or not.
    public static void SetActive(bool tog)
    {
        //1. Set active bool
        active = tog;
        //2. If disabling, destroy canvas
        if (!active) { if (canvas != null) GameObject.Destroy(canvas.gameObject); }
        //3. Else, spawn canvas
        else InitializeCanvas();
        
    }

    //GetStyle gets a style from the dictionary. Returns null if not found.
    public static GameObject GetStyle(string title)
    {
        //1. Try and get value
        GameObject value = null;
        bool found = styles.TryGetValue(title, out value);
        //2. If txbx found, return it
        return value;
    }

    //SpawnTextbox is called to spawn a textbox onto the canvas.
    public static Textbox SpawnTextbox(string tbxName, float? height = null, float? width = null, float? xPos = null, float? yPos = null)
    {
        //0. If not disabled...
        if (active)
        {
            //1. Try and get prefab
            GameObject style = GetStyle(tbxName);
            if (style == null)
            {
                Debug.Log($"SpawnTextbox: Style not found! {tbxName}");
                return null;
            }
            //2. Spawn textbox
            Textbox tbx = GameObject.Instantiate(style, canvas.transform).GetComponent<Textbox>();
            //3. Instantiate vars
            if (height != null) tbx.height = (float)height;
            if (width != null) tbx.width = (float)width;
            if (xPos != null) tbx.xPos = (float)xPos;
            if (yPos != null) tbx.yPos = (float)yPos;
            tbx.ToggleTextbox(false, false);
            //4. If name already exists in dictionary..
            if (textboxes.ContainsKey(tbx.name))
            {
                //4a. create new name for it
                string key = tbx.name;
                int ascii = 49;
                while (textboxes.ContainsKey(key))
                {
                    key = tbx.name + "_" + Convert.ToChar(ascii);
                    ascii++;
                }
                //4b. Set new name
                tbx.name = key;
            }
            //5. Add to dictionary
            textboxes.Add(tbx.name, tbx);
            //6. Return 
            return tbx;
        }
        return null;

    }

    //RemoveTextbox removes a specified textbox.
    public static void DestroyTextbox(Textbox tbx)
    {
        //0. If not disabled..
        if (active)
        {
            //1. Remove tbx from dictionary
            if (textboxes.ContainsKey(tbx.name))
            {
                textboxes.Remove(tbx.name);
                //2. Destroy tbx
                GameObject.Destroy(tbx.gameObject);
                Debug.Log("Destroyed texbox");
            }
            else Debug.Log($"Error! Couldn't destroy textbox {tbx.name} because key not found!");
        }
    }


    //SetMainTextbox sets a textbox from the dictionary as the main one.
    public static void SetMainTextbox(Textbox_Main tbxMain)
    {
        //0. If not disabled..
        if (active)
        {
            //1. Check if textbox is signed to dictionary
            if (!textboxes.ContainsValue(tbxMain)) return;
            //2. If valid, update main textbox
            //2a. Update ref
            mainTextbox = tbxMain;
            //2b. assign reader
            reader.SetTextbox(mainTextbox);
            //2c. Assign reader to textbox
            tbxMain.reader = reader;
        }
    }

    public static void SetMainTextbox(string tbxMain)
    {
        //0. If not disabled..
        if (active)
        {
            //1. Check to see if key exists
            if (!textboxes.ContainsKey(tbxMain)) return;
            //2. If vaild, try getting value
            Textbox value;
            textboxes.TryGetValue(tbxMain, out value);
            //3. If type is Textbox_Main, update main textbox
            if (value != null && value.GetType() == typeof(Textbox_Main))
            {
                //3a. update ref
                mainTextbox = (Textbox_Main)value;
                //3b. Assign reader
                reader.SetTextbox(mainTextbox);
            }
        }
    }

    //ToggleTextbox is called to enable/disable a specified textbox.
    public static void ToggleTextbox(bool tog, string box)
    {
        //0. If not disabled..
        if (active)
        {
            //1. If key is in dictionary..
            if (textboxes.ContainsKey(box))
            {
                // 2. Get value
                Textbox value;
                textboxes.TryGetValue(box, out value);
                if (value != null)
                {
                    //3. If enabling, set gameoject to active before calling functions
                    if (tog) value.gameObject.SetActive(true);
                    //3a. Toggle box
                    value.ToggleTextbox(tog, true);

                }
            }
            else Debug.Log("Textbox " + box + " not found!");
        }
    }



    // StartDialogue is called by an NPC to trigger a dialogue in the main textbox.
    public static void StartDialogue(Dialogue myDia, int startLine = 0)
    {
        //0. If not disabled..
        if (active)
        {
            //1. Initialize
            ToggleTextbox(true, mainTextbox.name);
            mainTextbox.StartTextbox(ref myDia, startLine);
        }
    }





}
