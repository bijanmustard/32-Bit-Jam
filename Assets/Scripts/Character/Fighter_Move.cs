using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 9/16/21
 * Abstract class for Fighter_Move. Contains func for knockback, etc. exclusive to fighter movement.
 */

public abstract class Fighter_Move : Character_Move
{

    public bool isStun;

    public bool isKB = false;
    public Vector3 kbDir = new Vector3(0,1,0.5f);
    public float kbVertVel = 4f;
    public float kbForce = 0;


    protected override void SetMoveVel()
    {
        //1. Set KB speed if knockback
        if (isKB && speed != kbForce) speed = kbForce;
        else if (!isKB && speed != moveSpeed) speed = moveSpeed;
        //2. Call root setMoveVel
        base.SetMoveVel();
    }

   
}
