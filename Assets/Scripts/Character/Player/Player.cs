using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 9/15/21
 * Controller script for player.
 */

[RequireComponent(typeof(Player_Move))]
[RequireComponent(typeof(Player_Action))]
public class Player : Fighter
{
    private static Player current;
    public static Player Current => current;
    public Player_Move pMove => (Player_Move)_move;
    public Player_Action pAction => GetComponent<Player_Action>();

    //Inventory
    protected Inventory myInventory;

    protected override void Awake()
    {
        base.Awake();
        if (current != null && current != this) Destroy(gameObject);
        else current = this;
        //DontDestroyOnLoad(gameObject);
    }







}

