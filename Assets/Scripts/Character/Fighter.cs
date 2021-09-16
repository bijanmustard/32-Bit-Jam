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

    public float hp = 10;

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
}
