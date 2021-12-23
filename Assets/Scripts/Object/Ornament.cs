using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 12/22/2021
 * Script for Ornament projectiles
 */

public class Ornament : Sprite_Obj
{

    public bool isThrown = false;

    Collider col;

    public Vector3 moveDir = Vector3.right;
    static float speed = 0.8f;
    static float gravity = 0.05f;
    float curGravity = 0.1f;
    Vector3 movePos;

    protected void Awake()
    {
        col = GetComponent<Collider>();
    }

    protected void Update()
    {
        if (isThrown)
        {
            //1. Move ornament
            curGravity -= gravity;
            movePos = transform.position + (moveDir * speed) + new Vector3(0, curGravity, 0);
            transform.position = movePos;
        }
    }

    //Throw is called to set the ornament in motion.
    public void Throw(Vector3 dir)
    {
        if (!isThrown)
        {
            moveDir = dir;
            isThrown = true;
        }
    }

    protected override void LateUpdate()
    {
        //1. Update offset
        rotationOffset += new Vector3(0, 0, 5);
        if (rotationOffset.z >= 360) rotationOffset = new Vector3(rotationOffset.x, rotationOffset.y, 360 - rotationOffset.z);
        base.LateUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        //1. If trigger collides with something, destroy
        if (other.tag != "Player")
            Destroy(gameObject);

    }
}
