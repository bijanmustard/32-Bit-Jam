using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 9/13/21
 * Script for player movement.
 */

public class Player_Move : Fighter_Move
{
    Player_Action pAction;

    protected override void Awake()
    {
        base.Awake();
        //1. Set ref to pAction
        pAction = GetComponent<Player_Action>();
    }

    protected override Vector3 GetInputDir()
    {
        if (!isKB && !pAction.isStun && !pAction.isCrouch)
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
            return new Vector3(h, 0, v);
        }
        else if (isKB && !pAction.isStun) return kbDir;
        else if (pAction.isCrouch) return Vector3.zero;
        else return Vector3.zero;
    }


    protected override bool Jump()
    {
        ////1. Check for jump input
        //if (Input.GetKeyDown(KeyCode.Space) && charCont.isGrounded && !jumpFrame)
        //    return true;
        //else return false;
        return false;
    }
}
