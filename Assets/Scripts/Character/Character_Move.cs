using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code � Bijan Pourmand
 * Authored 9/13/21
 * Base script for character movement.
 */

public abstract class Character_Move : MonoBehaviour
{
    //Vars & Refs
    protected Collider col;
    protected CharacterController charCont;
    protected Animator anim;

    protected float h, v;
    public float vertVel;
    public Vector3 moveDir, moveVel;

    public bool isVelOverride = false;
    public Vector3 velOverride = Vector3.zero;

    protected float? speed = null;
    public float moveSpeed = 5f;
    public float gravity = 3f;
    public float jumpHeight = 5f;
    public float rotateSpeed = 7f;

    public bool freeze = false;

    public bool isGrounded => charCont.isGrounded;

    /// <summary> Trigger bool for jump./// </summary>
    protected bool isJump;
    /// <summary> Is True while waiting for jump to process. /// </summary>
    protected bool jumpFrame = false;
    public bool isJumping => jumpFrame;

    public bool useGravity = true;
    public bool canMove = true;
    protected bool lockMove = false;

    public bool restrictX, restrictY, restrictZ;
    public bool rotToDir = true;
    [SerializeField]
    protected bool useCameraTransform = false;

    protected virtual void Awake()
    {
       
        Application.targetFrameRate = 30; //DSLFKJSDF:LKSJF:LSKJDF:SODLKFJ; REMOVE THIS
        col = GetComponent<BoxCollider>();
        charCont = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        if (speed == null) speed = moveSpeed; 
    }

    /// <summary>
    /// Returns a Vector3 from the current move axis input.
    /// </summary>
    protected virtual Vector3 GetInputDir()
    {
        return Vector3.zero;
    }

    protected virtual void SetMoveVel()
    {
        //1. Set moveVel base
        moveVel = moveDir;
        //2. Set x, z speed
        moveVel.x *= (float)speed; 
        moveVel.z *= (float)speed; 
        //3. Set vert vel
        moveVel.y = vertVel;
        //4. Multiply by time.deltaTime
        moveVel *= Time.deltaTime;
    }

    ///<summary>Called to check for jump input and call jump funciton. Returns true if jump confirmed.</summary>
    protected virtual bool Jump() { return false; }
 

    /// <summary> Applys gravity to vertVel. </summary>
    protected virtual void Gravity()
    {
        ////1. If gravity is enabled..
        if (useGravity)
        {
            //1a. If not jump frame..
            if (!jumpFrame)
            {
                //2.If is grounded, apply grounded vel; else gravity
                if (charCont.isGrounded) vertVel = -1;
                else vertVel -= gravity * Time.deltaTime;
            }
        }
    }

    

    // Update is called once per frame
    protected void Update()
    {
        //0. If not freeze and gameScale > 0...
        if (!GetComponent<Character>().isFreeze  && !PauseController.IsPause && GameController.gameTimeScale > 0)
        {

            //1. Get Input Direction
            if (!lockMove) moveDir = (canMove ? GetInputDir() : Vector3.zero);

            //1a. Constrain moveDir
            Restrict(ref moveDir);
            //1b. If transform by camera, transform moveDir by camera
            if (useCameraTransform) moveDir = CameraTransformDir(moveDir);

            //2. Check for jump
            jumpFrame = Jump();
            if (jumpFrame) vertVel = jumpHeight;

            //3. Set gravity
            Gravity();

            //4. Set moveTo velocity
            if (isVelOverride) moveVel = velOverride;
            else SetMoveVel();

            //5. If rotToDir enabled, rotate towards moveDir
            if (rotToDir && moveDir != Vector3.zero)
                transform.rotation = RotateTo(transform.rotation, new Vector3(moveVel.x, 0, moveVel.z), transform.up, rotateSpeed);

            //6. MovePosition
            if (charCont != null) charCont.Move(moveVel);
            //5a. Reset jump frame
            jumpFrame = false;
        }
        else Debug.Log($"{name} can't move!{!GetComponent<Character>().isFreeze}, {GameController.gameTimeScale > 0}, {!PauseController.IsPause} ");
    }





    /// <summary> Lerps a quaternion to a new forward direction. </summary>
    /// <param name="curRot"></param>
    /// <param name="newFwd"></param>
    /// <param name="newUp"></param>
    /// <param name="speed"></param>
    /// <returns></returns>
    public static Quaternion RotateTo(Quaternion curRot, Vector3 newFwd, Vector3 newUp, float speed)
    {
        //5. if moveDir != 0, rotate towards moveDir
        
            return Quaternion.Lerp(curRot, Quaternion.LookRotation(newFwd, newUp), speed * Time.deltaTime);

        //var norm = Quaternion.FromToRotation(transform.up, normal);
        //transform.rotation = norm * transform.rotation;
    }
    /// <summary> Transforms a direction relative to the main camera, ignoring X-rotation.</summary>
    /// <param name="dir"></param> <returns></returns>
    public static Vector3 CameraTransformDir(Vector3 dir)
    {
        Vector3 camDir = Vector3.zero;
        //3a. Get forward based on y rotation of camera based as direction vector
        Vector3 fwd = Quaternion.Euler(new Vector3(0, Camera_Controller.curCamera.transform.eulerAngles.y, 0)) * Vector3.forward;
        //3b. forward relative to camera
        camDir += dir.z * fwd;
        //3c. horizontal relative to camera
        camDir += dir.x * Camera_Controller.curCamera.transform.right;
        return camDir;
    }

    /// <summary> Constrains an input vector by the Character_Move's axis constraints.</summary>
    /// <param name="input"></param>
    protected void Restrict(ref Vector3 input)
    {
        //1d. Restrict moveDir movement
        if (restrictX) input.x = 0;
        if (restrictY) input.y = 0;
        if (restrictZ) input.z = 0;
    }

    /// <summary> Called to lock/unlock move direcion.</summary>
    /// <param name="tog"></param>
    public void LockDir(bool tog) {lockMove = tog;}

    /// <summary>Toggles whether or not the player can move via controller input. Gravity still runs. </summary>
    /// <param name="tog"></param>
    public void ToggleMoveDir(bool tog) {canMove = tog;}

    /// <summary> Toggles restraint on X-Axis movement. </summary>
    /// <param name="tog"></param>
    public void RestrictX(bool tog) { restrictX = tog; }

    /// <summary> Toggles restraint on Y-Axis movement. </summary>
    /// <param name="tog"></param>
    public void RestrictY(bool tog) { restrictY = tog; }

    /// <summary> Toggles restraint on Z-Axis movement. </summary>
    /// <param name="tog"></param>
    public void RestrictZ(bool tog) { restrictZ = tog; }

    /// <summary> Toggles restraint on all axis movement. </summary>
    /// <param name="tog"></param>
    public void ResetrictAll(bool tog)
    {
        RestrictX(tog);
        RestrictY(tog);
        RestrictZ(tog);
    }
}
