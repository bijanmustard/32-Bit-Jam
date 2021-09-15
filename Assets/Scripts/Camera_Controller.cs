using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 9/13/21
 * Script for Camera Controller, controls camera movement and mode
 */

public class Camera_Controller : MonoBehaviour
{

    public Transform target;

    public Vector3 camFollow_offset = new Vector3(0, 2, -4);
    public Vector3 camRotation = Vector3.forward;
    public float camFollow_speed = 20f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {

        transform.position = Vector3.Lerp(transform.position,
            target.position + camFollow_offset,
            camFollow_speed * Time.smoothDeltaTime);
    }
}
