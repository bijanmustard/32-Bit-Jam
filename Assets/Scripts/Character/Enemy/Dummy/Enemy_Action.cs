using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 9/15/21
 * Basic script for enemy action.
 */

public class Enemy_Action : Fighter_Action
{
    Enemy_Move eMove;

    float attackDist = 1f;
    int attackCt = 0;

    float attackCooldown = 2f;
    float attackCooldown_timer;

    Coroutine curAttackIE;


    protected override void Awake()
    {
        base.Awake();
        eMove = GetComponent<Enemy_Move>();
        //1. Start in Wait
        //Wait();
    }

    // Update is called once per frame
    protected override void MyUpdate()
    {
        if (!eMove.isKB && !isStun && canAttack)
        {
            Debug.Log($"EnemyAction: Is {eMove.distToPlayer} < {attackDist}? {eMove.distToPlayer < attackDist}");
            if (eMove.distToPlayer <= attackDist)
            {
                if (!isAttack)
                {
                    if (curAttackIE != null) StopCoroutine(curAttackIE);
                    curAttackIE = StartCoroutine(AttackIE());

                }
            }
        }

    }

    IEnumerator AttackIE()
    {
        Attack();
        yield return null;
        while (!IsAnimationFinished()) yield return null;
        EndAttack();
        Wait();
        while(attackCooldown_timer < attackCooldown)
        {
            attackCooldown_timer += Time.deltaTime;
            yield return null;
        }
        EndWait();
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
        attackCooldown_timer = 0;
        //3. Increase attackCombo
        attackCt++;
        //4. LockMovement
        eMove.canMove = false;
        //5. Play anim
        Debug.Log(name + " attacking!");
        anim.SetInteger("Hit_Combo", attackCt);
    }

    //EndAttack is called to end the attack sequence.
    protected void EndAttack()
    {
        curMethod = null;
        curAction = "Idle";
        //1. set isAttack to false.
        isAttack = false;
        //2. reset timer
        attackCooldown_timer = 0;
        //3. reset attackCombo
        attackCt = 0;
        //4. re-enable movement
        eMove.canMove = true;

        //5. Set anim state
        Debug.Log(name + " is attacking no more...");
        anim.SetInteger("Hit_Combo", attackCt);

        hbxController.ToggleAllHitboxes(0);
    }

    public void Wait()
    {
        onInterrupt = EndWait;
        curAction = "Wait";
        isAttack = false;
        eMove.canMove = false;
        canAttack = false;
    }

    protected void EndWait()
    {
        onInterrupt = null;
        curAction = null;
        eMove.canMove = true;
        canAttack = true;
    }

    // HIT FUNCS
    public void AttackHit(Fighter f, Hitbox hbx)
    {
        f.GetComponent<Fighter_Action>().Hit(2);
        
    }
}
