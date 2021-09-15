using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 9/9/21
 * Base class for scripted Textbox animations.
 */

public abstract class Textbox_Animation : MonoBehaviour
{
    protected Animation anim;
    protected AnimationClip myClip;

    private void Awake()
    {
        anim = GetComponent<Animation>();
        Debug.Log(anim.wrapMode);
    }

    private void Start()
    {
        // 1. Create clip
        myClip = CreateClip();
        //2. Ensure clip is legacy
        myClip.legacy = true;
        //3. Add clip to animation
        anim.AddClip(myClip, myClip.name);
        anim.Play(myClip.name);
    }

    //CreateClip creates an animation clip and adds it to anim.
    protected abstract AnimationClip CreateClip();
}
