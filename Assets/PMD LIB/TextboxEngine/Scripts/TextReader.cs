using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Code � Bijan Pourmand
 * Authored 9/10/21
 * Script for TextReader, to be used by textboxes and other things that require it.
 */

public class TextReader : MonoBehaviour
{
    //Ref to text components
    Textbox myTextbox;
    public Dialogue curDialogue;
    public Text[] texts;
    Text title => texts[0];
    Text sub => texts[1];
    Text body => texts[2];

    //static vars
    protected static char[] punctuation = { '.', '?', '!' };
    public static float speedMultiplier = 2f;  //for holding down space

    //TextReader Vals
    readonly static float def_speed = 3f;
    readonly static float def_sentenceBreak = 0.2f;
    readonly static float def_pauseDur = 0;
    public float speed = 3f;
    public float sentenceBreak = 0.2f;
    public float pauseDur = 0f;


    [SerializeField]
    protected int rows;
    [SerializeField]
    protected int maxRowChars;

    protected int MAXCHARS = 215;

    protected bool isAnim = false;
    protected bool isBreak = false;
    protected bool isExit = false;
    protected bool waitForOption = false;
    protected bool goToLine = false;

    protected string[] lines = new string[0];
    protected int linePointer = 0;

    //Coroutine Vars
    public Coroutine runIE;
    public bool isRunning = false;
    public bool IsRunning { get { return isRunning; } }

    protected static Action exitEvent;

    #region Initialize Funcs
    /* ----------------------------------------
    * Initialize Funcs
    * ---------------------------------------- */
    //SetTextbox sets the textbox for the reader to output to.
    public void SetTextbox(Textbox tbx)
    {
        //1. Set refs
        myTextbox = tbx;
        //1. If textbox is present, set texts to it's texts
        if (myTextbox != null) texts = myTextbox.texts;
        else Debug.Log("my textbox is gone!");

    }

    //SetDialogue initializes the textreader with a dialogue.
    public void SetDialogue(ref Dialogue cur, int startLine)
    {
        //0. Reset reader state
        ResetAll();
        //1. Set dialogue
        curDialogue = cur;
        //2. Initialize text asset
        lines = cur.lines;
        //3. Set starting line
        linePointer = Mathf.Clamp(startLine - 1, 0, lines.Length - 1);
    }

    //StartReader starts the reader IE.
    public void StartReader()
    {
        // 1. If not running start IE
        if (!isRunning)
        {
            runIE = StartCoroutine(RunScript());
        }
    }

    //ResetAll is called to clear all resettable vars.
    public void ResetAll()
    {
        //2. Reset line pointer
        linePointer = 0;
        //3. Clear lines array
        lines = new string[0];
        //4. if running, stop ie
        if (isRunning || runIE != null)
        {
            StopCoroutine(runIE);
            isRunning = false;
        }
        //5. reset dialog ref
        curDialogue = null;
        //6. reset bools
        isAnim = false;
        isBreak = false;
        isExit = false;
        waitForOption = false;
        //7. reader vals
        speed = def_speed;
        pauseDur = def_pauseDur;
        sentenceBreak = def_sentenceBreak;
    }
    #endregion

    #region ReaderFuncs
    /* ----------------------------------------
     * Reader Funcs
     * ---------------------------------------- */
    //CheckOptions checks if the input line is an options line. returns list of options.
    protected string[] CheckOptions(string line)
    {
        // 1. If line opens with option tag..
        if (line.Contains("{"))
        {
            // 2. Enable options flag for runIE
            waitForOption = true;
            // 3. Format options textstring into string array
            string ops = line.TrimStart('{');
            ops = ops.TrimEnd('}');
            string[] options = ops.Split(';');
            //4. Return options
            return options;
        }
        //5. If not option tag, return false.
        return new string[0];
    }

