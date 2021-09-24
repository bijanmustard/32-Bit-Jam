using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 9/15/21
 * Hitbox Controller for player.
 */

public class Player_HitboxController : Hitbox_Controller
{
    Player_Action playerAct;

    protected override void Awake()
    {
        base.Awake();
        //1. Get refs
        hitboxes = GetComponentsInChildren<Hitbox>();
        playerAct = transform.root.GetComponentInChildren<Player_Action>();
        //2. Disable hitboxes by default
        ToggleAllHitboxes(0);

        //2. Create animation events
        //2a. Punch_1
        AddAnimationEvent("Fighter|Punch_1", "ToggleAllHitboxes", 0,0);
        AddAnimationEvent("Fighter|Punch_1", "ToggleLeftHand", 0.03f, 1);
        //1b. Punch_2
        AddAnimationEvent("Fighter|Punch_2", "ToggleAllHitboxes", 0, 0);
        AddAnimationEvent("Fighter|Punch_2", "ToggleRightHand", 0.05f, 1);
        //1c. Front_Kick
        AddAnimationEvent("Fighter|Front_Kick", "ToggleAllHitboxes", 0, 0);
        AddAnimationEvent("Fighter|Front_Kick", "ToggleRightFoot", 0.06f, 1);
        //1d. Crouch Kick
        AddAnimationEvent("Fighter|Crouch_Kick", "ToggleAllHitboxes", 0, 0);
        AddAnimationEvent("Fighter|Crouch_Kick", "ToggleRightFoot", 0.11f, 1);
        

        //4. Set layermask
        mask = LayerMask.GetMask("Character/Player", "Character/Enemy", "Character/NPC");
        
    }

    public override void SignalHit(Hitbox source, Fighter hit)
    {
        Debug.Log($"Hitbox {source.name}, hit {hit.name}: notifying {playerAct}");
        //1. Toggle enemy HP UI
        Game_Canvas.ShowEnemyHP(hit);
        playerAct.curMethod.Invoke(hit, source); 
    }

    //AnimEvent toggles
    public void ToggleLeftHand(int tog)
    {
        Debug.Log("Left hand toggle!");
        hitboxes[1].GetComponent<Collider>().enabled = (tog >= 1 ? true : false);
        hitboxes[1].contacts.Clear();
    }

    public void ToggleRightHand(int tog)
    {
        hitboxes[0].GetComponent<Collider>().enabled = (tog > 0 ? true : false);
        hitboxes[0].contacts.Clear();
    }

    public void ToggleLeftFoot(int tog)
    {
        hitboxes[2].GetComponent<Collider>().enabled = (tog > 0 ? true : false);
        hitboxes[2].contacts.Clear();
    }

    public void ToggleRightFoot(int tog)
    {
        hitboxes[3].GetComponent<Collider>().enabled = (tog > 0 ? true : false);
        hitboxes[3].contacts.Clear();
    }
    
}
