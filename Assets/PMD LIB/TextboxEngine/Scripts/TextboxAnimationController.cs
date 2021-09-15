using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 9/9/21
 * Script utilized by TextboxManager to handle loading of textbox animations
 */

public class TextboxAnimationController : MonoBehaviour
{

    //MainTextbox animator
    Animation anim;

    //Refs to main open/close anim
    public AnimationClip mainOpen;
    public AnimationClip mainClose;

    //SetMainOpen sets the main open animation;
    public void SetMainOpen(AnimationClip open) { mainOpen = open; }
    //SetMainClose sets the main close animation
    public void SetMainClose(AnimationClip close) { mainClose = close; }

}
