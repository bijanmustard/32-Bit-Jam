using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 12/21/21
 * Camera Controller script. Handles various different cam modes.
 */

public class CameraController : MonoBehaviour
{

    static CameraController controller;

    //Ref to main camera
    public static Camera main => mainCam;
    static Camera mainCam;

    //current cam mode
    static CamMode mode = CamMode.Static;

    //Shared attributes
    static bool lookAtTarget = false;

    //Focus target refs
    static Transform focusTarget;
    static Vector3 focusPoint;
    static float focusDist;
    static Vector3 focusOffset;
    static Quaternion focusAngle;
    static bool followLocalSpace;

    



    private void Awake()
    {
        //1. Singleton
        if (controller != null && controller != this) Destroy(gameObject);
        else controller = this;
        //2. Set main cam ref
        mainCam = GetComponent<Camera>();
        //3. Disable all other cameras in scene
        foreach (Camera cam in FindObjectsOfType<Camera>())
        {
            if (cam != mainCam) cam.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        Transform tempTarget = Player.Current.transform;//GameObject.Find("Platforms").transform.Find("Cube");
        SetFocus(tempTarget, 3, Quaternion.LookRotation(-Vector3.forward), new Vector3(0,1), false);
    }

    private void LateUpdate()
    {
        //1. If cam mode is follow, follow target
        if (mode == CamMode.Focus)
        {
            if (focusTarget == null) mode = CamMode.None;
            else
            {
                //Get focus position
                focusPoint = focusTarget.transform.position + focusOffset;
                Vector3 direction = (focusAngle * Vector3.forward) * focusDist;
                Vector3 goToPos = focusPoint + direction;
                //move to pos
                transform.position = Vector3.Lerp(transform.position, goToPos, Time.deltaTime * 15f);
                //Look at target
                Quaternion rotTo = Quaternion.LookRotation((focusPoint - transform.position));
                transform.rotation = Quaternion.Slerp(transform.rotation, rotTo, 5f * Time.deltaTime);
            }
        }
    }

    //SetFollow sets a focus transform and switches the cam mode to focus.
    public static void SetFocus(Transform focus, float distance, Quaternion angle, Vector3 offset, bool useLocalSpace)
    {
        //1. If controller is null, return
        if (controller == null) return;
        if (focus == null) return;
        //2. Set transform
        focusTarget = focus;
        focusDist = distance;
        focusAngle = angle;
        focusOffset = offset;
        followLocalSpace = useLocalSpace;
        lookAtTarget = true;

        //3. Set mode accordingly
        if (mode != CamMode.Focus) mode = CamMode.Focus;
    }

    #region HelperFunctions
    /// <summary> Transforms a direction relative to the main camera, ignoring X-rotation.</summary>
    /// <param name="dir"></param> <returns></returns>
    public static Vector3 CameraTransformDir(Vector3 dir)
    {
        //1. Setup base vars
        Vector3 camDir = Vector3.zero;
        //3a. Get forward based on y rotation of camera based as direction vector
        Vector3 fwd = Quaternion.Euler(new Vector3(0, main.transform.eulerAngles.y, 0)) * Vector3.forward;
        //3b. forward relative to camera
        camDir += dir.z * fwd;
        //3c. horizontal relative to camera
        camDir += dir.x * main.transform.right;
        return camDir;
    }
    #endregion
}

//Enum for cam mode
public enum CamMode
{
    None,
    Static,
    Follow,
    Focus,
    LockOn,
    Free
}
