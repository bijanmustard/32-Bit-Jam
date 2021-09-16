using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Code © Bijan Pourmand
 * Authored 9/15/21
 * Controller script for basic enemy.
 */

[RequireComponent(typeof(Enemy_Action))]
[RequireComponent(typeof(Enemy_Move))]
public class Enemy : Fighter
{
    public Enemy_Move move => (Enemy_Move)_move;
    public Enemy_Action action => (Enemy_Action)_action;
}
