using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 9/15/21
 * Base script for fighter actions.
 */

public abstract class Fighter_Action : MonoBehaviour
{
    public string curAction;
    public delegate void ActionMethod(Fighter f);
    public ActionMethod curMethod;

    /// <summary> Dictionary mapping action strings to their on hit functions. /// </summary>
    public Dictionary<string, Action> hitMap= new Dictionary<string, Action>();

    protected Hitbox_Controller hbxController;

    protected virtual void Awake()
    {
        hbxController = GetComponentInChildren<Hitbox_Controller>();
    }
    
}
