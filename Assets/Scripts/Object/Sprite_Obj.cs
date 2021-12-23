using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 12/22/21
 * Base script for sprites meant for a 3D space. Handles making them face the camera.
 */

public class Sprite_Obj : MonoBehaviour
{

    public Vector3 rotationOffset = Vector3.zero;

    protected virtual void LateUpdate()
    {
        //1. Set base rotation
        Quaternion rot = CameraController.main.transform.rotation;
        rot *= Quaternion.AngleAxis(-180, Vector3.up);
        //2. Apply rotation offset
        rot *= Quaternion.Euler(rotationOffset);
        //3. Set new rotation
        transform.rotation = rot;
    }
}
