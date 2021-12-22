using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 12/22/21
 * Script for Santa player movement
 */

public class Santa_Move : Character_Move
{
    bool isCrouch = false;

    //Anim bools  
    bool isMoving, isWalk, isRun;
    protected override Vector3 GetInputDir()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        //If crouching, limit input axis to half
        if (isCrouch)
        {
            h = Mathf.Clamp(h, -0.5f, 0.5f);
            v = Mathf.Clamp(v, -0.5f, 0.5f);
        }
        return new Vector3(h, 0, v);
    }

    protected override void Update()
    {
        //1. Set crouch
        isCrouch = Input.GetKey(KeyCode.LeftShift);
        //2. Base.Update
        base.Update();
        
    }

    protected override void SetMoveVel()
    {     
        base.SetMoveVel();
    }

    protected override void UpdateAnimation()
    {
        isMoving = inputDir != Vector3.zero;
        isWalk = isMoving && (Mathf.Abs(inputDir.x) <= 0.5f && Mathf.Abs(inputDir.z) <= 0.5f);
        isRun = isMoving && (Mathf.Abs(inputDir.x) > 0.5f || Mathf.Abs(inputDir.z) > 0.5f);
        anim.SetBool("IsCrouch", isCrouch);
        anim.SetBool("IsMoving", isMoving);
        anim.SetBool("IsWalking", isWalk);
        anim.SetBool("IsRunning", isRun);
    }
}
