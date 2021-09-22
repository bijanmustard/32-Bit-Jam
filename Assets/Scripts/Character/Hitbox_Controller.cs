using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 9/15/21
 * Script for Hitbox Controller to be used by fighter characters. 
 * Contains hit layermask, toggles, sets animation events, etc.
 */

public abstract class Hitbox_Controller : MonoBehaviour
{
    
    protected Animator anim;
    public Hitbox[] hitboxes;
    [SerializeField]
    public LayerMask mask; 

    

    protected virtual void Awake()
    {
        //1. Set refs
        anim = GetComponent<Animator>();
    }

    public void SignalHit(Hitbox source, Hittable hit) { hit.OnHit(source); }
    public abstract void SignalHit(Hitbox source, Fighter hit);

    /// <summary>
    /// Toggles all hitboxes to a given bool.
    /// </summary>
    /// <param name="tog"></param>
    public void ToggleAllHitboxes(int tog)
    {
        foreach (Hitbox hbx in hitboxes) hbx.GetComponent<Collider>().enabled = (tog >= 1 ? true : false);
    }

    /// <summary>
    /// Adds an animation event with int parameter to a specified clip in the runtimeController.
    /// </summary>
    /// <param name="clipName"></param>
    /// <param name="funcName"></param>
    /// <param name="time"></param>
    /// <param name="intParam"></param>
    /// <returns></returns>
    public void AddAnimationEvent(string clipName, string funcName, float time, int intParam)
    {
        //1. Find clip in animator
        AnimationClip clip = Array.Find(anim.runtimeAnimatorController.animationClips, c => c.name == clipName);
        if (clip == null)
        {
            Debug.Log("Clip " + clipName + " not found!");
            return;
        }
        //2. Check if function exists in script
        if (GetType().GetMethod(funcName) == null)
        {
            Debug.Log("Error! Method " + funcName + " not found!");
            return;
        }
        //3. Create event
        AnimationEvent ev = new AnimationEvent();
        //4. Set animation event vars
        ev.functionName = funcName;
        ev.time = time;
        ev.intParameter = intParam;
        //5. Add event
        clip.AddEvent(ev);
        //Debug.Log("Added event to " + clip.name + " at time " + time);
    }

    /// <summary>
    /// Adds an animation event with float parameter to a specified clip in the runtimeController.
    /// </summary>
    /// <param name="clipName"></param>
    /// <param name="funcName"></param>
    /// <param name="time"></param>
    /// <param name="floatParam"></param>
    /// <returns></returns>
    public void AddAnimationEvent(string clipName, string funcName, float time, float floatParam)
    {
        //1. Find clip in animator
        AnimationClip clip = Array.Find(anim.runtimeAnimatorController.animationClips, c => c.name == clipName);
        if (clip == null)
        {
            Debug.Log("Clip " + clipName + " not found!");
            return;
        }
        //2. Check if function exists in script
        Debug.Log(this.GetType());
        if (GetType().GetMethod(funcName) == null)
        {
            Debug.Log("Error! Method " + funcName + " not found!");
            return;
        }
        //3. Create event
        AnimationEvent ev = new AnimationEvent();
        //4. Set animation event vars
        ev.functionName = funcName;
        ev.time = time;
        ev.floatParameter = floatParam;
        //5. Add event
        clip.AddEvent(ev);
        Debug.Log("Added event to " + clip.name + " at time " + time);
    }
    /// <summary>
    /// Adds an animation event with string parameter to a specified clip in the runtimeController.
    /// </summary>
    /// <param name="clipName"></param>
    /// <param name="funcName"></param>
    /// <param name="time"></param>
    /// <param name="stringParam"></param>
    /// <returns></returns>
    public void AddAnimationEvent(string clipName, string funcName, float time, string stringParam)
    {
        //1. Find clip in animator
        AnimationClip clip = Array.Find(anim.runtimeAnimatorController.animationClips, c => c.name == clipName);
        if (clip == null)
        {
            Debug.Log("Clip " + clipName + " not found!");
            return;
        }
        //2. Check if function exists in script
        Debug.Log(this.GetType());
        if (GetType().GetMethod(funcName) == null)
        {
            Debug.Log("Error! Method " + funcName + " not found!");
            return;
        }
        //3. Create event
        AnimationEvent ev = new AnimationEvent();
        //4. Set animation event vars
        ev.functionName = funcName;
        ev.time = time;
        ev.stringParameter = stringParam;
        //5. Add event
        clip.AddEvent(ev);
        Debug.Log("Added event to " + clip.name + " at time " + time);
    }
    
}
