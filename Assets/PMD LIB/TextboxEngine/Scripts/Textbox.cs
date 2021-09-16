using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Code © Bijan Pourmand
 * Authored 9/9/21
 * Base script for textbox main/blurb. Includes animation management, text reading, etc.
 */

public abstract class Textbox : MonoBehaviour
{
    //Ref to texts
    [SerializeField]
    public Text[] texts = new Text[3];
    [SerializeField]
    protected GameObject breakSymbol;
    protected TextReader reader;

    //Vars & Refs
    public Animation anim;
    public AnimationClip intro;
    public AnimationClip outro;
    bool isClose;
    Coroutine animIE;

    protected virtual void Awake()
    {
        // 1. Initialize texts
        if (texts[0] == null) try { texts[0] = transform.Find("Textbox_Title").GetComponent<Text>(); } catch { }
        if (texts[1] == null) try { texts[1] = transform.Find("Textbox_Subtitle").GetComponent<Text>(); } catch { }
        if (texts[2] == null) try { texts[2] = transform.Find("Textbox_Body").GetComponent<Text>(); } catch { }

        //2. Set refs
        if (breakSymbol == null) try { breakSymbol = transform.Find("Textbox_BreakSymbol").gameObject; } catch { }
        reader = GetComponent<TextReader>();
        anim = GetComponent<Animation>();
        if (intro != null) anim.AddClip(intro, intro.name);
        if (outro != null) anim.AddClip(outro, outro.name);
    }

    private void OnEnable()
    {
        ToggleTextbox(true);
    }


    private void Update()
    {
        //1. If outro, wait to finish before disabling textbox.
        if(isClose && anim.isPlaying)
        {
            isClose = false;
            gameObject.SetActive(false);
        }
    }

    //Toggle whether this textbox is enabled
    public void ToggleTextbox(bool tog)
    {
        // 1. Clear vars before read/close
        //1a. Stop runIE if running
        if (reader.runIE != null) StopCoroutine(reader.runIE);

        //1c. Disable option buttons
        if(GetType() == typeof(Textbox_Main)) ((Textbox_Main)this).ToggleButtons(false);
        //1d. Reset textbox, text reader vals
        reader.SetLine(0);
        reader.ClearTexts();
        //1e. Hide break img
        if(breakSymbol != null) breakSymbol.SetActive(false);
        
        OpenCloseBox(tog);

    }

    //PlayAnim plays either the intro or outro anim. True = enter, false = exit.
    public void OpenCloseBox(bool isIntro)
    {
        if (animIE != null) StopCoroutine(animIE);
        animIE = StartCoroutine(OpenCloseIE(isIntro));
    }

    IEnumerator OpenCloseIE(bool tog)
    {
        //1. Trigger animation
        if((tog && intro != null) || (!tog && outro != null)) anim.Play((tog ? intro.name : outro.name));
        //2. Wait for animation to finish
        while (anim.isPlaying) yield return null;
        //3. When done, finish IE
        animIE = null;
        if (!tog) gameObject.SetActive(false);

    }

    //StartTextbox is called to start the textbox IE.
    public void StartTextbox(ref Dialogue dia, int startLine = 0)
    {
        //0. If this textbox currently isn't running..
        if (!reader.isRunning)
        {
            //1. Set dialogue in reader
            reader.SetDialogue(ref dia, startLine);
            //2. Play intro anim
            if(intro != null) anim.Play(intro.name);
            //3. Start Reader IE
            reader.StartReader();
        }
    }

    ////StartTextbox variant where lines are manually passed in.
    //public void StartTextbox(string[] toRead, int startLine = 0)
    //{
    //    //0. If this textbox reader isn't currently running...
    //    if (!reader.isRunning)
    //    {
    //        //1. set reader lines
    //        reader.SetDialogue(toRead, startLine);
    //        if (intro != null) anim.Play(intro.name);
    //        reader.StartReader();
    //    }
    //}

    //OnBreak is called when breaking. Put any icon animations, etc. in here.
    public void OnBreak(bool enabled)
    {
        if (breakSymbol != null) breakSymbol.SetActive(enabled);
    }


    //SetIntroAnim sets the intro animation clip.
    public void SetIntroAnim(AnimationClip clip)
    {
        //1. Set intro clip
        intro = clip;
        //2. If clip isn't added to animation, add to animation
        if (anim.GetClip(intro.name) == null) anim.AddClip(intro, intro.name);
    }

    //SetOutroAnim sets the outro animation clip.
    public void SetOutroAnim(AnimationClip clip)
    {
        //1. Set outro clip
        outro = clip;
        //2. If clip isn't on animation component, add to anim
        if (anim.GetClip(outro.name) == null) anim.AddClip(outro, outro.name);
    }

    //IsAnimation returns whether or not the animator is playing.
    public bool IsAnimation() { return anim.isPlaying; }
}
