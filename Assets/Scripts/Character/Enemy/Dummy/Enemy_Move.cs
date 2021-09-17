using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Move :Fighter_Move
{
    /*
     * Code © Bijan Pourmand
     * Authored 9/15/21
     * Simple move script for basic enemy.
     */

    Player player;
    float followDist = 15f;
    public float distToPlayer;
    Vector3 dirToPlayer;

    bool followPlayer = false;

    protected override void Awake()
    {
        base.Awake();
        //1. Get set player ref
        player = FindObjectOfType<Player>();
        Vector3 posZX = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 playerZX = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        //1. Get distance from self and player on Z, X axis
        distToPlayer = Vector3.Distance(posZX, playerZX);
    }

    protected override bool Jump()
    {
        return false;
    }

    protected override Vector3 GetInputDir()
    {
        if (!isKB)
        {
            Vector3 posZX = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 playerZX = new Vector3(player.transform.position.x, 0, player.transform.position.z);
            //1. Get distance from self and player on Z, X axis
            distToPlayer = Vector3.Distance(posZX, playerZX);
            dirToPlayer = (playerZX - posZX).normalized;

            //2. If player is within distance, go towards player
            if (distToPlayer <= followDist) followPlayer = true;
            else followPlayer = false;
            if (followPlayer) return dirToPlayer;
            else return Vector3.zero;
        }
        else return kbDir;
    }

  






}
