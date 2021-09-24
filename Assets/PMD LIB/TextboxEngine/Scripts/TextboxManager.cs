using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor.Events;

/*
 * Code � Bijan Pourmand
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

public class TextboxManager : MonoBehaviour
{
    //Static ref to Textbox canvas
    static TextboxManager instance;
    static Textbox_Canvas canvas;

    //Variables for current loaded dialogue
    public static Dialogue curDialogue;
    static TextAsset asset;
    static string[] lines;

    //Ref to current main textbox 
    static Textbox_Main mainTextbox;

    static TextReader reader;

    //Dictionary for all currently loaded textboxes
    static Dictionary<string, GameObject> styles = new Dictionary<string, GameObject>();
    //Dictionary that keeps tab of spawned textboxes
    static Dictionary<string, Textbox> textboxes = new Dictionary<string, Textbox>();

    //bools for if mainTextbox is running
    public static bool IsRunning { get { return reader.isRunning; } }

    //Text sounds
    public static AudioClip charSound;

    //DEBUG
    bool txb_spawn = false;
    bool dbg_tog = false;

    private void Awake()
    {

        //0. Set singleton
        if (instance != null && instance != this) Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        //1. Get ref to text reader
        reader = GetComponent<TextReader>();
        //2. Init dictionary
        GameObject[] prefabs = Resources.LoadAll<GameObject>("TextboxEngine/Textbox_Main");
        foreach (GameObject obj in prefabs) styles.Add(obj.name, obj);
        Debug.Log($"Styles loaded! Ct: {styles.Count}");
    }

    protected void Update()
    {
        //DEBUG
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (txb_spawn == false)
            {
                txb_spawn = true;
                Textbox t = SpawnTextbox("Testbox");
                if (mainTextbox == null) SetMainTextbox((Textbox_Main)t);
            }
            else
            {
                Textbox t = SpawnTextbox("Testbox", null, null, 200, 200);
            }
        }
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
        //1. Try and get prefab
        GameObject style = GetStyle(tbxName);
        if (style == null) return null ;
        //2. Spawn textbox
        Textbox tbx = Instantiate(style, instance.transform).GetComponent<Textbox>();
        //3. Instantiate vars
        if(height != null) tbx.height = (float)height;
        if(width!= null) tbx.width = (float)width;
        if(xPos != null) tbx.xPos = (float)xPos;
        if(yPos != null) tbx.yPos = (float)yPos;
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
        Debug.Log($"Added textbox {tbx.name}, textboxCt: {textboxes.Count}");
        //6. Return 
        return tbx;

    }

    //RemoveTextbox removes a specified textbox.
    public static void DestroyTextbox(Textbox tbx)
    {
        //1. Remove tbx from dictionary
        if (textboxes.ContainsKey(tbx.name))
        {
            textboxes.Remove(tbx.name);
            //2. Destroy tbx
            Destroy(tbx.gameObject);
            Debug.Log("Destroyed texbox");
        }
        else Debug.Log($"Error! Couldn't destroy textbox {tbx.name} because key not found!");
    }


    //SetMainTextbox sets a textbox from the dictionary as the main one.
    public static void SetMainTextbox(Textbox_Main tbxMain)
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

    public static void SetMainTextbox(string tbxMain)
    {
        //1. Check to see if key exists
        if (!textboxes.ContainsKey(tbxMain)) return;
        //2. If vaild, try getting value
        Textbox value;
        textboxes.TryGetValue(tbxMain, out value);
        //3. If type is Textbox_Main, update main textbox
        if(value != null && value.GetType() == typeof(Textbox_Main))
        {
            //3a. update ref
            mainTextbox = (Textbox_Main)value;
            //3b. Assign reader
            reader.SetTextbox(mainTextbox);
        }
    }

    //ToggleTextbox is called to enable/disable a specified textbox.
    public static void ToggleTextbox(bool tog, string box)
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



    // StartDialogue is called by an NPC to trigger a dialogue in the main textbox.
    public static void StartDialogue(Dialogue myDia, int startLine = 0)
    {
        //1. Initialize
        ToggleTextbox(true, mainTextbox.name);
        mainTextbox.StartTextbox(ref myDia, startLine);
    }





}
