using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_HBXController :Hitbox_Controller
{
    // Start is called before the first frame update
    Enemy_Action act;

    protected override void Awake()
    {
        base.Awake();
        //1. Get refs
        hitboxes = GetComponentsInChildren<Hitbox>();
        act = transform.root.GetComponentInChildren<Enemy_Action>();
        //2. Disable hitboxes by default
        ToggleAllHitboxes(0);

        //2. Create animation events
        //2a. Punch_1
        AddAnimationEvent("Armature|Punch_1", "ToggleAllHitboxes", 0, 0);
        AddAnimationEvent("Armature|Punch_1", "ToggleLeftHand", 0.03f, 1);
        //1b. Punch_2
        AddAnimationEvent("Armature|Punch_2", "ToggleAllHitboxes", 0, 0);

        //4. Set layermask
        mask = LayerMask.GetMask("Character/Player", "Character/Enemy", "Character/NPC");

    }

    public override void SignalHit(Hitbox source, Fighter hit)
    {
        act.curMethod.Invoke(hit);
    }

    //Toggles Left hand
    public void ToggleLeftHand(int tog)
    {
        Debug.Log("Left hand toggle!");
        hitboxes[0].GetComponent<Collider>().enabled = (tog >= 1 ? true : false);
        hitboxes[0].contacts.Clear();
    }
}

