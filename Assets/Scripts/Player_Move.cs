using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 9/13/21
 * Script for player movement.
 */

public class Player_Move : Character_Move
{
    public override bool useCameraTransform => true;

  

    protected override Vector3 GetInputDir()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        return new Vector3(h, 0, v);
    }

    protected override void Jump()
    {
        //1. Check for jump input
        if (Input.GetKeyDown(KeyCode.Space) && charCont.isGrounded && !jumpFrame)
        {
            Debug.Log("Jump");
            jumpFrame = true;
        }
    }
}
