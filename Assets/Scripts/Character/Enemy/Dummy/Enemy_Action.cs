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
    Animator anim;

    float attackDist = 2f;
    bool isAttack;
    int attackCt = 0;

    float attackCooldown = 2f;
    float attackCooldown_timer;

   

    protected override void Awake()
    {
        base.Awake();
        eMove = GetComponent<Enemy_Move>();
        anim = GetComponentInChildren<Animator>();

       
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(eMove.distFromPlayer <= attackDist)
        {
            if (!isAttack)
            {
                Attack();
            }
        }

        // 2. If is attacking, update timer
        if (isAttack)
        {
            attackCooldown_timer += Time.deltaTime;
            //2a. if timer reaches limit, stop attack seq
            if(attackCooldown_timer >= attackCooldown)
            {
                EndAttack();
            }
        }
    }

    //Attack is called to launch an attack.
    public void Attack()
    {
        curMethod = AttackHit;
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
    }

    // HIT FUNCS
    public void AttackHit(Fighter f)
    {
        Debug.Log("Attack landed!");
    }
}
