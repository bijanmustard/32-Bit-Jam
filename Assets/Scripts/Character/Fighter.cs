using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Code © Bijan Pourmand
 *  Authored 9/15/21
 *  Base script for fighter character controller.
 */

public abstract class Fighter : Character
{
    protected Fighter_Action _action;
    public Fighter_Move move => (Fighter_Move)_move;

    public float hp = 10;
    public float maxHP = 100;

    protected override void Awake()
    {
        base.Awake();
        _action = GetComponent<Fighter_Action>();
    }

    public override void ToggleInput(bool tog)
    {
        _move.canMove = tog;
        _action.enabled = tog;
    }

    //UpdateHP updates HP by a given integer.
    public void UpdateHP(int inc)
    {
        hp = Mathf.Clamp(hp + inc, 0, maxHP);
        if (hp == 0) Die();
    }

    //Die is called to trigger death events, such as animation change, toggles, etc.
    public virtual void Die()
    {
        Debug.Log($"{name} defeated!");
    }


}
