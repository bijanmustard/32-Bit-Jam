using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Code © Bijan Pourmand
 * Authored 9/13/21
 * Script for WorldCamera, controls camera movement and mode
 */

public class WorldCamera : MonoBehaviour
{

    private static WorldCamera instance;
    public static WorldCamera Current => instance;
    //public static List<WorldCamera> camsInScene = new List<WorldCamera>();

    public CameraSettings myCamSettings = new CameraSettings();

    Vector3 moveTo;
    Quaternion rotTo;


    private void Awake()
    {
        //1. Set singleton
        if (instance != null && instance != this) Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

    }

    private void Start()
    { 
        //if (target == null) target = Player.Current.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!PauseController.IsPause)
        {
            //1. Move to position
            if (myCamSettings.mode == CamMode.Follow)
            {
                moveTo = myCamSettings.pivotFwd * myCamSettings.camFollow_offset.z;
                moveTo.y += myCamSettings.camFollow_offset.y;
                moveTo += myCamSettings.target.position;
                rotTo = myCamSettings.camRotation;

            }

            //2. If cam mode is static...

            if (myCamSettings.mode == CamMode.Static)
            {
                moveTo = (Vector3)myCamSettings.staticPoint;
                if (myCamSettings.isLookAt)
                {
                    rotTo = Quaternion.LookRotation((myCamSettings.target.position - transform.position).normalized, Vector3.up);
                }
            }

            transform.position = Vector3.Slerp(transform.position, moveTo, myCamSettings.camFollow_speed * Time.smoothDeltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotTo, 3 * Time.smoothDeltaTime);
        }
    }
  

}

[System.Serializable]
public class CameraSettings
{

    public Transform target;
    public Transform lockOnTarget = null;
    [Header("Camera Mode")]
    public CamMode mode = CamMode.Static;
    [Header("Constraints")]
    public bool lockXMove;
    public bool lockYMove;
    public bool lockZMove;
    public bool lockXRot;
    public bool lockYRot;
    public bool locZRot;
    [Header("Static Mode Properties")]
    public Vector3 staticPoint = Vector3.zero;
    public bool isLookAt = false;
    [Header("Follow Mode Properties")]
    public bool followPlayer = true;
    public Vector3 pivotFwd = Vector3.forward;
    public float camFollow_speed = 100f;
    public Vector3 camFollow_offset = new Vector3(0, 1.5f, -5);
    public Quaternion camRotation = Quaternion.identity;

}

public enum CamMode
{
    Static,
    Follow,
    LockOn,
    Free
}