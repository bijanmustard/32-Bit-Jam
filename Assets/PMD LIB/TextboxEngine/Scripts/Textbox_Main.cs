using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*
 * Code © Bijan Pourmand
 * Authored 9/10/21
 * Script for Main-styled textboxes. 
 * Contains funcs for displaying options, option funcs, and oter main-exclusive textbox functions.
 */

public abstract class Textbox_Main : Textbox
{

    protected static SelectableOption[] options;
    protected static new AudioSource audio;

    GameObject curSelectedOption = null;


    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();

        // 1. Get refs
        options = transform.Find("Buttons").GetComponentsInChildren<SelectableOption>();
        audio = GetComponent<AudioSource>();

        //2. Add self to manager's textbox list if not present
        if (!TextboxManager.textboxes.ContainsKey(name) && !TextboxManager.textboxes.ContainsValue(this))
        {
            Debug.Log("Adding " + name + " to textbox manager");
            TextboxManager.textboxes.Add(name, this);
            //2a. If textbox main hasn't been set, set as textbox main
            if (TextboxManager.textboxes.Count < 2) TextboxManager.SetMainTextbox(name);
        }
    }

   
    protected virtual void Start()
    {
        //1. Disable self
        gameObject.SetActive(false);
    }

    protected virtual void OnDestroy()
    {
        //1. Remove self from textbox manager
        TextboxManager.textboxes.Remove(name);
        //2. If this was main textbox, set mainTextbox to something else

    }

    //Listner Funcs

    /// <summary>
    /// Listner called after options have been displayed.
    /// </summary>
    public abstract void OnDisplayOptions();

    /// <summary>
    /// Listener for when setting title.
    /// </summary>
    public abstract void OnTitleSet();

    /// <summary>
    /// Listener for when setting subtitle.
    /// </summary>
    public abstract void OnSubtitleSet();

   

    //ToggleButtons enables/disables selection buttons.
    public void ToggleButtons(bool tog)
    {
        //1. For each button, toggle active
        foreach (Selectable s in options) s.gameObject.SetActive(tog);
    }


    //DisplayOptions loads the dialogue option buttons with event listeners.
    public void DisplayOptions(string[] ops)
    {
        //1. Get button info
        for (int i = 0; i < ops.Length; i++)
        {
            if (i < options.Length)
            {
                // 1a. Parse as char array
                char[] c = ops[i].ToCharArray();
                //2. check event code and add listener
                int ev = (int)Char.GetNumericValue(c[1]);
                options[i].onSelectedEvent = delegate { reader.curDialogue.Event(ev, reader); };

                //3. Get text from string
                string opt = ops[i];
                opt = opt.Trim('>');
                opt = opt.Remove(0, 2);

                //4. set to button text
                options[i].GetComponentInChildren<Text>().text = opt;

                //5. enable button
                options[i].gameObject.SetActive(true);
            }
        }
        //6. Set first option to selected
        EventSystem.current.SetSelectedGameObject(options[0].gameObject, new BaseEventData(EventSystem.current));
        //7. OnDisplayOptions listner
        OnDisplayOptions();
        
    }



}
