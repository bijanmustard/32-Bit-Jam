using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 9/13/21
 * Base script for character movement.
 */

public abstract class Character_Move : MonoBehaviour
{
    //Vars & Refs
    protected Collider col;
    protected CharacterController charCont;

    protected float h, v;
    protected float vertVel;
    protected Vector3 moveDir, moveVel, moveTo;

    public float speed = 5f;
    public float gravity = 3f;
    public float jumpHeight = 5f;
    public float rotateSpeed = 7f;

    public bool isGrounded => charCont.isGrounded;
    protected bool jumpFrame = false;
    public bool isJumping => jumpFrame;
    public bool useGravity = true;

    public bool canMove = true;
    protected bool lockMove = false;


    public abstract bool useCameraTransform { get; }

    protected virtual void Awake()
    {
       
        Application.targetFrameRate = 30; //DSLFKJSDF:LKSJF:LSKJDF:SODLKFJ; REMOVE THIS
        col = GetComponent<BoxCollider>();
        charCont = GetComponent<CharacterController>();
    }



    /// <summary>
    /// Returns a Vector3 from the current move axis input.
    /// </summary>
    protected abstract Vector3 GetInputDir();

    /// <summary>
    /// Checks for Jump input and sets velocity accordingly.
    /// </summary>
    protected virtual void Jump() { }

    // Update is called once per frame
    void Update()
    {
        // 1. Get direction input
        if (!canMove) moveDir = Vector3.zero;
        else if (!lockMove) moveDir = GetInputDir();   

        //2. Set moveTo
        moveTo = new Vector3(0, 0, 0);

        //3. if cameraTransform is enable, set moveTo relative to camera
        if (useCameraTransform)
        {
            //3a. Get forward based on y rotation of camera based as direction vector
            Vector3 fwd = Quaternion.Euler(new Vector3(0, Camera.main.transform.eulerAngles.y, 0)) * Vector3.forward;
            //3b. forward relative to camera
            moveTo += moveDir.z * fwd;
            //3c. horizontal relative to camera
            moveTo += moveDir.x * Camera.main.transform.right;
        }
        else moveTo = moveDir;

        //4. Apply speed to moveTo
        moveTo.x *= speed * Time.deltaTime;
        moveTo.z *= speed * Time.deltaTime;

        //5. Check for jump
        if(canMove)Jump();
        if (jumpFrame) vertVel = jumpHeight * Time.deltaTime; ;

        //6. Set gravity
        if (useGravity)
        {
            //6a. If not jump frame..
            if (!jumpFrame)
            {
                //6b. If is grounded, apply grounded vel; else gravity
                if (charCont.isGrounded) vertVel = -1;
                else vertVel -= gravity * Time.deltaTime;
            }
        }

        moveTo.y = vertVel;

        //5. if moveDir != 0, rotate towards moveDir
        if (moveDir != Vector3.zero)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(new Vector3(moveTo.x, 0, moveTo.z), transform.up), rotateSpeed * Time.deltaTime);   
        }
        //var norm = Quaternion.FromToRotation(transform.up, normal);
        //transform.rotation = norm * transform.rotation;

        //5. MovePosition

        if (charCont != null) charCont.Move(moveTo);
        // Reset jump frame
        jumpFrame = false;
    }


    /// <summary>
    ///Called to lock/unlock move direcion.
    /// </summary>
    /// <param name="tog"></param>
    public void LockDir(bool tog) {lockMove = tog;}
    /// <summary>
    /// Toggles whether or not the player can move via controller input. Gravity still runs.
    /// </summary>
    /// <param name="tog"></param>
    public void ToggleMove(bool tog) {canMove = tog;}
}
