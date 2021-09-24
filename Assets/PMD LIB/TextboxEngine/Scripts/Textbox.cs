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
    //Ref to rect
    CanvasScaler tbxCanvas;
    RectTransform rect;
    RectTransform parentRect;
    RectAnchorMode anchorMode;

    //Vector2 anchorPos => new Vector2(rect.anchoredP)

    //Ref to texts
    [SerializeField]
    public Text[] texts = new Text[3];
    [SerializeField]
    protected GameObject breakSymbol;
    public TextReader reader;

    //Vars & Refs
    public Animation anim;
    public AnimationClip intro;
    public AnimationClip outro;
    bool isClose;
    Coroutine animIE;

    //Positioning and Height
    [SerializeField]
    public float height = 150, width = 300, xPos = 0, yPos = 0;


    protected virtual void Awake()
    {
        // 1. Initialize texts
        if (texts[0] == null) try { texts[0] = transform.Find("Textbox_Title").GetComponent<Text>(); } catch { }
        if (texts[1] == null) try { texts[1] = transform.Find("Textbox_Subtitle").GetComponent<Text>(); } catch { }
        if (texts[2] == null) try { texts[2] = transform.Find("Textbox_Body").GetComponent<Text>(); } catch { }

        //2. Set refs
        rect = GetComponent<RectTransform>();
        parentRect = transform.parent.GetComponent<RectTransform>();

        if (breakSymbol == null) try { breakSymbol = transform.Find("Textbox_BreakSymbol").gameObject; } catch { }
        reader = GetComponent<TextReader>();
        anim = GetComponent<Animation>();
        if (intro != null) anim.AddClip(intro, intro.name);
        if (outro != null) anim.AddClip(outro, outro.name);

        tbxCanvas = transform.root.GetComponent<CanvasScaler>();
    }

    private void Start()
    {
        SetPositioning();
    }

    private void Update()
    {

        //1. Set rect anchor mode
        if (rect.anchorMin == Vector2.zero && rect.anchorMax == Vector2.one) anchorMode = RectAnchorMode.Expand;
        else if (rect.anchorMin.x == rect.anchorMax.x && rect.anchorMin.y == rect.anchorMax.y) anchorMode = RectAnchorMode.Point;
        else anchorMode = RectAnchorMode.Custom;

        //2. Set size and pos
        SetPositioning();


            
    }

    //SetPositioning
    public void SetPositioning()
    {
        if (anchorMode == RectAnchorMode.Expand)
        {
            rect.sizeDelta = new Vector2(Screen.width - width, Screen.height - height) * -1;
            rect.anchoredPosition = new Vector2(xPos, yPos);
        }
        else if (anchorMode == RectAnchorMode.Point)
        {
            rect.sizeDelta = new Vector2(width, height);
            Vector2 offset = new Vector2(tbxCanvas.referenceResolution.x * rect.anchorMin.x, tbxCanvas.referenceResolution.y * rect.anchorMin.y);
            Debug.Log($"Offset: {offset}");
            rect.anchoredPosition = new Vector2(xPos, yPos) + -offset;
        }
        else
        {

        }
    }

    //ResetTextbox is called to reset the textbox components to their default state.
    public void ResetTextbox(bool resetReader)
    {
        //1. Disable option buttons
        if (GetType() == typeof(Textbox_Main)) ((Textbox_Main)this).ToggleButtons(false);
        //2. Hide break img
        if (breakSymbol != null) breakSymbol.SetActive(false);
        //3. Clear all text strings
        ClearTexts();
        //4. If resetReader, clear reader
        if(resetReader) reader.ResetAll();

    }

    //ClearTexts is called to clear all texts.
    public void ClearTexts()
    {
        foreach (Text tx in texts) if (tx != null) tx.text = "";
    }

    //Toggle whether this textbox is visually enabled
    public void ToggleTextbox(bool tog, bool useAnimations)
    {
        //1. Clear textbox values
        ClearTexts();
        //2. Set open/close
        OpenCloseBox(tog, useAnimations);
    }

    //OpenCloseBox opens/closes the textbox, with an optional bool for skipping animation.
    public virtual void OpenCloseBox(bool isOpen, bool useAnimation)
    {
        //1. If using animation...
        if (useAnimation)
        {
            //1a. If animationIE is already running, stop and restart it.
            if (animIE != null) StopCoroutine(animIE);
            animIE = StartCoroutine(OpenCloseIE(isOpen));
        }
        //2. Else if not using animation...
        else
        {
            //2a. If enabling, enable textbox
            gameObject.SetActive(isOpen);
        }
    }

    IEnumerator OpenCloseIE(bool tog)
    {
        //1. Trigger animation
        if((tog && intro != null) || (!tog && outro != null)) anim.Play((tog ? intro.name : outro.name));
        //2. Wait for animation to finish
        while (anim.isPlaying) yield return null;
        //3. When done, finish IE
        animIE = null;
        //4. If closing textbox, disable textbox gameObject
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

public enum RectAnchorMode { 
    Expand,
    Point, 
    Custom
}
