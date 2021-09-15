using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Code © Bijan Pourmand
 * Authored 4/6/21
 * Base script for transition objs. To be used by TransitionManager.
 */

public class Transition : MonoBehaviour
{
    //vars
    [SerializeField]
    protected Animator anim;
    [SerializeField]
    protected float speed = 1;
    public bool isTransitioning => IsAnimating();
    

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // In() is called when transitioning back into a scene
    public virtual void In() { }
    public virtual void In(float speed) { }

    // Out is called when transitioniong out of a scene.
    public virtual void Out() { }
    public virtual void Out(float speed) { }
    public virtual void Out(Color c, float speed = 1) { }

    ///<summary>
    ///Returns whether or not the animator is running.
    ///</summary>
    protected bool IsAnimating()
    {
        // 1. If animation is done and not transitioning...
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0)) return false;
        else return true;
           
    }

}

