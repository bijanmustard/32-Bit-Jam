using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Move :Fighter_Move
{
    /*
     * Code ? Bijan Pourmand
     * Authored 9/15/21
     * Simple move script for basic enemy.
     */

    float followDist = 10f;
    public float distToPlayer;
    Vector3 dirToPlayer;

    bool followPlayer = false;

    protected override void Awake()
    {
        base.Awake();
        //1. Get set player ref
        //Vector3 posZX = new Vector3(transform.position.x, 0, transform.position.z);
        //Vector3 playerZX = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        //1. Get distance from self and player on Z, X axis
        //distToPlayer = Vector3.Distance(transform.position, Player.Current.transform.position);
    }

    protected override bool Jump()
    {
        return false;
    }

    protected override void Update()
    {
        //1. Get distance from self and player on Z, X axis
        dirToPlayer = (Player.Current.transform.position - transform.position).normalized;
        distToPlayer = Vector3.Distance(transform.position, Player.Current.transform.position);
        base.Update();
    }

    protected override Vector3 GetInputDir()
    {
        if (!isKB && !isStun)
        {         
            //2. If player is within distance, go towards player
            if (distToPlayer <= followDist) followPlayer = true;
            else followPlayer = false;
            if (followPlayer) return dirToPlayer;
            else return Vector3.zero;
        }
        else
        {
            Debug.Log("Is not KB and Stun: " + !isKB + " " + !isStun);
            if (isKB) return kbDir;
            else return Vector3.zero;
        }
    }

  






}
