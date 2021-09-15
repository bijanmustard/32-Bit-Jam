using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor.Events;

/*
 * Code � Bijan Pourmand
 * Authored 6/25/21, Revised 9/7/21
 * Script for textbox manager
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
    //Static ref to current instance in scene
    private static TextboxManager instance;

    //Current dialogue vars
    public static Dialogue curDialogue;
    protected static TextAsset asset;
    static string[] lines;

    //Ref to current main textbox 
    static Textbox_Main mainTextbox;
    static TextReader mainReader;
    static RectTransform boxRect;
    //Ref to all main textboxes present
    public static Dictionary<string, Textbox_Main> textboxes = new Dictionary<string, Textbox_Main>();

    //bools for if mainTextbox is running
    public static bool IsRunning { get { return mainReader.isRunning; } }
    
    //Text sounds
    public static AudioClip charSound;

    static Vector2 placementOffset { get { return new Vector2(Screen.width * 0.5f, 0); } }
    static Vector2 defaultPlacement { get { return new Vector2(6, 117) + placementOffset; } }

    //SetMainTextbox sets a textbox from the dictionary as the main one.
    public static void SetMainTextbox(string box)
    {
        // 1. Check for box
        Textbox_Main value;
        
        bool boxFound = textboxes.TryGetValue(box, out value);
        // 2. If box found set as mainBox
        if (boxFound && value != null)
        {
            mainTextbox = value;
            mainReader = mainTextbox.GetComponent<TextReader>();
        }
    }


    //ToggleTextbox is called to enable/disable a specified textbox.
    public static void ToggleTextbox(bool tog, string box)
    {
        //1. If key is in dictionary..
        Textbox_Main value;
        if (textboxes.ContainsKey(box))
        {
            // 2. Get value
            textboxes.TryGetValue(box, out value);
            // 3. Toggle Textbox
            if (tog)
            {
                Debug.Log("Setting " + value.name + " to active");
                value.gameObject.SetActive(true);
            }
            else value.ToggleTextbox(tog);

        }
        else Debug.Log("Textbox " + box + " not found!");
    }


    // StartDialogue is called by an NPC to trigger a dialogue in the main textbox.
    public static void StartDialogue(Dialogue myDia)
    {
        //1. Initialize
        ToggleTextbox(true, mainReader.name);
        mainTextbox.StartTextbox(ref myDia);
    }


  



}
