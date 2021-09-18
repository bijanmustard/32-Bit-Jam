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
    public bool isAttack = false;
    int hitCombo = 0;
    float comboTimer = 0;
    float comboSpan = 0.25f;

    public bool isGuard = false;


    protected override void Awake()
    {
        base.Awake();
        pMove = GetComponent<Player_Move>();
    }

    //ComboTimer is called after attacking to start/reset the combo timer
    protected void ComboTimer()
    {
        comboTimer = 0;
    }

    protected override void MyUpdate()
    {


        //Guard
        //if (isGuard != Input.GetKey(KeyCode.LeftShift)) anim.SetBool("IsGuard", !isGuard);
        //isGuard = Input.GetKey(KeyCode.LeftShift);
        //if (isGuard) pMove.canMove = false;
        //else pMove.canMove = true;


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
                    Attack();
                }
                //3. If punching and on first punch, trigger second punch
                else
                {
                    if (hitCombo == 1)
                    {
                        Debug.Log("Two punch!");
                        Attack();
                    }
                }
            }

        }
        //If timer runs out, cancel attack.
        if(isAttack && comboTimer >= comboSpan)
        {
            EndAttack();
        }
    }
    //Attack is called to launch an attack.
    public void Attack()
    {
        curMethod = AttackHit;
        onInterrupt = EndAttack;
        //0. Set curAction
        curAction = "Attack";
        //1. Set isAttack to true
        isAttack = true;
        //2. Reset attack timer
        comboTimer = 0;
        //3. Increase attackCombo
        hitCombo++;
        //4. LockMovement
        pMove.canMove = false;
        //5. Play anim
        Debug.Log(name + " attacking!");
        anim.SetInteger("Hit_Combo", hitCombo);
    }

    //EndAttack is called to end the attack sequence.
    protected void EndAttack()
    {
        curMethod = null;
        curAction = "Idle";
        //1. set isAttack to false.
        isAttack = false;
        //2. reset timer
        comboTimer = 0;
        //3. reset attackCombo
        hitCombo = 0;
        //4. re-enable movement
        pMove.canMove = true;
        //5. Set anim state
        Debug.Log(name + " is attacking no more...");
        anim.SetInteger("Hit_Combo", hitCombo);
    }

    // HIT FUNCS
    public void AttackHit(Fighter f)
    {
        Debug.Log("Attack landed!");
        f.GetComponent<Fighter_Action>().Knockback(7, 5, (f.transform.position - transform.position).normalized);
        f.UpdateHP(-2);
    }
}
