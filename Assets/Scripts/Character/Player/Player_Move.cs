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

    protected override Vector3 GetInputDir()
    {
        if (!isKB && !isStun)
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
            return new Vector3(h, 0, v);
        }
        else return kbDir;
    }


    protected override bool Jump()
    {
        //1. Check for jump input
        if (Input.GetKeyDown(KeyCode.Space) && charCont.isGrounded && !jumpFrame)
            return true;
        else return false;
    }
}