    //CheckMetaTag checks a string for a meta tag. Performs functions accordingly.
    protected bool CheckMetaTag(string myWord)
    {
        //1. If string starts with meta tag....
        if (myWord.Contains("<&"))
        {
            //1a. Parse word into char array to check tag
            char[] chars = myWord.ToCharArray();
            //Debug.Log("Cheking metatag " + myWord);
            //2. (Sub)Title Update
            if (chars[2] == 't')
            {
                //2a. If title tag, update title
                if (chars[3] == '>')
                {
                    //If title box is present..
                    if (texts[0] != null)
                    {
                        //2a.a. Extract title from line
                        string myName = myWord;
                        myName = myName.Trim('>');
                        Debug.Log($"MyName: {myName}");
                        myName = myName.Remove(0, 4);
                        //2a.b. Replace spaces
                        myName = myName.Replace("&'_'", " ");
                        //2a.c. Set new title text
                        Debug.Log(texts[0]);
                        texts[0].text = myName;
                    }
                }
                //2b. If subtitle tag, update subtitle
                else if (chars[3] == 's')
                {
                    //If subtitle box is present..
                    if (texts[1] != null)
                    {
                        //2b.a. Extract subtitle form line
                        string mySub = myWord;
                        mySub = mySub.Trim('>');
                        mySub = mySub.Remove(0, 4);
                        //2b.b. Replace spaces
                        mySub = mySub.Replace("&'_'", " ");
                        //2b.c. Set new subtitle
                        texts[1].text = mySub;
                    }
                }
            }


            //3. Break
            else if (chars[2] == 'w') isBreak = true;

            //4. Set text reader speed
            else if (chars[2] == 's')
            {
                // 4a. extract speed int
                string spd = myWord;
                spd = spd.Trim('>');
                spd = spd.Remove(0, 3);
                //4b. set speed
                speed = int.Parse(spd);
            }

            //5. Call DialogEvent
            else if (chars[2] == 'e')
            {
                // 5a. Extract dialogue event number
                int ev = (int)Char.GetNumericValue(chars[3]);
                //5b. Call event
                curDialogue.Event(ev, this);
            }

            //6. Force exit
            else if (chars[2] == 'x') { isExit = true; }

            //7. New Line
            else if (chars[2] == 'n')
            {
                if (chars[3] == 'l')
                {
                    //7a. Add new line to texts
                    texts[2].text += "\n";
                    //7b. Add
                }
            }

            //8. Pause for duration
            else if (chars[2] == 'p')
            {
                pauseDur = (int)Char.GetNumericValue(chars[3]) * 0.2f;
            }

            // 9. Return true if was meta
            return true;
        }
        //If not meta tag, return false
        return false;
    }
    #endregion

    #region EventFuncs

    /* ----------------------------------------
     * Event Functions
     * ---------------------------------------- */ 

    // SetLine is called by events to set the linePointer.
    public void SetLine(int line)
    {
        // 1. clamp new linepointer val to ensure it doesn't surpass total lines
        if (line > 0)
        {
            goToLine = true;
            linePointer = Mathf.Clamp(line - 1, 0, lines.Length);
        }

    }

    // SetExitEvent sets a delgate function to be called upon exiting the dialogue event.
    public void SetExitEvent(Action a)
    {
        exitEvent = a;
    }

    // EndOption is called in Dialogue to end the optionWait bool
    public void EndOptionWait() { waitForOption = false; }
    #endregion

