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

    Coroutine comboIE;
    int hitCombo = 0;
    float comboTimer = 0;
    float comboSpan = 0.15f;
    float normalizedTimerStart = 2f;
    public bool canChain = false;

    public bool isGuard = false;
    public bool isCrouch = false;


    protected override void Awake()
    {
        base.Awake();
        pMove = GetComponent<Player_Move>();
    }


    //IE for hit combo
    IEnumerator AttackComboIE()
    {
        Attack();
        //1. While is attacking...
        while (isAttack)
        {
            yield return null;
            //2. Once animation is finished, increment combo timer
            if ((!IsAnimationFinished() && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > normalizedTimerStart && hitCombo <=2)
                || IsAnimationFinished())
            {
                //2a. Set canChain bool
                if (!canChain)
                {
                    //Debug.Log($"Re-Enabling canChain at normalized {anim.GetCurrentAnimatorStateInfo(0).normalizedTime}." +
                    //    $"Was animation finished? {IsAnimationFinished()}");
                    canChain = true;
                }
                if (canChain)
                {
                    comboTimer += Time.deltaTime;
                }

                //3. If combo timer ends, end attack
                if (comboTimer >= comboSpan && IsAnimationFinished())
                {
                    //Debug.Log($"Chain timer ended at time {comboTimer}");
                    EndAttack();
                    Debug.Log("Attack End");
                }
            }
        }
    }

    //IE for normal attacks (e.g. crouch)
    IEnumerator AttackIE()
    {
        while (isAttack)
        {
            yield return null;
            Debug.Log($"Waiting for {curAction} to finish...");
            if (!IsAnimationFinished()) yield return null;
            else EndAttack();

        }
    }

    protected override void MyUpdate()
    {

        DEBUG();

        //If not guarding or airborne..
        if (!isGuard && !pMove.isJumping && pMove.isGrounded)
        {
            // 1. If punch button is pressed...
            if (Input.GetKeyDown(KeyCode.F))
            {

                //2. If not punching, initiate punch
                if (!isAttack)
                {
                    //2a. If not crouching, punch attack
                    if (!isCrouch)
                    {
                        Debug.Log("One punch!");
                        comboIE = StartCoroutine(AttackComboIE());

                    }
                    //2b. Else if crouching, crouch attack
                    else
                    {
                        //2b.a. End routine
                        CrouchAttack();
                        comboIE = StartCoroutine(AttackIE());
                    }

                }
                //3. Else if already attacking..
                else
                {
                    // 3a. If player hasn't reached max attack move and is within chain input window, trigger attack
                    if (canChain && hitCombo < 3)
                    {
                        Debug.Log("Two punch!");
                        Attack();
                        
                    }
                }

            }

            // 4. If crouch button is pressed...
            if (Input.GetKey(KeyCode.C))
            {
                // 4a. If Crouch conditions are met...
                if ((!pMove.IsMoving || pMove.IsWalking))
                {
                    // 4b. If not attacking and not crouched yet, enable crouch
                    if ((!isAttack && !isCrouch) || isCrouch) isCrouch = true;
                }
            }
            else isCrouch = false;

        }
    }

    public void DEBUG()
    {
        //1. Hit
        if (Input.GetKeyDown(KeyCode.H)) Hit(2);
    }

    protected override void UpdateAnimation()
    {
        base.UpdateAnimation();
        anim.SetBool("IsCrouch", isCrouch);

    }

    //Attack is called to launch an attack.
    public void Attack()
    {
        onInterrupt = EndAttack;
        //0. Set curAction
        curAction = "Attack";
        //1. Set isAttack to true
        isAttack = true;
        //2. Reset attack timer
        comboTimer = 0;
        canChain = false;
        //3. Increase attackCombo
        hitCombo++;
        if (hitCombo <= 2) curMethod = AttackHit;
        else curMethod = FrontKickHit;
        //4. LockMovement
        pMove.canMove = false;
        //5. Play anim
        anim.SetInteger("Hit_Combo", hitCombo);
    }

    public void CrouchAttack()
    {
        curMethod = AttackHit;
        onInterrupt = EndAttack;
        curAction = "CrouchAttack";
        isAttack = true;
    }

    //EndAttack is called to end the attack sequence.
    protected void EndAttack()
    {
        curMethod = null;
        curAction = "Idle";
        //1. set isAttack to false.
        isAttack = false;
        canChain = false;
        //2. reset timer
        comboTimer = 0;
        //3. reset attackCombo
        hitCombo = 0;
        //4. re-enable movement
        pMove.canMove = true;
        //5. Set anim state
        anim.SetInteger("Hit_Combo", hitCombo);
        //6. unToggle isAttack
        isAttack = false;
        //7. Stop IE
        StopCoroutine(comboIE);
        hbxController.ToggleAllHitboxes(0);

    }

    // HIT FUNCS
    public void AttackHit(Fighter f, Hitbox hbx)
    {
        Debug.Log("Attack landed!");
        //f.GetComponent<Fighter_Action>().Knockback(7, 5, (f.transform.position - transform.position).normalized);
        f.GetComponent<Fighter_Action>().Hit(1);
    }

    public void FrontKickHit(Fighter f, Hitbox hbx)
    {
        Debug.Log("Front kick!");
        //1. Deal damage and knockback
        f.GetComponent<Fighter_Action>().Hit(2);
        f.GetComponent<Fighter_Action>().Knockback(3, 5, transform.forward);
    }
}
