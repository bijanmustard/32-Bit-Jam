using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 9/15/21
 * Base script for fighter actions.
 */

public abstract class Fighter_Action : MonoBehaviour
{

    Fighter fighter;

    public string? curAction;
    public delegate void ActionMethod(Fighter f);
    public delegate void ActionInterrupt();
    public ActionInterrupt onInterrupt;
    public ActionMethod curMethod;
    protected Animator anim;

    Fighter_Move fMove;
    protected bool isStun;
    float stunTimer = 0;
    float stunDur = 0;
    float knockback_dur = 0.75f;
    float knockback_timer = 0;

    /// <summary> Dictionary mapping action strings to their on hit functions. /// </summary>
    public Dictionary<string, Action> hitMap= new Dictionary<string, Action>();

    protected Hitbox_Controller hbxController;

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        hbxController = GetComponentInChildren<Hitbox_Controller>();
        fMove = GetComponent<Fighter_Move>();
        fighter = GetComponent<Fighter>();
    }

    protected virtual void Update()
    {
        //0. If fighter isn't frozen and not paused..
        if (!fighter.isFreeze && !PauseController.IsPause && GameController.gameTimeScale > 0)
        {
            //1. If KB, increment KB timer
            if (fMove.isKB)
            {
                knockback_timer += Time.deltaTime;
                if (knockback_timer >= knockback_dur && fMove.isGrounded)
                {
                    EndKnockback();
                    Stun(1, 0);
                }
                //If still knock back, ignore other actions
                else return;
            }

            //2. If stun, increment stun timer
            if (isStun)
            {
                stunTimer += Time.deltaTime;
                //2a. if Stun timer runs out, return to default.
                if (stunTimer >= stunDur)
                {
                    EndStun();
                }
                else return;
            }

            //3. Scripts call update
            MyUpdate();
        }
    }

    //Personalized update for each fighter
    protected abstract void MyUpdate();

    //DealKnockback is called to deal knockback.
    public void Knockback(float force, float vertVel, Vector3 dir)
    {
        //0. Call onInterrupt
        if(onInterrupt != null) onInterrupt.Invoke();

        //1. Set vars
        curAction = "Knockback";
        fMove.isKB = true;
        fMove.kbDir = dir;
        fMove.kbVertVel = vertVel;
        fMove.kbForce = force;
        fMove.rotToDir = false;
        //2. Set anim state
        anim.SetBool("IsKnockback", true);
        anim.SetTrigger("Knockback");
    }

    public void EndKnockback()
    {
        //1. Reset vars
        curAction = null;
        onInterrupt = null;

        Debug.Log("End knockback");
        knockback_timer = 0;
        fMove.isKB = false;
        fMove.rotToDir = true;
        anim.SetBool("IsKnockback", false);
       
        
    }

    //Stun takes the duration of the stun and the clip name for the animation
    public void Stun(float duration, int stunAnim)
    {
        //1. Set vars
        onInterrupt = EndStun;
        curAction = "Stunned";

        fMove.canMove = false;
        stunDur = duration;
        stunTimer = 0;
        isStun = true;
        //2. Set anim
        anim.SetBool("Stunned", true);
        anim.SetInteger("StunType", stunAnim);
    }

    public void EndStun()
    {
        //1. Reset vars
        onInterrupt = null;
        curAction = null;

        fMove.canMove = true;
        stunDur = 0;
        stunTimer = 0;
        isStun = false;

        //2. SetAnim
        anim.SetBool("Stunned", false);
        
    }

}