    #region TextReader & Exit Func
    /* ----------------------------------------
     * Text Reader IE & Exit Func
     * ---------------------------------------- */
    protected virtual IEnumerator RunScript()
    {
        // 1. Init values
        isRunning = true;
        int charCount = 0;
        isExit = false;
        bool endDialogue = false;
        //if (charSound != null) GetComponent<AudioSource>().clip = charSound;

        //1. Call dialogues start event
        curDialogue.OnDialogueStart();

        //2. If there is a textbox attached to this...
        if (myTextbox != null)
        {
            while (myTextbox.IsAnimation())
            {
                Debug.Log("Waiting for textbox anim...");
                yield return null;
            }
        }

        // 3. Read text file into textbox
        while (!endDialogue)
        {
            //3a. Get line at current linepointer
            string s = lines[linePointer];
            //4. If next line isn't an options line...
            string[] opts = CheckOptions(s);
            if (opts.Length <= 0)
            {
                //4a. If isExit, exit reader loop
                if (isExit) break;
                // 4b. Split line into words
                string[] words = s.Split(' ');
                foreach (string w in words)
                {
                    // 4c. set EOS (End of Sentence) = false upon staring loading new word.
                    bool eos = false;
                    if (isExit) break;

                    // 4d. If word is not meta tag...
                    if (!CheckMetaTag(w))
                    {
                        // 4e. If pause is set, wait for pause
                        if (pauseDur > 0)
                        {
                            yield return new WaitForSeconds(pauseDur);
                            pauseDur = 0;
                        }
                        // 4f. Trigger flag for playing textbox blip
                        bool playSound = true;
                        // 4g. If word won't fit in textbox, clear textbox
                        if (IsOverflow(body.text, w).y == 1)
                        {
                            texts[2].text = "";
                            charCount = 0;
                        }
                        // 4h. Add word
                        foreach (char c in w)
                        {
                            // 4h.a. add char and increment char count
                            texts[2].text += c.ToString();
                            charCount++;
                            // 4h.b. play char sound
                            if (playSound)
                            {
                                //audio.Play();
                                playSound = false;
                            }
                            else playSound = true;

                            // 4h.c. wait for duration of speed; speed up if space is held down
                            speedMultiplier = (Input.GetKey(KeyCode.Space) ? 0.5f : 1);
                            yield return new WaitForSeconds(5 / (speed * 50f) * speedMultiplier);
                            // 4h.d.  if char is last and is punctuation, set EOS flag
                            if (c == w[w.Length - 1] && (Array.Exists(punctuation, p => p == c)
                                || w == words[words.Length - 1])) eos = true;
                        }
                        // 4i. add space between words
                        texts[2].text += " ";
                        charCount++;
                        // 4j. If not end of sentence, wait for readSpeed (?????); else wait for sentence break
                        if (!eos) yield return new WaitForSeconds(5 / (speed * 50f) * speedMultiplier);
                        else yield return new WaitForSeconds(sentenceBreak);
                        //4k. Stop textbox blip audio if still playing
                        //if (audio.isPlaying) audio.Stop();
                    }
                }
            }

            // 5. Otherwise if option string..
            else
            {
                //5a. If this is a main textbox..
                if (myTextbox != null &&
                    (myTextbox.GetType().IsSubclassOf(typeof(Textbox_Main)) ||
                    myTextbox.GetType() == typeof(Textbox_Main)))
                {
                    Debug.Log("Preparing to display options");
                    //5a. Display options and wait
                    ((Textbox_Main)myTextbox).DisplayOptions(opts);
                    while (waitForOption) yield return null;
                    //5b. Close options when done
                    texts[2].text = "";
                    ((Textbox_Main)myTextbox).ToggleButtons(false);
                }
                continue;
            }

            // 6. If at textbox break..
            if (isBreak)
            {
                //6a. Enable image
                myTextbox.OnBreak(true);
                //6b. wait for player to press continue key
                while (isBreak)
                {
                    if (Input.GetButtonDown("Submit"))
                    {
                        //6c. End break
                        isBreak = false;
                        myTextbox.OnBreak(false);
                        //clear text
                        charCount = 0;
                        texts[2].text = "";
                        //breakImg.SetActive(false);
                    }
                    else yield return null;
                }
            }

            // 7. Increment line pointer if new pointer hasn't already been set
            if (!goToLine) linePointer++;
            else goToLine = false;
            // 7a. If linePointer is larger than lines length, end dialogue.
            if (linePointer >= lines.Length || isExit) endDialogue = true;

        }
        // 8. At end of dialogue, enable break
        isBreak = true;
        myTextbox.OnBreak(true);

        //8a. Wait for player to exit break
        while (isBreak)
        {
            //Wait for input
            if (Input.GetButtonDown("Submit"))
            {
                isBreak = false;
                myTextbox.OnBreak(false);
                //clear text
                texts[2].text = "";
            }
            else yield return null;
        }
        //9. Clear and close box.
        if (myTextbox != null) myTextbox.ToggleTextbox(false, true);
        //10. Wait for textbox to close before calling OnTextboxExit
        yield return null;
        if (myTextbox != null)
        {
            while (myTextbox.IsAnimation())
            {
                Debug.Log("Waiting for textbox anim...");
                yield return null;
            }
        }
        OnTextboxExit();
    }

    //OnTextboxExit is called when the final break is made to close the textbox.
    protected void OnTextboxExit()
    {
        Debug.Log("on textbox exit");
        //1. Set isRunning to false
        isRunning = false;
        runIE = null;
        //2. if exit event exits, call exit event
        //if (exitEvent != null)
        if (curDialogue != null)
        {

            curDialogue.OnDialogueEnd();
            //exitEvent.Invoke();
            exitEvent = null;
        }

        //3. Reset vars
        ResetAll();

        //4. Disable textbox
        TextboxManager.ToggleTextbox(false, myTextbox.name);

    }
    #endregion

    #region TextGeneratorFunctions
    //IsOverflowHorizontal returns whether or not a string would cause vertical overflow.
    protected Vector2 IsOverflow(string text, string add)
    {
        //1. Get refs
        RectTransform bodyRect = body.GetComponent<RectTransform>();
        //2. Temporarily set additive string
        body.text = text + " " + add;
        //3. Get whether the new string exceeds the rect horizontal bounds
        int h = (LayoutUtility.GetPreferredWidth(bodyRect) > bodyRect.rect.width ? 1 : 0);
        int v = (LayoutUtility.GetPreferredHeight(bodyRect) > bodyRect.rect.height ? 1 : 0);
        //4. Set string back to normal
        body.text = text;
        //5. Return result
        return new Vector2(h, v);
    }



    #endregion
}

