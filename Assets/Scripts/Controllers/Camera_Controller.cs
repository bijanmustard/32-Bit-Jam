using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Code © Bijan Pourmand
 * Authored 9/13/21
 * Script for Camera Controller, controls camera movement and mode
 */

public class Camera_Controller : MonoBehaviour
{

    public static Camera curCamera;
    public static Transform target;

    public static CamMode mode = CamMode.Follow;

    Vector3 moveTo;
    Quaternion rotTo;

    public static Vector3 pivotFwd = Vector3.forward;

    static bool lockXMove, lockYMove, lockZMove;
    static bool lockXRot, lockYRot, locZRot;

    public static bool useSticky;
    public static Vector3? curSticky = null;
    public static bool isLookAt = false;

    public static Vector3 camFollow_offset = new Vector3(0, 1.5f, -5);
    public static Quaternion camRotation = Quaternion.identity;
    public static float camFollow_speed = 20f;

    // Start is called before the first frame update
    static Camera_Controller()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Awake()
    {
        if (curCamera == null) curCamera = Camera.main;
        Debug.Log(curCamera);
    }

    private void Start()
    {
        
        if (target == null) target = Player.Current.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!PauseController.IsPause)
        {
            //1. Move to position
            if (mode == CamMode.Follow)
            {
                moveTo = pivotFwd * camFollow_offset.z;
                moveTo.y += camFollow_offset.y;
                moveTo += target.position;
                rotTo = camRotation;

            }

            //2. If cam mode is static...

            if (mode == CamMode.Static)
            {
                if (useSticky) moveTo = (Vector3)curSticky;
                if (isLookAt)
                {
                    rotTo = Quaternion.LookRotation((target.position - transform.position).normalized, Vector3.up);
                }
            }

            curCamera.transform.position = Vector3.Slerp(transform.position, moveTo, camFollow_speed * Time.smoothDeltaTime);
            curCamera.transform.rotation = Quaternion.Slerp(transform.rotation, rotTo, 3 * Time.smoothDeltaTime);
        }
    }

    static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //1. Set targets
        target = Player.Current.transform;
        //2. Init curCamera as camera main.
        curCamera = Camera.main;
        //1a. If camera.main doesn't have camera controller, attach camera controller
        if (Camera.main.GetComponent<Camera_Controller>() == null) Camera.main.gameObject.AddComponent<Camera_Controller>();
        //2b. Enab
        else Camera.main.GetComponent<Camera_Controller>().enabled = true;
    }
}

public enum CamMode
{
    Static,
    Follow,
    Free
}