using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 12/22/21
 * Script for Santa player movement
 */

public class Santa_Move : Character_Move
{
    bool isCrouch = false;
    bool isOrnamentAim = false;

    public LayerMask mask;

    Vector3 cursorOffset = Vector3.zero;
    Vector3 cursorPos;
    float cursorMinDist = 3f, cursorMaxDist = 7f;

    //obj refs
    static string fp_ornament = "Objects/Ornament";
    static GameObject obj_ornament;

    protected override void Awake()
    {
        //1. Set obj refs
        if (obj_ornament == null) obj_ornament = Resources.Load<GameObject>(fp_ornament);
        base.Awake();
    }

    //Anim bools  
    bool isMoving, isWalk, isRun;
    protected override Vector3 GetInputDir()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        //If crouching, limit input axis to half
        if (isCrouch)
        {
            h = Mathf.Clamp(h, -0.5f, 0.5f);
            v = Mathf.Clamp(v, -0.5f, 0.5f);
        }
        return new Vector3(h, 0, v);
    }

    protected override void Update()
    {
        //1. Set crouch
        isCrouch = Input.GetKey(KeyCode.LeftShift);


        //2. ornament toss behavior
        //If button is held..
        if (Input.GetKey(KeyCode.E))
        {
            //Lock movement
            canMove = false;
            //If this is frame button was pressed, set cursorPos to default
            if (Input.GetKeyDown(KeyCode.E))
            {
                cursorOffset = (transform.forward * cursorMinDist);
                cursorPos = transform.position + cursorOffset;
                isOrnamentAim = true;
            }

            inputDir = GetInputDir();
            moveVel = (useCameraTransform ? CameraController.CameraTransformDir(inputDir) : inputDir);
            if(inputDir != Vector3.zero) transform.rotation = RotateTo(transform.rotation, new Vector3(moveVel.x, 0, moveVel.z), transform.up, rotateSpeed);

            //Update cursorPos based on input
            cursorOffset += moveVel * 0.5f;
            cursorOffset.x = Mathf.Clamp(cursorOffset.x, -cursorMaxDist, cursorMaxDist);
            cursorOffset.z = Mathf.Clamp(cursorOffset.z, -cursorMaxDist, cursorMaxDist);
            cursorPos = transform.position + cursorOffset;

            //Cast ray to find point of collision 
            Vector3 origin = transform.TransformPoint(new Vector2(0, 4));
            RaycastHit hit;
            Physics.Linecast(origin, cursorPos, out hit, mask);
            if (hit.collider != null) cursorPos = hit.point;

            //Put marker at position
            GameObject.Find("Ornament").transform.position = cursorPos;
            CameraController.focusOffset.z = Vector3.Distance(transform.position, cursorPos)/4;
        }

        else if(isOrnamentAim && Input.GetKeyUp(KeyCode.E))
        {
            canMove = true;
            isOrnamentAim = false;
            CameraController.focusOffset.z = 0;
            //Fire ornament
            FireOrnament(Quaternion.AngleAxis(-20,Vector3.right) * transform.forward);
        }

        //2. Base.Update
        if (canMove) base.Update();


    }

    protected override void SetMoveVel()
    {     
        base.SetMoveVel();
    }

    protected override void UpdateAnimation()
    {
        isMoving = inputDir != Vector3.zero;
        isWalk = isMoving && (Mathf.Abs(inputDir.x) <= 0.5f && Mathf.Abs(inputDir.z) <= 0.5f);
        isRun = isMoving && (Mathf.Abs(inputDir.x) > 0.5f || Mathf.Abs(inputDir.z) > 0.5f);
        anim.SetBool("IsCrouch", isCrouch);
        anim.SetBool("IsMoving", isMoving);
        anim.SetBool("IsWalking", isWalk);
        anim.SetBool("IsRunning", isRun);
    }

    //FireOrnament is called to throw an ornament.
    public void FireOrnament(Vector3 direction)
    {
        //1. Instantiate ornament
        Ornament obj = Instantiate(obj_ornament).GetComponent<Ornament>();
        obj.transform.position = transform.TransformPoint(new Vector3(0, 2, 0));
        obj.Throw(direction);
    }
}
