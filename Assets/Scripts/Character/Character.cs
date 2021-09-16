using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 9/15/21
 * Base controller script for characters. Contains funcs pertaining to locking movement, etc.
 */

public abstract class Character : MonoBehaviour
{
    protected Character_Move _move;

    protected virtual void Awake()
    {
        _move = GetComponent<Character_Move>();
    }

    /// <summary>
    /// Toggles player input and movement. Gravity is still enacted regardless.
    /// </summary>
    /// <param name="tog"></param>
    public abstract void ToggleInput(bool tog);
}
