using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 9/15/21
 * Base controller script for characters. Contains funcs pertaining to locking movement, etc.
 */

public abstract class Character : MonoBehaviour
{
    protected Character_Move _move;
    protected Animator anim;
    protected float prevAnimSpeed;

    protected bool freeze = false;
    public bool isFreeze => freeze;


    protected virtual void Awake()
    {
        _move = GetComponent<Character_Move>();
        anim = GetComponentInChildren<Animator>();
    }

    /// <summary>Toggles player input and movement. Gravity is still enacted regardless.</summary>
    /// <param name="tog"></param>
    public abstract void ToggleInput(bool tog);

    public void SetFreeze(bool tog)
    {
        //1. Set freeze bool
        freeze = tog;
        //2. Set anim speed
        if (tog)
        {
            prevAnimSpeed = anim.speed;
            anim.speed = 0;
        }
        else
        {
            anim.speed = prevAnimSpeed;
            prevAnimSpeed = 0;
        }
    }
}
