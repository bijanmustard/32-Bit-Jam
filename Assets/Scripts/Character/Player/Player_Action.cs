using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authroed 9/14/21
 * Action script for player.
 */

public class Player_Action : Fighter_Action
{
    Player_Move pMove;
    Animator anim;
    public bool isAttack = false;
    int hitCombo = 0;
    float comboTimer = 0;
    float comboSpan = 0.25f;

    public bool isGuard = false;

    Hitbox lHand;
    Collider lHand_col;


    private void Awake()
    {
        pMove = GetComponent<Player_Move>();
        anim = GetComponentInChildren<Animator>();
        lHand = GetComponentInChildren<Hitbox>();
        lHand_col = lHand.GetComponent<BoxCollider>();

        //lHand_col.enabled = false;

    }

    //ComboTimer is called after attacking to start/reset the combo timer
    protected void ComboTimer()
    {
        comboTimer = 0;
    }

    private void Update()
    {

        //Guard
        if (isGuard != Input.GetKey(KeyCode.LeftShift)) anim.SetBool("IsGuard", !isGuard);
        isGuard = Input.GetKey(KeyCode.LeftShift);
        if (isGuard) pMove.canMove = false;
        else pMove.canMove = true;


        //0. If is attacking, run combo timer
        if (isAttack) comboTimer += Time.deltaTime;

        // 1. If punch button is pressed...
        if (Input.GetKeyDown(KeyCode.F))
        {
            //If not guarding or airborne..
            if (!isGuard && !pMove.isJumping && pMove.isGrounded)
            {
                //2. If not punching, initiate punch
                if (!isAttack)
                {
                    Debug.Log("One punch!");
                    pMove.canMove = false;
                    isAttack = true;
                    hitCombo++;
                    anim.SetInteger("Hit_Combo", hitCombo);
                    ComboTimer();
                }
                //3. If punching and on first punch, trigger second punch
                else
                {
                    if (hitCombo == 1)
                    {
                        Debug.Log("Two punch!");
                        hitCombo++;
                        anim.SetInteger("Hit_Combo", hitCombo);
                        ComboTimer();
                    }
                }
            }

        }

        //If there are contacts, do stuff
        if (lHand.IsContact)
        {
            foreach(KeyValuePair<GameObject,Vector3> kvp in lHand.contacts)
            {
                
            }
        }

        //If timer runs out, cancel attack.
        if(isAttack && comboTimer >= comboSpan)
        {
            Debug.Log("Combo over...");
            pMove.canMove = true;
            isAttack = false;
            comboTimer = 0;
            hitCombo = 0;
            anim.SetInteger("Hit_Combo", hitCombo);
        }
    }
}
